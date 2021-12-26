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

            //Pathfinding algorithm - Dijkstra - https://de.wikipedia.org/wiki/Dijkstra-Algorithmus
            List<Coordinate> riskList = buildRiskList(cave);
            Dictionary<Coordinate,Coordinate> predecessors = Dijkstra(riskList, riskList.Find(r => r.X == 0 && r.Y == 0));
            List<Coordinate> path = getShortestPath(riskList.Find(r => r.X == cave.GetLength(0) - 1 && r.Y == cave.GetLength(1) - 1), predecessors);
            Console.WriteLine("Lowest risk top left to bottom right: {0}", getRiskForPath(cave, path));
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

        static Dictionary<Coordinate, Coordinate> Dijkstra(int[,] cave, Coordinate start)
        {
            Dictionary<Coordinate, int> distance = new Dictionary<Coordinate, int>();
            Dictionary<Coordinate, Coordinate> predecessor = new Dictionary<Coordinate, Coordinate>(); 
            List<Coordinate> coordinatesWithoutPath = new List<Coordinate>();

            initialize(cave, start, distance, predecessor, coordinatesWithoutPath);
            while (coordinatesWithoutPath.Count() > 0)
            {
                Coordinate lowestDistance = findCoordinateWithLowestDinstance(coordinatesWithoutPath, distance);
                coordinatesWithoutPath.Remove(lowestDistance);
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
            int alternative = distance[from] + cave[from.X, from.Y];

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

        private static Coordinate findCoordinateWithLowestDinstance(List<Coordinate> coordinatesWithoutPath, Dictionary<Coordinate, int> distance)
        {
            Coordinate coordinateLowestDistance = null;
            distance.OrderBy(d => d.Value);
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

        static void initialize(int[,] cave, Coordinate start, Dictionary<Coordinate, int> distance, Dictionary<Coordinate,Coordinate> predecessor, List<Coordinate> coordinatesWithoutPath)
        {
            distance.Clear();
            predecessor.Clear();
            coordinatesWithoutPath.Clear();

            for(int x = 0; x < cave.GetLength(0); x++)
            {
                for (int y = 0; y < cave.GetLength(1); y++)
                {
                    Coordinate coordinate = new Coordinate(x, y);
                    distance.Add(coordinate, -1);
                    predecessor.Add(coordinate,null);
                    coordinatesWithoutPath.Add(coordinate);
                }
            }
            distance[start] = 0;
        }

    }
}
