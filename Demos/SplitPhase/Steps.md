Approach

1. Extract second method using automated Extract Method (Edit > Refactor > Extract method) refactoring

New method named `CalcaluteOrderPrice`

```

        private static decimal CalcaluteOrderPrice(string[] orderData, decimal productPrice)
        {
            return int.Parse(orderData[1]) * productPrice;
        }
```

And test!

2. Create intermediate datastructure and add to second method (using `Change signature` refactor, Ctrl+'.')

New class `OrderData.cs`

```
namespace SplitPhase
{
    internal class OrderData {

    }

/*
 * Extract second method
 * Test
 * create intermediate data structure (+ add to second method)
 * for each parameter, move to intermediate data structure.
 * extract first method
 */
}
```

```
return CalcaluteOrderPrice(orderData, productPrice, new OrderData());
```

```
        private static decimal CalcaluteOrderPrice(string[] orderData, decimal productPrice, OrderData data)
        {
            return int.Parse(orderData[1]) * productPrice;
        }
```

And test!

3. For each parameter used in second method, create property

3.1:

Create property

```
namespace SplitPhase
{
    internal class OrderData {
        public decimal ProductPrice;
    }

}
```

Use property in second method
```
        private static decimal CalcaluteOrderPrice(string[] orderData, decimal productPrice, OrderData data)
        {
            return int.Parse(orderData[1]) * data.ProductPrice;
        }
```

Initialize property
```
var parsedOrderData = new OrderData() { ProductPrice = productPrice };
return CalcaluteOrderPrice(orderData, productPrice, parsedOrderData);
```

Remove unused parameter (using refactor Change Signature)

```
        private static decimal CalcaluteOrderPrice(string[] orderData, OrderData data)
        {
            return int.Parse(orderData[1]) * data.ProductPrice;
        }
```

And test!
3.2

Create property

```
namespace SplitPhase
{
    internal class OrderData {
        public decimal ProductPrice;
        public int Quantity;
    }

}
```


Use property in second method
```
        private static decimal CalcaluteOrderPrice(string[] orderData, OrderData data)
        {
            return data.Quantity * data.ProductPrice;
        }
```

Initialize property
```
 var parsedOrderData = new OrderData() { ProductPrice = productPrice, Quantity = int.Parse(orderData[1]) };
 return CalcaluteOrderPrice(orderData, parsedOrderData);
```

Remove unused parameter
```
        private static decimal CalcaluteOrderPrice(OrderData data)
        {
            return data.Quantity * data.ProductPrice;
        }
```

And test!

4. Extract first part using Extract Method

```
        private OrderData ParseOrder(string order)
        {
            var orderData = order.Split(" ");

            var productPrice = priceList[orderData[0].Split("-")[1]];
            var parsedOrderData = new OrderData() { ProductPrice = productPrice, Quantity = int.Parse(orderData[1]) };
            return parsedOrderData;
        }
```

```
        public decimal Calculate(string order)
        {
            var parsedOrderData = ParseOrder(order);
            var orderPrice = CalcaluteOrderPrice(parsedOrderData);
            return orderPrice;
        }
```

And test!