using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.SharedKernel;

namespace AdventOfCode2021.Day10
{
    class Program
    {
        static List<(char open, char close, int score)> bracketDefinition = new List<(char, char, int)> { ('(', ')', 3), ('[', ']', 57), ('{', '}', 1197), ('<', '>', 25137) };
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 10: Syntax Scoring"));
            Console.WriteLine("Navigation subsystem: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            int sumSyntaxErrorScore = getSyntaxErrorScores(puzzleInput.Lines).Sum();
            Console.WriteLine("Sum syntax error scxore: {0}", sumSyntaxErrorScore);
        }

        private static List<int> getSyntaxErrorScores(List<string> lines)
        {
            List<int> scores = new List<int>();

            foreach(string line in lines)
            {
                scores.Add(getSyntaxErrorScore(line));
            }

            return scores;
        }

        private static int getSyntaxErrorScore(string line)
        {
            int score = 0;
            List<char> openedChunks = new List<char>();

            foreach(char bracket in line)
            {
                if (isOpeningBracket(bracket))
                {
                    openedChunks.Add(bracket);
                }
                else if (isValidClosingBracket(openedChunks[openedChunks.Count - 1], bracket))
                {
                    openedChunks.RemoveAt(openedChunks.Count - 1);
                }
                else
                {
                    score = getScoreForBracket(bracket);
                    break;
                }
            }

            return score;
        }

        private static bool isOpeningBracket(char bracket)
        {
            return bracketDefinition.Where(b => b.open == bracket).Any();
        }

        private static bool isValidClosingBracket(char openingBracket, char closingBracket)
        {
            return bracketDefinition.Where(b => b.open == openingBracket).First().close == closingBracket;
        }

        private static int getScoreForBracket(char bracket)
        {
            return bracketDefinition.Where(b => b.close == bracket).First().score;
        }
    }
}
