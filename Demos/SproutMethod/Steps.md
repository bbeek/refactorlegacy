
## Why:
When you really really need to add new functionality to a bit of legacy code but do not have the time to get the whole method in a testharness.
And the functionality you need to add fits a single method.

## How:

Creating the sprout method:
1. Identify where the new code should be called.

2. Determine what local variables to include from the source method and make those as arguments of the sprouted method.

3. Determine whether the sprouted method need to return values to the source method.
	If so, change the call so that its returned value is assigned to a variable in the source method.

4. Develop the sprout method

Testing:
1. Ensure that the sprout method is accessible from a subclass.

2. Create an encapsulating testing class.

3. Make the sprouted method available for testing.

4. Test sprout method

Finishing:
1. Add sprouted method to source function

For demo:

Create new method
```
        protected bool MatchEmail(Customer target)
        {
            var beforeDomainSource = this.Source.Email.Split("@")[0];
            var beforeDomainTarget = target.Email.Split('@')[0];

            var sourceTld = Source.Email.Substring(Source.Email.LastIndexOf("."));
            var targetTld = target.Email.Substring(target.Email.LastIndexOf("."));

            return beforeDomainSource == beforeDomainTarget && sourceTld == targetTld;
        }
```

Testing:
Create testing sprout: CustomerMatcherSprout
```
using SproutMethod;

namespace SproutMethodTests
{
    internal class CustomerMatcherSprout : CustomerMatcher
    {
        internal CustomerMatcherSprout(Customer source) : base(source)
        {
        }

        internal bool IsMatchEmail(Customer target)
        {
            return base.MatchEmail(target);
        }
    }
}
```

Finishing:
```
                case MatchingCriteria.Email:
                    return MatchEmail(target);
```