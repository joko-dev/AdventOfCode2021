using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day8
{
    class DisplaySignal
    {
        public List<char> Segments { get; }

        public DisplaySignal(string segments)
        {
            this.Segments = new List<char>();
            foreach(char segment in segments)
            {
                this.Segments.Add(segment);
            }
        }
    }
}
