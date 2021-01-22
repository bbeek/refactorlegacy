using System;

namespace TheatricalPlays
{
    class PerformanceCalculator
    {
        public Performance Performance { get; init; }
        public Play Play { get; init; }

        public virtual int Amount => throw new NotImplementedException("subclass responsibility");

        public virtual int VolumeCredits => Math.Max(Performance.Audience - 30, 0);

        public int Audience => Performance.Audience;
    }
}
