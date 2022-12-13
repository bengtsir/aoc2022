using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Monkey
    {
        public int MonkeyNo { get; set; }
        public List<long> Items { get; } = new List<long>();
        public string Operation { get; set; } = "";
        public int OperationOperand { get; set; }
        public int TestDivisible { get; set; }
        public int ThrowToFalse { get; set; }
        public int ThrowToTrue { get; set; }

        public long NumberOfInspections { get; set; } = 0;
    }

    internal class Day11
    {
        internal int NMonkeys = 8;
        internal Monkey[] Monkeys = new Monkey[8];

        void ParseMonkey(string[] def)
        {
            var mno = Int32.Parse(def[0].Split(' ')[1].Substring(0, 1));

            var m = new Monkey();

            m.MonkeyNo = mno;
            m.Items.AddRange(def[1].Split(':')[1].Split(',').Select(Int64.Parse));
            m.Operation = def[2].Contains("old * old") ? "square" : (def[2].Contains('*') ? "mul" : "add");
            if (m.Operation != "square")
            {
                m.OperationOperand = Int32.Parse(def[2].Split(' ').Last());
            }
            m.TestDivisible = Int32.Parse(def[3].Split(' ').Last());
            m.ThrowToTrue = Int32.Parse(def[4].Split(' ').Last());
            m.ThrowToFalse = Int32.Parse(def[5].Split(' ').Last());

            Monkeys[mno] = m;
        }

        public void Iterate(int mno)
        {
            Monkey m = Monkeys[mno];

            while (m.Items.Count > 0)
            {
                var item = m.Items.First();
                m.Items.RemoveAt(0);
                m.NumberOfInspections++;

                switch (m.Operation)
                {
                    case "square":
                        item *= item;
                        break;
                    case "add":
                        item += m.OperationOperand;
                        break;
                    case "mul":
                        item *= m.OperationOperand;
                        break;
                }

                item /= 3;

                if ((item % m.TestDivisible) == 0)
                {
                    Monkeys[m.ThrowToTrue].Items.Add(item);
                }
                else
                {
                    Monkeys[m.ThrowToFalse].Items.Add(item);
                }
            }
        }

        internal long Ceiling;

        public void IteratePart2(int mno)
        {
            Monkey m = Monkeys[mno];

            while (m.Items.Count > 0)
            {
                var item = m.Items.First();
                m.Items.RemoveAt(0);
                m.NumberOfInspections++;

                switch (m.Operation)
                {
                    case "square":
                        item *= item;
                        break;
                    case "add":
                        item += m.OperationOperand;
                        break;
                    case "mul":
                        item *= m.OperationOperand;
                        break;
                }

                item %= Ceiling;

                if ((item % m.TestDivisible) == 0)
                {
                    Monkeys[m.ThrowToTrue].Items.Add(item);
                }
                else
                {
                    Monkeys[m.ThrowToFalse].Items.Add(item);
                }
            }
        }

        public void PrintMonkeys()
        {
            for (int i = 0; i < NMonkeys; i++)
            {
                var items = string.Join(", ", Monkeys[i].Items.Select(v => v.ToString()).ToArray());
                Console.WriteLine($"Monkey {Monkeys[i].MonkeyNo}: {items}");
            }

            Console.WriteLine();
        }

        public void PrintInspections()
        {
            for (int i = 0; i < NMonkeys; i++)
            {
                Console.WriteLine($"Monkey {i} inspected items {Monkeys[i].NumberOfInspections} times.");
            }
            Console.WriteLine();
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day11.txt");
            
            /*
            var data = new string[]
            {
                "Monkey 0:",
                "Starting items: 79, 98",
                "Operation: new = old * 19",
                "Test: divisible by 23",
                "If true: throw to monkey 2",
                "If false: throw to monkey 3",
                "",
                "Monkey 1:",
                "Starting items: 54, 65, 75, 74",
                "Operation: new = old + 6",
                "Test: divisible by 19",
                "If true: throw to monkey 2",
                "If false: throw to monkey 0",
                "",
                "Monkey 2:",
                "Starting items: 79, 60, 97",
                "Operation: new = old * old",
                "Test: divisible by 13",
                "If true: throw to monkey 1",
                "If false: throw to monkey 3",
                "",
                "Monkey 3:",
                "Starting items: 74",
                "Operation: new = old + 3",
                "Test: divisible by 17",
                "If true: throw to monkey 0",
                "If false: throw to monkey 1",
            };
            NMonkeys = 4;
            */

            for (int i = 0; i < NMonkeys; i++)
            {
                ParseMonkey(data.Skip(i * 7).ToArray());
            }

            PrintMonkeys();

            for (int i = 0; i < 20; i++)
            {
                for (int m = 0; m < NMonkeys; m++)
                {
                    Iterate(m);
                }
                PrintMonkeys();
            }

            var inspections = Monkeys.Take(NMonkeys).Select(m => m.NumberOfInspections).OrderBy(i => i).ToList();
            inspections.Reverse();

            Console.WriteLine($"Answer is {inspections[0] * inspections[1]}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day11.txt");

            /*
            var data = new string[]
            {
                "Monkey 0:",
                "Starting items: 79, 98",
                "Operation: new = old * 19",
                "Test: divisible by 23",
                "If true: throw to monkey 2",
                "If false: throw to monkey 3",
                "",
                "Monkey 1:",
                "Starting items: 54, 65, 75, 74",
                "Operation: new = old + 6",
                "Test: divisible by 19",
                "If true: throw to monkey 2",
                "If false: throw to monkey 0",
                "",
                "Monkey 2:",
                "Starting items: 79, 60, 97",
                "Operation: new = old * old",
                "Test: divisible by 13",
                "If true: throw to monkey 1",
                "If false: throw to monkey 3",
                "",
                "Monkey 3:",
                "Starting items: 74",
                "Operation: new = old + 3",
                "Test: divisible by 17",
                "If true: throw to monkey 0",
                "If false: throw to monkey 1",
            };
            NMonkeys = 4;
            */

            for (int i = 0; i < NMonkeys; i++)
            {
                ParseMonkey(data.Skip(i * 7).ToArray());
            }

            Ceiling = Monkeys.Take(NMonkeys).Select(m => m.TestDivisible).Aggregate(1, (acc, x) => acc * x);

            int[] printRounds = new[]
            {
                1,
                20,
                1000,
                2000,
                3000,
                4000,
                5000,
                6000,
                7000,
                8000,
                9000,
                10000
            };

            for (int i = 0; i < 10000; i++)
            {
                for (int m = 0; m < NMonkeys; m++)
                {
                    IteratePart2(m);
                }

                if (printRounds.Contains(i + 1))
                {
                    Console.WriteLine($"After round {i + 1}");
                    PrintInspections();
                }
            }

            var inspections = Monkeys.Take(NMonkeys).Select(m => m.NumberOfInspections).OrderBy(i => i).ToList();
            inspections.Reverse();

            Console.WriteLine($"Answer is {inspections[0] * inspections[1]}");
        }

    }
}
