using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Valve
    {
        public string Name { get; set; }
        public int FlowRate { get; set; }
        public List<Valve> Exits { get; } = new List<Valve>();
        public bool IsOpen { get; set; } = false;

        public int Set { get; set; }
        public int LastOpenIdx { get; set; } = 100;

        public bool BelongsToSet(int i)
        {
            return (Set == i) || (FlowRate == 0);
        }
    }

    internal class Move
    {
        public int Minute { get; set; }
        public Valve AValve { get; set; }
        public Valve EValve { get; set; }
        public int AAction { get; set; }
        public int EAction { get; set; }

        public int FlowRate { get; set; }

        internal Move(int m, Valve a, Valve e, int aa, int ea, int fr)
        {
            Minute = m;
            AValve = a;
            EValve = e;
            AAction = aa;
            EAction = ea;
            FlowRate = fr;
        }

        public override string ToString()
        {
            return $"{Minute}: A: {AValve.Name} E: {EValve.Name} AAction {AAction} EAction {EAction} FlowRate {FlowRate}";
        }

        public Move Copy()
        {
            return new Move(Minute, AValve, EValve, AAction, EAction, FlowRate);
        }
    }

    internal class Day16
    {
        internal Dictionary<string, Valve> Valves = new Dictionary<string, Valve>();

        internal void Parse(string[] lines)
        {
            foreach (var line in lines)
            {
                var m = Regex.Match(line, @"Valve (..) has flow rate=([\d]+);.* val[^\s]* (.*)$");
                Valve valve;
                string valveName = m.Groups[1].Value;

                if (Valves.ContainsKey(valveName))
                {
                    valve = Valves[valveName];
                }
                else
                {
                    valve = new Valve()
                    {
                        Name = valveName
                    };
                    Valves.Add(valveName, valve);
                }

                valve.FlowRate = Int32.Parse(m.Groups[2].Value);
                if (valve.FlowRate == 0)
                {
                    // Force open all flow 0 nodes
                    valve.IsOpen = true;
                }

                var exits = m.Groups[3].Value.Split(',').Select(g => g.Trim()).ToList();

                foreach (var exit in exits)
                {
                    if (!Valves.ContainsKey(exit))
                    {
                        var newv = new Valve()
                        {
                            Name = exit
                        };
                        Valves.Add(exit, newv);
                    }

                    valve.Exits.Add(Valves[exit]);
                }
            }

            // Sort'em in reverse
            foreach (var valve in Valves.Values)
            {
                valve.Exits.Sort((a,b) => b.FlowRate - a.FlowRate);
            }
        }

        private int MaxReleaseLeft(int minutesLeft, int setIdx)
        {
            var closedValves = Valves.Values.Where(v => !v.IsOpen && v.FlowRate > 0 && v.BelongsToSet(setIdx)).OrderBy(v => -v.FlowRate);

            var maxReleaseLeft = 0;
            foreach (var valve in closedValves)
            {
                minutesLeft -= 1;
                if (minutesLeft <= 0)
                {
                    return maxReleaseLeft;
                }
                
                maxReleaseLeft += minutesLeft * valve.FlowRate;
                minutesLeft -= 1;
            }

            return maxReleaseLeft;
        }

        private int maxRelease = 0;
        private string releasePath = "";

        private void Traverse(string path, Valve v, int currentFlow, int currentRelease, int minutesLeft, int setIdx, int lastOpen)
        {
            path += v.Name;

            if (minutesLeft <= 0)
            {
                if (currentRelease > maxRelease)
                {
                    maxRelease = currentRelease;
                    releasePath = path;
                }

                return;
            }

            if (!Valves.Values.Any(vv => !vv.IsOpen && vv.BelongsToSet(setIdx)))
            {
                currentRelease += minutesLeft * currentFlow;
                if (currentRelease > maxRelease)
                {
                    maxRelease = currentRelease;
                    releasePath = path;
                }

                return;
            }

            if (!v.IsOpen && v.BelongsToSet(setIdx))
            {
                // "Open and run through" case

                v.IsOpen = true;

                minutesLeft--;

                int lastOpenSave = v.LastOpenIdx;
                v.LastOpenIdx = minutesLeft;

                currentRelease += currentFlow;
                currentFlow += v.FlowRate;

                if (maxRelease < currentRelease)
                {
                    maxRelease = currentRelease;
                    releasePath = path;
                }

                if (minutesLeft > 0)
                {
                    // Go through all subnodes, closed first, order by flow
                    var exits = v.Exits.Where(e => !e.IsOpen && e.BelongsToSet(setIdx) && e.LastOpenIdx > v.LastOpenIdx).ToList();
                    exits.AddRange(v.Exits.Where(e => e.IsOpen && e.BelongsToSet(setIdx) && e.LastOpenIdx > v.LastOpenIdx));
                    
                    foreach (var e in exits)
                    {
                        // Early break
                        if (currentRelease + (minutesLeft * currentFlow) + MaxReleaseLeft(minutesLeft - 1, setIdx) <= maxRelease)
                        {
                            break;
                        }
                        Traverse(path + "**", e, currentFlow, currentRelease + currentFlow, minutesLeft - 1, setIdx, v.LastOpenIdx);
                    }
                }

                v.IsOpen = false;
                currentFlow -= v.FlowRate;
                currentRelease -= currentFlow;
                minutesLeft++;
                v.LastOpenIdx = lastOpenSave;
            }

            // "Only run through" case
            {
                int lastOpenSave = v.LastOpenIdx;
                v.LastOpenIdx = lastOpen;

                // Go through all subnodes, closed first, order by flow
                var exits = v.Exits.Where(e => !e.IsOpen && e.BelongsToSet(setIdx) && e.LastOpenIdx > v.LastOpenIdx).ToList();
                exits.AddRange(v.Exits.Where(e => e.IsOpen && e.BelongsToSet(setIdx) && e.LastOpenIdx > v.LastOpenIdx));

                foreach (var e in exits)
                {
                    // Early break
                    if (currentRelease + (minutesLeft * currentFlow) + MaxReleaseLeft(minutesLeft - 1, setIdx) <= maxRelease)
                    {
                        break;
                    }
                    Traverse(path, e, currentFlow, currentRelease + currentFlow, minutesLeft - 1, setIdx, lastOpen);
                }

                v.LastOpenIdx = lastOpenSave;
            }
        }


        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day16.txt");

            /*
            var data = new string[]
            {
                "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB",
                "Valve BB has flow rate=13; tunnels lead to valves CC, AA",
                "Valve CC has flow rate=2; tunnels lead to valves DD, BB",
                "Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE",
                "Valve EE has flow rate=3; tunnels lead to valves FF, DD",
                "Valve FF has flow rate=0; tunnels lead to valves EE, GG",
                "Valve GG has flow rate=0; tunnels lead to valves FF, HH",
                "Valve HH has flow rate=22; tunnel leads to valve GG",
                "Valve II has flow rate=0; tunnels lead to valves AA, JJ",
                "Valve JJ has flow rate=21; tunnel leads to valve II",
            };
            */

            Parse(data);

            var startValve = Valves["AA"];

            Traverse("", startValve, 0, 0, 30, 0, 35);

            Console.WriteLine($"Answer is {maxRelease}");
            Console.WriteLine($"Path: {releasePath}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day16.txt");

            /*
            var data = new string[]
            {
                "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB",
                "Valve BB has flow rate=13; tunnels lead to valves CC, AA",
                "Valve CC has flow rate=2; tunnels lead to valves DD, BB",
                "Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE",
                "Valve EE has flow rate=3; tunnels lead to valves FF, DD",
                "Valve FF has flow rate=0; tunnels lead to valves EE, GG",
                "Valve GG has flow rate=0; tunnels lead to valves FF, HH",
                "Valve HH has flow rate=22; tunnel leads to valve GG",
                "Valve II has flow rate=0; tunnels lead to valves AA, JJ",
                "Valve JJ has flow rate=21; tunnel leads to valve II",
            };
            */

            Parse(data);

            var valvesToOpen = Valves.Values.Count(vv => vv.FlowRate > 0);

            var startValve = Valves["AA"];

            var maxMax = 0;

            for (long bitMask = 0; bitMask < (1 << valvesToOpen); bitMask++)
            {
                if ((bitMask % 250) == 0)
                {
                    Console.WriteLine($"Iter {bitMask} / {1 << valvesToOpen}");
                }
                long mask = 1;
                foreach (var valve in Valves.Values.Where(vv => vv.FlowRate > 0))
                {
                    valve.Set = (bitMask & mask) > 0 ? 1 : 0;
                    mask <<= 1;
                }

                maxRelease = 0;

                Traverse("", startValve, 0, 0, 26, 0, 30);

                var releaseLeft = maxRelease;

                maxRelease = 0;

                Traverse("", startValve, 0, 0, 26, 1, 30);

                var releaseRight = maxRelease;

                if (releaseLeft + releaseRight > maxMax)
                {
                    maxMax = releaseLeft + releaseRight;
                    Console.WriteLine($"Found new max at iter {bitMask}: {releaseLeft} + {releaseRight} = {maxMax}");
                }
            }

            Console.WriteLine($"Answer is {maxMax}");
        }
    }
}
