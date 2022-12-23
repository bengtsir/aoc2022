using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aoc2022.Structs;

namespace aoc2022
{
    internal class Day23
    {
        private const int NORTH = 0;
        private const int SOUTH = 1;
        private const int WEST = 2;
        private const int EAST = 3;

        internal Point[] Offsets = new []
        {
            new Point( 0, -1),
            new Point( 0,  1),
            new Point(-1,  0),
            new Point( 1,  0)
        };

        internal char[][] Board;
        internal int[][] Proposals;
        internal int[][] ProposalDestCounts;

        internal Point BoardSize;

        internal void CreateBoard(string[] data)
        {
            int margin = data[0].Length;

            string sideBuf = new string('.', margin);

            var tempBoard = new string[data.Length + 2 * margin];

            for (int i = 0; i < margin; i++)
            {
                tempBoard[i] = sideBuf + new string('.', data[0].Length) + sideBuf;
                tempBoard[i+data.Length+margin] = sideBuf + new string('.', data[0].Length) + sideBuf;
            }

            for (int i = 0; i < data.Length; i++)
            {
                tempBoard[i + margin] = sideBuf + data[i] + sideBuf;
            }

            Board = tempBoard.Select(r => r.Select(c => c).ToArray()).ToArray();

            Proposals = Board.Select(r => r.Select(c => 0).ToArray()).ToArray();
            ProposalDestCounts = Board.Select(r => r.Select(c => 0).ToArray()).ToArray();

            BoardSize = new Point(Board[0].Length, Board.Length);
        }

        internal int FirstDir = NORTH;

        internal bool AnyAround(int r, int k)
        {
            bool found = false;

            if (r > 0)
            {
                if (k > 0 && Board[r - 1][k - 1] == '#')
                {
                    found = true;
                }

                if (Board[r - 1][k] == '#')
                {
                    found = true;
                }

                if (k < BoardSize.X - 1 && Board[r - 1][k + 1] == '#')
                {
                    found = true;
                }
            }

            if (k > 0 && Board[r][k - 1] == '#')
            {
                found = true;
            }

            if (k < BoardSize.X - 1 && Board[r][k + 1] == '#')
            {
                found = true;
            }

            if (r < BoardSize.Y - 1)
            {
                if (k > 0 && Board[r + 1][k - 1] == '#')
                {
                    found = true;
                }

                if (Board[r + 1][k] == '#')
                {
                    found = true;
                }

                if (k < BoardSize.X - 1 && Board[r + 1][k + 1] == '#')
                {
                    found = true;
                }
            }

            return found;
        }

        internal bool AnyInDir(int r, int k, int dir)
        {
            bool found = false;

            switch (dir)
            {
                case NORTH:
                    if (r > 0)
                    {

                        if (k > 0 && Board[r - 1][k - 1] == '#')
                        {
                            found = true;
                        }

                        if (Board[r - 1][k] == '#')
                        {
                            found = true;
                        }

                        if (k < BoardSize.X - 1 && Board[r - 1][k + 1] == '#')
                        {
                            found = true;
                        }
                    }

                    break;
                case SOUTH:
                    if (r < BoardSize.Y - 1)
                    {
                        if (k > 0 && Board[r + 1][k - 1] == '#')
                        {
                            found = true;
                        }

                        if (Board[r + 1][k] == '#')
                        {
                            found = true;
                        }

                        if (k < BoardSize.X - 1 && Board[r + 1][k + 1] == '#')
                        {
                            found = true;
                        }
                    }

                    break;
                case WEST:
                    if (k > 0)
                    {
                        if (r > 0 && Board[r - 1][k - 1] == '#')
                        {
                            found = true;
                        }

                        if (Board[r][k - 1] == '#')
                        {
                            found = true;
                        }

                        if (r < BoardSize.Y - 1 && Board[r + 1][k - 1] == '#')
                        {
                            found = true;
                        }
                    }

                    break;
                case EAST:
                    if (k < BoardSize.X - 1)
                    {
                        if (r > 0 && Board[r - 1][k + 1] == '#')
                        {
                            found = true;
                        }

                        if (Board[r][k + 1] == '#')
                        {
                            found = true;
                        }

                        if (r < BoardSize.Y - 1 && Board[r + 1][k + 1] == '#')
                        {
                            found = true;
                        }
                    }

                    break;
            }

            return found;
        }

