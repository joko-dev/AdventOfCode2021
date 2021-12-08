using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hydrothermal vents file: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            int[,] ventsMap = getMap(puzzleInput.Lines, false);
            int overlappingPoints = getCountOverlappingPoints(ventsMap, 2);
            Console.WriteLine("Horizontal/Vertical Overlapping Points: {0}", overlappingPoints );

            ventsMap = getMap(puzzleInput.Lines, true);
            overlappingPoints = getCountOverlappingPoints(ventsMap, 2);
            Console.WriteLine("All Overlapping Points: {0}", overlappingPoints);
        }

        private static int getCountOverlappingPoints(int[,] ventsMap, int minOverlapping)
        {
            int count = 0;

            count = (from int item in ventsMap where item >= minOverlapping select item).Count();

            return count;
        }

        private static int[,] getMap(List<string> lines, bool includeDiagonal)
        {
            List<(Coordinate from, Coordinate to)> coordinates = getCoorinatePair(lines);
            int maxX, maxY;
            getMaxCoordinateValues(coordinates, out maxX, out maxY);
            // Array-Boundaries are + 1
            int[,] map = new int[maxX + 1,maxY + 1];

            foreach((Coordinate from, Coordinate to) pair in coordinates)
            {
                //horizontal lines
                if(pair.from.X == pair.to.X)
                {
                    for(int i = Math.Min(pair.from.Y, pair.to.Y); i <= Math.Max(pair.from.Y, pair.to.Y); i++)
                    {
                        map[pair.from.X, i] += 1;
                    }
                }
                //vertical lines
                if (pair.from.Y == pair.to.Y)
                {
                    for (int i = Math.Min(pair.from.X, pair.to.X); i <= Math.Max(pair.from.X, pair.to.X); i++)
                    {
                        map[i, pair.from.Y] += 1;
                    }
                }
                // diagonal, only viable if 45° --> difference betwenn X-coords = Difference between Y-coords
                if (includeDiagonal && (Math.Abs(pair.from.X - pair.to.X) == Math.Abs(pair.from.Y - pair.to.Y)))
                {
                    for (int i = 0; i <= Math.Abs(pair.from.X - pair.to.X); i++)
                    {
                        int x = 0;
                        int y = 0;
                        if (pair.from.X < pair.to.X)
                        {
                            x = pair.from.X + i;
                            y = pair.from.Y;
                            y += (pair.from.Y < pair.to.Y) ? i : -i;
                        }
                        else
                        {
                            x = pair.to.X + i;
                            y = pair.to.Y;
                            y += (pair.to.Y < pair.from.Y) ? i : -i;
                        }
                        
                        map[x, y] += 1;
                    }
                }
            }

            return map;
        }

        private static void getMaxCoordinateValues(List<(Coordinate from, Coordinate to)> coordinates, out int maxX, out int maxY)
        {
            maxX = 0;
            maxY = 0;

            foreach((Coordinate from, Coordinate to) pair in coordinates)
            {
                maxX = Math.Max(maxX, Math.Max(pair.from.X, pair.to.X));
                maxY = Math.Max(maxY, Math.Max(pair.from.Y, pair.to.Y));
            }
        }

        private static List<(Coordinate from, Coordinate to)> getCoorinatePair(List<string> lines)
        {
            List<(Coordinate from, Coordinate to)> coordinates = new List<(Coordinate from, Coordinate to)>();

            foreach(string line in lines)
            {
                string[] temp = line.Split("->");
                Coordinate from = new Coordinate(temp[0]);
                Coordinate to = new Coordinate(temp[1]);
                coordinates.Add((from, to));
            }

            return coordinates;
        }
    }
}
