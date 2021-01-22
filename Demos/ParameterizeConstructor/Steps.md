Introduce additional parameters to constructor without breaking existing clients.

## Why:
If we want to or need to control behavior on the target class that is influenced by objects created in its constructor.

## How:

1. Copy the existing constructor

2.1. Add parameter to copied constructor (either manual or via **Change function declaration** refactoring)

2.2. Assign new parameter to instance variable in copied constructor.

3. Modify the original constructor to call the new constructor

### Alternative steps:

1. use default arguments feature. In C# called `Optional arguments` but this only works on:
- a constant expression;
- an expression of the form `new ValType()`, where `ValType` is a value type, such as an *enum* or a *struct*;
- an expression of the form `default(ValType)`, where `ValType` is a *value type*.
https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/named-and-optional-arguments

Instead we need to use then the following construct:

```
        public Discount(MarketingCampaign campaign = null)
        {
            this.marketingCampaign = campaign ?? new MarketingCampaign();
        }
```

## Disadvantage
Opens the door to future dependencies on the class of the new parameter.
Other users might want to use the object passed into the new parameter for other reason.
Overall a small risk.


For demo:
Also create a new class `FridayMarketingCampaign`

```
using ParameterizeConstructor;

namespace ParameterizeConstructorTests
{
    internal class FridayMarketingCampaign : MarketingCampaign
    {
        public override bool IsCrazySalesDay()
        {
            return true;
        }
    }
}
```

Adjust the test to always use the crazy sale
```
var discount = new Discount(new FridayMarketingCampaign());
```