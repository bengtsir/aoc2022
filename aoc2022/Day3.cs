using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Day3
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day3.txt");

            var values = data.Select(r => r).ToArray();

            int sum = 0;

            foreach (var value in values)
            {
                var s1 = value.Substring(0, value.Length / 2);
                var s2 = value.Substring(value.Length / 2);

                foreach (var ch in s1)
                {
                    if (s2.Contains(ch))
                    {
                        if (ch >= 'A' && ch <= 'Z')
                        {
                            sum += (ch - 'A') + 27;
                        }
                        else
                        {
                            sum += (ch - 'a') + 1;
                        }

                        break;
                    }
                }
            }

            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day3.txt");

            var values = data.Select(r => r).ToArray();

            int sum = 0;

            while (values.Length > 2)
            {
                var currVal = values.Take(3).ToArray();
                values = values.Skip(3).ToArray();

                foreach (var ch in currVal[0])
                {
                    if (currVal[1].Contains(ch) && currVal[2].Contains(ch))
                    {
                        if (ch >= 'A' && ch <= 'Z')
                        {
                            sum += (ch - 'A') + 27;
                        }
                        else
                        {
                            sum += (ch - 'a') + 1;
                        }

                        break;
                    }
                }
            }

            Console.WriteLine($"Answer is {sum}");
        }

    }
}
