using System;
using System.IO;
using System.Linq;
using IronRure;

namespace grep
{
    class Program
    {
        static void Main(string[] args)
        {
            var reg = new Regex(args[0]);
            foreach (var path in args.Skip(1))
            {
                int lineNo = 1;
                foreach (var line in File.ReadLines(path))
                {
                    if (reg.IsMatch(line))
                    {
                        Console.WriteLine("{0}:{1}:{2}", path, lineNo, line);
                    }
                    lineNo++;
                }
            }
        }
    }
}
