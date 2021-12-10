using AdventOfCode2021.SharedKernel;
using System;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 9: Smoke Basin"));
            Console.WriteLine("Signal Pattern Notes: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            int[,] matrix = puzzleInput.getInputAsMatrixInt();
            int sumRiskLevels = getSumRiskLevels(matrix);
            Console.WriteLine("Sum of risk levels: {0}", sumRiskLevels);
        }

        private static int getSumRiskLevels(int[,] matrix)
        {
            int sum = 0;
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    if(isLowPoint(matrix, x, y))
                    {
                        sum += matrix[x,y] + 1;
                    }
                }
            }
            return sum;
        }

        private static bool isLowPoint(int[,] matrix, int x, int y)
        {
            bool lowpoint = true;
            int point = matrix[x, y];

            if( x > 0) { lowpoint &= point < matrix[x-1,y]; }
            if( x < matrix.GetLength(0) - 1) { lowpoint &= point < matrix[x+1, y]; }
            if (y > 0) { lowpoint &= point < matrix[x, y-1]; }
            if (y < matrix.GetLength(1) - 1) { lowpoint &= point < matrix[x, y+1]; }

            return lowpoint;
        }
    }
}
