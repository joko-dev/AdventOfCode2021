using AdventOfCode2021.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Signal Pattern Notes: ");
            PuzzleInput puzzleInput = new PuzzleInput(Console.ReadLine(), true);

            List<DisplaySignal> outputDigits = getOutputSignals(puzzleInput.Lines);
            int countUniqueNumberSequences = countNumbersWithUniqueSequenceCount(outputDigits);
            Console.WriteLine("Appeareance 1,4,7,8 in output: {0}", countUniqueNumberSequences);

            int outputSum = getOutputSum(puzzleInput.Lines);
            Console.WriteLine("Output sum: {0}", outputSum);
        }

        private static int getOutputSum(List<string> lines)
        {
            int sum = 0;
            foreach(string line in lines)
            {
                DisplaySolver solver = new DisplaySolver(line);
                solver.Solve();
                sum += solver.getDisplayNumber();
            }

            return sum;
        }

        private static List<DisplaySignal> getOutputSignals(List<string> lines)
        {
            List<DisplaySignal> outputSignals = new List<DisplaySignal>();
            foreach (string line in lines)
            {
                DisplaySolver solver = new DisplaySolver(line);
                outputSignals.AddRange(solver.outputDigits);
            }

            return outputSignals;
        }

        private static int countNumbersWithUniqueSequenceCount(List<DisplaySignal> outputDigits)
        {
            int count = 0;

            foreach (DisplaySignal digit in outputDigits)
            {
                if(DisplaySolver.isUniqueSignal(digit))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
