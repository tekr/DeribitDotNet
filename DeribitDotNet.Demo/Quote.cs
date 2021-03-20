using System;
using System.Collections.Generic;
using System.Linq;

namespace DeribitDotNet.Demo
{
    public class Quote
    {
        public DateTime Time { get; }

        public IList<Level> Bids { get; }

        public IList<Level> Asks { get; }

        public Quote(DateTime time, IList<Level> bids, IList<Level> asks)
        {
            Time = time;

            Bids = bids;
            Asks = asks;
        }

        public override string ToString() => $"{Time:HH:mm:ss.ffffff},{string.Join(',', Bids)} - {string.Join(',', Asks)}";

        public override bool Equals(object obj) => obj is Quote other && Bids.SequenceEqual(other.Bids) && Asks.SequenceEqual(other.Asks);

        public override int GetHashCode() => ((Bids?.GetHashCode() ?? 0) * 397) ^ (Asks?.GetHashCode() ?? 0);

        public static bool operator ==(Quote q1, Quote q2) => Equals(q1, q2);

        public static bool operator !=(Quote q1, Quote q2) => !(q1 == q2);
    }
}