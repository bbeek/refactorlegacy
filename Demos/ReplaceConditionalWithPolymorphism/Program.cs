using System;

namespace ReplaceConditionalWithPolymorphism
{
    class Program
    {
        static void Main(string[] args)
        {
            var norwegian = new BirdProperties { Type = "NorwegianBlueParrot", Voltage = 1000 };

            var calculator = new FlightCalculator();
            var airSpeed = calculator.GetAirSpeed(norwegian);

            Console.WriteLine($"Flight speed of NorwegianBlueParrot is {airSpeed}");
        }
    }
}
