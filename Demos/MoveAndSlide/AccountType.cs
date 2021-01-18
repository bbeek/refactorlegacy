using System;
using System.Collections.Generic;
using System.Text;

namespace MoveAndSlide
{
    public class AccountType
    {
        public AccountType(bool isPremium)
        {
            IsPremium = isPremium;
        }

        public bool IsPremium { get; }

    }
}
