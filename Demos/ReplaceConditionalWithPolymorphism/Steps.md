1. Create factory function

If the polymorphic behaviour does not exist (as is here the case), create them together with the factory function

First create new base class Bird.cs

```

namespace ReplaceConditionalWithPolymorphism
{
    internal class Bird
    {
        protected readonly BirdProperties properties;

        internal Bird(BirdProperties properties)
        {
            this.properties = properties;
        }
    }
}
```

Then create a new subclass EuropeanSwallow.cs

```

namespace ReplaceConditionalWithPolymorphism
{
    internal class EuropeanSwallow : Bird
    {
        internal EuropeanSwallow(BirdProperties properties) : base(properties)
        {
        }
    }
}
```

AfricanSwallow.cs
```

namespace ReplaceConditionalWithPolymorphism
{
    internal class AfricanSwallow : Bird
    {
        internal AfricanSwallow(BirdProperties properties) : base(properties)
        {
        }
    }
}
```

NorwegianBlueParrot.cs
```

namespace ReplaceConditionalWithPolymorphism
{
    internal class NorwegianBlueParrot : Bird
    {
        internal NorwegianBlueParrot(BirdProperties properties) : base(properties)
        {
        }
    }
}
```

Then create the factory class

```
        
        private Bird CreateBird(BirdProperties properties)
        {
            switch (properties.Type)
            {
                case "EuropeanSwallow":
                    return new EuropeanSwallow(properties);
                case "AfricanSwallow":
                    return new AfricanSwallow(properties);
                case "NorwegianBlueParrot":
                    return new NorwegianBlueParrot(properties);
                default:
                    return new Bird(properties);
            }
        }
```

2. Move conditional code to superclass (and use the factory function)

Copy the conditional code into Bird.cs and make it fit the new context (so removed the parameter properties)

Bird.cs
```

namespace ReplaceConditionalWithPolymorphism
{
    internal class Bird
    {
        private readonly BirdProperties properties;

        internal Bird(BirdProperties properties)
        {
            this.properties = properties;
        }

        internal virtual int GetAirSpeed()
        {
            switch (properties.Type)
            {
                case "EuropeanSwallow":
                    return 35;
                case "AfricanSwallow":
                    return 40 - 2 * properties.NumberOfCoconuts;
                case "NorwegianBlueParrot":
                    return properties.IsNailed ? 0 : 10 + properties.Voltage / 10;
                default:
                    return 0;
            }
        }
    }
}

```

Use the factory function and call the copied code.

FlightCalculator.cs
```
        public int GetAirSpeed(BirdProperties properties)
        {
            return CreateBird(properties).GetAirSpeed();
        }
```

Test

3. Take a subclass and create an override

EuropeanSwallow.cs
```

        internal override int GetAirSpeed()
        {
            return 35;
        }
```

4. Repeat:

AfricanSwallow.cs
```
        internal override int GetAirSpeed()
        {
            return 40 - 2 * properties.NumberOfCoconuts;
        }
```

5. Repeat:

NorwegianBlueParrot.cs
```

        internal override int GetAirSpeed()
        {
            return properties.IsNailed ? 0 : 10 + properties.Voltage / 10;
        }
```

6. Adjust function is superclass (consider making superclass abstract)

Bird.cs

```

        internal virtual int GetAirSpeed()
        {
            return 0;
        }
```