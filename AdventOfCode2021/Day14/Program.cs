using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 14: Extended Polymerization"));
            Console.WriteLine("Polymer data: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            string polymer = polymerization(puzzleInput.Lines, 10);
            var polymerElements = polymer.GroupBy(p => p).Select(p => new { Element = p.Key, Count = p.Count() }).OrderBy(p => p.Count);
            Console.WriteLine("Most common - least Common: {0}", polymerElements.Last().Count - polymerElements.First().Count);
        }

        private static string polymerization(List<string> lines, int steps)
        {
            string polymer = lines.First();
            List<(string pair, string insert)> rules = getInsertionRules(lines);

            for(int s = 1; s <= steps; s++)
            {
                string polymerTemp = "";
                for(int i = 0; i < polymer.Length - 1; i++)
                {
                    polymerTemp += polymer[i];
                    foreach ((string pair, string insert) rule in rules)
                    {
                        if (polymer.Substring(i,2) == rule.pair)
                        {
                            polymerTemp += rule.insert;
                            break;
                        }
                    }
                }
                polymerTemp += polymer.Last();
                polymer = polymerTemp;
            }

            return polymer;
        }

        private static List<(string pair, string insert)> getInsertionRules(List<string> lines)
        {
            List<(string pair, string insert)> rules = new List<(string pair, string insert)>();
            for (int i = 1; i < lines.Count; i++)
            {
                string[] rule = lines[i].Split("->");
                rules.Add((rule[0].Trim(), rule[1].Trim()));
            }

            return rules;
        }
    }
}
