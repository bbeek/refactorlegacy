using ExtractAndRename;
using FluentAssertions;
using Xunit;

namespace ExtractAndRenameTests
{
    public class OrderTests
    {
        [Fact]
        public void PrintOwning_zero_price_is_allowed()
        {
            // Arrange
            var order = new Order(null, 600, 0);

            // Act
            var owing = order.PrintOwning();

            // Assert
            owing.Should().EndWithEquivalent($"Amount: 0,00");
        }

        [Fact]
        public void PrintOwning_zero_quantity_is_allowed()
        {
            // Arrange
            var order = new Order(null, 0, 10);

            // Act
            var owing = order.PrintOwning();

            // Assert
            owing.Should().EndWithEquivalent($"Amount: 0,00");
        }

        [Fact]
        public void PrintOwning_negative_price_is_allowed()
        {
            // Arrange
            var expectedPrice = (1 * -10) - 0 + -1;
            var order = new Order(null, 1, -10);

            // Act
            var owing = order.PrintOwning();

            // Assert
            owing.Should().Contain($"Amount: {expectedPrice}");
        }


        [Fact]
        public void PrintOwning_above_500_item_discount_is_substracted()
        {
            // Arrange
            var expectedPrice = (600 * 1m) - (100 * 0.05m) + 60m;
            var order = new Order(null, 600, 1);

            // Act
            var owing = order.PrintOwning();

            // Assert
            owing.Should().EndWithEquivalent($"Amount: {expectedPrice}");
        }

        [Fact]
        public void PrintOwning_shipping_capped_to_100()
        {
            // Arrange
            var expectedPrice = (1100 * 1m) - (600 * 0.05m) + 100m;
            var order = new Order(null, 1100, 1);

            // Act
            var owing = order.PrintOwning();

            // Assert
            owing.Should().EndWithEquivalent($"Amount: {expectedPrice}", "Uncapped shipping price would be 110");
        }
    }
}
