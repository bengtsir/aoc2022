using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using aoc2022.Structs;

namespace aoc2022
{
    internal class Day9
    {
        internal const int maxX = 1000;
        internal const int maxY = 1000;

        internal int[,] Board = new int[maxY, maxX];
        
        internal const int startX = maxX / 2;
        internal const int startY = maxY / 2;

        internal int headPosX = startX;
        internal int headPosY = startY;
        internal int tailPosX = startX;
        internal int tailPosY = startY;

        internal Point[] Positions = new Point[10];

        internal Point MinPos = new Point() { X = startX, Y = startY };
        internal Point MaxPos = new Point() { X = startX, Y = startY };

        internal void Move(int dx, int dy)
        {
            headPosX += dx;
            headPosY += dy;

            if (headPosX >= maxX || headPosX < 0)
            {
                throw new Exception("X too small");
            }

            if (headPosY >= maxY || headPosY < 0)
            {
                throw new Exception("Y too small");
            }

            if (Math.Abs(tailPosY - headPosY) > 1)
            {
                if (tailPosY < headPosY)
                {
                    tailPosY++;
                }
                else
                {
                    tailPosY--;
                }

                tailPosX = headPosX;
            }
            else if (Math.Abs(tailPosX - headPosX) > 1)
            {
                if (tailPosX < headPosX)
                {
                    tailPosX++;
                }
                else
                {
                    tailPosX--;
                }

                tailPosY = headPosY;
            }

            Board[tailPosY, tailPosX]++;
        }

        internal List<Point> TailPositions = new List<Point>();

        internal void MovePart2(int dx, int dy)
        {
            Positions[0].X += dx;
            Positions[0].Y += dy;

            if (Positions[0].X < MinPos.X)
            {
                MinPos.X = Positions[0].X;
            }
            if (Positions[0].X > MaxPos.X)
            {
                MaxPos.X = Positions[0].X;
            }
            if (Positions[0].Y < MinPos.Y)
            {
                MinPos.Y = Positions[0].Y;
            }
            if (Positions[0].Y > MaxPos.Y)
            {
                MaxPos.Y = Positions[0].Y;
            }

            if (Positions[0].X >= maxX || Positions[0].X < 0)
            {
                throw new Exception("X too small");
            }

            if (Positions[0].Y >= maxY || Positions[0].Y < 0)
            {
                throw new Exception("Y too small");
            }

            for (int i = 1; i < 10; i++)
            {
                if (Math.Abs(Positions[i].Y - Positions[i-1].Y) > 1)
                {
                    if (Positions[i].Y < Positions[i-1].Y)
                    {
                        Positions[i].Y++;
                    }
                    else
                    {
                        Positions[i].Y--;
                    }

                    if (Positions[i].X < Positions[i - 1].X)
                    {
                        Positions[i].X++;
                    }
                    else if (Positions[i].X > Positions[i-1].X)
                    {
                        Positions[i].X--;
                    }
                }
                else if (Math.Abs(Positions[i].X - Positions[i-1].X) > 1)
                {
                    if (Positions[i].X < Positions[i-1].X)
                    {
                        Positions[i].X++;
                    }
                    else
                    {
                        Positions[i].X--;
                    }

                    if (Positions[i].Y < Positions[i - 1].Y)
                    {
                        Positions[i].Y++;
                    }
                    else if (Positions[i].Y > Positions[i - 1].Y)
                    {
                        Positions[i].Y--;
                    }
                }
                else
                {
                    // No move
                    break;
                }
            }

            if (!TailPositions.Any(p => p.X == Positions[9].X && p.Y == Positions[9].Y))
            {
                TailPositions.Add(new Point() { X = Positions[9].X, Y = Positions[9].Y });
            }

            Board[Positions[9].Y, Positions[9].X]++;
        }

        void PrintBoard()
        {
            for (int r = MinPos.Y; r <= MaxPos.Y; r++)
            {
                for (int k = MinPos.X; k <= MaxPos.X; k++)
                {
                    var any = false;
                    for (int i = 0; i < 10; i++)
                    {
                        if (Positions[i].X == k && Positions[i].Y == r)
                        {
                            any = true;
                            Console.Write($"{i}");
                            break;
                        }
                    }
                    if (!any)
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day9.txt");

            var values = data.Select(r => r.Split(' ')).ToArray();

            foreach (var move in values)
            {
                int dx = 0;
                int dy = 0;

                switch (move[0])
                {
                    case "U":
                        dx = 0;
                        dy = -1;
                        break;
                    case "D":
                        dx = 0;
                        dy = 1;
                        break;
                    case "L":
                        dx = -1;
                        dy = 0;
                        break;
                    case "R":
                        dx = 1;
                        dy = 0;
                        break;
                }

                for (int i = 0; i < Int32.Parse(move[1]); i++)
                {
                    Move(dx, dy);
                }
            }

            var sum = Board.Cast<int>().Count(v => v > 0);

            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day9.txt");
            
            /*
            var data = new string[]
            {
                "R 5",
                "U 8",
                "L 8",
                "D 3",
                "R 17",
                "D 10",
                "L 25",
                "U 20",
            };
            */
            /*
            var data = new string[]
            {
                "R 4",
                "U 4",
                "L 3",
                "D 1",
                "R 4",
                "D 1",
                "L 5",
                "R 2",
            };
            */

            var values = data.Select(r => r.Split(' ')).ToArray();

            for (int i = 0; i < 10; i++)
            {
                Positions[i].X = maxX / 2;
                Positions[i].Y = maxY / 2;
            }

            foreach (var move in values)
            {
                int dx = 0;
                int dy = 0;

                switch (move[0])
                {
                    case "U":
                        dx = 0;
                        dy = -1;
                        break;
                    case "D":
                        dx = 0;
                        dy = 1;
                        break;
                    case "L":
                        dx = -1;
                        dy = 0;
                        break;
                    case "R":
                        dx = 1;
                        dy = 0;
                        break;
                }

                for (int i = 0; i < Int32.Parse(move[1]); i++)
                {
                    MovePart2(dx, dy);
                    //PrintBoard();
                }
            }

            var sum = Board.Cast<int>().Count(v => v > 0);

            Console.WriteLine($"MinPos X: {MinPos.X} Y: {MinPos.Y} MaxPos X: {MaxPos.X} Y: {MaxPos.Y}");

            Console.WriteLine($"TailPositions: {TailPositions.Count}");

            Console.WriteLine($"Answer is {sum}");
        }

    }
}
