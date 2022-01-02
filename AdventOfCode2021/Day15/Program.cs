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
            Console.WriteLine("Full map (y)?");
            if (Console.ReadLine() == "y")
            {
                cave = buildFullMap(cave);
            }


            //Pathfinding algorithm - Dijkstra - https://de.wikipedia.org/wiki/Dijkstra-Algorithmus
            List<Coordinate> riskList = buildRiskList(cave);
            Dictionary<Coordinate,Coordinate> predecessors = Dijkstra(cave,riskList, riskList.Find(r => r.X == 0 && r.Y == 0), riskList.Find(r => r.X == cave.GetLength(0) - 1 && r.Y == cave.GetLength(1) - 1));
            List<Coordinate> path = getShortestPath(riskList.Find(r => r.X == cave.GetLength(0) - 1 && r.Y == cave.GetLength(1) - 1), predecessors);
            Console.WriteLine("Lowest risk top left to bottom right: {0}", getRiskForPath(cave, path));
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

        private static List<Coordinate> getShortestPath(Coordinate endPoint, Dictionary<Coordinate, Coordinate> predecessors)
        {
            List<Coordinate> path = new List<Coordinate>();
            path.Add(endPoint);
            Coordinate u = endPoint;
            while (predecessors[u] is not null)
            {
                u = predecessors[u];
                path.Insert(0, u);
            }
            return path;
        }

        static Dictionary<Coordinate, Coordinate> Dijkstra(int[,] cave, List<Coordinate> riskList, Coordinate start, Coordinate end)
        {
            Dictionary<Coordinate, int> distance = new Dictionary<Coordinate, int>();
            Dictionary<Coordinate, Coordinate> predecessor = new Dictionary<Coordinate, Coordinate>(); 
            List<Coordinate> coordinatesWithoutPath = new List<Coordinate>();

            initialize(riskList, start, distance, predecessor, coordinatesWithoutPath);
            while (coordinatesWithoutPath.Count() > 0)
            {
                Coordinate lowestDistance = findCoordinateWithLowestDistance(coordinatesWithoutPath, ref distance);
                coordinatesWithoutPath.Remove(lowestDistance);
                if(end is not null && lowestDistance == end)
                {
                    break;
                }
                List<Coordinate> adjacent = getAdjacentCoordinates(cave, lowestDistance, coordinatesWithoutPath);
                foreach(Coordinate adj in adjacent)
                {
                    if (coordinatesWithoutPath.Exists(c => c == adj))
                    {
                        distanceUpdate(cave, lowestDistance, adj, distance, predecessor);
                    }
                }
            }
            return predecessor;
        }

        private static void distanceUpdate(int[,] cave, Coordinate from, Coordinate to, Dictionary<Coordinate, int> distance, Dictionary<Coordinate, Coordinate> predecessor)
        {
            int alternative = distance[from] + cave[to.X, to.Y];

            if (alternative < distance[to])
            {
                distance[to] = alternative;
                predecessor[to] = from;
            }
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

        private static Coordinate findCoordinateWithLowestDistance(List<Coordinate> coordinatesWithoutPath, ref Dictionary<Coordinate, int> distance)
        {
            Coordinate coordinateLowestDistance = null;
            distance = distance.OrderBy(d => d.Value).ToDictionary(d => d.Key, d => d.Value);
            foreach(KeyValuePair<Coordinate, int> coordinate in distance)
            {
                if(coordinatesWithoutPath.Exists(c => c == coordinate.Key))
                {
                    coordinateLowestDistance = coordinate.Key;
                    break;
                }
                                    
            }

            return coordinateLowestDistance;
        }

        static void initialize(List<Coordinate> riskList, Coordinate start, Dictionary<Coordinate, int> distance, Dictionary<Coordinate,Coordinate> predecessor, List<Coordinate> coordinatesWithoutPath)
        {
            distance.Clear();
            predecessor.Clear();
            coordinatesWithoutPath.Clear();

            foreach (Coordinate coordinate in riskList)
            {
                distance.Add(coordinate, int.MaxValue);
                predecessor.Add(coordinate, null);
                coordinatesWithoutPath.Add(coordinate);
            }
            
            distance[start] = 0;
        }

    }
}
