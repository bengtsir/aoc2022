using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class BitRock
    {
        public int Type { get; }
        public byte[] Rep { get; }

        public BitRock(int type)
        {
            Type = type;

            Rep = new byte[4];

            Reset();
        }

        public void Reset()
        {
            switch (Type)
            {
                case 0:
                    Rep[0] = 0b0011110;
                    break;
                case 1:
                    Rep[0] = 0b0001000;
                    Rep[1] = 0b0011100;
                    Rep[2] = 0b0001000;
                    break;
                case 2:
                    Rep[0] = 0b0011100;
                    Rep[1] = 0b0000100;
                    Rep[2] = 0b0000100;
                    break;
                case 3:
                    Rep[0] = 0b0010000;
                    Rep[1] = 0b0010000;
                    Rep[2] = 0b0010000;
                    Rep[3] = 0b0010000;
                    break;
                case 4:
                    Rep[0] = 0b0011000;
                    Rep[1] = 0b0011000;
                    break;
            }
        }

        public void Nudge(char dir, byte[] cave, int ypos)
        {
            if (dir == '<')
            {
                if (((Rep[0] | Rep[1] | Rep[2] | Rep[3]) & 0b1000000) != 0)
                {
                    return;
                }
                for (int i = 0; i < 4; i++)
                {
                    if (((Rep[i] << 1) & cave[ypos + i]) != 0)
                    {
                        return;
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    Rep[i] <<= 1;
                }
            }
            else
            {
                if (((Rep[0] | Rep[1] | Rep[2] | Rep[3]) & 0b0000001) != 0)
                {
                    return;
                }
                for (int i = 0; i < 4; i++)
                {
                    if (((Rep[i] >> 1) & cave[ypos + i]) != 0)
                    {
                        return;
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    Rep[i] >>= 1;
                }
            }
        }

        public bool DropTest(byte[] cave, int ypos)
        {
            for (int i = 0; i < 4; i++)
            {
                if ((Rep[i] & cave[ypos + i]) != 0)
                {
                    return false;
                }
            }
            return true;
        }
    }

    internal class Day17
    {
        internal char[] Pushes;
        internal int pushIdx = 0;

        internal const int MaxHeight = 100000;

        internal char NextPush()
        {
            var c = Pushes[pushIdx++];

            if (pushIdx >= Pushes.Length)
            {
                pushIdx = 0;
            }

            return c;
        }

        private byte[] Cave = new byte[MaxHeight];

        internal void BuildCave()
        {
            Cave[0] = 0b1111111; // Floor
        }

        internal int CaveHeight = 1;

        internal void UpdateHeight()
        {
            while (Cave[CaveHeight] != 0)
            {
                CaveHeight++;
            }
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

            var rocks = new BitRock[5];

            for (int i = 0; i < 5; i++)
            {
                rocks[i] = new BitRock(i);
            }

            for (int i = 0; i < 2022; i++)
            {
                var r = rocks[rockType];
                r.Reset();

                rockType = (rockType + 1) % 5;

                var ypos = CaveHeight + 3;

                r.Nudge(NextPush(), Cave, ypos);
                while (r.DropTest(Cave, ypos - 1))
                {
                    ypos--;
                    r.Nudge(NextPush(), Cave, ypos);
                }

                for (int k = 0; k < 4; k++)
                {
                    Cave[ypos + k] |= r.Rep[k];
                }

                UpdateHeight();
            }

            Console.WriteLine($"Answer is {CaveHeight - 1}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day17.txt");

            /*
            var data = new string[]
            {
                ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
            };
            */

            Pushes = data.Select(r => r.Select(c => c).ToArray()).First().ToArray();

            var periods = 5;

            var maxPeriod = Pushes.Length * 5;

            var caveSize = periods * 4 * maxPeriod + 1000;

            var rocksToDrop = periods * maxPeriod + 10;

            Cave = new byte[caveSize];

            var heightDiff = new byte[rocksToDrop];

            BuildCave();

            int rockType = 0;

            var rocks = new BitRock[5];

            for (int i = 0; i < 5; i++)
            {
                rocks[i] = new BitRock(i);
            }

            Console.WriteLine($"maxPeriod: {maxPeriod}, caveSize: {caveSize}");

            for (int i = 0; i < rocksToDrop; i++)
            {
                var r = rocks[rockType];
                r.Reset();

                rockType = (rockType + 1) % 5;

                var ypos = CaveHeight + 3;

                r.Nudge(NextPush(), Cave, ypos);
                while (r.DropTest(Cave, ypos - 1))
                {
                    ypos--;
                    r.Nudge(NextPush(), Cave, ypos);
                }

                for (int k = 0; k < 4; k++)
                {
                    Cave[ypos + k] |= r.Rep[k];
                }

                var oldHeight = CaveHeight;
                UpdateHeight();

                heightDiff[i] = (byte)(CaveHeight - oldHeight);

                if (CaveHeight > caveSize - 10)
                {
                    Console.WriteLine($"Max height reached at rock {i}");
                    break;
                }
            }

            int junk = maxPeriod;

            var period = 0;

            for (int stride = 100; stride < maxPeriod + 2; stride++)
            {
                int hd = heightDiff[junk];
                var found = true;

                for (int i = 0; i < stride; i++)
                {
                    if (heightDiff[junk + i] != heightDiff[stride + junk + i])
                    {
                        found = false;
                        break;
                    }
                    if (heightDiff[junk + i] != heightDiff[stride + stride + junk + i])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    Console.WriteLine($"Found period at stride = {stride}");
                    period = stride;
                    break;
                }
            }

            var initialSum = heightDiff.Take(junk).Select(b => (long)b).Sum();
            var periodSum = heightDiff.Skip(junk).Take(period).Select(b => (long)b).Sum();
            var calcPeriods = (1_000_000_000_000 - junk) / period;
            var restLength = (1_000_000_000_000 - junk) % period;
            var restSum = heightDiff.Skip(junk + period).Take((int)restLength).Select(b => (long)b).Sum();

            Console.WriteLine($"Answer is {initialSum} + {calcPeriods}*{periodSum} + {restSum} = {initialSum + calcPeriods * periodSum + restSum}");
        }
    }
}
