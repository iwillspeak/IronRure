using System;
using System.Linq;
using System.Collections.Generic;

namespace Alice
{
    public class BenchResult
    {
        public BenchResult(string name, List<long> ticks)
        {
            Name = name;
            _ticks = ticks;
            _ticks.Sort();
            Mean = _ticks.Sum() / _ticks.Count;
            var idx = (_ticks.Count / 2);
            Median = _ticks.Count % 2 == 0 ? (_ticks[idx] + _ticks[idx - 1]) / 2 : _ticks[idx];
            Variance = Math.Sqrt(_ticks.Select(t => Math.Pow(t - Mean, 2)).Sum() / _ticks.Count);
        }

        public string Name { get; }

        public long Mean { get; }

        public long Median { get; }

        public double Variance { get; }

        private List<long> _ticks;
        public IEnumerable<long> Ticks => _ticks;

        public override string ToString()
        {
            var ticks = string.Join(",", _ticks.Select(t => string.Format("{0,10}", t)));
            return $"{Name,15}: {ticks} (mean: {Mean,10}, median: {Median,10}, r: {Variance:0.00})";
        }
    }
}
