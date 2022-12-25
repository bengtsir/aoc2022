using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Day25
    {
        internal string[] SnafuInput;

        internal long Val(char c)
        {
            switch (c)
            {
                case '2':
                    return 2;
                case '1':
                    return 1;
                case '0':
                    return 0;
                case '-':
                    return -1;
                case '=':
                    return -2;
            }

            throw new ArgumentException("Incorrect digit", nameof(c));
        }

        internal long Decode(string snafu)
        {
            long res = 0;

            for (int i = 0; i < snafu.Length; i++)
            {
                res = (5 * res) + Val(snafu[i]);
            }

            return res;
        }

        internal string Encode(long val)
        {
            string s = "";

            while (val > 0)
            {
                var rest = val % 5;
                val = val / 5;

                switch (rest)
                {
                    case 0:
                        s = "0" + s;
                        break;
                    case 1:
                        s = "1" + s;
                        break;
                    case 2:
                        s = "2" + s;
                        break;
                    case 3:
                        s = "=" + s;
                        val += 1;
                        break;
                    case 4:
                        s = "-" + s;
                        val += 1;
                        break;
                }
            }

            return s;
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day25.txt");

            /*
            var data = new string[]
            {
                "1=-0-2",
                "12111",
                "2=0=",
                "21",
                "2=01",
                "111",
                "20012",
                "112",
                "1=-1=",
                "1-12",
                "12",
                "1=",
                "122",
            };
            */

            SnafuInput = data.Select(s => s).ToArray();

            foreach (var s in SnafuInput)
            {
                Console.WriteLine($"{s} => {Decode(s)} => {Encode(Decode(s))}");
            }

            Console.WriteLine($"Answer is {Encode(SnafuInput.Select(s => Decode(s)).Sum())}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day25.txt");

            var values = data.Select(r => r.Select(c => Int32.Parse(c.ToString())).ToArray()).ToArray();

            Console.WriteLine($"Answer is {42}");
        }

    }
}
