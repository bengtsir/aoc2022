using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Packet
    {
        public List<Packet> packets;
        public int Value { get; set; }

        public bool Mark { get; set; } = false;

        public override string ToString()
        {
            if (packets != null)
            {
                return $"[{string.Join(",", packets)}]";
            }

            return Value.ToString();
        }
    }


    internal class Day13
    {
        internal List<Packet> Left = new List<Packet>();
        internal List<Packet> Right = new List<Packet>();

        internal int FindMatchingClose(string s)
        {
            int c = 0;

            if (s[0] == '[')
            {
                for (int p = 1; p < s.Length; p++)
                {
                    if (s[p] == ']' && c == 0)
                    {
                        return p;
                    }
                    else if (s[p] == '[')
                    {
                        c++;
                    }
                    else if (s[p] == ']')
                    {
                        c--;
                    }
                }
            }

            throw new Exception("Malformed list");
        }

        internal Packet ParsePacket(string packet)
        {
            var p = new Packet();

            p.packets = new List<Packet>();

            var sub = packet.Substring(1, FindMatchingClose(packet)-1);
            while (sub.Length > 0)
            {
                if (sub[0] == '[')
                {
                    var p2 = FindMatchingClose(sub);
                    p.packets.Add(ParsePacket(sub.Substring(0, p2 + 1)));
                    sub = sub.Substring(p2 + 1);
                }
                else
                {
                    var p2 = sub.IndexOf(",");
                    if (p2 < 0)
                    {
                        p.packets.Add(new Packet() { Value = Int32.Parse(sub) });
                        sub = "";
                    }
                    else if (p2 == 0)
                    {
                        sub = sub.Substring(1);
                    }
                    else
                    {
                        p.packets.Add(new Packet() { Value = Int32.Parse(sub.Substring(0, p2)) });
                        sub = sub.Substring(p2 + 1);
                    }
                }
            }

            return p;
        }

        internal void Parse(string[] lines)
        {
            for (int r = 0; r < lines.Length; r += 3)
            {
                Left.Add(ParsePacket(lines[r]));
                Right.Add(ParsePacket(lines[r+1]));
            }
        }

        internal int CorrectOrder(Packet L, Packet R)
        {
            if (L.packets == null && R.packets == null)
            {
                if (L.Value < R.Value)
                {
                    return 1;
                }
                else if (L.Value > R.Value)
                {
                    return -1;
                }

                return 0;
            }
            
            if (L.packets == null && R.packets != null)
            {
                L.packets = new List<Packet>();
                L.packets.Add(new Packet() { Value = L.Value });
            }
            
            if (L.packets != null && R.packets == null)
            {
                R.packets = new List<Packet>();
                R.packets.Add(new Packet() { Value = R.Value });
            }

            int pos = 0;
            while (pos < L.packets.Count && pos < R.packets.Count)
            {
                var lval = L.packets[pos];

                var rval = R.packets[pos];

                var v = CorrectOrder(lval, rval);
                if (v > 0)
                {
                    return 1;
                }
                else if (v < 0)
                {
                    return -1;
                }

                pos++;
            }

            if (L.packets.Count == R.packets.Count)
            {
                return 0;
            }
            else if (L.packets.Count < R.packets.Count)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day13.txt");

            /*
            var data = new string[]
            {
                "[1, 1, 3, 1, 1]",
                "[1, 1, 5, 1, 1]",
                "",
                "[[1],[2, 3, 4]]",
                "[[1],4]",
                "",
                "[9]",
                "[[8,7,6]]",
                "",
                "[[4,4],4,4]",
                "[[4,4],4,4,4]",
                "",
                "[7,7,7,7]",
                "[7,7,7]",
                "",
                "[]",
                "[3]",
                "",
                "[[[]]]",
                "[[]]",
                "",
                "[1,[2,[3,[4,[5,6,7]]]],8,9]",
                "[1,[2,[3,[4,[5,6,0]]]],8,9]",
            };*/

            Parse(data);

            var L = Left.ToArray();
            var R = Right.ToArray();

            int sum = 0;

            for (int i = 0; i < L.Length; i++)
            {
                if (CorrectOrder(L[i], R[i]) > 0)
                {
                    sum += (i + 1);
                }
            }

            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day13.txt");

            /*
            var data = new string[]
            {
                "[1, 1, 3, 1, 1]",
                "[1, 1, 5, 1, 1]",
                "",
                "[[1],[2, 3, 4]]",
                "[[1],4]",
                "",
                "[9]",
                "[[8,7,6]]",
                "",
                "[[4,4],4,4]",
                "[[4,4],4,4,4]",
                "",
                "[7,7,7,7]",
                "[7,7,7]",
                "",
                "[]",
                "[3]",
                "",
                "[[[]]]",
                "[[]]",
                "",
                "[1,[2,[3,[4,[5,6,7]]]],8,9]",
                "[1,[2,[3,[4,[5,6,0]]]],8,9]",
            };*/

            Parse(data);

            var packets = new List<Packet>();
            packets.AddRange(Left);
            packets.AddRange(Right);

            var p2 = new Packet()
            {
                packets = new List<Packet>()
                {
                    new Packet() { Value = 2 }
                },
                Mark = true
            };
            var p6 = new Packet()
            {
                packets = new List<Packet>()
                {
                    new Packet() { Value = 6 }
                },
                Mark = true
            };

            packets.Add(p2);
            packets.Add(p6);

            packets.Sort(CorrectOrder);
            packets.Reverse();

            int pos = 1;
            foreach (var packet in packets)
            {
                Console.WriteLine($"{pos}: {packet}");
                pos++;
            }

            var pos1 = packets.FindIndex(p => p.Mark) + 1;
            var pos2 = packets.FindIndex(pos1 + 1, p => p.Mark) + 1;

            Console.WriteLine($"Answer is {pos1 * pos2}");
        }

    }
}
