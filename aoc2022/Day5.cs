using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Day5
    {
        private string[] stacks = new string[]
        {
            "", // Dummy for 1-based idx
            "TVJWNRMS",
            "VCPQJDWB",
            "PRDHFJB",
            "DNMBPRF",
            "BTPRVH",
            "TPBC",
            "LPRJB",
            "WBZTLSCN",
            "GSL",
        };

        private void PrintBoard()
        {
            for (int i = 1; i <= 9; i++)
            {
                Console.WriteLine($"{i}: {stacks[i]}");
            }
            Console.WriteLine();
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day5.txt");

            var moves = data.Skip(10).Select(s => s.Split(' ')).ToArray();

            PrintBoard();

            foreach (var move in moves)
            {
                var count = Int32.Parse(move[1]);
                var from = Int32.Parse(move[3]);
                var to = Int32.Parse(move[5]);

                for (int i = 0; i < count; i++)
                {
                    stacks[to] = stacks[from][0] + stacks[to];
                    stacks[from] = stacks[from].Substring(1);
                }

                Console.WriteLine($"{count} from {from} to {to}");
                PrintBoard();
            }

            Console.WriteLine($"Answer is {new string(stacks.Skip(1).Select(s => s[0]).ToArray())}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day5.txt");

            var moves = data.Skip(10).Select(s => s.Split(' ')).ToArray();

            PrintBoard();

            foreach (var move in moves)
            {
                var count = Int32.Parse(move[1]);
                var from = Int32.Parse(move[3]);
                var to = Int32.Parse(move[5]);

                stacks[to] = stacks[from].Substring(0, count) + stacks[to];
                stacks[from] = stacks[from].Substring(count);

                Console.WriteLine($"{count} from {from} to {to}");
                PrintBoard();
            }

            Console.WriteLine($"Answer is {new string(stacks.Skip(1).Select(s => s[0]).ToArray())}");
        }

    }
}
