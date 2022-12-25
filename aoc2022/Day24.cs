using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aoc2022.Structs;

namespace aoc2022
{
    internal class Day24
    {
        internal bool[][] Walls;
        internal bool[][] UpDrafts;
        internal bool[][] DownDrafts;
        internal bool[][] RightDrafts;
        internal bool[][] LeftDrafts;

        internal Point CurrentPosition;
        internal Point StartPosition;
        internal Point EndPosition;
        internal Point BlizzardShuffle;
        internal Point BlizzardArea;

        internal void CreateBoard(string[] data)
        {
            BlizzardShuffle = new Point(0, 0);

            Walls = data.Select(r => r.Select(c => c == '#').ToArray()).ToArray();
            UpDrafts = data.Select(r => r.Select(c => c == '^').ToArray()).ToArray();
            DownDrafts = data.Select(r => r.Select(c => c == 'v').ToArray()).ToArray();
            RightDrafts = data.Select(r => r.Select(c => c == '>').ToArray()).ToArray();
            LeftDrafts = data.Select(r => r.Select(c => c == '<').ToArray()).ToArray();

            BlizzardArea = new Point(Walls[0].Length - 2, Walls.Length - 2);

            StartPosition = new Point(1, 0);
            EndPosition = new Point(Walls[0].Length - 2, Walls.Length - 1);

            CurrentPosition.X = StartPosition.X;
            CurrentPosition.Y = StartPosition.Y;
        }

        internal bool CanMove(Point pos, int dx, int dy)
        {
            if (pos.Y + dy < 0 || pos.Y + dy >= Walls.Length)
            {
                return false;
            }

            if (Walls[pos.Y + dy][pos.X + dx])
            {
                return false;
            }

            if (UpDrafts[((pos.Y + dy + BlizzardShuffle.Y - 1) % BlizzardArea.Y) + 1][pos.X + dx])
            {
                return false;
            }

            if (DownDrafts[((pos.Y + dy - BlizzardShuffle.Y + BlizzardArea.Y - 1) % BlizzardArea.Y) + 1][pos.X + dx])
            {
                return false;
            }

            if (RightDrafts[pos.Y + dy][((pos.X + dx - BlizzardShuffle.X + BlizzardArea.X - 1) % BlizzardArea.X) + 1])
            {
                return false;
            }

            if (LeftDrafts[pos.Y + dy][((pos.X + dx + BlizzardShuffle.X - 1) % BlizzardArea.X) + 1])
            {
                return false;
            }

            return true;
        }

        internal int InfMoves = 999999;

        internal Point[] Offsets = new[]
        {
            new Point(0, 1),
            new Point(1, 0),
            new Point(0, -1),
            new Point(-1, 0),
            new Point(0, 0),
        };

        internal List<Point> CurrentPositions = new List<Point>();

        internal List<Point> NewPositions = new List<Point>();

        internal void AddMoves(Point pos)
        {
            foreach (var check in Offsets)
            {
                if (CanMove(pos, check.X, check.Y))
                {
                    if (!NewPositions.Any(p => p.X == pos.X + check.X && p.Y == pos.Y + check.Y))
                    {
                        NewPositions.Add(new Point(pos.X + check.X, pos.Y + check.Y));
                    }
                }
            }
        }

        internal int IterateMoves()
        {
            CurrentPositions.Clear();
            CurrentPositions.Add(StartPosition);
            int iter = 0;

            while (!CurrentPositions.Any(p => p.X == EndPosition.X && p.Y == EndPosition.Y))
            {
                BlizzardShuffle.X = (BlizzardShuffle.X + 1) % BlizzardArea.X;
                BlizzardShuffle.Y = (BlizzardShuffle.Y + 1) % BlizzardArea.Y;
                
                iter++;

                //PrintBoard();

                if (iter % 10 == 0)
                {
                    Console.WriteLine($"Iter = {iter}");
                }

                foreach (var pos in CurrentPositions)
                {
                    AddMoves(pos);
                }

                CurrentPositions = NewPositions.OrderBy(p => p.ManhattanDist(EndPosition)).Take(2000).ToList();
                NewPositions.Clear();
            }

            return iter;
        }

        internal void PrintBoard()
        {
            for (int r = 0; r < Walls.Length; r++)
            {
                for (int k = 0; k < Walls[0].Length; k++)
                {
                    if (CurrentPosition.X == k && CurrentPosition.Y == r)
                    {
                        Console.Write('E');
                    }
                    else if (Walls[r][k])
                    {
                        Console.Write('#');
                    }
                    else if (UpDrafts[((r + BlizzardShuffle.Y - 1) % BlizzardArea.Y) + 1][k])
                    {
                        Console.Write('^');
                    }
                    else if (DownDrafts[((r - BlizzardShuffle.Y + BlizzardArea.Y - 1) % BlizzardArea.Y) + 1][k])
                    {
                        Console.Write('v');
                    }
                    else if (RightDrafts[r][((k - BlizzardShuffle.X - 1 + BlizzardArea.X) % BlizzardArea.X) + 1])
                    {
                        Console.Write('>');
                    }
                    else if (LeftDrafts[r][((k + BlizzardShuffle.X - 1 + BlizzardArea.X) % BlizzardArea.X) + 1])
                    {
                        Console.Write('<');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Pos {CurrentPosition}");
            Console.WriteLine();
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day24.txt");

            /*
            var data = new string[]
            {
                "#.######",
                "#>>.<^<#",
                "#.<..<<#",
                "#>v.><>#",
                "#<^v^^>#",
                "######.#",
            };
            */

            CreateBoard(data);

            PrintBoard();

            InfMoves = (Walls.Length + Walls[0].Length) * 2;

            var moves = IterateMoves();

            Console.WriteLine($"Answer is {moves}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day24.txt");

            /*
            var data = new string[]
            {
                "#.######",
                "#>>.<^<#",
                "#.<..<<#",
                "#>v.><>#",
                "#<^v^^>#",
                "######.#",
            };
            */

            CreateBoard(data);

            PrintBoard();

            InfMoves = (Walls.Length + Walls[0].Length) * 2;

            var m1 = IterateMoves();

            (EndPosition.X, StartPosition.X) = (StartPosition.X, EndPosition.X);
            (EndPosition.Y, StartPosition.Y) = (StartPosition.Y, EndPosition.Y);

            var m2 = IterateMoves();

            (EndPosition.X, StartPosition.X) = (StartPosition.X, EndPosition.X);
            (EndPosition.Y, StartPosition.Y) = (StartPosition.Y, EndPosition.Y);

            var m3 = IterateMoves();

            Console.WriteLine($"Answer is {m1}+{m2}+{m3} = {m1+m2+m3}");
        }

    }
}
