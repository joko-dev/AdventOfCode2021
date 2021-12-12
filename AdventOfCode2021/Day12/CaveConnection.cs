using System;

namespace AdventOfCode2021.Day12
{
    internal class CaveConnection : ICloneable
    {
        public string From { get;  }
        public string To { get; }

        public CaveConnection(string connection)
        {
            string[] temp = connection.Split('-');
            From = temp[0];
            To = temp[1];
        }

        public CaveConnection(CaveConnection caveConnection)
        {
            From = caveConnection.From;
            To = caveConnection.To;
        }

        public object Clone()
        {
            return new CaveConnection(this);
        }
    }
}