        internal int Iterate()
        {
            int numberOfMoves = 0;

            for (int i = 0; i < BoardSize.Y; i++)
            {
                for (int j = 0; j < BoardSize.X; j++)
                {
                    Proposals[i][j] = -1;
                    ProposalDestCounts[i][j] = 0;
                }
            }

            // First half: Check
            for (int r = 0; r < BoardSize.Y; r++)
            {
                for (int k = 0; k < BoardSize.X; k++)
                {
                    if (Board[r][k] == '#')
                    {
                        // Check if no others around
                        if (!AnyAround(r, k))
                        {
                            continue;
                        }

                        for (int i = 0; i < 4; i++)
                        {
                            var testDir = (FirstDir + i) % 4;

                            if (!AnyInDir(r, k, testDir))
                            {
                                Proposals[r][k] = testDir;
                                ProposalDestCounts[r + Offsets[testDir].Y][k + Offsets[testDir].X]++;
                                break;
                            }
                        }
                    }
                }
            }

            // Second half: Move if possible
            for (int r = 0; r < BoardSize.Y; r++)
            {
                for (int k = 0; k < BoardSize.X; k++)
                {
                    if (Board[r][k] == '#' && Proposals[r][k] >= 0)
                    {
                        var ofs = Offsets[Proposals[r][k]];

                        if (r + ofs.Y < 0 || r + ofs.Y >= BoardSize.Y ||
                            k + ofs.X < 0 || k + ofs.X >= BoardSize.X)
                        {
                            throw new Exception("Board too small!");
                        }

                        if (ProposalDestCounts[r + ofs.Y][k + ofs.X] <= 1)
                        {
                            // Move
                            Board[r][k] = '.';
                            Board[r + ofs.Y][k + ofs.X] = '#';
                            numberOfMoves++;
                        }
                    }
                }
            }

            // Switch to new first direction
            FirstDir = (FirstDir + 1) % 4;

            return numberOfMoves;
        }

        internal Point TopLeft = new Point(0, 0);
        internal Point BottomRight = new Point(1, 1);

        internal void FindCorners()
        {
            int r;
            int k;

            r = 0;
            while (Board[r].All(c => c != '#'))
            {
                r++;
            }

            k = 0;
            while (Board.Select(row => row.Skip(k).First()).All(c => c != '#'))
            {
                k++;
            }

            TopLeft.X = k;
            TopLeft.Y = r;

            r = BoardSize.Y - 1;
            while (Board[r].All(c => c != '#'))
            {
                r--;
            }

            k = BoardSize.X - 1;
            while (Board.Select(row => row.Skip(k).First()).All(c => c != '#'))
            {
                k--;
            }

            BottomRight.X = k;
            BottomRight.Y = r;
        }

        internal void PrintBoard()
        {
            for (int r = TopLeft.Y; r <= BottomRight.Y; r++)
            {
                for (int k = TopLeft.X; k <= BottomRight.X; k++)
                {
                    Console.Write(Board[r][k]);
                }

                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day23.txt");

            /*
            var data = new string[]
            {
                "....#..",
                "..###.#",
                "#...#.#",
                ".#...##",
                "#.###..",
                "##.#.##",
                ".#..#..",
            };
            */

            CreateBoard(data);

            FindCorners();
            PrintBoard();

            for (int i = 0; i < 10; i++)
            {
                Iterate();
                Console.WriteLine($"After round {i+1}:");
                FindCorners();
                //PrintBoard();
            }

            FindCorners();

            var emptySpaces = Board.Skip(TopLeft.Y).Take(BottomRight.Y - TopLeft.Y + 1)
                .Select(r => r.Skip(TopLeft.X).Take(BottomRight.X - TopLeft.X + 1).Count(c => c == '.')).Sum();

            Console.WriteLine($"Answer is {emptySpaces}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day23.txt");

            /*
            var data = new string[]
            {
                "....#..",
                "..###.#",
                "#...#.#",
                ".#...##",
                "#.###..",
                "##.#.##",
                ".#..#..",
            };
            */

            CreateBoard(data);

            FindCorners();
            PrintBoard();

            int round = 0;
            int numberOfMoves;

            while (true)
            {
                round++;
                numberOfMoves = Iterate();

                Console.WriteLine($"Round {round}: {numberOfMoves} moves");

                if (numberOfMoves == 0)
                {
                    break;
                }
            }

            Console.WriteLine($"Answer is {round}");
        }

    }
}
