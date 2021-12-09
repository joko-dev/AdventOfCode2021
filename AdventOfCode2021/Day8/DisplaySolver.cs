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
        internal static bool isUniqueSignal(DisplaySignal signal)
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

        private string[] segmentsDefined = new string[7] { "", "", "", "", "", "", "" };

        private List<DisplaySignal> inputSignals;
        internal List<DisplaySignal> outputDigits { get; }

        internal DisplaySolver(string line)
        {
            string[] temp = line.Split('|');
            string input = temp[0].Trim();
            string output = temp[1].Trim();

            inputSignals = createSignalList(input);
            outputDigits = createSignalList(output);
        }

        internal void Solve()
        {
            List<DisplaySignal> signals = inputSignals;
            signals.AddRange(outputDigits);
            signals = signals.OrderBy(item => item.Segments.Count()).ToList();

            // Approach: mark 1 -> mark 7 (segment 0 defined) -> mark 4 -> mark 3 (segments 1, 3, 6 defined) --> mark 5 (segment 5 and 2 defined) --> define last segment 4;
            // Maybe there is a common solution, but i'm to dumb to figure it out.
            setOne(signals.Where(item => item.Segments.Count == 2).First());
            setSeven(signals.Where(item => item.Segments.Count == 3).First());
            setFour(signals.Where(item => item.Segments.Count == 4).First());
            setThree(signals.Where(item => item.Segments.Count == 5).ToList());
            setFive(signals.Where(item => item.Segments.Count == 5).ToList());
            setEight(signals.Where(item => item.Segments.Count == 7).First());
        }

        private void setOne(DisplaySignal displaySignal)
        {
            if(displaySignal.Segments.Count == 2)
            {
                segmentsDefined[2] = new string(displaySignal.Segments.ToArray());
                segmentsDefined[5] = new string(displaySignal.Segments.ToArray());
            }
            else
            {
                throw new ArgumentException();
            }            
        }

        private void setSeven(DisplaySignal displaySignal)
        {
            if (displaySignal.Segments.Count == 3)
            {
                foreach(char segment in displaySignal.Segments)
                {
                    if(segmentsDefined[2].IndexOf(segment) == -1)
                    {
                        segmentsDefined[0] = segment.ToString();
                    }
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }
        private void setFour(DisplaySignal displaySignal)
        {
            if (displaySignal.Segments.Count == 4)
            {
                foreach (char segment in displaySignal.Segments)
                {
                    if (segmentsDefined[2].IndexOf(segment) == -1)
                    {
                        segmentsDefined[1] += segment;
                        segmentsDefined[3] += segment;
                    }
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }
        private void setThree(List<DisplaySignal> displaySignals)
        {
            foreach(DisplaySignal displaySignal in displaySignals)
            {
                if (displaySignal.Segments.Count == 5)
                {
                    string coded = getCodedDigitString( displaySignal);
                    //check both elements in segment 2 are in the signal. If yes it's the digit 3 
                    var tempList = segmentsDefined[2].Where(c => displaySignal.Segments.Any(c2 => c2 == c));

                    if (coded == "0123 5 " && tempList.Count() == 2)
                    {
                        // determine segments
                        segmentsDefined[6] = displaySignal.Segments.Where(s => !segmentsDefined.Any(s2 => s2.Contains(s))).First().ToString();
                        segmentsDefined[3] = segmentsDefined[3].Where(c => displaySignal.Segments.Any(c2 => c2 == c)).First().ToString();
                        segmentsDefined[1] = segmentsDefined[1].Replace(segmentsDefined[3], "");
                        break;
                    }
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }
        private void setFive(List<DisplaySignal> displaySignals)
        {
            foreach (DisplaySignal displaySignal in displaySignals)
            {
                if (displaySignal.Segments.Count == 5)
                {
                    string coded = getCodedDigitString(displaySignal);
                    //check both elements in segment 2 are in the signal. If yes it's the digit 5
                    var tempList = segmentsDefined[2].Where(c => displaySignal.Segments.Any(c2 => c2 == c));

                    if (coded == "0123 56" && tempList.Count() == 1)
                    {
                        // determine segments 
                        segmentsDefined[5] = segmentsDefined[5].Where(c => displaySignal.Segments.Any(c2 => c2 == c)).First().ToString();
                        segmentsDefined[2] = segmentsDefined[2].Replace(segmentsDefined[5], "");
                        break;
                    }
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }
        private void setEight(DisplaySignal displaySignal)
        {
            if (displaySignal.Segments.Count == 7)
            {
                segmentsDefined[4] = displaySignal.Segments.Where(s => !segmentsDefined.Any(s2 => s2.Contains(s))).First().ToString();
            }
            else
            {
                throw new ArgumentException();
            }
        }

        internal int? getDigit(DisplaySignal signal)
        {
            int? digit = null;
            string coded = getCodedDigitString(signal);

            if (coded == "012 456") { digit = 0; }
            else if (coded == "  2  5 ") { digit = 1; }
            else if (coded == "0 234 6") { digit = 2; }
            else if (coded == "0 23 56") { digit = 3; }
            else if (coded == " 123 5 ") { digit = 4; }
            else if (coded == "01 3 56") { digit = 5; }
            else if (coded == "01 3456") { digit = 6; }
            else if (coded == "0 2  5 ") { digit = 7; }
            else if (coded == "0123456") { digit = 8; }
            else if (coded == "0123 56") { digit = 9; }

            return digit;
        }

        private string getCodedDigitString(DisplaySignal signal)
        {
            string coded = "       ";
            foreach (char segment in signal.Segments)
            {
                for(int i = 0; i < segmentsDefined.Count(); i++)
                {
                    if (segmentsDefined[i].Contains(segment))
                    {
                        StringBuilder sb = new StringBuilder(coded);
                        sb[i] = Convert.ToChar(Convert.ToString(i));
                        coded = sb.ToString();
                    }
                }
            }

            return coded;
        }

        internal int getDisplayNumber()
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

