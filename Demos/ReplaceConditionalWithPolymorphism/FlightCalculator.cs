namespace ReplaceConditionalWithPolymorphism
{
    public class FlightCalculator
    {
        /*
         * 1. Create factory function
         * 2. Move conditional code to superclass
         * 3. Take a subclass, create override
         * 4. Repeat for every conditional
         * 5. Adjust function is superclass (consider making superclass abstract)
        */
        public int GetAirSpeed(BirdProperties properties)
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
