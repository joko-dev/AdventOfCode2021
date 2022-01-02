using AdventOfCode2021.SharedKernel;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2021.Day15
{
    class ProgramAStar
    {
        static void Main(string[] args) 
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 15: Chiton - AStar"));
            Console.WriteLine("Cave risk level: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            int[,] cave = PuzzleConverter.getInputAsMatrixInt(puzzleInput.Lines);
            Console.WriteLine("Full map (y)?");
            if (Console.ReadLine() == "y")
            {
                cave = buildFullMap(cave);
            }


            //Pathfinding algorithm - A* - https://de.wikipedia.org/wiki/A*-Algorithmus
            List<Coordinate> riskList = buildRiskList(cave);
            List<Coordinate> path = AStar(cave, riskList, riskList.Find(r => r.X == 0 && r.Y == 0), riskList.Find(r => r.X == cave.GetLength(0) - 1 && r.Y == cave.GetLength(1) - 1));
            Console.WriteLine("Lowest risk top left to bottom right: {0}", getRiskForPath(cave, path));
        }

        private static List<Coordinate> AStar(int[,] cave, List<Coordinate> riskList, Coordinate start, Coordinate end)
        {
            List<Coordinate> path = new List<Coordinate>();
            PriorityQueue<Coordinate, int> openList = new PriorityQueue<Coordinate, int>();
            Dictionary<Coordinate, Coordinate> predecessors = new Dictionary<Coordinate, Coordinate>();
            Dictionary<Coordinate, int> gValue = new Dictionary<Coordinate, int>();
            HashSet<Coordinate> closedList = new HashSet<Coordinate>();

            openList.Enqueue(start, 0);
            do
            {
                Coordinate current = openList.Dequeue();
                if(current == end)
                {
                    break;
                }

                closedList.Add(current);
                expandNode(cave, current, riskList, openList, closedList, predecessors, gValue);

            }
            while (openList.Count > 0);

            return path;
        }

        private static void expandNode(int[,] cave, Coordinate current, List<Coordinate> riskList, PriorityQueue<Coordinate, int> openList, HashSet<Coordinate> closedList,
                                            Dictionary<Coordinate, Coordinate> predecessor, Dictionary<Coordinate, int> gValue)
        {
            List<Coordinate> adjacent = getAdjacentCoordinates(cave, current, riskList);
            foreach(Coordinate adj in adjacent)
            {
                if (!closedList.Contains(current))
                {
                    int tentative_g = gValue[current] + cave[adj.X, adj.Y];

                    if (!(openList.UnorderedItems.Any(i => i.Element == adj) && (tentative_g >= gValue[adj])))
                    {
                        predecessor[adj] = current;
                        gValue[adj] = tentative_g;
                        int f = tentative_g + getH();
                        if(openList.UnorderedItems.Any(i => i.Element == adj))
                        {
                            openList..Priority = f;
                        }
                        else
                        {
                            openList.Enqueue(adj, f);
                        }
                    }
                }
            }
        }

        private static int[,] buildFullMap(int[,] cave)
        {
            int originalWidth = cave.GetLength(0);
            int originalHeigth = cave.GetLength(1);
            int[,] map = new int[originalWidth * 5, originalHeigth * 5];

            for (int x = 0; x < cave.GetLength(0); x++)
            {
                for (int y = 0; y < cave.GetLength(1); y++)
                {
                    map[x, y] = cave[x, y];
                }
            }

            for (int y = 0; y < cave.GetLength(1); y++)
            {
                for (int x = 0; x < cave.GetLength(0); x++)
                {
                    for (int stepY = 0; stepY < 5; stepY++)
                    {
                        for (int stepX = 0; stepX < 5; stepX++)
                        {
                            int SourceX = -1;
                            int SourceY = -1;
                            int TargetX = -1;
                            int TargetY = -1;
                            int toAdd = 1;
                            if (stepX == 0 && stepY == 0) 
                            {
                                continue;
                            }
                            else if (stepY == 0)
                            {
                                SourceX = x + ((stepX - 1) * originalWidth);
                                SourceY = y;
                                TargetX = x + (stepX * originalWidth);
                                TargetY = y;
                            }
                            else if (stepX == 0)
                            {
                                SourceX = x;
                                SourceY = y + ((stepY - 1) * originalHeigth);
                                TargetX = x;
                                TargetY = y + (stepY * originalHeigth);
                            }
                            else
                            {
                                SourceX = x + (stepX * originalWidth);
                                SourceY = y + ((stepY - 1) * originalHeigth);
                                TargetX = x + (stepX * originalWidth);
                                TargetY = y + (stepY * originalHeigth);
                            }

                            int newValue = map[SourceX, SourceY] + toAdd;
                            if (newValue > 9)
                            {
                                newValue = 1;
                            }

                            map[TargetX,TargetY] = newValue;
                        }
                    } 
                }
            }

            return map;
        }

        private static List<Coordinate> buildRiskList(int[,] cave)
        {
            List<Coordinate> riskList = new List<Coordinate>();

            for (int x = 0; x < cave.GetLength(0); x++)
            {
                for (int y = 0; y < cave.GetLength(1); y++)
                {
                    Coordinate coordinate = new Coordinate(x, y);
                    riskList.Add(coordinate);
                }
            }

            return riskList;
        }

        private static int getRiskForPath(int[,] cave, List<Coordinate> path)
        {
            int risk = 0;

            for(int i = 1; i < path.Count; i++)
            {
                risk += cave[path[i].X, path[i].Y];
            }

            return risk;
        }

        private static List<Coordinate> getAdjacentCoordinates(int[,] cave, Coordinate coordinate, List<Coordinate> coordinates)
        {
            List<Coordinate> adjacent = new List<Coordinate>();
            List<(int,int)> points = PuzzleConverter.getAdjacentPoints(cave, (coordinate.X, coordinate.Y), true, true, false);

            foreach((int x, int y) point in points)
            {
                Coordinate adj = coordinates.Find(c => c.X == point.x && c.Y == point.y);
                if(adj is not null)
                {
                    adjacent.Add(adj);
                }
            }

            return adjacent;
        }

    }
}
