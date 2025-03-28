﻿using System;
using System.Linq;
using IronRure;

namespace filtering;

internal class Program
{
    private static readonly string[] BadWords = ["red", "green", "blue"];

    private static void Main()
    {
        RegexSet set = new(BadWords);
        var expressions = BadWords.Select(p => new Regex(p)).ToList();

        while (Console.ReadLine() is { } line)
        {
            var match = set.Matches(line);
            if (match.Matched)
            {
                for (var i = 0; i < match.Matches.Length; i++)
                {
                    if (!match.Matches[i])
                    {
                        continue;
                    }

                    foreach (var found in expressions[i].FindAll(line))
                    {
                        line = line.Replace(found.ExtractedString, new string('*', found.ExtractedString.Length));
                    }
                }
            }

            Console.WriteLine(line);
        }
    }
}
