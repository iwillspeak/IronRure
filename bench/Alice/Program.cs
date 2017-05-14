using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

using NetRegex = System.Text.RegularExpressions.Regex;

using IronRure;

namespace Alice
{
    class Program
    {
        static void Main(string[] args)
        {
            var exePath = Assembly.GetEntryAssembly().Location;;
            var exeFolder = Path.GetDirectoryName(exePath);
            var path = Path.Combine(exeFolder, AliceFilename);
            var text = File.ReadAllText(path, Encoding.UTF8);

            BenchRegex("legged", @"\b(\w+)-legged", text);
            BenchRegex("alice", @"Alice", text);
            BenchRegex("alice_rooted", @"^Alice", text);
            BenchRegex("alice_rooted2", @"Alice$", text);
            BenchRegex("numbers", @"\d+", text);
            BenchRegex("abwords", "a[^x]{20}b", text);
            BenchRegex("email", @"\w+@\w+.\w+", text);
            BenchRegex("quotes", "\"[^\"]+\"", text);
            BenchRegex("quotes2", "\"[^\"]{0,30}[?!.]\"", text);
            BenchRegex("quote_said", "\"[^\"]+\"\\s+said", text);
            BenchRegex("section", @"(\*\s+){4}\*", text);
            BenchRegex("repeated_negation", @"[a-q][^u-z]{13}x", text);
            BenchRegex("ing_suffix", @"[a-zA-Z]+ing", text);
            BenchRegex("name_alt", @"Alice|Adventure", text);
            BenchRegex("name_alt2", @"Alice|Hatter|Cheshire|Dinah", text);
            BenchRegex("name_alt3", @".{0,3}(Alice|Hatter|Cheshire|Dinah)", text);
            BenchRegex("nomatch_uncommon", @"zqj", text);
            BenchRegex("nomatch_common", @"aei", text);
            BenchRegex("common", "(?i)the", text);
            BenchRegex("short_lines", "^.{16,20}$", text);
            BenchRegex("dotplus", ".+", text);
            BenchRegex("dotplus_nl", "(?s).+", text);
            BenchRegex("alice_hattter", "Alice.{0,25}Hatter|Hatter.{0,25}Alice", text);
            BenchRegex("name_suffixes", "([A-Za-z]lice|[A-Za-z]heshire)[^a-zA-Z]", text);
            BenchRegex("dotstar", ".*", text);

            Console.WriteLine("Benchmark completed");
        }

        /// <summary>
        ///   Benchmark the execution of a given regex of the given
        ///   text. Compiles the regex using both the .NET engine and
        ///   Rure outside of the benchmark and then benches finding
        ///   all the matches in the text.
        /// </summary>
        private static void BenchRegex(string name, string pattern, string text)
        {
            var rure = new Regex(pattern);
            var net = new NetRegex(pattern);
            var bytes = Encoding.UTF8.GetBytes(text);

            Console.WriteLine("{0} ({1})", name, pattern);

            var results = new List<BenchResult>();
            
            // by doing the search this way we have to convert all 172k into
            // a .NET string for each step in the search. Surprisingly this
            // is _still_ faster than .NET in some cases.
            results.Add(Bench($"rure::{name}", () => {
                    foreach (var match in rure.FindAll(text))
                        ;
                }));
            results.Add(Bench($"byts::{name}", () => {
                    foreach (var match in rure.FindAll(bytes))
                        ;
                }));
            results.Add(Bench($".net::{name}", () => {
                    var match = net.Match(text);
                    while (match.Success)
                    {
                        match = match.NextMatch();
                    }
                }));

            var winner = results.OrderBy(r => r.Median).First();

            foreach (var r in results)
            {
                if (r == winner)
                {
                    Console.WriteLine("{0}, Winner", r);
                }
                else
                {
                    var diff = (r.Median - winner.Median) / (double)winner.Median;
                    Console.WriteLine("{0}, {1:0.0}x slower", r, diff);
                }             
            }
        }

        /// <summary>
        ///   Benchmark an Action. Executes the action repeatedly and
        ///   returns a results object detailing the outcome.
        /// </summary>
        private static BenchResult Bench(string name, Action benchee)
        {
            var sw = new Stopwatch();
            var resultTicks = new List<long>(4);
            for (int i = 0; i < 4; i++)
            {
                sw.Start();
                benchee();
                sw.Stop();
                resultTicks.Add(sw.ElapsedTicks);
                sw.Reset();
            }

            return new BenchResult(name, resultTicks);
        }

        private static string AliceFilename = "Alice's_Adventures_in_Wonderland.txt";
    }
}
