using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021.Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 13: Transparent Origami"));
            Console.WriteLine("Transparent paper: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            int[,] pointsOriginal = null;
            int[,] points = null;
            List<(string axis, int value)> instructions = null;
            extractPointsAndInstruction(puzzleInput.Lines, ref pointsOriginal, ref instructions);
            points = foldPaper(pointsOriginal, instructions[0]);
            Console.WriteLine("Visible dots: {0}", countDots(points));

            points = foldPaperComplete(pointsOriginal, instructions);
            Console.WriteLine(getOutput(points));
        }

        private static string getOutput(int[,] points)
        {
            StringBuilder sb = new StringBuilder();

            for(int y = 0; y < points.GetLength(1); y++)
            {
                for (int x = 0; x < points.GetLength(0); x++)
                {
                    if(points[x, y] > 0)
                    {
                        sb.Append('#');
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static int[,] foldPaperComplete(int[,] points, List<(string axis, int value)> instructions)
        {
            int[,] paper = points;

            foreach((string axis, int value) instruction in instructions)
            {
                paper = foldPaper(paper, instruction);
            }

            return paper;
        }

        private static int countDots(int[,] points)
        {
            int count = 0;
            for(int x = 0; x < points.GetLength(0); x++)
            {
                for (int y = 0; y < points.GetLength(1); y++)
                {
                    if(points[x,y] > 0)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private static int[,] foldPaper(int[,] points, (string axis, int value) instruction)
        {
            int[,] paper;
            int width = points.GetLength(0);
            int height = points.GetLength(1);
            if(instruction.axis == "x")
            {
                width = instruction.value;
                paper = new int[width, height];
                for (int x = 0; x < points.GetLength(0); x++)
                {
                    for(int y = 0; y < points.GetLength(1); y++)
                    {
                        if(x < width)
                        {
                            paper[x, y] = points[x, y];
                        }
                        else if (x > width)
                        {
                            paper[instruction.value - Math.Abs(x - width), y] += points[x, y];
                        }
                    }
                }
            }
            else
            {
                height = instruction.value;
                paper = new int[width, height];
                for (int x = 0; x < points.GetLength(0); x++)
                {
                    for (int y = 0; y < points.GetLength(1); y++)
                    {
                        if (y < height)
                        {
                            paper[x, y] = points[x, y];
                        }
                        else if(y > height)
                        {
                            paper[x, instruction.value - Math.Abs(y - height)] += points[x, y];
                        }
                    }
                }
            }
            
            return paper;
        }

        private static void extractPointsAndInstruction(List<string> lines, ref int[,] points, ref List<(string axis, int value)> instructions)
        {
            List<string> pointstrings = new List<string>();
            instructions = new List<(string axis, int value)>();

            foreach(string line in lines)
            {
                if(line.StartsWith("fold along"))
                {
                    string temp = line.Replace("fold along", "").Trim();
                    string[] instruction = temp.Split("=");

                    instructions.Add(( instruction[0], int.Parse(instruction[1])));
                }
                else
                {
                    pointstrings.Add(line);
                }
            }

            points = PuzzleConverter.getInputCoordinateAsMatrix(pointstrings, 1, ",");
        }
    }
}
