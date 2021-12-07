using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;

namespace AdventOfCode2021.Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Course file:");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine());

            List<(string direction, int distance)> course = getCourse(puzzleInput.Lines);

            Console.WriteLine("Larger measurements: {0}");
        }

        static List<(string direction, int distance)> getCourse (List<string> input)
        {
            List<(string direction, int distance)> course = new List<(string, int)>();

            foreach(string move in input)
            {
                string[] temp = move.Split(' ');
                course.Add((temp[0], int.Parse(temp[1])));
            }

            return course;
        }
    }
}
