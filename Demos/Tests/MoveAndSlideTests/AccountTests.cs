using FluentAssertions;
using MoveAndSlide;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MoveAndSlideTests
{
    public class AccountTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetBankCharge_not_overdraft_should_return_basecharge(bool isPremium) 
        {
            // Arrange
            const decimal baseCharge = 4.5m;
            var accountType = new AccountType(isPremium);
            var account = new Account(accountType, 0);

            // Act
            var outcome = account.GetBankCharge();

            // Assert
            outcome.Should().Be(baseCharge);
        }

        [Fact]
        public void GetBankCharge_standard_account_overdraft_should_return_expected_charge()
        {
            // Arrange
            const decimal expectedCharge = 4.5m + 1.75m;
            var accountType = new AccountType(false);
            var account = new Account(accountType, 1);

            // Act
            var outcome = account.GetBankCharge();

            // Assert
            outcome.Should().Be(expectedCharge);
        }

        public static IEnumerable<object[]> UnChargedPremiumOverdraftDays => Enumerable.Range(1, 7).Select(i => new object[] { i }).ToList();

        [Theory]
        [MemberData(nameof(UnChargedPremiumOverdraftDays))]
        public void GetBankCharge_premium_account_overdraft_less_then_seven_days_should_return_basecharge(int daysOverDraft)
        {
            // Arrange
            const decimal expectedCharge = 14.5m;
            var accountType = new AccountType(true);
            var account = new Account(accountType, daysOverDraft);

            // Act
            var outcome = account.GetBankCharge();

            // Assert
            outcome.Should().Be(expectedCharge);
        }

        [Fact]
        public void GetBankCharge_premium_account_overdraft_more_then_seven_days_should_return_surcharge()
        {
            // Arrange
            const decimal expectedCharge = 14.5m + (6*0.85m);
            var accountType = new AccountType(true);
            var account = new Account(accountType, 13);

            // Act
            var outcome = account.GetBankCharge();

            // Assert
            outcome.Should().Be(expectedCharge);
        }
    }
}
