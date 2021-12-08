using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;

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
        }

        private static int getFuelConsumption(List<int> positions, int optimalPosition)
        {
            int totalFuelConsumption = 0;

            foreach(int position in positions)
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
