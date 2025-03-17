using System;
using System.IO;
using System.Linq;
using IronRure;

namespace grep;

internal class Program
{
    private static void Main(string[] args)
    {
        Regex reg = new(args[0]);
        foreach (var path in args.Skip(1))
        {
            var lineNo = 1;
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
