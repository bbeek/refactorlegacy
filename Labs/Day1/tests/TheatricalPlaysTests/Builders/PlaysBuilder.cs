using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheatricalPlays;

namespace TheatricalPlaysTests.Builders
{
    internal class PlaysBuilder
    {
        private Dictionary<string, Play> plays = new Dictionary<string, Play>
                {
                    {"hamlet", new Play("Hamlet", PlayType.Tragedy)},
                    {"as-like", new Play("As You Like It", PlayType.Comedy)},
                    {"othello", new Play("Othello", PlayType.Tragedy)}
                };

        public IReadOnlyDictionary<string, Play> Build()
        {
            return plays.ToImmutableDictionary();
        }

        internal PlaysBuilder WithPlay(string playId, PlayType playType)
        {
            plays.Add(playId, new Play(playId, playType));

            return this;
        }
    }
}
