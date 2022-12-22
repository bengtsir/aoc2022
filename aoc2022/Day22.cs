using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aoc2022.Structs;

namespace aoc2022
{
    internal class TurtleMove
    {
        public int Steps { get; set; } = -1;
        public bool TurnLeft { get; set; }
        public bool TurnRight { get; set; }

        public bool IsTurn => Steps < 0;
    }
    
    internal class Day22
    {
        private string[] Board;

        private List<TurtleMove> Moves = new List<TurtleMove>();

        private Point Pos;

        private const int RIGHT = 0;
        private const int DOWN = 1;
        private const int LEFT = 2;
        private const int UP = 3;
        
        private int Dir = RIGHT;

        private Point[] Offsets = new Point[]
        {
            new Point(1, 0),
            new Point(0, 1),
            new Point(-1, 0),
            new Point(0, -1)
        };

        void CreateBoard(string[] data)
        {
            int split = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == "")
                {
                    split = i;
                }
            }

            Board = data.Take(split).ToArray();

            int boardWidth = Board.Max(r => r.Length);

            for (int i = 0; i < Board.Length; i++)
            {
                while (Board[i].Length < boardWidth)
                {
                    Board[i] += " ";
                }
            }

            Pos.Y = 0;
            Pos.X = Board[0].IndexOf('.');
            
