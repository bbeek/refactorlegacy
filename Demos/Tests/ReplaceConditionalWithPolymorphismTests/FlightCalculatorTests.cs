using FluentAssertions;
using ReplaceConditionalWithPolymorphism;
using Xunit;

namespace ReplaceConditionalWithPolymorphismTests
{
    public class FlightCalculatorTests
    {
        [Fact]
        public void GetAirSpeed_EuropeanSwallow_should_return_35()
        {
            // Arrange
            const int expected = 35;
            var sut = new BirdProperties() { Type = "EuropeanSwallow" };

            // Act
            var outcome = new FlightCalculator().GetAirSpeed(sut);

            // Assert
            outcome.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, 40)]
        [InlineData(2, 36)]
        [InlineData(21, -2)]
        public void GetAirSpeed_AfricanSwallow_should_return_expectedSpeed(int numberOfCoconuts, int expectedSpeed)
        {
            // Arrange
            var sut = new BirdProperties() { Type = "AfricanSwallow", NumberOfCoconuts = numberOfCoconuts };

            // Act
            var outcome = new FlightCalculator().GetAirSpeed(sut);

            // Assert
            outcome.Should().Be(expectedSpeed);
        }

        [Fact]
        public void GetAirSpeed_Nailed_NorwegianBlueParrot_should_return_0()
        {
            // Arrange
            const int expected = 0;
            var sut = new BirdProperties() { Type = "NorwegianBlueParrot", IsNailed = true };

            // Act
            var outcome = new FlightCalculator().GetAirSpeed(sut);

            // Assert
            outcome.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(99, 19)]
        [InlineData(2020, 212)]
        public void GetAirSpeed_NotNailed_NorwegianBlueParrot_should_return_expectedSpeed(int voltage, int expectedSpeed)
        {
            // Arrange
            var sut = new BirdProperties() { Type = "NorwegianBlueParrot", Voltage = voltage, IsNailed = false};

            // Act
            var outcome = new FlightCalculator().GetAirSpeed(sut);

            // Assert
            outcome.Should().Be(expectedSpeed);
        }

        [Theory]
        [InlineData("Unknown")]
        [InlineData("Parrot")]
        [InlineData("Chimpansee")]
        public void GetAirSpeed_Unknown_type_should_return_0(string type)
        {
            // Arrange
            const int expected = 0;
            var sut = new BirdProperties() { Type = type };

            // Act
            var outcome = new FlightCalculator().GetAirSpeed(sut);

            // Assert
            outcome.Should().Be(expected);
        }
    }
}
