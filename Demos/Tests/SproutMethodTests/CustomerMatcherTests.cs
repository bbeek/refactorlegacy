using FluentAssertions;
using SproutMethod;
using Xunit;

namespace SproutMethodTests
{
    public class CustomerMatcherTests
    {
        //[Fact]
        //public void MatchEmail_only_username_and_tld_match_should_return_true()
        //{
        //    // Arrange
        //    var source = new Customer(0, null, null, "test@domain.verycool");
        //    var target = new Customer(0, null, null, "test@example.verycool");

        //    var matcher = new CustomerMatcherSprout(source);

        //    // Act
        //    var matched = matcher.IsMatchEmail(target);

        //    // Assert
        //    matched.Should().BeTrue();
        //}

        //[Fact]
        //public void MatchEmail_only_username_and_domain_match_should_return_false()
        //{
        //    // Arrange
        //    var source = new Customer(0, null, null, "test@domain.not");
        //    var target = new Customer(0, null, null, "test@domain.matching");

        //    var matcher = new CustomerMatcherSprout(source);

        //    // Act
        //    var matched = matcher.IsMatchEmail(target);

        //    // Assert
        //    matched.Should().BeFalse();
        //}

        //[Fact]
        //public void MatchEmail_only_domain_and_tld_match_should_return_false()
        //{
        //    // Arrange
        //    var source = new Customer(0, null, null, "username@domain.matching");
        //    var target = new Customer(0, null, null, "test@domain.matching");

        //    var matcher = new CustomerMatcherSprout(source);

        //    // Act
        //    var matched = matcher.IsMatchEmail(target);

        //    // Assert
        //    matched.Should().BeFalse();
        //}
    }
}
