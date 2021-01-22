Introduce a getter of the global reference with which to steer behavior

## Why
When a global variable blocks the class of getting under control in a testharness.

## How
1. Identify the global reference to replace

In this example, we want to replace the `DateTime` getter

2. Write an overridable getter for the global reference

Apply the **extract method** refactoring on `DateTime.Now`.
Change into an overridable method:
```
        protected virtual DateTime GetDateTime()
        {
            return DateTime.Now;
        }
```

3. Replace references to global with calls with the getter

4. Create a testing subclass and override the getter

Create a new subclass of the global reference with the required functionality. `TestingFridayMarketingCampaign`

```
using NodaTime;
using ReplaceGlobalReference;
using System;

namespace ReplaceGlobalReferenceTests
{
    internal class TestingFridayMarketingCampaign : MarketingCampaign
    {
        protected override DateTime GetDateTime()
        {
            return new LocalDate().Next(IsoDayOfWeek.Friday).ToDateTimeUnspecified();
        }
    }
}
```

Finally replace the `MarketingCampaign` to `TestingFridayMarketingCampaign` in the unittest