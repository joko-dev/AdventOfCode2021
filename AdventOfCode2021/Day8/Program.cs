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

            Console.WriteLine("Appeareance 1,4,7,8 in output: {0}", countUniqueNumberSequences);
        }

        private static List<DisplaySignal> getOutputSignals(List<string> lines)
        {
            List<DisplaySignal> outputSignals = new List<DisplaySignal>();
            foreach (string line in lines)
            {
                string output = line.Split('|')[1];
                string[] digits = output.Split(' ');
                foreach(string digit in digits)
                {
                    outputSignals.Add(new DisplaySignal(digit));
                }
            }

            return outputSignals;
        }

        private static int countNumbersWithUniqueSequenceCount(List<DisplaySignal> outputDigits)
        {
            int count = 0;
            DisplaySolver solver = new DisplaySolver();

            foreach (DisplaySignal digit in outputDigits)
            {
                if(solver.isUniqueSignal(digit))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