            string instr = data[split + 1];
            while (instr.Length > 0)
            {
                var m = new TurtleMove();

                if (instr[0] == 'L')
                {
                    m.TurnLeft = true;
                    instr = instr.Substring(1);
                }
                else if (instr[0] == 'R')
                {
                    m.TurnRight = true;
                    instr = instr.Substring(1);
                }
                else
                {
                    string num = "";
                    while (instr.Length > 0 && instr[0] >= '0' && instr[0] <= '9')
                    {
                        num += instr[0];
                        instr = instr.Substring(1);
                    }
                    m.Steps = Int32.Parse(num);
                }

                Moves.Add(m);
            }
        }

        internal void Walk(TurtleMove m)
        {
            if (m.IsTurn)
            {
                if (m.TurnRight)
                {
                    Dir = (Dir + 1) % 4;
                }
                else
                {
                    Dir = (Dir + 3) % 4;
                }

                return;
            }

            var nextPoint = new Point(Pos.X, Pos.Y);

            for (int i = 0; i < m.Steps; i++)
            {
                nextPoint.X = Pos.X + Offsets[Dir].X;
                nextPoint.Y = Pos.Y + Offsets[Dir].Y;

                switch (Dir)
                {
                    case RIGHT:
                        if (nextPoint.X >= Board[nextPoint.Y].Length || Board[nextPoint.Y][nextPoint.X] == ' ')
                        {
                            nextPoint.X = 0;
                            while (Board[nextPoint.Y][nextPoint.X] == ' ')
                            {
                                nextPoint.X++;
                            }
                        }

                        break;

                    case DOWN:
                        if (nextPoint.Y >= Board.Length || Board[nextPoint.Y][nextPoint.X] == ' ')
                        {
                            nextPoint.Y = 0;
                            while (Board[nextPoint.Y][nextPoint.X] == ' ')
                            {
                                nextPoint.Y++;
                            }
                        }

                        break;

                    case LEFT:
                        if (nextPoint.X < 0 || Board[nextPoint.Y][nextPoint.X] == ' ')
                        {
                            nextPoint.X = Board[nextPoint.Y].Length - 1;
                            while (Board[nextPoint.Y][nextPoint.X] == ' ')
                            {
                                nextPoint.X--;
                            }
                        }

                        break;

                    case UP:
                        if (nextPoint.Y < 0 || Board[nextPoint.Y][nextPoint.X] == ' ')
                        {
                            nextPoint.Y = Board.Length - 1;
                            while (Board[nextPoint.Y][nextPoint.X] == ' ')
                            {
                                nextPoint.Y--;
                            }
                        }

                        break;
                }

                if (Board[nextPoint.Y][nextPoint.X] == '#')
                {
                    return;
                }

                Pos.X = nextPoint.X;
                Pos.Y = nextPoint.Y;
            }
        }

        internal void WalkCube(TurtleMove m)
        {
            if (m.IsTurn)
            {
                if (m.TurnRight)
                {
                    Dir = (Dir + 1) % 4;
                }
                else
                {
                    Dir = (Dir + 3) % 4;
                }

                return;
            }

            var nextPoint = new Point(Pos.X, Pos.Y);
            var nextDir = Dir;

            for (int i = 0; i < m.Steps; i++)
            {
                nextPoint.X = Pos.X + Offsets[nextDir].X;
                nextPoint.Y = Pos.Y + Offsets[nextDir].Y;

                switch (nextDir)
                {
                    case RIGHT:
                        if (nextPoint.X >= Board[nextPoint.Y].Length || Board[nextPoint.Y][nextPoint.X] == ' ')
                        {
                            switch (nextPoint.Y / 50)
                            {
                                case 0:
                                    nextPoint.Y = 149 - Pos.Y;
                                    nextPoint.X = Pos.X - 50;
                                    nextDir = LEFT;
                                    break;
                                case 1:
                                    nextPoint.X = 100 + Pos.Y - 50;
                                    nextPoint.Y = 49;
                                    nextDir = UP;
                                    break;
                                case 2:
                                    nextPoint.X = Pos.X + 50;
                                    nextPoint.Y = 49 - (Pos.Y % 50);
                                    nextDir = LEFT;
                                    break;
                                case 3:
                                    nextPoint.X = 50 + (Pos.Y % 50);
                                    nextPoint.Y = 149;
                                    nextDir = UP;
                                    break;
                            }
                        }

                        break;

                    case DOWN:
                        if (nextPoint.Y >= Board.Length || Board[nextPoint.Y][nextPoint.X] == ' ')
                        {
                            switch (nextPoint.X / 50)
                            {
                                case 0:
                                    nextPoint.X += 100;
                                    nextPoint.Y = 0;
                                    nextDir = DOWN;
                                    break;
                                case 1:
                                    nextPoint.X = 49;
                                    nextPoint.Y = 150 + (Pos.X % 50);
                                    nextDir = LEFT;
                                    break;
                                case 2:
                                    nextPoint.X = 99;
                                    nextPoint.Y = 50 + (Pos.X % 50);
                                    nextDir = LEFT;
                                    break;
                            }
                        }

                        break;

                    case LEFT:
                        if (nextPoint.X < 0 || Board[nextPoint.Y][nextPoint.X] == ' ')
                        {
                            switch (nextPoint.Y / 50)
                            {
                                case 0:
                                    nextPoint.X = 0;
                                    nextPoint.Y = 149 - (Pos.Y % 50);
                                    nextDir = RIGHT;
                                    break;
                                case 1:
                                    nextPoint.X = Pos.Y % 50;
                                    nextPoint.Y = 100;
                                    nextDir = DOWN;
                                    break;
                                case 2:
                                    nextPoint.X = 50;
                                    nextPoint.Y = 49 - (Pos.Y % 50);
                                    nextDir = RIGHT;
                                    break;
                                case 3:
                                    nextPoint.X = 50 + (Pos.Y % 50);
                                    nextPoint.Y = 0;
                                    nextDir = DOWN;
                                    break;
                            }
                        }

                        break;

                    case UP:
                        if (nextPoint.Y < 0 || Board[nextPoint.Y][nextPoint.X] == ' ')
                        {
                            switch (nextPoint.X / 50)
                            {
                                case 0:
                                    nextPoint.X = 50;
                                    nextPoint.Y = 50 + (Pos.X % 50);
                                    nextDir = RIGHT;
                                    break;
                                case 1:
                                    nextPoint.X = 0;
                                    nextPoint.Y = 150 + (Pos.X % 50);
                                    nextDir = RIGHT;
                                    break;
                                case 2:
                                    nextPoint.X = (Pos.X % 50);
                                    nextPoint.Y = 199;
                                    nextDir = UP;
                                    break;
                            }
                        }

                        break;
                }

                if (Board[nextPoint.Y][nextPoint.X] == '#')
                {
                    return;
                }

                if (Board[nextPoint.Y][nextPoint.X] == ' ')
                {
                    Console.WriteLine($"Invalid pos!");
                }

                Pos.X = nextPoint.X;
                Pos.Y = nextPoint.Y;
                Dir = nextDir;
            }
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day22.txt");

            CreateBoard(data);

            foreach (var turtleMove in Moves)
            {
                Walk(turtleMove);
            }

            Console.WriteLine($"Final position: Y = {Pos.Y}, X = {Pos.X}, Dir = {Dir}");

            Console.WriteLine($"Answer is {(Pos.Y+1)*1000 + (Pos.X+1)*4 + Dir}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day22.txt");

            CreateBoard(data);

            //TestFold();

            foreach (var turtleMove in Moves)
            {
                WalkCube(turtleMove);
            }

            Console.WriteLine($"Final position: Y = {Pos.Y}, X = {Pos.X}, Dir = {Dir}");

            Console.WriteLine($"Answer is {(Pos.Y + 1) * 1000 + (Pos.X + 1) * 4 + Dir}");
        }

    }
}
