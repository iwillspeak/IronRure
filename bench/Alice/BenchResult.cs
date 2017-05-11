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
            Mean = _ticks.Sum() / _ticks.Count;
            Median = _ticks[_ticks.Count / 2];
            Variance = Math.Sqrt(_ticks.Select(t => Math.Pow(t - Mean, 2)).Sum() / _ticks.Count);
        }

        public string Name { get; }

        public long Mean { get; }

        public long Median { get; }

        public double Variance { get; }

        private List<long> _ticks;
        public IEnumerable<long> Ticks => _ticks;

        public override string ToString() =>
            $"{Name}: {string.Join(",\t", _ticks)} (mean: {Mean}, median: {Median}, r: {Variance:0.00})";
    }
}