﻿using Day2.Logic;
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
        private readonly TicketService ticketService;

        public PricesController(TicketService ticketService)
        {
            this.ticketService = ticketService;
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

        public async Task<ActionResult> GetAsync(int? age, string liftPassType, DateTime? skidate)
        {
            var response = await ticketService.CalculatePrice(age, liftPassType, skidate);

            return Ok(response.RoundUp());

        }


    }
}
