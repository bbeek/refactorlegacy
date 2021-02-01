Subclass and override a dependency that we either want to separate in order to test or to sense in order to detect results in test

## Why:
- Safe, only modifies accessibility modifiers
- Minimal impact
- Breaks hard dependency with a fake

## How:

1. Identify the dependencies separate or sense

2. Make each method overridable

    Add `virtual` to `CreateForwardMessage` method

3. Adjust visibility so that it can be overridden in a subclass

    Change `private` to `protected` for `CreateForwardMessage` method

4. Create subclass that overrides the methods

```c#
using SubclassAndOverride;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubclassAndOverrideTests
{
    class FakeMessageForwarder : MessageForwarder
    {
        

    }
}
```

And override the required methods

```c#
        protected override MailMessage CreateForwardMessage(MailMessage message)
        {
            return null;
        }
```

## Disadvantage
Only works on classes you can edit (obviously), meaning that this does not directly work on hard dependencies outside of you control. 
(Use *Skin and Wrap* for these hard dependencies). Out-of-scope for these labs

Is an [antipattern](https://wiki.c2.com/?SubclassToTestAntiPattern) when used for "normal" unittests due to:
* Complexity grows rapidly and unpredictably.
* Reuse of test code is difficult.
* Refactoring of production code becomes more and more difficult. 
