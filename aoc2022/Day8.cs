using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Day8
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day8.txt");

            var values = data.Select(r => r.Select(c => Int32.Parse(c.ToString())).ToArray()).ToArray();

            var visible = values.Select(r => r.Select(c => false).ToArray()).ToArray();

            for (int r = 0; r < values.Length; r++)
            {
                for (int k = 0; k < values[0].Length; k++)
                {
                    if (k == 0 || values[r].Take(k).All(val => val < values[r][k]))
                    {
                        visible[r][k] = true;
                    }

                    if (k == values[0].Length - 1 || values[r].Skip(k+1).All(val => val < values[r][k]))
                    {
                        visible[r][k] = true;
                    }

                    var sliceAbove = values.Take(r).Select(row => row.Skip(k).First()).ToArray();
                    var sliceBelow = values.Skip(r + 1).Select(row => row.Skip(k).First()).ToArray();

                    if (r == 0 || sliceAbove.All(val => val < values[r][k]))
                    {
                        visible[r][k] = true;
                    }

                    if (r == values.Length - 1 || sliceBelow.All(val => val < values[r][k]))
                    {
                        visible[r][k] = true;
                    }
                }
            }

            var sum = visible.Select(r => r.Sum(v => v ? 1 : 0)).Sum();

            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day8.txt");

            var values = data.Select(r => r.Select(c => Int32.Parse(c.ToString())).ToArray()).ToArray();

            var score = values.Select(r => r.Select(c => (int) 0).ToArray()).ToArray();

            for (int r = 0; r < values.Length; r++)
            {
                for (int k = 0; k < values[0].Length; k++)
                {
                    int up = 0;
                    int right = 0;
                    int down = 0;
                    int left = 0;

                    int rr, kk;

                    // Left

                    rr = r;
                    kk = k - 1;

                    while (kk >= 0 && values[rr][kk] < values[r][k])
                    {
                        left++;
                        kk--;
                    }
                    
                    if (kk >= 0)
                    {
                        left++;
                    }

                    // Right

                    rr = r;
                    kk = k + 1;

                    while (kk < values[0].Length && values[rr][kk] < values[r][k])
                    {
                        right++;
                        kk++;
                    }

                    if (kk < values[0].Length)
                    {
                        right++;
                    }

                    // Up

                    rr = r - 1;
                    kk = k;

                    while (rr >= 0 && values[rr][kk] < values[r][k])
                    {
                        up++;
                        rr--;
                    }

                    if (rr >= 0)
                    {
                        up++;
                    }

                    // Down

                    rr = r + 1;
                    kk = k;

                    while (rr < values.Length && values[rr][kk] < values[r][k])
                    {
                        down++;
                        rr++;
                    }

                    if (rr < values.Length)
                    {
                        down++;
                    }

                    score[r][k] = up * down * left * right;
                }
            }

            /*
            foreach (var row in score)
            {
                foreach (var val in row)
                {
                    Console.Write($"{val} ");
                }
                Console.WriteLine();
            }
            */
            
            var largest = score.Max(r => r.Max());
            
            Console.WriteLine($"Answer is {largest}");
        }

    }
}
