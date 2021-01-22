using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheatricalPlays
{
    record EnrichedPerformance : Performance
    {
        public Play Play { get; }
        public int Amount { get;  }
        public int VolumeCredits { get; }

        internal EnrichedPerformance(Performance original, Play play, int amount, int volumeCredits) : base(original)
        {
            Play = play;
            Amount = amount;
            VolumeCredits = volumeCredits;
        }
    }
}
