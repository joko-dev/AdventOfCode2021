using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 12: Passage Pathing"));
            Console.WriteLine("Cave map: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            List<CaveConnection> caveMap = createCaveMape(puzzleInput.Lines);
            List<List<string>> paths = getAllConnectionsSmallCavesOnce(caveMap);
            Console.WriteLine("Paths through cave system (small cave once): {0}", paths.Count);

            paths = getAllConnectionsOneSmallCaveTwo(caveMap);
            Console.WriteLine("Paths through cave system (one small cave two times): {0}", paths.Count);
        }

        private static List<List<string>> getAllConnectionsSmallCavesOnce(List<CaveConnection> caveMap)
        {
            List<List<string>> paths = new List<List<string>>();

            paths.AddRange(getCavePathsSmallCavesOnlyOnce(caveMap, "start", null));

            checkCompletePaths(paths);

            return paths;

        }

        private static List<List<string>> getAllConnectionsOneSmallCaveTwo(List<CaveConnection> caveMap)
        {
            List<List<string>> paths = new List<List<string>>();

            paths.AddRange(getCavePathsSmallCavesOnlyOnce(caveMap, "start", null));

            checkCompletePaths(paths);

            return paths;

        }

        private static List<List<string>> getCavePathsSmallCavesOnlyOnce(List<CaveConnection> caveMap, string cave, List<string> currentPath)
        {
            List<List<string>> newPaths = new List<List<string>>();
            bool firstPath = true;

            if (currentPath == null)
            {
                currentPath = new List<string>();
                newPaths.Add(currentPath);
            }
            currentPath.Add(cave);


            List<string> currentPathOriginaL = new List<string>(currentPath);

            // new connection is endpoint --> no new paths 
            if (!isEndingCave(cave))
            {
                foreach (CaveConnection connection in caveMap)
                {
                    string nextCave = "";
                    if(connection.From == cave && !existsSmallCaveInPath(connection.To, currentPathOriginaL))
                    {
                        nextCave = connection.To;
                    }
                    else if (connection.To == cave && !existsSmallCaveInPath(connection.From, currentPathOriginaL))
                    {
                        nextCave = connection.From;
                    }

                    if(nextCave != "" && !isStartingCave(nextCave))
                    {
                        List<string> path = null;
                        if (firstPath)
                        {
                            path = currentPath;
                        }
                        else
                        {
                            path = new List<string>(currentPathOriginaL);
                            newPaths.Add(path);
                        }

                        newPaths.AddRange(getCavePathsSmallCavesOnlyOnce(caveMap, nextCave, path));

                        firstPath = false;
                    }
                }
            }

            return newPaths;
        }

        private static bool existsSmallCaveInPath(string cave, List<string> path)
        {
            if (isSmallCave(cave))
            {
                return path.Where(c => c == cave).Any();
            }
            else
            {
                return false;
            }
        }

        private static void checkCompletePaths(List<List<string>> paths)
        {
            for(int i = paths.Count - 1; i>= 0;  i--)
            {
                if(paths[i].Last() != "end")
                {
                    paths.RemoveAt(i);
                }
            }
        }

        public static bool isStartingCave(string cave)
        {
            return (cave == "start");
        }

        public static bool isEndingCave(string cave)
        {
            return (cave == "end");
        }

        public static bool isSmallCave(string cave)
        {
            return (cave.ToLower() == cave);
        }

        private static List<CaveConnection> createCaveMape(List<string> lines)
        {
            List<CaveConnection> caveMap = new List<CaveConnection>();
            foreach(string line in lines)
            {
                caveMap.Add(new CaveConnection(line));
            }

            return caveMap;
        }
    }
}
