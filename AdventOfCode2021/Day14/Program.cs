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

            string polymer = polymerizationBruteForce(puzzleInput.Lines, 10);
            var polymerElements = polymer.GroupBy(p => p).Select(p => new { Element = p.Key, Count = p.Count() }).OrderBy(p => p.Count);
            Console.WriteLine("10 steps - Most common - least Common: {0}", polymerElements.Last().Count - polymerElements.First().Count);

            Dictionary<char, long> polymerCalculation = polymerizationCalculate(puzzleInput.Lines, 40);
            var polymerCalculationElements = polymerCalculation.OrderBy(p => p.Value);
            Console.WriteLine("40 steps - Most common - least Common: {0}", polymerCalculationElements.Last().Value - polymerCalculationElements.First().Value);
        }

        private static Dictionary<char, long> polymerizationCalculate(List<string> lines, int steps)
        {
            Dictionary<char, long> polymers = new Dictionary<char, long>();
            Dictionary<string, long> pairs = new Dictionary<string, long>();
            List<(string pair, char insert)> rules = getInsertionRules(lines);

            // initialize chars
            foreach(char c in lines.First())
            {
                if (!polymers.ContainsKey(c))
                {
                    polymers.Add(c,0);
                }
                polymers[c]++;
            }

            // initialize pairs
            for(int i = 0; i < lines.First().Length - 1; i++)
            {
                string pair = lines.First()[i].ToString() + lines.First()[i + 1].ToString();
                if (!pairs.ContainsKey(pair))
                {
                    pairs.Add(pair, 0);
                }
                pairs[pair]++;
            }

            // process steps
            for (int s = 1; s <= steps; s++)
            {
                Dictionary<string, long> pairsTemp = new Dictionary<string, long>(pairs);

                foreach((string pair, char insert) rule in rules)
                {
                    if (pairs.ContainsKey(rule.pair))
                    {
                        if (!polymers.ContainsKey(rule.insert)) { polymers.Add(rule.insert, 0); }

                        polymers[rule.insert] += pairs[rule.pair];

                        pairsTemp[rule.pair] = pairsTemp[rule.pair] - pairs[rule.pair];

                        string newLeft = rule.pair[0].ToString() + rule.insert;
                        string newRight = rule.insert.ToString() + rule.pair[1];

                        if (!pairsTemp.ContainsKey(newLeft)) { pairsTemp.Add(newLeft, 0); }
                        if (!pairsTemp.ContainsKey(newRight)) { pairsTemp.Add(newRight, 0); }

                        pairsTemp[newLeft] += pairs[rule.pair];
                        pairsTemp[newRight] += pairs[rule.pair];
                    }
                }

                pairs = pairsTemp;
            }

            return polymers;
        }

        private static string polymerizationBruteForce(List<string> lines, int steps)
        {
            string polymer = lines.First();
            List<(string pair, char insert)> rules = getInsertionRules(lines);

            for(int s = 1; s <= steps; s++)
            {
                string polymerTemp = "";
                for(int i = 0; i < polymer.Length - 1; i++)
                {
                    polymerTemp += polymer[i];
                    foreach ((string pair, char insert) rule in rules)
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

        private static List<(string pair, char insert)> getInsertionRules(List<string> lines)
        {
            List<(string pair, char insert)> rules = new List<(string pair, char insert)>();
            for (int i = 1; i < lines.Count; i++)
            {
                string[] rule = lines[i].Split("->");
                rules.Add((rule[0].Trim(), rule[1].Trim()[0]));
            }

            return rules;
        }
    }
}
