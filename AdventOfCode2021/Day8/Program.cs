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

            List<DisplayDigit> outputDigits = getOutputDigits(puzzleInput.Lines);
            int countUniqueNumberSequences = countNumbersWithUniqueSequenceCount(outputDigits);

            Console.WriteLine("Appeareance 1,4,7,8 in output: {0}", countUniqueNumberSequences);
        }

        private static List<DisplayDigit> getOutputDigits(List<string> lines)
        {
            List<DisplayDigit> outputDigits = new List<DisplayDigit>();
            foreach (string line in lines)
            {
                string output = line.Split('|')[1];
                string[] digits = output.Split(' ');
                foreach(string digit in digits)
                {
                    outputDigits.Add(new DisplayDigit(digit));
                }
            }

            return outputDigits;
        }

        private static int countNumbersWithUniqueSequenceCount(List<DisplayDigit> outputDigits)
        {
            int count = 0;

            foreach (DisplayDigit digit in outputDigits)
            {
                if(digit.getDigit().HasValue && DisplayDigit.uniqueDigits.Contains((int)digit.getDigit()))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
