using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Day10
    {
        internal int Cycle = 1;
        internal int CurrentX = 1;
        internal int[] XValues = new int[1000];
        internal string Screen = "";

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day10.txt");

            var values = data.Select(r => r.Split(' ')).ToArray();

            foreach (var instr in values)
            {
                switch (instr[0])
                {
                    case "noop":
                        XValues[Cycle] = CurrentX * Cycle;
                        Cycle++;
                        break;
                    case "addx":
                        XValues[Cycle] = CurrentX * Cycle;
                        Cycle++;
                        XValues[Cycle] = CurrentX * Cycle;
                        Cycle++;
                        CurrentX += Int32.Parse(instr[1]);
                        break;
                }
            }

            var sum = XValues[20] + XValues[60] + XValues[100] + XValues[140] + XValues[180] + XValues[220];

            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day10.txt");

            var values = data.Select(r => r.Split(' ')).ToArray();

            Cycle = 1;
            int LinePos;

            foreach (var instr in values)
            {
                LinePos = (Cycle - 1) % 40;

                switch (instr[0])
                {
                    case "noop":
                        if (CurrentX >= LinePos - 1 && CurrentX <= LinePos + 1)
                        {
                            Screen += "#";
                        }
                        else
                        {
                            Screen += ".";
                        }
                        Cycle++;
                        break;
                    case "addx":
                        if (CurrentX >= LinePos - 1 && CurrentX <= LinePos + 1)
                        {
                            Screen += "#";
                        }
                        else
                        {
                            Screen += ".";
                        }
                        Cycle++;
                        LinePos = (LinePos + 1) % 40;
                        if (CurrentX >= LinePos - 1 && CurrentX <= LinePos + 1)
                        {
                            Screen += "#";
                        }
                        else
                        {
                            Screen += ".";
                        }
                        Cycle++;
                        CurrentX += Int32.Parse(instr[1]);
                        break;
                }
            }

            Console.WriteLine($"Answer is:");

            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine(Screen.Substring(i * 40, 40));
            }

        }

    }
}
