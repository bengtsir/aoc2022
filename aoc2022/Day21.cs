using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class MonkeyCalculator
    {
        public string Name { get; set; }
        public long Value { get; set; } = -1;
        public bool IsConstant { get; set; } = false;
        public string Op1 { get; set; }
        public string Op2 { get; set; }
        public char Op { get; set; }
        public bool Done { get; set; }
        public bool IsRoot => Name == "root";
        public bool IsHuman => Name == "humn";

        public void Reset()
        {
            Done = IsConstant;
        }
    }

    internal class Day21
    {
        public Dictionary<string, MonkeyCalculator> Monkeys = new Dictionary<string, MonkeyCalculator>();

        internal void Parse(string[] lines)
        {
            var r1 = @"(....): ([\d]+)$";
            var r2 = @"(....): (....) (.) (....)$";

            foreach (var line in lines)
            {
                MonkeyCalculator m;

                var m1 = Regex.Match(line, r1);
                var m2 = Regex.Match(line, r2);

                if (m2.Groups.Count == 5)
                {
                    m = new MonkeyCalculator()
                    {
                        Name = m2.Groups[1].Value,
                        Op1 = m2.Groups[2].Value,
                        Op2 = m2.Groups[4].Value,
                        Op = m2.Groups[3].Value[0],
                        IsConstant = false,
                        Done = false
                    };
                }
                else
                {
                    m = new MonkeyCalculator()
                    {
                        Name = m1.Groups[1].Value,
                        Value = Int64.Parse(m1.Groups[2].Value),
                        IsConstant = true,
                        Done = true
                    };
                }

                Monkeys[m.Name] = m;
            }
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day21.txt");

            /*
            var data = new string[]
            {
                "root: pppw + sjmn",
                "dbpl: 5",
                "cczh: sllz + lgvd",
                "zczc: 2",
                "ptdq: humn - dvpt",
                "dvpt: 3",
                "lfqf: 4",
                "humn: 5",
                "ljgn: 2",
                "sjmn: drzm * dbpl",
                "sllz: 4",
                "pppw: cczh / lfqf",
                "lgvd: ljgn * ptdq",
                "drzm: hmdt - zczc",
                "hmdt: 32",
            };
            */

            Parse(data);

            while (Monkeys.Values.Any(m => !m.Done))
            {
                foreach (var monkey in Monkeys.Values.Where(mm => !mm.Done))
                {
                    if (Monkeys[monkey.Op1].Done && Monkeys[monkey.Op2].Done)
                    {
                        long v1 = Monkeys[monkey.Op1].Value;
                        long v2 = Monkeys[monkey.Op2].Value;

                        switch (monkey.Op)
                        {
                            case '+':
                                monkey.Value = v1 + v2;
                                break;
                            case '-':
                                monkey.Value = v1 - v2;
                                break;
                            case '*':
                                monkey.Value = v1 * v2;
                                break;
                            case '/':
                                monkey.Value = v1 / v2;
                                break;
                        }

                        monkey.Done = true;
                    }
                }
            }

            Console.WriteLine($"Answer is {Monkeys["root"].Value}");
        }

        public bool SubtreeContainsHuman(string node, bool calc = false)
        {
            var m = Monkeys[node];

            if (m.IsHuman)
            {
                return true;
            }

            if (m.IsConstant)
            {
                return false;
            }

            if (calc)
            {
                var s1 = SubtreeContainsHuman(m.Op1);
                var s2 = SubtreeContainsHuman(m.Op2);

                if (Monkeys[m.Op1].Done && Monkeys[m.Op2].Done)
                {
                    long v1 = Monkeys[m.Op1].Value;
                    long v2 = Monkeys[m.Op2].Value;

                    switch (m.Op)
                    {
                        case '+':
                            m.Value = v1 + v2;
                            break;
                        case '-':
                            m.Value = v1 - v2;
                            break;
                        case '*':
                            m.Value = v1 * v2;
                            break;
                        case '/':
                            m.Value = v1 / v2;
                            break;
                    }

                    m.Done = true;
                }

                return s1 || s2;
            }

            return SubtreeContainsHuman(m.Op1) || SubtreeContainsHuman(m.Op2);
        }

        public void BreakItDown(string node, long targetVal)
        {
            var m = Monkeys[node];

            if (m.IsHuman)
            {
                m.Value = targetVal;
            }

            var c1 = Monkeys[m.Op1].Value;
            var c2 = Monkeys[m.Op2].Value;

            if (SubtreeContainsHuman(m.Op1))
            {
                // m = H (op) c
                switch (m.Op)
                {
                    case '+':
                        BreakItDown(m.Op1, targetVal - c2);
                        break;
                    case '-':
                        BreakItDown(m.Op1, targetVal + c2);
                        break;
                    case '*':
                        BreakItDown(m.Op1, targetVal / c2);
                        break;
                    case '/':
                        BreakItDown(m.Op1, targetVal * c2);
                        break;
                }
            }
            else
            {
                if (SubtreeContainsHuman(m.Op2))
                {
                    throw new Exception("Double trouble!");
                }

                // m = c (op) H
                switch (m.Op)
                {
                    case '+':
                        BreakItDown(m.Op2, targetVal - c1);
                        break;
                    case '-':
                        BreakItDown(m.Op2, targetVal + c1);
                        break;
                    case '*':
                        BreakItDown(m.Op2, targetVal / c1);
                        break;
                    case '/':
                        BreakItDown(m.Op2, c1 / targetVal);
                        break;
                }
            }
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day21.txt");

            /*
            var data = new string[]
            {
                "root: pppw + sjmn",
                "dbpl: 5",
                "cczh: sllz + lgvd",
                "zczc: 2",
                "ptdq: humn - dvpt",
                "dvpt: 3",
                "lfqf: 4",
                "humn: 5",
                "ljgn: 2",
                "sjmn: drzm * dbpl",
                "sllz: 4",
                "pppw: cczh / lfqf",
                "lgvd: ljgn * ptdq",
                "drzm: hmdt - zczc",
                "hmdt: 32",
            };
            */

            Parse(data);

            var r = Monkeys["root"];
            var h = Monkeys["humn"];

            var s1 = SubtreeContainsHuman(r.Op1, true);
            var s2 = SubtreeContainsHuman(r.Op2, true);

            var dd = new Dictionary<string, int>();

            foreach (var m in Monkeys.Values)
            {
                dd[m.Name] = 0;
            }

            foreach (var m in Monkeys.Values)
            {
                if (!m.IsConstant)
                {
                    dd[m.Op1]++;
                    dd[m.Op2]++;
                }
            }

            foreach (var m in Monkeys.Values)
            {
                if (dd[m.Name] > 1)
                {
                    Console.WriteLine($"Found dupe: {m.Name}");
                }
            }

            if (s1)
            {
                BreakItDown(r.Op1, Monkeys[r.Op2].Value);
            }
            else
            {
                BreakItDown(r.Op2, Monkeys[r.Op1].Value);
            }


            Console.WriteLine($"Answer is {h.Value}");
        }

    }
}
