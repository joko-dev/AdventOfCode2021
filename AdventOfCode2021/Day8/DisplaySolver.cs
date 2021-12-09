using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day8
{
    internal class DisplaySolver
    {
        private static readonly int[] uniqueDigits = { 1, 4, 7, 8 };
        public static bool isUniqueSignal(DisplaySignal signal)
        {
            int digit = -1;

            // digits with unique segment count can easily be retrieved
            switch (signal.Segments.Count())
            {
                case 2:
                    digit = 1;
                    break;
                case 4:
                    digit = 4;
                    break;
                case 3:
                    digit = 7;
                    break;
                case 7:
                    digit = 8;
                    break;
            }

            return uniqueDigits.Contains(digit);
        }

        private List<DisplaySignal> inputSignals;
        public List<DisplaySignal> outputDigits { get; }

        public DisplaySolver(string line)
        {
            string[] temp = line.Split('|');
            string input = temp[0];
            string output = temp[1];

            inputSignals = getSignalList(input);
            outputDigits = getSignalList(output);
        }

        private List<DisplaySignal> getSignalList(string signalList)
        {
            List<DisplaySignal> list = new List<DisplaySignal>();

            string[] temp = signalList.Split(' ');
            foreach(string signal in temp)
            {
                list.Add(new DisplaySignal(signal));
            }

            return list;
        }
    }
}

