using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 11: Dumbo Octopus"));
            Console.WriteLine("Octopus energy levels: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            int flashes = countFlashes(generateOctopuses(PuzzleConverter.getInputAsMatrixInt(puzzleInput.Lines)), 100);
            Console.WriteLine("Count flashes: {0}", flashes);
        }

        private static int countFlashes((int energyLevel, bool flashed)[,] octopuses, int stepCount)
        {
            int flashes = 0;

            for (int step = 1; step <= stepCount; step++)
            {
                increaseAllEnergyLevels(octopuses);
                checkFlashes(octopuses);

                flashes += (from (int energyLevel, bool flashed) octo in octopuses where octo.energyLevel > 9 select octo.energyLevel).Count();

                resetAllFlashedOctopuses(octopuses);
            }

            return flashes;
        }

        private static void increaseAllEnergyLevels((int energyLevel, bool flashed)[,] octopuses)
        {
            for(int x = 0; x < octopuses.GetLength(0); x++)
            {
                for(int y = 0; y < octopuses.GetLength(1); y++)
                {
                    octopuses[x, y].energyLevel++;
                }
            }
        }

        private static void checkFlashes((int energyLevel, bool flashed)[,] octopuses)
        {
            for (int x = 0; x < octopuses.GetLength(0); x++)
            {
                for (int y = 0; y < octopuses.GetLength(1); y++)
                {
                    checkFlash(octopuses, (x, y), false);
                }
            }
        }

        private static void checkFlash((int energyLevel, bool flashed)[,] octopuses, (int x, int y) point, bool increaseEnergyLevel)
        {
            if (increaseEnergyLevel) { octopuses[point.x, point.y].energyLevel++; }
            if (octopuses[point.x, point.y].energyLevel >= 10 && !octopuses[point.x, point.y].flashed)
            {
                octopuses[point.x, point.y].flashed = true;
                List <(int x, int y)> adjacents = PuzzleConverter.getAdjacentPoints(generateMatrix(octopuses), point, true, true, true);

                foreach ((int x, int y) adjacent in adjacents )
                {
                    checkFlash(octopuses, adjacent, true);
                }
            }
        }

        private static (int energyLevel, bool flashed)[,] generateOctopuses(int[,] matrix)
        {
            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);
            (int energyLevel, bool flashed)[,] octopuses = new (int energyLevel, bool flashed)[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    octopuses[x, y].energyLevel = matrix[x, y];
                    octopuses[x, y].flashed = false;
                }
            }
            return octopuses;
        }
        private static int[,] generateMatrix((int energyLevel, bool flashed)[,] octopuses)
        {
            int width = octopuses.GetLength(0);
            int height = octopuses.GetLength(1);
            int[,] matrix = new int[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                     matrix[x, y] = octopuses[x, y].energyLevel;
                }
            }
            return matrix;
        }

        private static void resetAllFlashedOctopuses((int energyLevel, bool flashed)[,] octopuses)
        {
            for (int x = 0; x < octopuses.GetLength(0); x++)
            {
                for (int y = 0; y < octopuses.GetLength(1); y++)
                {
                    if (octopuses[x, y].energyLevel >= 10) 
                    { 
                        octopuses[x, y].energyLevel = 0;
                        octopuses[x, y].flashed = false;
                    }
                }
            }
        }
    }
}
