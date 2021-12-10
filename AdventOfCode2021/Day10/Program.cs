using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.SharedKernel;

namespace AdventOfCode2021.Day10
{
    class Program
    {
        static List<(char open, char close, int corruptedScore, int incompleteScore)> bracketDefinition 
                        = new List<(char, char, int, int)> { ('(', ')', 3, 1), ('[', ']', 57, 2), ('{', '}', 1197, 3), ('<', '>', 25137, 4) };
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 10: Syntax Scoring"));
            Console.WriteLine("Navigation subsystem: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            int sumSyntaxErrorScore = getSyntaxErrorScores(puzzleInput.Lines).Sum();
            Console.WriteLine("Sum syntax error scxore: {0}", sumSyntaxErrorScore);

            List<Int64> incompletedScores = getIncompletedScores(puzzleInput.Lines);
            //allways odd, division 2 and following upround will give the correct index
            Int64 middleScoreIncompleted = incompletedScores.OrderBy(s => s).ToList()[(int)incompletedScores.Count/2];
            Console.WriteLine("Middle score of incompleted lines: {0}", middleScoreIncompleted);
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

        private static List<Int64> getIncompletedScores(List<string> lines)
        {
            List<Int64> scores = new List<Int64>();

            foreach (string line in lines)
            {
                if (isLineIncompleted(line))
                {
                    scores.Add(getInvalidScore(line));
                }
            }

            return scores;
        }

        private static bool isLineIncompleted(string line)
        {
            List<char> openedChunks = getChunkList(line);

            return (openedChunks.Count > 0) & (getSyntaxErrorScore(line) == 0);
        }

        private static List<char> getChunkList(string line, out int lastIndex)
        {
            List<char> openedChunks = new List<char>();
            lastIndex = -1;

            foreach (char bracket in line)
            {
                lastIndex++;
                if (isOpeningBracket(bracket))
                {
                    openedChunks.Add(bracket);
                }
                else if (isValidClosingBracket(openedChunks[openedChunks.Count - 1], bracket))
                {
                    openedChunks.RemoveAt(openedChunks.Count - 1);
                }
                
            }

            return openedChunks;
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
                    score = getCurruptedScoreForBracket(bracket);
                    break;
                }
            }

            return score;
        }
        private static Int64 getInvalidScore(string line)
        {
            Int64 score = 0;
            List<char> openedChunks = getChunkList(line);
            openedChunks.Reverse();

            foreach (char chunk in openedChunks)
            {
                score *= 5;
                score += getIncompleteScoreForBracket(getClosingBracket(chunk));
            }

            return score;
        }

        private static bool isOpeningBracket(char bracket)
        {
            return bracketDefinition.Where(b => b.open == bracket).Any();
        }

        private static bool isValidClosingBracket(char openingBracket, char closingBracket)
        {
            return getClosingBracket(openingBracket) == closingBracket;
        }

        private static char getClosingBracket(char openingBracket)
        {
            return bracketDefinition.Where(b => b.open == openingBracket).First().close;
        }

        private static int getCurruptedScoreForBracket(char bracket)
        {
            return bracketDefinition.Where(b => b.close == bracket).First().corruptedScore;
        }

        private static int getIncompleteScoreForBracket(char bracket)
        {
            return bracketDefinition.Where(b => b.close == bracket).First().incompleteScore;
        }
    }
}
