﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Day14
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day14.txt");

            var values = data.Select(r => r.Select(c => Int32.Parse(c.ToString())).ToArray()).ToArray();

            Console.WriteLine($"Answer is {42}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day14.txt");

            var values = data.Select(r => r.Select(c => Int32.Parse(c.ToString())).ToArray()).ToArray();

            Console.WriteLine($"Answer is {42}");
        }

    }
}
