using System;

namespace MoveAndSlide
{
    class Program
    {
        static void Main(string[] args)
        {
            var premiumAccount = new AccountType(true);
            var account = new Account(premiumAccount, 14);
            Console.WriteLine($"Bank charge {account.GetBankCharge()}");
        }
    }
}
