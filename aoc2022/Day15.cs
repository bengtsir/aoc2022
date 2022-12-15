using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using aoc2022.Structs;

namespace aoc2022
{
    internal class SB
    {
        public Point Sensor { get; set; }
        public Point Beacon { get; set; }
        public int SBDist { get; }

        public SB(string s)
        {
            var m = Regex.Match(s, @".*x=([-]?\d+), y=([-]?\d+):.*x=([-]?\d+), y=([-]?\d+)$");

            if (m.Groups.Count != 5)
            {
                throw new Exception("Invalid input string");
            }

            Sensor = new Point(m.Groups[1].Value + "," + m.Groups[2].Value);
            Beacon = new Point(m.Groups[3].Value + "," + m.Groups[4].Value);

            SBDist = MDist(Sensor, Beacon);
        }

        public static int MDist(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
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

    internal class Day15
    {
        private const int MaxX = 4_000_000;

        private bool IsPart2 = false;

        internal List<Segment> ScanAt(SB[] values, int row)
        {
            var segments = new List<Segment>();

            foreach (var sb in values)
            {
                int rdiff = Math.Abs(sb.Sensor.Y - row);
                int xdiff = sb.SBDist - rdiff;

                if (xdiff < 0)
                {
                    continue;
                }

                var s = new Segment()
                {
                    Start = sb.Sensor.X - xdiff,
                    End = sb.Sensor.X + xdiff
                };

                if (IsPart2)
                {
                    if (s.Start < 0)
                    {
                        s.Start = 0;
                    }
                    else if (s.Start > MaxX)
                    {
                        // Out of bounds
                        continue;
                    }

                    if (s.End < 0)
                    {
                        // Out of bounds
                        continue;
                    }
                    else if (s.End > MaxX)
                    {
                        s.End = MaxX;
                    }
                }

                while (segments.Any(o => o.Intersects(s)))
                {
                    var o = segments.First(oo => oo.Intersects(s));
                    if (o.Start >= s.Start && o.End <= s.End)
                    {
                        // Eat the smaller
                        segments.Remove(o);
                    }
                    else if (o.Start <= s.Start && o.End >= s.End)
                    {
                        // Replace
                        s.Start = o.Start;
                        s.End = o.End;
                        segments.Remove(o);
                    }
                    else
                    {
                        // Split
                        if (s.Start < o.Start)
                        {
                            s.End = o.Start - 1;
                        }
                        else
                        {
                            s.Start = o.End + 1;
                        }
                    }
                }

                // Reduce 1
                if (segments.Any(o => o.End == s.Start - 1))
                {
                    var o = segments.First(oo => oo.End == s.Start - 1);

                    // Replace
                    s.Start = o.Start;
                    segments.Remove(o);
                }

                // Reduce 2
                if (segments.Any(o => o.Start == s.End + 1))
                {
                    var o = segments.First(oo => oo.Start == s.End + 1);

                    // Replace
                    s.End = o.End;
                    segments.Remove(o);
                }

                segments.Add(s);
            }

            return segments;
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day15.txt");
            var rowToInspect = 2_000_000;

            /*
            var data = new string[]
            {
                "Sensor at x=2, y=18: closest beacon is at x=-2, y=15",
                "Sensor at x=9, y=16: closest beacon is at x=10, y=16",
                "Sensor at x=13, y=2: closest beacon is at x=15, y=3",
                "Sensor at x=12, y=14: closest beacon is at x=10, y=16",
                "Sensor at x=10, y=20: closest beacon is at x=10, y=16",
                "Sensor at x=14, y=17: closest beacon is at x=10, y=16",
                "Sensor at x=8, y=7: closest beacon is at x=2, y=10",
                "Sensor at x=2, y=0: closest beacon is at x=2, y=10",
                "Sensor at x=0, y=11: closest beacon is at x=2, y=10",
                "Sensor at x=20, y=14: closest beacon is at x=25, y=17",
                "Sensor at x=17, y=20: closest beacon is at x=21, y=22",
                "Sensor at x=16, y=7: closest beacon is at x=15, y=3",
                "Sensor at x=14, y=3: closest beacon is at x=15, y=3",
                "Sensor at x=20, y=1: closest beacon is at x=15, y=3",
            };
            rowToInspect = 10;
            */

            var values = data.Select(r => new SB(r)).ToArray();

            var uniqueBeacons = new List<Point>();

            int maxSbDist = values.Max(sb => sb.SBDist);
            Point topLeft = new Point()
            {
                X = values.Min(sb => Math.Min(sb.Beacon.X, sb.Sensor.X)),
                Y = values.Min(sb => Math.Min(sb.Beacon.Y, sb.Sensor.Y))
            };
            Point bottomRight = new Point()
            {
                X = values.Max(sb => Math.Max(sb.Beacon.X, sb.Sensor.X)),
                Y = values.Max(sb => Math.Max(sb.Beacon.Y, sb.Sensor.Y))
            };

            foreach (var sb in values)
            {
                if (!uniqueBeacons.Any(b => b.X == sb.Beacon.X && b.Y == sb.Beacon.Y))
                {
                    uniqueBeacons.Add(new Point(sb.Beacon.X, sb.Beacon.Y));
                }
            }

            var segments = ScanAt(values, rowToInspect);

            var sum = segments.Sum(s => s.Length) - uniqueBeacons.Count(b => b.Y == rowToInspect);

            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day15.txt");

            IsPart2 = true;
            
            /*
            var data = new string[]
            {
                "Sensor at x=2, y=18: closest beacon is at x=-2, y=15",
                "Sensor at x=9, y=16: closest beacon is at x=10, y=16",
                "Sensor at x=13, y=2: closest beacon is at x=15, y=3",
                "Sensor at x=12, y=14: closest beacon is at x=10, y=16",
                "Sensor at x=10, y=20: closest beacon is at x=10, y=16",
                "Sensor at x=14, y=17: closest beacon is at x=10, y=16",
                "Sensor at x=8, y=7: closest beacon is at x=2, y=10",
                "Sensor at x=2, y=0: closest beacon is at x=2, y=10",
                "Sensor at x=0, y=11: closest beacon is at x=2, y=10",
                "Sensor at x=20, y=14: closest beacon is at x=25, y=17",
                "Sensor at x=17, y=20: closest beacon is at x=21, y=22",
                "Sensor at x=16, y=7: closest beacon is at x=15, y=3",
                "Sensor at x=14, y=3: closest beacon is at x=15, y=3",
                "Sensor at x=20, y=1: closest beacon is at x=15, y=3",
            };
            */

            var values = data.Select(r => new SB(r)).ToArray();

            var uniqueBeacons = new List<Point>();

            int maxSbDist = values.Max(sb => sb.SBDist);
            Point topLeft = new Point()
            {
                X = values.Min(sb => Math.Min(sb.Beacon.X, sb.Sensor.X)),
                Y = values.Min(sb => Math.Min(sb.Beacon.Y, sb.Sensor.Y))
            };
            Point bottomRight = new Point()
            {
                X = values.Max(sb => Math.Max(sb.Beacon.X, sb.Sensor.X)),
                Y = values.Max(sb => Math.Max(sb.Beacon.Y, sb.Sensor.Y))
            };

            foreach (var sb in values)
            {
                if (!uniqueBeacons.Any(b => b.X == sb.Beacon.X && b.Y == sb.Beacon.Y))
                {
                    uniqueBeacons.Add(new Point(sb.Beacon.X, sb.Beacon.Y));
                }
            }

            long prod = 0;

            for (int i = 0; i < MaxX; i++)
            {
                var segments = ScanAt(values, i);
                if (segments.Count > 1)
                {
                    int x = segments.First().End + 1;
                    if (segments.Skip(1).First().End < x)
                    {
                        x = segments.Skip(1).First().End + 1;
                    }

                    Console.WriteLine($"Found at y = {i}, x = {x}");

                    prod = (long)(x) * (long)MaxX + (long)i;
                    
                    break;
                }
            }
            

            Console.WriteLine($"Answer is {prod}");
        }

    }
}
