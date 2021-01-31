using Day2.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Day2.Repositories
{
    public class PriceRepository : IPriceRepository
    {

        private static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(@"Database=lift_pass;Data Source=localhost;Trusted_Connection=true");
            connection.Open();
            return connection;
        }

        public async Task<Ticket> RetrieveBasePrice(string liftPassType)
        {
            double result;
            using (var costCmd = new SqlCommand(@"SELECT cost FROM base_price WHERE type = @type", GetConnection()))
            {
                costCmd.Parameters.Add(new SqlParameter("@type", liftPassType) { DbType = DbType.String, Size = 255 });
                costCmd.Prepare();
                result = (int)(await costCmd.ExecuteScalarAsync());
            }

            return new Ticket(result);
        }

        public async Task<bool> IsHoliday(DateTime? skiDate)
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
    }
}
