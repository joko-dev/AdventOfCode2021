using AdventOfCode2021.SharedKernel;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2021.Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 15: Chiton"));
            Console.WriteLine("Cave risk level: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            int[,] cave = PuzzleConverter.getInputAsMatrixInt(puzzleInput.Lines);

            int lowestRisk = findLowestTotalRisk(cave);
            Console.WriteLine("lowest total risk: {0}", lowestRisk);
        }

        private static int findLowestTotalRisk(int[,] cave)
        {
            //determine starting risk for shortest path
            List<Coordinate> firstPath = getFirstPath(cave);
            int lowestRisk = getRiskForPath(cave, firstPath);

            List<List<Coordinate>> paths = getPathsForCave(cave, null, new Coordinate(0, 0), ref lowestRisk) ;

            paths = removeUnfinishedPaths(cave, paths);
            lowestRisk = getLowestRisk(cave, paths);

            return lowestRisk;
        }

        private static List<List<Coordinate>> removeUnfinishedPaths(int[,] cave, List<List<Coordinate>> paths)
        {
            List<List<Coordinate>> fixedPaths = new List<List<Coordinate>>();

            foreach(List<Coordinate> path in paths)
            {
                if(isEndingPoint(cave, path.Last()))
                {
                    fixedPaths.Add(path);
                }
            }

            return fixedPaths;
        }

        private static List<List<Coordinate>> getPathsForCave(int[,] cave, List<Coordinate> currentPath, Coordinate point, ref int lowestRisk)
        {
            List<List<Coordinate>> newPaths = new List<List<Coordinate>>();
            bool firstPath = true;

            if (currentPath == null)
            {
                currentPath = new List<Coordinate>();
                newPaths.Add(currentPath);
            }
            currentPath.Add(point);

            int currentPathRisk = getRiskForPath(cave, currentPath);
            List<Coordinate> currentPathOriginal = new List<Coordinate>(currentPath);
            
            if (isEndingPoint(cave, point))
            {
                if(currentPathRisk < lowestRisk)
                {
                    lowestRisk = currentPathRisk;
                }
            }
            else if (currentPathRisk < lowestRisk)
            {
                List<Coordinate> adjacent = PuzzleConverter.getAdjacentPoints(cave, ((int x, int y)) point, true, true, false).Select(x => new Coordinate(x)).ToList();

                foreach(Coordinate adj in adjacent)
                {
                    if (!currentPath.Exists(x => x.X == adj.X && x.Y == adj.Y))
                    {
                        List<Coordinate> path = null;
                        if (firstPath)
                        {
                            path = currentPath;
                        }
                        else
                        {
                            path = new List<Coordinate>(currentPathOriginal);
                            newPaths.Add(path);
                        }

                        newPaths.AddRange(getPathsForCave(cave, path, adj, ref lowestRisk));

                        firstPath = false;
                    }
                }
            }

            return newPaths;
        }

        private static bool isEndingPoint(int[,] cave, Coordinate point)
        {
            return (point.X == cave.GetLength(0) - 1) && (point.Y == cave.GetLength(1) - 1);
        }

        private static int getLowestRisk(int[,] cave, List<List<Coordinate>> paths)
        {
            int lowestRisk = int.MaxValue;

            foreach(List<Coordinate> path in paths)
            {
                int risk = getRiskForPath(cave, path);
                if(risk < lowestRisk)
                {
                    lowestRisk = risk;
                }
            }

            return lowestRisk;
        }

        private static int getRiskForPath(int[,] cave, List<Coordinate> path)
        {
            int risk = 0;
            
            for(int i = 1; i < path.Count(); i++) 
            {
                risk += cave[path[i].X, path[i].Y];
            }

            return risk;
        }

        private static List<Coordinate> getFirstPath(int[,] cave)
        {
            List<Coordinate> path = new List<Coordinate>();

            for (int h = 0; h < cave.GetLength(1); h++)
            {
                path.Add(new Coordinate(0, h));
            }

            for (int w = 1; w < cave.GetLength(0); w++)
            {
                path.Add(new Coordinate(w, cave.GetLength(1) - 1));
            }

            return path;
        }
    }
}
