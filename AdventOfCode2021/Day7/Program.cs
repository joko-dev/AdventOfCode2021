using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Crab submarine positions:");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            List<int> positions = getPositions(puzzleInput.Lines);
            int optimalPosition = getMedian(positions);
            int totalFuelConsumption = getFuelConsumption(positions, optimalPosition);
            Console.WriteLine("Total fuel for position ({0}): {1}", optimalPosition, totalFuelConsumption);

            // Mean ist the solution, but both variants (+-0.5) has to be checked
            // https://www.reddit.com/r/adventofcode/comments/rar7ty/comment/hnkd58g/?utm_source=share&utm_medium=web2x&context=3
            optimalPosition = getMean(positions, 0.5);
            totalFuelConsumption = getFuelConsumptionCrabWay(positions, optimalPosition);
            Console.WriteLine("Total fuel for position ({0}), manipulator{1} - the crab way: {2}", optimalPosition, 0.5, totalFuelConsumption);
            optimalPosition = getMean(positions, -0.5);
            totalFuelConsumption = getFuelConsumptionCrabWay(positions, optimalPosition);
            Console.WriteLine("Total fuel for position ({0}), manipulator{1} - the crab way: {2}", optimalPosition, -0.5, totalFuelConsumption);
        }

        private static int getFuelConsumptionCrabWay(List<int> positions, int optimalPosition)
        {
            int totalFuelConsumption = 0;

            foreach (int position in positions)
            {
                List<int> fuelDemand = Enumerable.Range(1, Math.Abs(optimalPosition - position)).ToList();
                totalFuelConsumption += fuelDemand.Sum();
            }

            return totalFuelConsumption;
        }

        private static int getMean(List<int> positions, double manipulator)
        {
            int average = (int)Math.Round(positions.Average() + manipulator);

            return average;
        }

        private static int getFuelConsumption(List<int> positions, int optimalPosition)
        {
            int totalFuelConsumption = 0;

            foreach (int position in positions)
            {
                totalFuelConsumption += Math.Abs(optimalPosition - position);
            }

            return totalFuelConsumption;
        }

        private static int getMedian(List<int> positions)
        {
            int median;
            int index = (int)Math.Round((decimal)positions.Count / 2) - 1;

            positions.Sort();
            median = positions[index];

            return median;
        }

        private static List<int> getPositions(List<string> lines)
        {
            List<int> positions = new List<int>();
            foreach(string line in lines)
            {
                string[] temp = line.Split(',');
                positions.AddRange(Array.ConvertAll(temp, s => int.Parse(s)));
            }

            return positions;
        }
    }
}
