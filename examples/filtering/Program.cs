using System;
using System.Linq;
using IronRure;

namespace filtering
{
    class Program
    {
        private static readonly string[] BadWords = new[] { "red", "green","blue" };

        static void Main(string[] args)
        {
            var set = new RegexSet(BadWords);
            var expressions = BadWords.Select(p => new Regex(p)).ToList();

            string line;
            while ((line = Console.ReadLine()) != null)
            {
                var match = set.Matches(line);
                if (match.Matched)
                {
                    for (int i = 0; i < match.Matches.Length; i++)
                    {
                        if (match.Matches[i])
                        {
                            foreach (var found in expressions[i].FindAll(line))
                            {
                                line = line.Replace(found.ExtractedString, new string('*', found.ExtractedString.Length));
                            }
                        }
                    }
                }
                Console.WriteLine(line);
            }
        }
    }
}
