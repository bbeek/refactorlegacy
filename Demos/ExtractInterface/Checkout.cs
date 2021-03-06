﻿using CommonObjects;

namespace ExtractInterface
{
    public class Checkout
    {
        private readonly ReceiptRepository repository;

        public Checkout(ReceiptRepository repository)
        {
            this.repository = repository;
        }

        public Receipt CreateReceipt(Money amount)
        {
            var receipt = new Receipt();
            var vat = amount.Percentage(20);

            receipt.Amount = amount;
            receipt.Tax = vat;
            receipt.Total = amount.Add(vat);

            repository.Store(receipt);

            return receipt;
        }
    }
}
