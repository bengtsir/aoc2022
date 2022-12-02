using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Day2
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day2.txt");

            var values = data.Select(r => r.Split(' ').ToArray()).ToArray();

            var score = 0;

            foreach (var row in values)
            {
                switch (row[1])
                {
                    case "X":
                        score += 1;
                        if (row[0] == "A")
                        {
                            score += 3;
                        }
                        else if (row[0] == "C")
                        {
                            score += 6;
                        }
                        break;
                    case "Y":
                        score += 2;
                        if (row[0] == "B")
                        {
                            score += 3;
                        }
                        else if (row[0] == "A")
                        {
                            score += 6;
                        }
                        break;
                    case "Z":
                        score += 3;
                        if (row[0] == "C")
                        {
                            score += 3;
                        }
                        else if (row[0] == "B")
                        {
                            score += 6;
                        }
                        break;
                }
            }

            Console.WriteLine($"Answer is {score}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day2.txt");

            var values = data.Select(r => r.Split(' ').ToArray()).ToArray();

            var score = 0;

            foreach (var row in values)
            {
                switch (row[1])
                {
                    case "X":
                        score += 0;
                        if (row[0] == "A")
                        {
                            score += 3;
                        }
                        else if (row[0] == "B")
                        {
                            score += 1;
                        }
                        else if (row[0] == "C")
                        {
                            score += 2;
                        }
                        break;
                    case "Y":
                        score += 3;
                        if (row[0] == "A")
                        {
                            score += 1;
                        }
                        else if (row[0] == "B")
                        {
                            score += 2;
                        }
                        else if (row[0] == "C")
                        {
                            score += 3;
                        }
                        break;
                    case "Z":
                        score += 6;
                        if (row[0] == "A")
                        {
                            score += 2;
                        }
                        else if (row[0] == "B")
                        {
                            score += 3;
                        }
                        else if (row[0] == "C")
                        {
                            score += 1;
                        }
                        break;
                }
            }

            Console.WriteLine($"Answer is {score}");
        }

    }
}
