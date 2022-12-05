using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Day4
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day4.txt");

            var values = data.Select(r =>
                r.Split(',').Select(t => t.Split('-').Select(Int32.Parse).ToArray()).ToArray()).ToArray();

            var count = 0;

            foreach (var tuple in values)
            {
                if ((tuple[0][0] >= tuple[1][0] && tuple[0][1] <= tuple[1][1]) ||
                    (tuple[1][0] >= tuple[0][0] && tuple[1][1] <= tuple[0][1]))
                {
                    count++;
                }
            }

            Console.WriteLine($"Answer is {count}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day4.txt");

            var values = data.Select(r =>
                r.Split(',').Select(t => t.Split('-').Select(Int32.Parse).ToArray()).ToArray()).ToArray();

            var count = 0;

            foreach (var tuple in values)
            {
                if (((tuple[0][0] >= tuple[1][0] && tuple[0][0] <= tuple[1][1]) ||
                    (tuple[0][1] >= tuple[1][0] && tuple[0][1] <= tuple[1][1])) ||
                    ((tuple[1][0] >= tuple[0][0] && tuple[1][0] <= tuple[0][1]) ||
                     (tuple[1][1] >= tuple[0][0] && tuple[1][1] <= tuple[0][1])))
                {
                    count++;
                }
            }

            Console.WriteLine($"Answer is {count}");
        }

    }
}
