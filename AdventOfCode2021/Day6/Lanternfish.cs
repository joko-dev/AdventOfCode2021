using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day6
{
    internal class Lanternfish : ICloneable
    {
        private const int DAYS_FIRST_CYCLE = 8;
        private const int DAYS_CYCLE = 6;
        private int daysToReproduce;

        internal Lanternfish(int daysToReproduce)
        {
            this.daysToReproduce = daysToReproduce;
        }

        internal Lanternfish() : this (DAYS_FIRST_CYCLE) { }

        internal Lanternfish OneDayOlder()
        {
            Lanternfish child = null;
            if(daysToReproduce == 0)
            {
                daysToReproduce = DAYS_CYCLE;
                child = new Lanternfish();
            }
            else
            {
                daysToReproduce--;
            }

            return child;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
