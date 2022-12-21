using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Day20
    {
        private long[] OrigSequence;

        void PrintBoard(long[] values)
        {
            //Console.WriteLine(string.Join(", ", values));
        }

        long IndexOf(long[] values, long v)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == v)
                {
                    return i;
                }
            }

            return -1;
        }

        void Mix(long[] values)
        {
            PrintBoard(values);

            for (int i = 0; i < values.Length; i++)
            {
                long idxToMove = IndexOf(OrigSequence, i);

                long newIdx = (idxToMove + values[idxToMove]) % (values.Length - 1);
                
                if (newIdx < 0)
                {
                    newIdx = newIdx + values.Length - 1;
                }

                /*
                Console.WriteLine($"Moving {values[idxToMove]} (at idx {idxToMove}) to {newIdx}");
                Console.WriteLine();
                */

                if (newIdx > idxToMove)
                {
                    long saveVal = values[idxToMove];

                    for (long j = idxToMove; j < newIdx; j++)
                    {
                        values[j] = values[j + 1];
                        OrigSequence[j] = OrigSequence[j + 1];
                    }

                    values[newIdx] = saveVal;
                    OrigSequence[newIdx] = i;
                }
                else if (newIdx < idxToMove)
                {
                    long saveVal = values[idxToMove];

                    for (long j = idxToMove; j > newIdx; j--)
                    {
                        values[j] = values[j - 1];
                        OrigSequence[j] = OrigSequence[j - 1];
                    }

                    values[newIdx] = saveVal;
                    OrigSequence[newIdx] = i;
                }

                PrintBoard(values);
            }
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day20.txt");

            /*
            var data = new string[]
            {
                "1",
                "2",
                "-3",
                "3",
                "-2",
                "0",
                "4",
            };
            */

            var values = data.Select(r => Int64.Parse(r)).ToArray();
            OrigSequence = Enumerable.Range(0, values.Length).Select(v => (long) v).ToArray();

            Mix(values);

            var zeroPos = IndexOf(values, 0);

            PrintBoard(values);

            long v1 = values[(zeroPos + 1000) % values.Length];
            long v2 = values[(zeroPos + 2000) % values.Length];
            long v3 = values[(zeroPos + 3000) % values.Length];

            Console.WriteLine($"Answer is {v1}+{v2}+{v3} = {v1+v2+v3}");
        }

        public void Part2()
        {
            const long key = 811589153;

            var data = File.ReadAllLines(@"data\day20.txt");

            /*
            var data = new string[]
            {
                "1",
                "2",
                "-3",
                "3",
                "-2",
                "0",
                "4",
            };
            */

            var values = data.Select(r => Int64.Parse(r) * key).ToArray();
            OrigSequence = Enumerable.Range(0, values.Length).Select(v => (long)v).ToArray();

            for (int i = 0; i < 10; i++)
            {
                Mix(values);
            }

            var zeroPos = IndexOf(values, 0);

            PrintBoard(values);

            long v1 = values[(zeroPos + 1000) % values.Length];
            long v2 = values[(zeroPos + 2000) % values.Length];
            long v3 = values[(zeroPos + 3000) % values.Length];

            Console.WriteLine($"Answer is {v1}+{v2}+{v3} = {v1 + v2 + v3}");
        }

    }
}
