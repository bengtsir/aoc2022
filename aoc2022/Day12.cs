using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using aoc2022.Structs;


namespace aoc2022
{
    internal class Day12
    {
        internal Point Start;
        internal Point End;

        internal int[][] Values;
        internal bool[][] Visited;
        internal int[][] Distances;

        internal Point BoardSize;

        internal const int Inf = 999999999;

        internal void FindStartAndEnd()
        {
            for (int r = 0; r < Values.Length; r++)
            {
                for (int k = 0; k < Values[0].Length; k++)
                {
                    if (Values[r][k] == (int)('S' - 'a'))
                    {
                        Start = new Point()
                        {
                            X = k,
                            Y = r
                        };
                        Values[r][k] = 0;
                    }
                    else if (Values[r][k] == (int)('E' - 'a'))
                    {
                        End = new Point()
                        {
                            X = k,
                            Y = r
                        };
                        Values[r][k] = (int)('z' - 'a');
                    }
                }
            }

            Visited = Values.Select(r => r.Select(c => false).ToArray()).ToArray();
            Distances = Values.Select(r => r.Select(c => Inf).ToArray()).ToArray();
            Distances[Start.Y][Start.X] = 0;

            BoardSize = new Point()
            {
                X = Values[0].Length,
                Y = Values.Length
            };
        }

        internal void Inspect(Point a, Point b)
        {
            if (b.X < 0 || b.X >= BoardSize.X || b.Y < 0 || b.Y >= BoardSize.Y)
            {
                return;
            }

            if (Values[b.Y][b.X] <= Values[a.Y][a.X] + 1 && !Visited[b.Y][b.X])
            {
                if (Distances[b.Y][b.X] > Distances[a.Y][a.X] + 1)
                {
                    Distances[b.Y][b.X] = Distances[a.Y][a.X] + 1;
                }
            }
        }

        internal void Traverse(Point p)
        {
            Inspect(p, new Point() { X = p.X + 1, Y = p.Y });
            Inspect(p, new Point() { X = p.X, Y = p.Y + 1 });
            Inspect(p, new Point() { X = p.X - 1, Y = p.Y });
            Inspect(p, new Point() { X = p.X, Y = p.Y - 1 });
            Visited[p.Y][p.X] = true;
        }

        internal void Inspect2(Point a, Point b)
        {
            if (b.X < 0 || b.X >= BoardSize.X || b.Y < 0 || b.Y >= BoardSize.Y)
            {
                return;
            }

            if (Values[b.Y][b.X] >= Values[a.Y][a.X] - 1 && !Visited[b.Y][b.X])
            {
                if (Distances[b.Y][b.X] > Distances[a.Y][a.X] + 1)
                {
                    Distances[b.Y][b.X] = Distances[a.Y][a.X] + 1;
                }
            }
        }

        internal void Traverse2(Point p)
        {
            Inspect2(p, new Point() { X = p.X + 1, Y = p.Y });
            Inspect2(p, new Point() { X = p.X, Y = p.Y + 1 });
            Inspect2(p, new Point() { X = p.X - 1, Y = p.Y });
            Inspect2(p, new Point() { X = p.X, Y = p.Y - 1 });
            Visited[p.Y][p.X] = true;
        }

        void Dijkstra(bool part2 = false)
        {
            while (true)
            {
                var currVal = Inf;
                Point selected = new Point()
                {
                    X = -1,
                    Y = -1
                };

                for (int r = 0; r < BoardSize.Y; r++)
                {
                    for (int k = 0; k < BoardSize.X; k++)
                    {
                        if (!Visited[r][k] && Distances[r][k] < currVal)
                        {
                            selected = new Point()
                            {
                                X = k,
                                Y = r
                            };
                            currVal = Distances[r][k];
                        }
                    }
                }

                if (selected.X < 0)
                {
                    break;
                }

                if (part2)
                {
                    Traverse2(selected);
                }
                else
                {
                    Traverse(selected);
                }
            }
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day12.txt");

            /*
            var data = new string[]
            {
                "Sabqponm",
                "abcryxxl",
                "accszExk",
                "acctuvwj",
                "abdefghi",
            };
            */

            Values = data.Select(r => r.Select(c => (int) (c - 'a')).ToArray()).ToArray();

            FindStartAndEnd();

            Dijkstra();

            Console.WriteLine($"Answer is {Distances[End.Y][End.X]}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day12.txt");

            /*
            var data = new string[]
            {
                "Sabqponm",
                "abcryxxl",
                "accszExk",
                "acctuvwj",
                "abdefghi",
            };
            */
            
            Values = data.Select(r => r.Select(c => (int)(c - 'a')).ToArray()).ToArray();

            FindStartAndEnd();

            (End, Start) = (Start, End);

            Distances[End.Y][End.X] = Inf;
            Distances[Start.Y][Start.X] = 0;

            Dijkstra(true);

            var lowest = Inf;

            for (int r = 0; r < BoardSize.Y; r++)
            {
                for (int k = 0; k < BoardSize.X; k++)
                {
                    if (Values[r][k] == 0 && Distances[r][k] < lowest)
                    {
                        lowest = Distances[r][k];
                    }
                }
            }

            Console.WriteLine($"Answer is {lowest}");
        }

    }
}
