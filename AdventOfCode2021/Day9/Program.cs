using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 9: Smoke Basin"));
            Console.WriteLine("Heightmap: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            int[,] matrix = PuzzleConverter.getInputAsMatrixInt(puzzleInput.Lines);
            int sumRiskLevels = getSumRiskLevelsLowpoints(matrix);
            Console.WriteLine("Sum of risk levels: {0}", sumRiskLevels);

            List<int> basinSizes = getBasinSizes(matrix);
            int faktorBasinSize = basinSizes.OrderByDescending(b => b).Take(3).Aggregate((x,y) => x*y);
            Console.WriteLine("Factor size of basins: {0}", faktorBasinSize);
        }

        private static int getSumRiskLevelsLowpoints(int[,] matrix)
        {
            List<(int, int)> lowpoints = getLowPoints(matrix);
            int sum = 0;
            
            foreach((int x,int y) point in lowpoints)
            {
                sum += matrix[point.x, point.y] + 1;
            }

            return sum;
        }

        private static List<(int x, int y)> getLowPoints(int[,] matrix)
        {
            List<(int, int)> points = new List<(int, int)>();
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    if (isLowPoint(matrix, (x, y)))
                    {
                        points.Add((x,y));
                    }
                }
            }
            return points;
        }

        private static bool isLowPoint(int[,] matrix, (int x, int y) point)
        {
            bool lowpoint = true;
            int lowpointValue = matrix[point.x, point.y];
            List <(int x, int y)> adjacentPoints = PuzzleConverter.getAdjacentPoints(matrix, point, true, true, false);

            foreach((int x, int y) adjacent in adjacentPoints)
            {
                lowpoint &= lowpointValue < matrix[adjacent.x, adjacent.y];
            }

            return lowpoint;
        }

        

        private static List<int> getBasinSizes(int[,] matrix)
        {
            List<int> basinSizes = new List<int>();
            List<(int,int)> lowpoints = getLowPoints(matrix);

            foreach((int x, int y) point in lowpoints)
            {
                basinSizes.Add(getBasinSize(matrix, point));
            }

            return basinSizes;
        }

        private static int getBasinSize(int[,] matrix, (int x, int y) lowpoint)
        {
            List<(int x, int y)> basin = createBasin(matrix, lowpoint);
            int size = basin.Count;

            return size;
        }

        private static List<(int x, int y)> createBasin(int[,] matrix, (int x, int y) lowpoint)
        {
            // task defines that every lowpoint and every other point != height 9 is in exactly one basin--> easy recursion
            List<(int x, int y)> basin = new List<(int x, int y)>();
            fillBasin(matrix, basin, lowpoint);

            return basin;
        }

        private static void fillBasin(int[,] matrix, List<(int x, int y)> basin, (int x, int y) point)
        {
            // Location width height 9 arent in any basin, all others are
            if(matrix[point.x, point.y] != 9)
            {
                basin.Add(point);
                List<(int x, int y)> pointsToCheck = PuzzleConverter.getAdjacentPoints(matrix, point, true, true, false);
                foreach((int x, int y) adjacent in pointsToCheck)
                {
                    if (!basin.Contains(adjacent))
                    {
                        fillBasin(matrix, basin, adjacent);
                    }
                }
            }
        }
    }
}
