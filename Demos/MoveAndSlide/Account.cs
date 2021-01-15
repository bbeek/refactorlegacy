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
            var result = 4.5m;
            if (DaysOverdrawn > 0)
            {
                result += GetOverdraftCharge();
            }


            return result;
        }

        private decimal GetOverdraftCharge()
        {
            if (accountType.IsPremium)
            {
                const decimal baseCharge = 10m;

                if (DaysOverdrawn > 7)
                {
                    return baseCharge + (DaysOverdrawn - 7) * 0.85m;
                }
                return baseCharge;
            }
            else
            {
                return DaysOverdrawn * 1.75m;
            }
        }
    }
}
