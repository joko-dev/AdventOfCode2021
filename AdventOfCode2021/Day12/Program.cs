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
            //Console.WriteLine(getVisual(paths));
            Console.WriteLine("Paths through cave system (one small cave two times): {0}", paths.Count);
        }

        private static string getVisual(List<List<string>> paths)
        {
            string vis = "";
            foreach(List<string> path in paths)
            {
                vis += String.Join(",", path);
                vis += "\n";
            }
            return vis;
        }

        private static List<List<string>> getAllConnectionsSmallCavesOnce(List<CaveConnection> caveMap)
        {
            List<List<string>> paths = new List<List<string>>();

            paths.AddRange(getCavePaths(caveMap, "start", null, ("",2)));

            paths = getCompletePaths(paths);

            return paths;

        }

        private static List<List<string>> getAllConnectionsOneSmallCaveTwo(List<CaveConnection> caveMap)
        {
            List<List<string>> paths = new List<List<string>>();
            
            List<string> smallCaves = getSmallCaves(caveMap);

            foreach(string cave in smallCaves)
            {
                if(!isStartingCave(cave) && !isEndingCave(cave))
                {
                    List<List<string>> newPaths = getCavePaths(caveMap, "start", null, (cave, 2));

                    for (int i = 0; i < newPaths.Count; i++)
                    {
                        bool exists = false;
                        for (int j = 0; j < paths.Count; j++)
                        {
                            if (newPaths[i].SequenceEqual(paths[j]))
                            {
                                exists = true;
                                break;
                            }
                        }

                        if (!exists)
                        {
                            paths.Add(newPaths[i]);
                        }
                    }
                }
            }
            
            paths = getCompletePaths(paths);

            return paths;

        }

        private static List<string> getSmallCaves(List<CaveConnection> caveMap)
        {
            List<string> smallCaves = new List<string>();

            foreach (CaveConnection connection in caveMap)
            {
                if (isSmallCave(connection.From) && !smallCaves.Exists(c => c == connection.From))
                { 
                    smallCaves.Add(connection.From);
                }
                if (isSmallCave(connection.To) && !smallCaves.Exists(c => c == connection.To))
                {
                    smallCaves.Add(connection.To);
                }
            }
            return smallCaves;
        }

        private static List<List<string>> getCavePaths(List<CaveConnection> caveMap, string cave, List<string> currentPath, (string cave, int visitTime) smallCaveSpecial )
        {
            List<List<string>> newPaths = new List<List<string>>();
            bool firstPath = true;

            if (currentPath == null)
            {
                currentPath = new List<string>();
                newPaths.Add(currentPath);
            }
            currentPath.Add(cave);


            List<string> currentPathOriginal = new List<string>(currentPath);

            // new connection is endpoint --> no new paths 
            if (!isEndingCave(cave))
            {
                foreach (CaveConnection connection in caveMap)
                {
                    string nextCave = "";
                    if(connection.From == cave && !existsSmallCaveInPath(connection.To, currentPathOriginal, smallCaveSpecial))
                    {
                        nextCave = connection.To;
                    }
                    else if (connection.To == cave && !existsSmallCaveInPath(connection.From, currentPathOriginal, smallCaveSpecial))
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
                            path = new List<string>(currentPathOriginal);
                            newPaths.Add(path);
                        }

                        newPaths.AddRange(getCavePaths(caveMap, nextCave, path, smallCaveSpecial));

                        firstPath = false;
                    }
                }
            }

            return newPaths;
        }

        private static bool existsSmallCaveInPath(string cave, List<string> path, (string cave, int visitTime) smallCaveSpecial)
        {
            bool exists = false;
            if (isSmallCave(cave))
            {
                if(cave == smallCaveSpecial.cave)
                {
                    exists = path.Where(c => c == cave).Count() == smallCaveSpecial.visitTime;
                }
                else
                {
                    exists = path.Where(c => c == cave).Any();
                }
            }
            
            return exists;
            
        }

        private static List<List<string>> getCompletePaths(List<List<string>> paths)
        {
            List<List<string>> fixedPaths = new List<List<string>>(paths);

            for (int i = fixedPaths.Count - 1; i>= 0;  i--)
            {
                if(!isEndingCave(fixedPaths[i].Last()))
                {
                    fixedPaths.RemoveAt(i);
                }
            }

            return fixedPaths;
        }

        private static bool isStartingCave(string cave)
        {
            return (cave == "start");
        }

        private static bool isEndingCave(string cave)
        {
            return (cave == "end");
        }

        private static bool isSmallCave(string cave)
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
