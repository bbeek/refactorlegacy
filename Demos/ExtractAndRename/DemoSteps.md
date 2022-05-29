1. Extract variable:

```
var outstanding = quantity * itemPrice -
                Math.Max(0m, quantity - 500) * itemPrice * 0.05m +
                Math.Min(quantity * itemPrice * 0.1m, 100);
```

replace all local variables for basePrice
test after each rename

```
decimal basePrice = quantity * itemPrice;
decimal quantityDiscount = Math.Max(0m, quantity - 500) * itemPrice * 0.05m;
decimal shipping = Math.Min(basePrice * 0.1m, 100);
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
            decimal shipping = Math.Min(basePrice * 0.1m, 100);
            return basePrice - quantityDiscount + shipping;
        }

```
Test

3. Rename variable:

rename `outstanding` to `dueAmount`
Test

4. Rename function:

rename `CalculateOutstandingAmount` to `CalculateDueAmount`
Test

5. Rename class:

rename `Order` to `OrderItem`
Test