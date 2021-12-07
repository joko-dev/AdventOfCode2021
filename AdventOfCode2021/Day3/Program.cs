using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Diagnostic report:");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine());

            string gammaRateBitmask = determineCommonBitmask(puzzleInput.Lines, true);
            string epsilonRateBitmask = determineCommonBitmask(puzzleInput.Lines, false);

            int gammaRate = calculateBitmask(gammaRateBitmask);
            int epsilonRate = calculateBitmask(epsilonRateBitmask);

            Console.WriteLine("Power consumption = gamma rate ({0}) * epsilon rate ({1}) = {2}", gammaRate, epsilonRate, gammaRate * epsilonRate);
        }

        private static int calculateBitmask(string bitmask)
        {
            return Convert.ToInt32(bitmask, 2);
        }

        private static string determineCommonBitmask(List<string> input, bool mostCommon)
        {
            int length = input[0].Length;
            char[] bitmask = new char[length];

            for (int i = 0; i < length; i++)
            {
                bitmask[i] = determineCommonBit(input, i, mostCommon);
            }

            return new string(bitmask);
        }

        private static char determineCommonBit(List<string> input, int index, bool mostCommon)
        {
            int count0 = 0;
            int count1 = 0;

            foreach (string line in input)
            {
                if(line[index] == '0')
                {
                    count0++;
                }
                else if (line[index] == '1')
                {
                    count1++;
                }
            }

            if (mostCommon)
            {
                if (count0 > count1)
                {
                    return '0';
                }
                else
                {
                    return '1';
                }
            }
            else
            {
                if (count0 < count1)
                {
                    return '0';
                }
                else
                {
                    return '1';
                }
            }
        }
    }
}
