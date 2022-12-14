using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022.Structs
{
    internal struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point(string pair)
        {
            var tuple = pair.Split(',').Select(Int32.Parse).ToArray();
            if (tuple.Length != 2)
            {
                throw new ArgumentException($"Invalid pair: {pair}", nameof(pair));
            }
            X = tuple[0];
            Y = tuple[1];
        }
    };
}
