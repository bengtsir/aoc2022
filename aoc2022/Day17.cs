using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace aoc2022
{
    internal class Rock
    {
        // -, +, L, I, o
        public int Type { get; set; }

        public int XPos { get; set; }

        public int YPos { get; set; } // Bottom of rock

        public Rock(int type, int xPos, int yPos)
        {
            Type = type;
            XPos = xPos;
            YPos = yPos;
        }

        public void Nudge(char dir, int[][] cave)
        {
            if (dir == '>')
            {
                NudgeRight(cave);
            }
            else if (dir == '<')
            {
                NudgeLeft(cave);
            }
            else
            {
                throw new Exception("Invalid nudge");
            }
        }

        public void NudgeLeft(int[][] cave)
        {
            if (XPos == 0)
            {
                return;
            }

            switch (Type)
            {
                case 0:
                    if (cave[XPos - 1][YPos] == 0)
                    {
                        XPos--;
                    }

                    break;
                case 1:
                    if (cave[XPos][YPos] == 0 &&
                        cave[XPos - 1][YPos + 1] == 0 &&
                        cave[XPos][YPos + 2] == 0)
                    {
                        XPos--;
                    }

                    break;
                case 2:
                    if (cave[XPos-1][YPos] == 0 &&
                        cave[XPos+1][YPos + 1] == 0 &&
                        cave[XPos+1][YPos + 2] == 0)
                    {
                        XPos--;
                    }

                    break;
                case 3:
                    if (cave[XPos - 1][YPos] == 0 &&
                        cave[XPos - 1][YPos + 1] == 0 &&
                        cave[XPos - 1][YPos + 2] == 0 &&
                        cave[XPos - 1][YPos + 3] == 0)
                    {
                        XPos--;
                    }

                    break;
                case 4:
                    if (cave[XPos - 1][YPos] == 0 &&
                        cave[XPos - 1][YPos + 1] == 0)
                    {
                        XPos--;
                    }

                    break;
            }
        }

        public void NudgeRight(int[][] cave)
        {
            switch (Type)
            {
                case 0:
                    if (XPos >= 3)
                    {
                        return;
                    }

                    if (cave[XPos + 4][YPos] == 0)
                    {
                        XPos++;
                    }

                    break;
                case 1:
                    if (XPos >= 4)
                    {
                        return;
                    }

                    if (cave[XPos + 2][YPos] == 0 &&
                        cave[XPos + 3][YPos + 1] == 0 &&
                        cave[XPos + 2][YPos + 2] == 0)
                    {
                        XPos++;
                    }

                    break;
                case 2:
                    if (XPos >= 4)
                    {
                        return;
                    }

                    if (cave[XPos + 3][YPos] == 0 &&
                        cave[XPos + 3][YPos + 1] == 0 &&
                        cave[XPos + 3][YPos + 2] == 0)
                    {
                        XPos++;
                    }

                    break;
                case 3:
                    if (XPos >= 6)
                    {
                        return;
                    }

                    if (cave[XPos + 1][YPos] == 0 &&
                        cave[XPos + 1][YPos + 1] == 0 &&
                        cave[XPos + 1][YPos + 2] == 0 &&
                        cave[XPos + 1][YPos + 3] == 0)
                    {
                        XPos++;
                    }

                    break;
                case 4:
                    if (XPos >= 5)
                    {
                        return;
                    }

                    if (cave[XPos + 2][YPos] == 0 &&
                        cave[XPos + 2][YPos + 1] == 0)
                    {
                        XPos++;
                    }

                    break;
            }
        }

        public bool Drop(int[][] cave)
        {
            switch (Type)
            {
                case 0:
                    if (cave[XPos][YPos - 1] == 0 &&
                        cave[XPos + 1][YPos - 1] == 0 &&
                        cave[XPos + 2][YPos - 1] == 0 &&
                        cave[XPos + 3][YPos - 1] == 0)
                    {
                        YPos--;
                    }
                    else
                    {
                        cave[XPos][YPos] = 1;
                        cave[XPos + 1][YPos] = 1;
                        cave[XPos + 2][YPos] = 1;
                        cave[XPos + 3][YPos] = 1;
                        return true;
                    }

                    break;
                case 1:
                    if (cave[XPos][YPos] == 0 &&
                        cave[XPos + 1][YPos - 1] == 0 &&
                        cave[XPos + 2][YPos] == 0)
                    {
                        YPos--;
                    }
                    else
                    {
                        cave[XPos][YPos + 1] = 1;
                        cave[XPos + 1][YPos] = 1;
                        cave[XPos + 1][YPos + 1] = 1;
                        cave[XPos + 1][YPos + 2] = 1;
                        cave[XPos + 2][YPos + 1] = 1;
                        return true;
                    }

                    break;
                case 2:
                    if (cave[XPos][YPos - 1] == 0 &&
                        cave[XPos + 1][YPos - 1] == 0 &&
                        cave[XPos + 2][YPos - 1] == 0)
                    {
                        YPos--;
                    }
                    else
                    {
                        cave[XPos][YPos] = 1;
                        cave[XPos + 1][YPos] = 1;
                        cave[XPos + 2][YPos] = 1;
                        cave[XPos + 2][YPos + 1] = 1;
                        cave[XPos + 2][YPos + 2] = 1;
                        return true;
                    }

                    break;
                case 3:
                    if (cave[XPos][YPos - 1] == 0)
                    {
                        YPos--;
                    }
                    else
                    {
                        cave[XPos][YPos] = 1;
                        cave[XPos][YPos + 1] = 1;
                        cave[XPos][YPos + 2] = 1;
                        cave[XPos][YPos + 3] = 1;
                        return true;
                    }

                    break;
                case 4:
                    if (cave[XPos][YPos - 1] == 0 &&
                        cave[XPos + 1][YPos - 1] == 0)
                    {
                        YPos--;
                    }
                    else
                    {
                        cave[XPos][YPos] = 1;
                        cave[XPos][YPos + 1] = 1;
                        cave[XPos + 1][YPos] = 1;
                        cave[XPos + 1][YPos + 1] = 1;
                        return true;
                    }

                    break;
            }

            return false;
        }
    }


    internal class Day17
    {
        internal char[] Pushes;
        internal int pushIdx = 0;

        internal const int MaxHeight = 100000;

        internal long Reductions = 0;

        internal char NextPush()
        {
            var c = Pushes[pushIdx++];

            if (pushIdx >= Pushes.Length)
            {
                pushIdx = 0;
            }

            return c;
        }

        private int[][] Cave = new int[7][];

        internal void BuildCave()
        {
            for (int i = 0; i < 7; i++)
            {
                Cave[i] = new int[MaxHeight];
                Cave[i][0] = 1; // Floor
            }
        }

        internal int CaveHeight()
        {
            for (int i = MaxHeight - 1; i >= 0; i--)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (Cave[j][i] != 0)
                    {
                        return i + 1;
                    }
                }
            }

            return 0;
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day17.txt");

            /*
            var data = new string[]
            {
                ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
            };
            */

            Pushes = data.Select(r => r.Select(c => c).ToArray()).First().ToArray();

            BuildCave();

            int rockType = 0;

            for (int i = 0; i < 2022; i++)
            {
                var h = CaveHeight();
                var r = new Rock(rockType, 2, h + 3);
                rockType = (rockType + 1) % 5;

                r.Nudge(NextPush(), Cave);
                while (!r.Drop(Cave))
                {
                    r.Nudge(NextPush(), Cave);
                }
            }

            Console.WriteLine($"Answer is {CaveHeight()-1}");
        }

        public void Part2()
        {
            //var data = File.ReadAllLines(@"data\day17.txt");

            
            var data = new string[]
            {
                ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
            };
            

            Pushes = data.Select(r => r.Select(c => c).ToArray()).First().ToArray();

            BuildCave();

            const int MaxIter = 16384;

            double[] HeightAdd = new double[MaxIter];
            int[] Heights = new int[MaxIter];

            int rockType = 0;

            for (long i = 0; i < MaxIter; i++)
            {
                var h = CaveHeight();
                var r = new Rock(rockType, 2, h + 3);
                rockType = (rockType + 1) % 5;

                r.Nudge(NextPush(), Cave);
                while (!r.Drop(Cave))
                {
                    r.Nudge(NextPush(), Cave);
                }

                HeightAdd[i] = CaveHeight() - h;
                Heights[i] = h;

                if (h >= MaxHeight - 20)
                {
                    break;
                }
            }

            for (int i = 0; i < MaxIter; i++)
            {
                Console.Write($"{HeightAdd[i]}, ");
            }
            Console.WriteLine();

            double[] fftMag = FftSharp.Transform.FFTmagnitude(HeightAdd);

            for (int i = 0; i < fftMag.Length; i++)
            {
                Console.WriteLine($"{i}: {fftMag[i]}");
                if (i % 100 == 0)
                {
                    var x = 0;
                }
            }

            Console.WriteLine("---------------");

            double maxPeak = 0;
            int maxPeakIdx = 0;

            for (int i = 1; i < fftMag.Length - 1; i++)
            {
                if (fftMag[i-1] < fftMag[i] && fftMag[i] > fftMag[i+1])
                {
                    Console.WriteLine($"Peak at {i} with mag {fftMag[i]}");
                    if (fftMag[i] > maxPeak)
                    {
                        maxPeak = fftMag[i];
                        maxPeakIdx = i;
                    }
                }
            }

            long totHeight;

            const long totIter = 1_000_000_000_000;

            totHeight = (totIter / maxPeakIdx) * Heights[maxPeakIdx] +
                        Heights[totIter % maxPeakIdx];
            Console.WriteLine($"Answer is {totHeight}");

            totHeight = (totIter / maxPeakIdx) * Heights[maxPeakIdx-1] +
                        Heights[totIter % maxPeakIdx];
            Console.WriteLine($"Answer is {totHeight}");

            totHeight = ((totIter / maxPeakIdx)-1) * Heights[maxPeakIdx] +
                        Heights[totIter % maxPeakIdx];
            Console.WriteLine($"Answer is {totHeight}");

            totHeight = ((totIter / maxPeakIdx)-1) * Heights[maxPeakIdx-1] +
                        Heights[totIter % maxPeakIdx];
            Console.WriteLine($"Answer is {totHeight}");

            totHeight = (totIter / maxPeakIdx) * Heights[maxPeakIdx] +
                        Heights[totIter % maxPeakIdx];
            Console.WriteLine($"Answer is {totHeight}");

            var periodSum = (long)(HeightAdd.Skip(maxPeakIdx).Take(maxPeakIdx).Sum());

            var rest = totIter % maxPeakIdx;

            totHeight = ((long)HeightAdd.Take(maxPeakIdx).Sum()) + ((totIter-1)*periodSum) + (long)(HeightAdd.Skip(2*maxPeakIdx).Take((int)rest).Sum());
            Console.WriteLine($"Answer is {totHeight}");

            totHeight = ((long)HeightAdd.Take(maxPeakIdx).Sum()) + ((totIter / maxPeakIdx - 2) * periodSum) + (long)(HeightAdd.Skip(2 * maxPeakIdx).Take((int)rest).Sum());
            Console.WriteLine($"Answer is {totHeight}");

            totHeight = ((long)HeightAdd.Take(maxPeakIdx).Sum()) + ((totIter / maxPeakIdx - 1) * periodSum) + (long)(HeightAdd.Skip(2 * maxPeakIdx).Take((int)rest).Sum());
            Console.WriteLine($"Answer is {totHeight}");
        }

    }
}
