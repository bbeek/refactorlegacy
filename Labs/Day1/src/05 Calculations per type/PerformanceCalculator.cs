using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheatricalPlays
{
    class PerformanceCalculator
    {
        public Performance Performance { get; init; }
        public Play Play { get; init; }

        public virtual int GetAmount() => throw new NotImplementedException("subclass responsibility");

        public virtual int GetVolumeCredits() => Math.Max(Performance.Audience - 30, 0);
    }
}
