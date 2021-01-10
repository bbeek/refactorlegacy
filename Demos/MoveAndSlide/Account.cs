using System;

namespace MoveAndSlide
{
    public class Account
    {
        private readonly AccountType accountType;

        public Account(AccountType accountType, int daysOverdrawn)
        {
            this.accountType = accountType ?? throw new ArgumentNullException(nameof(accountType));
            DaysOverdrawn = daysOverdrawn;
        }

        public int DaysOverdrawn { get; }

        /*
         * Slide statement:
         *      Move related code together
         * 
         * Move function:
         *   1. Check all calling functions, see if those also need to move (if so, move them first)
         *   2. Check that the function is not polymorphic
         *   3. Copy the function to the new class; adjust it to the new context
         *   4. Call the target class in the original source function.
         *   5. Test
         *   6. Consider Inline function on source.
         */
        public decimal GetBankCharge()
        {
            var premiumOverdraft = 0m;
            if (accountType.IsPremium)
            {
                premiumOverdraft = 10m;

                if (DaysOverdrawn > 7)
                {
                    premiumOverdraft += (DaysOverdrawn - 7) * 0.85m;
                }
            }

            decimal result;
            if (DaysOverdrawn > 0)
            {
                result = 4.5m;
                if (premiumOverdraft > 0)
                {
                    result += premiumOverdraft;
                }
                else
                {
                    result += DaysOverdrawn * 1.75m;
                }
            }
            else
            {
                result = 4.5m;
            }
           
            return result;
        }

    }
}
