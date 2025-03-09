using System;
using System.IO;
using System.Linq;
using IronRure;

namespace grep;

class Program
{
    static void Main(string[] args)
    {
        Regex reg = new(args[0]);
        foreach (string path in args.Skip(1))
        {
            int lineNo = 1;
            foreach (string line in File.ReadLines(path))
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
