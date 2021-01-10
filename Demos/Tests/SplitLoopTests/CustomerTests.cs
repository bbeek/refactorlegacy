using FluentAssertions;
using SplitLoop;
using SplitLoopTests.Builders;
using System;
using Xunit;

namespace SplitLoopTests
{
    public class CustomerTests
    {
        [Fact]
        public void Statement_Rented_movie_should_be_listed_once()
        {
            // Arrange
            const string expectedTitle = "Matrix";
            var rental = new RentalBuilder().WithTitle(expectedTitle).Build();
            var customer = new Customer(null);
            customer.AddRental(rental);

            // Act
            var outcome = customer.Statement();

            // Assert
            outcome.Should().Contain(expectedTitle, Exactly.Once());
        }

        [Fact]
        public void Statement_Rental_for_only_two_days_total_amount_should_be_movieprice()
        {
            const decimal movieprice = 2.0m;
            var rental = new RentalBuilder().HasPrice(movieprice).IsRentedFor(2).Build();
            var customer = new Customer(null);
            customer.AddRental(rental);

            // Act
            var outcome = customer.Statement();

            outcome.Should().Contain($"Amount owed is {movieprice}");
        }


        [Fact]
        public void Statement_Rental_for_more_than_two_days_total_amount_should_have_surcharge()
        {
            const decimal movieprice = 2.0m;
            const decimal expectedPrice = 5.0m;
            var rental = new RentalBuilder().HasPrice(movieprice).IsRentedFor(4).Build();
            var customer = new Customer(null);
            customer.AddRental(rental);

            // Act
            var outcome = customer.Statement();

            outcome.Should().Contain($"Amount owed is {expectedPrice}", "Price calculation should be: 2 + ((4 - 2) * 1.5)");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Statement_X_normal_rental_should_result_in_X_frequentrenterpoint(int rentals)
        {
            // Arrange
            var expectedPoints = rentals;
            var customer = new Customer(null);
            for(int i = 0; i < rentals; i++)
            {
                customer.AddRental(new RentalBuilder().Build());
            }

            // Act
            var outcome = customer.Statement();

            // Assert
            outcome.Should().EndWith($"{expectedPoints} frequent renter points");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Statement_Rental_for_new_release_and_rented_more_than_2_days_should_result_in_double_frequentrenterpoints(int newreleaseRentals)
        {
            // Arrange
            var expectedPoints = newreleaseRentals * 2;
            var customer = new Customer(null);
            for (int i = 0; i < newreleaseRentals; i++)
            {
                customer.AddRental(new RentalBuilder().ForNewRelease().IsRentedFor(3).Build());
            }

            // Act
            var outcome = customer.Statement();

            // Assert
            outcome.Should().EndWith($"{expectedPoints} frequent renter points");
        }
    }
}
