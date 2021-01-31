using Day2.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Day2Tests
{
    public class PricesTest
    {
        [Fact]
        public async Task GetAsync_price_for_child_nightticket_should_be_free()
        {
            // Arrange
            const int child = 5;
            const string nightTicket = "night";
            var sut = new PricesControllerBuilder().WithNightTicket().Build();

            // Act
            var price = await sut.GetAsync(child, nightTicket, null);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 0}");
        }

        [Fact]
        public async Task GetAsync_price_for_adult_dayticket_after_endofday_time_for_tomorrow_should_return_discounted_price()
        {
            // Arrange
            const int adult = 25;
            const string dayTicket = "day";
            var tomorrow = DateTime.Today.AddDays(1);
            var sut = new PricesControllerBuilder().WithDayTicket().WithEndOfDayDiscountTime().Build();

            // Act
            var price = await sut.GetAsync(adult, dayTicket, tomorrow);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 34}");
        }

        [Fact]
        public async Task GetAsync_price_for_adult_nightticket_should_be_full_price()
        {
            // Arrange
            const int adult = 25;
            const string nightTicket = "night";
            var sut = new PricesControllerBuilder().WithNightTicket().Build();

            // Act
            var price = await sut.GetAsync(adult, nightTicket, null);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 19}");
        }

        [Fact]
        public async Task GetAsync_price_for_senior_nightticket_should_be_8()
        {
            // Arrange
            const int senior = 75;
            const string nightTicket = "night";
            var sut = new PricesControllerBuilder().WithNightTicket().Build();

            // Act
            var price = await sut.GetAsync(senior, nightTicket, null);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 8}");
        }

        [Fact]
        public async Task GetAsync_price_for_child_dayticket_should_be_free()
        {
            // Arrange
            const int child = 5;
            const string dayTicket = "day";
            var sut = new PricesControllerBuilder().WithDayTicket().Build();

            // Act
            var price = await sut.GetAsync(child, dayTicket, null);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 0}");
        }

        [Fact]
        public async Task GetAsync_price_for_senior_dayticket_be_27()
        {
            // Arrange
            const int senior = 75;
            const string dayTicket = "day";
            var sut = new PricesControllerBuilder().WithDayTicket().Build();

            // Act
            var price = await sut.GetAsync(senior, dayTicket, null);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 27}");
        }

        [Fact]
        public async Task GetAsync_price_for_below15_dayticket_should_be_25()
        {
            // Arrange
            const int child = 12;
            const string dayTicket = "day";
            var sut = new PricesControllerBuilder().WithDayTicket().Build();

            // Act
            var price = await sut.GetAsync(child, dayTicket, null);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 25}");
        }

        [Fact]
        public async Task GetAsync_price_for_adult_dayticket_for_a_monday_not_a_holiday_should_be_23()
        {
            // Arrange
            const int adult = 25;
            const string dayTicket = "day";
            var monday = GetNextMonday();
            var sut = new PricesControllerBuilder().WithDayTicket().WithRequestedDateIsNotAHoliday().Build();

            // Act
            var price = await sut.GetAsync(adult, dayTicket, monday);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 23}");
        }

        [Fact]
        public async Task GetAsync_price_for_adult_dayticket_after_endofday_time_for_today_should_be_34()
        {
            // Arrange
            const int adult = 25;
            const string dayTicket = "day";
            var sut = new PricesControllerBuilder().WithDayTicket().WithEndOfDayDiscountTime().Build();

            // Act
            var price = await sut.GetAsync(adult, dayTicket, DateTime.Today);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 34}");
        }

        [Fact]
        public async Task GetAsync_price_for_adult_dayticket_after_endofday_time_for_a_monday_not_a_holiday_should_be_21()
        {
            // Arrange
            const int adult = 25;
            const string dayTicket = "day";
            var monday = GetNextMonday();
            var sut = new PricesControllerBuilder().WithDayTicket().WithEndOfDayDiscountTime().WithRequestedDateIsNotAHoliday().Build();

            // Act
            var price = await sut.GetAsync(adult, dayTicket, monday);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 21}");
        }

        [Fact]
        public async Task GetAsync_price_for_senior_dayticket_earlybird_for_today_should_be_23()
        {
            // Arrange
            const int senior = 75;
            const string dayTicket = "day";
            var sut = new PricesControllerBuilder().WithDayTicket().WithEarlyBirdDiscountTime().Build();

            // Act
            var price = await sut.GetAsync(senior, dayTicket, DateTime.Today);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 23}");
        }

        [Fact]
        public async Task GetAsync_price_for_senior_dayticket_earlybird_for_a_monday_not_a_holiday_should_be_14()
        {
            // Arrange
            const int senior = 75;
            const string dayTicket = "day";
            var monday = GetNextMonday();
            var sut = new PricesControllerBuilder().WithDayTicket().WithEarlyBirdDiscountTime().WithRequestedDateIsNotAHoliday().Build();

            // Act
            var price = await sut.GetAsync(senior, dayTicket, monday);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 14}");
        }

        [Fact]
        public async Task GetAsync_price_for_unknown_age_dayticket_should_be_fullprice()
        {
            // Arrange
            const string dayTicket = "day";
            var sut = new PricesControllerBuilder().WithDayTicket().Build();

            // Act
            var price = await sut.GetAsync(null, dayTicket, null);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 35}");
        }

        [Fact]
        public async Task GetAsync_price_for_adult_dayticket_should_be_fullprice()
        {
            // Arrange
            const int adult = 25;
            const string dayTicket = "day";
            var sut = new PricesControllerBuilder().WithDayTicket().Build();

            // Act
            var price = await sut.GetAsync(adult, dayTicket, null);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 35}");
        }

        [Fact]
        public async Task GetAsync_price_for_unknown_age_dayticket_a_monday_not_a_holiday_should_be_23()
        {
            // Arrange
            const string dayTicket = "day";
            var monday = GetNextMonday();
            var sut = new PricesControllerBuilder().WithDayTicket().WithRequestedDateIsNotAHoliday().Build();

            // Act
            var price = await sut.GetAsync(null, dayTicket, monday);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 23}");
        }

        [Fact]
        public async Task GetAsync_price_for_unknown_age_dayticket_endofday_for_a_monday_not_a_holiday_should_be_21()
        {
            // Arrange
            const string dayTicket = "day";
            var monday = GetNextMonday();
            var sut = new PricesControllerBuilder().WithDayTicket().WithRequestedDateIsNotAHoliday().WithEndOfDayDiscountTime().Build();

            // Act
            var price = await sut.GetAsync(null, dayTicket, monday);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 21}");
        }
		
        [Fact]
        public async Task GetAsync_price_for_unknown_age_nightticket_should_be_free()
        {
            // Arrange
            const string nightTicket = "night";
            var sut = new PricesControllerBuilder().WithNightTicket().Build();

            // Act
            var price = await sut.GetAsync(null, nightTicket, null);

            // Assert
            ((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 0}");
        }
		
        private DateTime GetNextMonday()
        {
            return new LocalDate().Next(IsoDayOfWeek.Monday).ToDateTimeUnspecified();
        }
    }
}
