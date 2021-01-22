using FluentAssertions;
using ReplaceGlobalReference;
using Xunit;

namespace ReplaceGlobalReferenceTests
{
    public class MarketingCampaignTest
    {
        [Fact]
        public void IsCrazySalesDay_on_friday_should_be_true()
        {
            // Arrange
            var campaign = new MarketingCampaign();

            // Act
            var isCrazySalesDay = campaign.IsCrazySalesDay();

            // Assert
            isCrazySalesDay.Should().BeTrue();
        }
    }
}
