using System;
using System.Collections.Generic;
using AdventOfCode2021.SharedKernel;

namespace AdventOfCode2021.Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 1: Sonar Sweep"));
            Console.WriteLine("Depth measurement file:");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine());

            List<int> depths = puzzleInput.Lines.ConvertAll(int.Parse);


            Console.WriteLine("Larger measurements: {0}", largerMeasurements(depths));
            Console.WriteLine("Larger measurements one-sliding-window: {0}", largerMeasurementsSliding(depths,1));
            Console.WriteLine("Larger measurements three-sliding-window: {0}", largerMeasurementsSliding(depths, 3));
        }

        private static int largerMeasurements(List<int> depths)
        {
            int inc = 0;

            for (int i = 0; i < depths.Count; i++)
            {
                if ((i > 0) && (depths[i] > depths[i - 1]))
                {
                    inc++;
                }
            }

            return inc;
        }

        private static int largerMeasurementsSliding(List<int> depths, int windowSize)
        {
            int inc = 0;

            for (int i = windowSize; i < depths.Count; i++)
            {
                int current = getSumWindow(depths, i, windowSize);
                int previous = getSumWindow(depths, i - 1, windowSize);
                if (current > previous)
                {
                    inc++;
                }
            }

            return inc;
        }

        private static int getSumWindow(List<int> depths, int upperIndex, int windowSize)
        {
            int sum = 0;
            for(int i = 0; i < windowSize; i++)
            {
                sum = sum + depths[upperIndex - i];
            }

            return sum;
        }
    }
}
