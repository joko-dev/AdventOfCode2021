using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day8
{
    class DisplayDigit
    {
        public static readonly int[] uniqueDigits = {1,4,7,8};

        private List<char> segments;

        public DisplayDigit(string segments)
        {
            this.segments = new List<char>();
            foreach(char segment in segments)
            {
                this.segments.Add(segment);
            }
        }

        public int? getDigit()
        {
            int? digit = null;

            switch (segments.Count())
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

            return digit;
        }
    }
}
