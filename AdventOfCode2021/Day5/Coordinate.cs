using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day5
{
    internal class Coordinate
    {
        internal Coordinate(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        internal Coordinate(string coordinate)
        {
            string[] temp = coordinate.Split(',');
            X = int.Parse(temp[0]);
            Y = int.Parse(temp[1]);
        }

        internal int X { get;  }
        internal int Y { get; }

    }
}
