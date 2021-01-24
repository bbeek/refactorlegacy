using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace Day2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class PricesController : ControllerBase
    {
        private readonly SqlConnection connection;

        public PricesController()
        {
            connection = new SqlConnection(@"Database=lift_pass;Data Source=localhost;Trusted_Connection=true");
            connection.Open();
        }

        [HttpPut]
        public async Task<string> PutAsync()
        {
            int liftPassCost = int.Parse(this.Request.Query["cost"]);
            string liftPassType = this.Request.Query["type"].ToString();

            using (var command = new SqlCommand(
@"MERGE INTO base_price AS prices
  USING 
    (SELECT type = @type, cost = @cost) AS source
  ON prices.type = source.type
  WHEN MATCHED THEN
    UPDATE SET cost = source.cost
  WHEN NOT MATCHED THEN
    INSERT (type, cost)
    VALUES (source.type, source.cost); 
", connection))
            {
                command.Parameters.Add(new SqlParameter("@type", liftPassType) { DbType = DbType.String, Size = 255 });
                command.Parameters.Add(new SqlParameter("@cost", liftPassCost) { DbType = DbType.Int32 });
                command.Prepare();
                await command.ExecuteNonQueryAsync();
            }

            return "";
        }

        [HttpGet]
        public async Task<string> Get()
        {
            int? age = this.Request.Query["age"] != StringValues.Empty ? Int32.Parse(this.Request.Query["age"]) : null;

            using (var costCmd = new SqlCommand(@"SELECT cost FROM base_price WHERE type = @type", connection))
            {
                costCmd.Parameters.Add(new SqlParameter("@type", this.Request.Query["type"].ToString()) { DbType = DbType.String, Size=255 });
                costCmd.Prepare();
                double result = (int)(await costCmd.ExecuteScalarAsync());

                int reduction;
                var isHoliday = false;

                if (age != null && age < 6)
                {
                    return "{ \"cost\": 0}";
                }
                else
                {
                    reduction = 0;

                    if (!"night".Equals(this.Request.Query["type"]))
                    {
                        using (var holidayCmd = new SqlCommand("SELECT * FROM holidays", connection))
                        {
                            holidayCmd.Prepare();
                            using (var holidays = await holidayCmd.ExecuteReaderAsync())
                            {
                                while (await holidays.ReadAsync())
                                {
                                    var holiday = holidays.GetDateTime(holidays.GetOrdinal("holiday"));
                                    if (this.Request.Query["date"] != StringValues.Empty)
                                    {
                                        DateTime d = DateTime.ParseExact(this.Request.Query["date"], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                        if (d.Year == holiday.Year &&
                                            d.Month == holiday.Month &&
                                            d.Date == holiday.Date)
                                        {
                                            isHoliday = true;
                                        }
                                    }
                                    else
                                    {
                                        if (DateTime.Now.Year == holiday.Year &&
                                            DateTime.Now.Month == holiday.Month &&
                                            DateTime.Now.Date == holiday.Date)
                                        {
                                            isHoliday = true;
                                        }
                                    }
                                }

                            }
                        }

                        if (this.Request.Query["date"] != StringValues.Empty)
                        {
                            DateTime d = DateTime.ParseExact(this.Request.Query["date"], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            if (!isHoliday && (int)d.DayOfWeek == 1)
                            {
                                reduction = 35;
                            }
                        }

                        // TODO apply reduction for others
                        if (age != null && age < 15)
                        {
                            return "{ \"cost\": " + (int)Math.Ceiling(result * .7) + "}";
                        }
                        else
                        {
                            if (age == null)
                            {
                                double cost = result * (1 - reduction / 100.0);
                                return "{ \"cost\": " + (int)Math.Ceiling(cost) + "}";
                            }
                            else
                            {
                                if (age > 64)
                                {
                                    double cost = result * .75 * (1 - reduction / 100.0);
                                    return "{ \"cost\": " + (int)Math.Ceiling(cost) + "}";
                                }
                                else
                                {
                                    double cost = result * (1 - reduction / 100.0);
                                    return "{ \"cost\": " + (int)Math.Ceiling(cost) + "}";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (age != null && age >= 6)
                        {
                            if (age > 64)
                            {
                                return "{ \"cost\": " + (int)Math.Ceiling(result * .4) + "}";
                            }
                            else
                            {
                                if (DateTime.Now.Hour < 9)
                                {
                                    return "{ \"cost\": " + (int)Math.Ceiling(result * .2) + "}";
                                }
                                else
                                {
                                    return "{ \"cost\": " + result + "}";
                                }

                                
                            }
                        }
                        else
                        {
                            return "{ \"cost\": 0}";
                        }
                    }
                }
            }
        }

    }
}
