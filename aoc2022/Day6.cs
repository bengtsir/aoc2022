using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Day6
    {
        bool AllDiff(string s)
        {
            while (s.Length > 0)
            {
                var c = s[0];
                s = s.Substring(1);

                if (s.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day6.txt");

            var values = data[0];

            var tail = values.Substring(0, 4);
            values = values.Substring(4);

            var chars = 4;

            while (values.Length > 0 && !AllDiff(tail))
            {
                chars++;
                tail = tail.Substring(1) + values[0];
                values = values.Substring(1);
            }

            Console.WriteLine($"Answer is {chars}, code is {tail}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day6.txt");

            var values = data[0];

            var tail = values.Substring(0, 14);
            values = values.Substring(14);

            var chars = 14;

            while (values.Length > 0 && !AllDiff(tail))
            {
                chars++;
                tail = tail.Substring(1) + values[0];
                values = values.Substring(1);
            }

            Console.WriteLine($"Answer is {chars}, code is {tail}");
        }

    }
}
