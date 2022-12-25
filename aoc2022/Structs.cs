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

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public void Offset(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        public int ManhattanDist(Point other)
        {
            return Math.Abs(other.X - X) + Math.Abs(other.Y - Y);
        }
    }

    internal class Segment
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Length => End - Start + 1;

        public bool Intersects(Segment other)
        {
            if (Start >= other.Start && Start <= other.End ||
                End >= other.Start && End <= other.End)
            {
                return true;
            }

            if (other.Start >= Start && other.Start <= End ||
                other.End >= Start && other.End <= End)
            {
                return true;
            }

            return false;
        }
    }
}
