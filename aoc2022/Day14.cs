using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aoc2022.Structs;

namespace aoc2022
{
    internal class Day14
    {
        private const int MaxX = 1100;
        private const int MaxY = 1000;

        internal char[][] Cave = new char[MaxY][];

        internal Point TopLeft = new Point(499, 0);
        internal Point BottomRight = new Point(501, 10);

        void PrintBoard()
        {
            for (int r = 0; r < BottomRight.Y + 3; r++)
            {
                for (int k = TopLeft.X - 30; k < BottomRight.X + 1 + 30; k++)
                {
                    Console.Write(Cave[r][k]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        internal void MakeWalls(string[][] pairs)
        {
            for (int i = 0; i < MaxY; i++)
            {
                Cave[i] = new char[MaxX];
                for (int j = 0; j < MaxX; j++)
                {
                    Cave[i][j] = '.';
                }
            }

            foreach (var line in pairs)
            {
                Point current = new Point(line[0]);
                
                int dx = 0;
                int dy = 0;

                for (int i = 1; i < line.Length; i++)
                {
                    var newPoint = new Point(line[i]);

                    if (newPoint.X == current.X)
                    {
                        if (newPoint.Y < current.Y)
                        {
                            dx = 0;
                            dy = -1;
                        }
                        else
                        {
                            dx = 0;
                            dy = 1;
                        }
                    }
                    else
                    {
                        if (newPoint.X < current.X)
                        {
                            dx = -1;
                            dy = 0;
                        }
                        else
                        {
                            dx = 1;
                            dy = 0;
                        }
                    }

                    while (current.X != newPoint.X || current.Y != newPoint.Y)
                    {
                        Cave[current.Y][current.X] = '#';
                        current.X += dx;
                        current.Y += dy;
                    }
                    Cave[current.Y][current.X] = '#';

                    if (current.X < TopLeft.X)
                    {
                        TopLeft.X = current.X;
                    }

                    if (current.X > BottomRight.X)
                    {
                        BottomRight.X = current.X;
                    }

                    if (current.Y > BottomRight.Y)
                    {
                        BottomRight.Y = current.Y;
                    }
                }

            }
        }

        internal void AddFloor()
        {
            for (int k = 0; k < MaxX; k++)
            {
                Cave[BottomRight.Y + 2][k] = '#';
            }
        }

        internal bool AddGrain(int x, int y)
        {
            int gposX = x;
            int gposY = y;

            while (true)
            {
                if (Cave[gposY+1][gposX] == '.')
                {
                    gposY++;
                }
                else if (Cave[gposY + 1][gposX - 1] == '.')
                {
                    gposY++;
                    gposX--;
                }
                else if (Cave[gposY + 1][gposX + 1] == '.')
                {
                    gposY++;
                    gposX++;
                }
                else
                {
                    Cave[gposY][gposX] = 'o';
                    if (gposY == y && gposX == x)
                    {
                        return false; // done
                    }
                    break;
                }

                if (gposY > 900)
                {
                    return false;
                }
            }

            return true;
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day14.txt");

            /*
            var data = new string[]
            {
                "498,4 -> 498,6 -> 496,6",
                "503,4 -> 502,4 -> 502,9 -> 494,9",
            };
            */

            var pairs = data.Select(r => r.Split(' ').Where(s => s != "->").ToArray()).ToArray();

            MakeWalls(pairs);

            PrintBoard();
            

            int nGrains = 0;
            while (AddGrain(500, 0))
            {
                nGrains++;
            }

            PrintBoard();

            Console.WriteLine($"Answer is {nGrains}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day14.txt");

            /*
            var data = new string[]
            {
                "498,4 -> 498,6 -> 496,6",
                "503,4 -> 502,4 -> 502,9 -> 494,9",
            };
            */
            

            var pairs = data.Select(r => r.Split(' ').Where(s => s != "->").ToArray()).ToArray();

            MakeWalls(pairs);

            PrintBoard();

            AddFloor();

            int nGrains = 0;
            while (AddGrain(500, 0))
            {
                nGrains++;
            }

            nGrains++; // Last one isn't counted

            var sum = Cave.Sum(r => r.Count(c => c == 'o'));

            PrintBoard();

            Console.WriteLine($"Answer is {nGrains} {sum}");
        }

    }
}
