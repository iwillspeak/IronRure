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
                if (set.IsMatch(line))
                {
                    Console.WriteLine(new String('*', line.Length));
                }
                else
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
