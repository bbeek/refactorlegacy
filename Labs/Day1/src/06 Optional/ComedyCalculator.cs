using System;

namespace TheatricalPlays
{
    class ComedyCalculator : PerformanceCalculator
    {
        public override int Amount
        {
            get
            {
                var result = 30000 + (300 * Performance.Audience);
                if (Performance.Audience > 20)
                {
                    result += 10000 + 500 * (Performance.Audience - 20);
                }

                return result;
            }
        }

        public override int VolumeCredits => base.VolumeCredits + (int)Math.Floor((decimal)Performance.Audience / 5);
    }
}
