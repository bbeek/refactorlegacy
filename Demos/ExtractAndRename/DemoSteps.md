1. Extract variable:

```
var outstanding = quantity * itemPrice -
                Math.Max(0m, quantity - 500) * itemPrice * 0.05m +
                Math.Min(quantity * itemPrice * 0.1m, 100);
```

```
decimal basePrice = quantity * itemPrice;
decimal quantityDiscount = Math.Max(0m, quantity - 500) * itemPrice * 0.05m;
decimal shipping = Math.Min(quantity * itemPrice * 0.1m, 100);
var outstanding = basePrice - quantityDiscount + shipping;
```

2. Extract function:

CalculateOutstandingAmount


```
decimal outstanding = CalculateOutstandingAmount();
```

```


        private decimal CalculateOutstandingAmount()
        {
            decimal basePrice = quantity * itemPrice;
            decimal quantityDiscount = Math.Max(0m, quantity - 500) * itemPrice * 0.05m;
            decimal shipping = Math.Min(quantity * itemPrice * 0.1m, 100);
            return basePrice - quantityDiscount + shipping;
        }

```

3. Rename variable:

rename `outstanding` to `dueAmount`

4. Rename function:

rename `CalculateOutstandingAmount` to `CalculateDueAmount`

5. Rename class:

rename `Order` to `OrderItem`