using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Day1
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day1.txt");

            List<int> cals = new List<int>();

            var values = data.Select(r => r.Length == 0 ? -1 : Int32.Parse(r)).ToArray();

            var acc = 0;
            foreach (var v in values)
            {
                if (v < 0 && acc > 0)
                {
                    cals.Add(acc);
                    acc = 0;
                }
                else
                {
                    acc += v;
                }
            }

            if (acc > 0)
            {
                cals.Add(acc);
            }

            Console.WriteLine($"Answer is {cals.Max()}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day1.txt");

            List<int> cals = new List<int>();

            var values = data.Select(r => r.Length == 0 ? -1 : Int32.Parse(r)).ToArray();

            var acc = 0;
            foreach (var v in values)
            {
                if (v < 0 && acc > 0)
                {
                    cals.Add(acc);
                    acc = 0;
                }
                else
                {
                    acc += v;
                }
            }

            if (acc > 0)
            {
                cals.Add(acc);
            }

            cals.Sort();
            cals.Reverse();

            Console.WriteLine($"Answer is {cals.Take(3).Sum()}");
        }

    }
}
