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
    }

    internal class Day16
    {
        internal Dictionary<string, Valve> Valves = new Dictionary<string, Valve>();

        //internal List<Valve> Valves = new List<Valve>();

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

        private int MaxReleaseLeft(int minutesLeft)
        {
            var closedValves = Valves.Values.Where(v => !v.IsOpen && v.FlowRate > 0).OrderBy(v => -v.FlowRate);

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

        private void Traverse(string path, Valve v, int currentFlow, int currentRelease, int minutesLeft)
        {
            path += v.Name;

            if (minutesLeft <= 0)
            {
                if (maxRelease < currentRelease)
                {
                    maxRelease = currentRelease;
                    releasePath = path;
                }

                return;
            }

            if (!v.IsOpen)
            {
                // "Open and run through" case

                v.IsOpen = true;

                minutesLeft--;

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
                    var exits = v.Exits.Where(e => !e.IsOpen).ToList();
                    exits.AddRange(v.Exits.Where(e => e.IsOpen));

                    foreach (var e in exits)
                    {
                        // Early break
                        if (currentRelease + (minutesLeft * currentFlow) + MaxReleaseLeft(minutesLeft - 1) <= maxRelease)
                        {
                            break;
                        }
                        Traverse(path + "**", e, currentFlow, currentRelease + currentFlow, minutesLeft - 1);
                    }
                }

                v.IsOpen = false;
                currentFlow -= v.FlowRate;
                currentRelease -= currentFlow;
                minutesLeft++;
            }

            // "Only run through" case
            {
                // Go through all subnodes, closed first, order by flow
                var exits = v.Exits.Where(e => !e.IsOpen).ToList();
                exits.AddRange(v.Exits.Where(e => e.IsOpen));

                foreach (var e in exits)
                {
                    // Early break
                    if (currentRelease + (minutesLeft * currentFlow) + MaxReleaseLeft(minutesLeft - 1) <= maxRelease)
                    {
                        break;
                    }
                    Traverse(path, e, currentFlow, currentRelease + currentFlow, minutesLeft - 1);
                }
            }
        }

        private string elephantPath = "";

        private void Traverse2(string path, string epath, Valve v, Valve evalve, int currentFlow, int currentRelease,
            int minutesLeft)
        {
            path += v.Name;

            if (minutesLeft <= 0)
            {
                if (maxRelease < currentRelease)
                {
                    maxRelease = currentRelease;
                    releasePath = path;
                    elephantPath = epath;
                }

                return;
            }

            // Case 1: Both open
            if (!v.IsOpen && !evalve.IsOpen && v != evalve)
            {
                v.IsOpen = true;
                evalve.IsOpen = true;

                minutesLeft--;

                currentRelease += currentFlow;
                currentFlow += v.FlowRate;
                currentFlow += evalve.FlowRate;

                if (maxRelease < currentRelease)
                {
                    maxRelease = currentRelease;
                    releasePath = path;
                }

                if (minutesLeft > 0)
                {
                    // Go through all subnodes, closed first, order by flow
                    /*
                    var exits = v.Exits.Where(e => !e.IsOpen).ToList();
                    exits.AddRange(v.Exits.Where(e => e.IsOpen));

                    foreach (var e in exits)
                    {
                        var eexits = evalve.Exits.Where(e => !e.IsOpen).ToList();
                        eexits.AddRange(evalve.Exits.Where(e => e.IsOpen));

                        // Early break
                        if (currentRelease + (minutesLeft * currentFlow) + MaxReleaseLeft(minutesLeft - 1) <=
                            maxRelease)
                        {
                            break;
                        }

                        foreach (var eexit in eexits)
                        {
                            if (currentRelease + (minutesLeft * currentFlow) + MaxReleaseLeft(minutesLeft - 1) <=
                                maxRelease)
                            {
                                break;
                            }

                            Traverse2(path, epath, e, eexit, currentFlow, currentRelease + currentFlow,
                                minutesLeft - 1);
                        }
                    }
                    */
                }

                evalve.IsOpen = false;
                v.IsOpen = false;

                currentFlow -= evalve.FlowRate;
                currentFlow -= v.FlowRate;

                currentRelease -= currentFlow;
                minutesLeft++;
            }

            // Case 2: You open, elephant run
            if (true)
            {

            }

            // Go through all subnodes, closed first, order by flow
            var exits = v.Exits.Where(e => !e.IsOpen).ToList();
            exits.AddRange(v.Exits.Where(e => e.IsOpen));

            var eexits = evalve.Exits.Where(e => !e.IsOpen).ToList();
            eexits.AddRange(evalve.Exits.Where(e => e.IsOpen));

            foreach (var e in exits)
            {
                // Early break
                if (currentRelease + (minutesLeft * currentFlow) + MaxReleaseLeft(minutesLeft - 1) <= maxRelease)
                {
                    break;
                }

                foreach (var eexit in eexits)
                {
                    if (currentRelease + (minutesLeft * currentFlow) + MaxReleaseLeft(minutesLeft - 1) <= maxRelease)
                    {
                        break;
                    }

                    Traverse2(path, epath, e, eexit, currentFlow, currentRelease + currentFlow, minutesLeft - 1);
                }
            }
    
            // Case 2: You run, elephant open
            {
            }
            // Case 3: You open, elephant run
            // Case 4: Both open


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

            var mr = MaxReleaseLeft(30);

            Traverse("", startValve, 0, 0, 30);

            Console.WriteLine($"Answer is {maxRelease}");
            Console.WriteLine($"Path: {releasePath}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day16.txt");

            var values = data.Select(r => r.Select(c => Int32.Parse(c.ToString())).ToArray()).ToArray();

            Console.WriteLine($"Answer is {42}");
        }

    }
}
