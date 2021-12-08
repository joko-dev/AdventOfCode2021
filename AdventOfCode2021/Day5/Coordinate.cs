using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day5
{
    public class Coordinate
    {
        public Coordinate(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Coordinate(string coordinate)
        {
            string[] temp = coordinate.Split(',');
            X = int.Parse(temp[0]);
            Y = int.Parse(temp[1]);
        }

        public int X { get;  }
        public int Y { get; }

    }
}
