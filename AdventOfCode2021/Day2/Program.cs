using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;

namespace AdventOfCode2021.Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 1: Sonar Sweep"));
            Console.WriteLine("Course file:");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine());

            List<(string direction, int distance)> course = getCourse(puzzleInput.Lines);
            (int horizontal, int depth) endpoint = driveCourse((0,0), course);
            (int horizontal, int depth, int aim) endpointAim = driveCourseAim((0, 0, 0), course);

            Console.WriteLine("Endpoint: Horizontal {0} * Depth {1}: {2}", endpoint.horizontal, endpoint.depth, endpoint.horizontal * endpoint.depth);
            Console.WriteLine("Endpoint with aim: Horizontal {0} * Depth {1}: {2}", endpointAim.horizontal, endpointAim.depth, endpointAim.horizontal * endpointAim.depth);
        }

        static List<(string direction, int distance)> getCourse(List<string> input)
        {
            List<(string direction, int distance)> course = new List<(string, int)>();

            foreach (string move in input)
            {
                string[] temp = move.Split(' ');
                course.Add((temp[0], int.Parse(temp[1])));
            }

            return course;
        }

        static (int horizontal, int depth) driveCourse((int horizontal, int depth) startPoint, List<(string direction, int distance)> course)
        {
            (int horizontal, int depth) endPoint = (0,0);

            foreach ((string direction, int distance) move in course)
            {
                if(move.direction == "forward")
                {
                    endPoint.horizontal = endPoint.horizontal + move.distance;
                }
                else if (move.direction == "up")
                {
                    endPoint.depth = endPoint.depth - move.distance;
                }
                else if (move.direction == "down")
                {
                    endPoint.depth = endPoint.depth + move.distance;
                }
            }

            return endPoint;
        }

        static (int horizontal, int depth, int aim) driveCourseAim((int horizontal, int depth, int aim) startPoint, List<(string direction, int distance)> course)
        {
            (int horizontal, int depth, int aim) endPoint = (0, 0, 0);

            foreach ((string direction, int distance) move in course)
            {
                if (move.direction == "forward")
                {
                    endPoint.horizontal = endPoint.horizontal + move.distance;
                    endPoint.depth = endPoint.depth + (endPoint.aim * move.distance);
                }
                else if (move.direction == "up")
                {
                    endPoint.aim = endPoint.aim - move.distance;
                }
                else if (move.direction == "down")
                {
                    endPoint.aim = endPoint.aim + move.distance;
                }
            }

            return endPoint;
        }

    }
}
