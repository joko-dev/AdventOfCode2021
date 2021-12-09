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

        private string[] segmentsDefined = new string[7] { "d", "e", "a", "f", "g", "b", "c" };

        private List<DisplaySignal> inputSignals;
        public List<DisplaySignal> outputDigits { get; }

        public DisplaySolver(string line)
        {
            string[] temp = line.Split('|');
            string input = temp[0].Trim();
            string output = temp[1].Trim();

            inputSignals = createSignalList(input);
            outputDigits = createSignalList(output);
        }

        public void Solve()
        {
            while(!segmentsComplete())
            {
                List<DisplaySignal> signals = inputSignals;
                signals.AddRange(outputDigits);

                foreach (DisplaySignal signal in signals)
                {
                    if (!getDigit(signal).HasValue)
                    {
                        //checkSegments(signal);
                    }
                }
            }
        }

        public int? getDigit(DisplaySignal signal)
        {
            int? digit = null;
            string temp = "       ";

            foreach (char segment in signal.Segments)
            {
                int index = Array.IndexOf(segmentsDefined, Convert.ToString(segment));
                if(index >= 0)
                {
                    StringBuilder sb = new StringBuilder(temp);
                    sb[index] = Convert.ToChar(Convert.ToString(index));
                    temp = sb.ToString();
                }
            }

            if (temp == "012 456") { digit = 0; }
            else if (temp == "  2  5 ") { digit = 1; }
            else if (temp == "0 234 6") { digit = 2; }
            else if (temp == "0 23 56") { digit = 3; }
            else if (temp == " 123 5 ") { digit = 4; }
            else if (temp == "01 3 56") { digit = 5; }
            else if (temp == "01 3456") { digit = 6; }
            else if (temp == "0 2  5 ") { digit = 7; }
            else if (temp == "0123456") { digit = 8; }
            else if (temp == "0123 56") { digit = 8; }

            return digit;
        }

        private bool segmentsComplete()
        {
            bool complete = true;

            foreach (string segment in segmentsDefined)
            {
                complete = complete && (segment.Length == 1);
            }

            return complete;
        }

        public int getDisplayNumber()
        {
            string output = "";
            foreach (DisplaySignal digit in outputDigits)
            {
                output += Convert.ToString(getDigit(digit));
            }

            return int.Parse(output);
        }

        private List<DisplaySignal> createSignalList(string signalList)
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

