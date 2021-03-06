﻿using Microsoft.AspNetCore.Mvc;
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
        public PricesController()
        {
        }

        private static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(@"Database=lift_pass;Data Source=localhost;Trusted_Connection=true");
            connection.Open();
            return connection;
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync()
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
", GetConnection()))
            {
                command.Parameters.Add(new SqlParameter("@type", liftPassType) { DbType = DbType.String, Size = 255 });
                command.Parameters.Add(new SqlParameter("@cost", liftPassCost) { DbType = DbType.Int32 });
                command.Prepare();
                await command.ExecuteNonQueryAsync();
            }

            return Ok("");
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            int? age = this.Request.Query["age"] != StringValues.Empty ? Int32.Parse(this.Request.Query["age"]) : null;
            string liftPassType = this.Request.Query["type"];
            DateTime? skiDate = this.Request.Query["date"] != StringValues.Empty ? DateTime.ParseExact(this.Request.Query["date"], "yyyy-MM-dd", CultureInfo.InvariantCulture) : null;

            return await GetAsync(age, liftPassType, skiDate);

        }

        public async Task<ActionResult> GetAsync(int? age, string liftPassType, DateTime? skiDate)
        {
            double result = await RetrieveBasePrice(liftPassType);

            int reduction;

            if (age != null && age < 6)
            {
                return Ok("{ \"Cost\": 0}");
            }
            else
            {
                reduction = 0;

                if (!"night".Equals(liftPassType))
                {
                    bool isHoliday = await IsHoliday(skiDate);

                    if (skiDate.HasValue)
                    {

                        if (!isHoliday && (int)skiDate.Value.DayOfWeek == 1)
                        {
                            reduction = 35;
                        }
                    }

                    // TODO apply reduction for others
                    if (age != null && age < 15)
                    {
                        return Ok("{ \"Cost\": " + (int)Math.Ceiling(result * .7) + "}");
                    }
                    else
                    {
                        if (age == null)
                        {
                            // End of day discount
                            if (GetHour() > 15)
                            {
                                reduction += 5;
                            }

                            double cost = result * (1 - reduction / 100.0);
                            return Ok("{ \"Cost\": " + (int)Math.Ceiling(cost) + "}");
                        }
                        else
                        {
                            if (age > 64)
                            {
                                // Early bird discount
                                if (GetHour() < 9)
                                {
                                    reduction += 15;
                                }

                                double cost = result * .75 * (1 - reduction / 100.0);
                                return Ok("{ \"Cost\": " + (int)Math.Ceiling(cost) + "}");
                            }
                            else
                            {
                                // End of day discount
                                if (GetHour() > 15)
                                {
                                    reduction += 5;
                                }

                                double cost = result * (1 - reduction / 100.0);
                                return Ok("{ \"Cost\": " + (int)Math.Ceiling(cost) + "}");
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
                            return Ok("{ \"Cost\": " + (int)Math.Ceiling(result * .4) + "}");

                        }
                        else
                        {
                            return Ok("{ \"Cost\": " + result + "}");
                        }
                    }
                    else
                    {
                        return Ok("{ \"Cost\": 0}");
                    }
                }
            }

        }

        protected virtual int GetHour()
        {
            return DateTime.Now.Hour;
        }

        private async Task<bool> IsHoliday(DateTime? skiDate)
        {
            var isHoliday = false;
            using (var holidayCmd = new SqlCommand("SELECT * FROM holidays", GetConnection()))
            {
                holidayCmd.Prepare();
                using (var holidays = await holidayCmd.ExecuteReaderAsync())
                {
                    while (await holidays.ReadAsync())
                    {
                        var holiday = holidays.GetDateTime(holidays.GetOrdinal("holiday"));
                        if (skiDate.HasValue)
                        {
                            if (skiDate.Value.Year == holiday.Year &&
                                skiDate.Value.Month == holiday.Month &&
                                skiDate.Value.Date == holiday.Date)
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

            return isHoliday;
        }

        private async Task<double> RetrieveBasePrice(string liftPassType)
        {
            double result;
            using (var costCmd = new SqlCommand(@"SELECT cost FROM base_price WHERE type = @type", GetConnection()))
            {
                costCmd.Parameters.Add(new SqlParameter("@type", liftPassType) { DbType = DbType.String, Size = 255 });
                costCmd.Prepare();
                result = (int)(await costCmd.ExecuteScalarAsync());
            }

            return result;
        }
    }
}
