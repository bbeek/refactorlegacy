1. Extract variable:

```
var outstanding = quantity * itemPrice -
                Math.Max(0m, quantity - 500) * itemPrice * 0.05m +
                Math.Min(quantity * itemPrice * 0.1m, 100);
```

replace all local variables for basePrice with Ctrl+. -> introduce local. (for basePrice, for all occurences)
test after each rename

```
decimal basePrice = quantity * itemPrice;
decimal quantityDiscount = Math.Max(0m, quantity - 500) * itemPrice * 0.05m;
decimal shipping = Math.Min(basePrice * 0.1m, 100);
var outstanding = basePrice - quantityDiscount + shipping;
```

2. Extract function:
Make PrintOwning adhere to clean code, ie. do one thing (and do it well ;), extract CalculateOutstandingAmount

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

rename `outstanding` to `dueAmount` as the it does not display the outstanding but the due amount
Test

4. Rename function:

rename `CalculateOutstandingAmount` to `CalculateDueAmount`, idem.
Test

5. Rename class:

rename `Order` to `OrderItem`
Test