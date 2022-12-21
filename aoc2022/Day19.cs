using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Resources
    {
        public int Ore { get; set; }
        public int Clay { get; set; }
        public int Obsidian { get; set; }
        public int Geodes { get; set; }

        public void AddOutput(Resources bots)
        {
            Ore += bots.Ore;
            Clay += bots.Clay;
            Obsidian += bots.Obsidian;
            Geodes += bots.Geodes;
        }

        public void RemoveOutput(Resources bots)
        {
            Ore -= bots.Ore;
            Clay -= bots.Clay;
            Obsidian -= bots.Obsidian;
            Geodes -= bots.Geodes;
        }
    }

    internal class Blueprint
    {
        public int BlueprintNumber { get; set; }
        public int OreBotCost { get; set; }
        public int ClayBotCost { get; set; }
        public int ObsidianBotCostOre { get; set; }
        public int ObsidianBotCostClay { get; set; }
        public int GeodeBotCostOre { get; set; }
        public int GeodeBotCostObsidian { get; set; }

        public Blueprint(int[] vals)
        {
            BlueprintNumber = vals[0];
            OreBotCost = vals[1];
            ClayBotCost = vals[2];
            ObsidianBotCostOre = vals[3];
            ObsidianBotCostClay = vals[4];
            GeodeBotCostOre = vals[5];
            GeodeBotCostObsidian = vals[6];
        }

        public bool CanBuyGeodeBot(Resources r)
        {
            return r.Ore >= GeodeBotCostOre && r.Obsidian >= GeodeBotCostObsidian;
        }

        public bool CanBuyObsidianBot(Resources r)
        {
            return r.Ore >= ObsidianBotCostOre && r.Clay >= ObsidianBotCostClay;
        }

        public bool CanBuyClayBot(Resources r)
        {
            return r.Ore >= ClayBotCost;
        }

        public bool CanBuyOreBot(Resources r)
        {
            return r.Ore >= OreBotCost;
        }
    }

    internal class Day19
    {
        internal Blueprint[] Blueprints;

        internal void ParseInput(string[] data)
        {
            var pat =
                @"print ([\d]+): .*costs ([\d]+) ore.*costs ([\d]+) ore.*costs ([\d]+) ore and ([\d]+) clay.*costs ([\d]+) ore and ([\d]+) ob.*";

            Blueprints = new Blueprint[data.Length];

            foreach (var s in data)
            {
                var m = Regex.Match(s, pat);

                var vals = new int[7];

                for (int i = 0; i < 7; i++)
                {
                    vals[i] = Int32.Parse(m.Groups[i + 1].Value);
                }

                Blueprints[vals[0] - 1] = new Blueprint(vals);
            }
        }

        public int MaxGeodes = 0;
        public int MaxGeodesBlueprint = 0;

        public Blueprint CurrentBlueprint;

        public Resources R;
        public Resources B;

        public long Nodes = 0;

        internal int E(int maxEst, int maxRateEst, int cost, int timeLeft)
        {
            int e = 0;

            for (int i = 0; i < maxEst / cost; i++)
            {
                e += (i + 1) * (timeLeft - (i*cost/maxRateEst) - 1);
            }

            return e;
        }
        
        public void Dive(int minutesLeft)
        {
            Nodes++;

            if (minutesLeft == 0)
            {
                if (R.Geodes >= MaxGeodes)
                {
                    MaxGeodes = R.Geodes;
                    MaxGeodesBlueprint = CurrentBlueprint.BlueprintNumber;
                }

                return;
            }

            int[] oreBots = new int[minutesLeft];
            int currOre = R.Ore;
            oreBots[0] = B.Ore;
            for (int i = 1; i < minutesLeft; i++)
            {
                bool newBot = currOre >= CurrentBlueprint.OreBotCost;
                oreBots[i] = oreBots[i - 1];
                if (newBot)
                {
                    oreBots[i]++;
                    currOre -= CurrentBlueprint.OreBotCost;
                }

                currOre += oreBots[i - 1];
            }

            int[] clayBots = new int[minutesLeft];
            currOre = R.Ore;
            clayBots[0] = B.Clay;
            for (int i = 1; i < minutesLeft; i++)
            {
                bool newBot = currOre >= CurrentBlueprint.ClayBotCost;
                clayBots[i] = clayBots[i - 1];
                if (newBot)
                {
                    clayBots[i]++;
                    currOre -= CurrentBlueprint.ClayBotCost;
                }

                currOre += oreBots[i - 1];
            }

            int[] obsBots = new int[minutesLeft];
            int currClay = R.Clay;
            obsBots[0] = B.Obsidian;
            for (int i = 1; i < minutesLeft; i++)
            {
                bool newBot = currClay >= CurrentBlueprint.ObsidianBotCostClay;
                obsBots[i] = obsBots[i - 1];
                if (newBot)
                {
                    obsBots[i]++;
                    currClay -= CurrentBlueprint.ObsidianBotCostClay;
                }

                currClay += clayBots[i - 1];
            }

            int[] geodeBots = new int[minutesLeft];
            int currObs = R.Obsidian;
            geodeBots[0] = B.Geodes;
            for (int i = 1; i < minutesLeft; i++)
            {
                bool newBot = currObs >= CurrentBlueprint.GeodeBotCostObsidian;
                geodeBots[i] = geodeBots[i - 1];
                if (newBot)
                {
                    geodeBots[i]++;
                    currObs -= CurrentBlueprint.GeodeBotCostObsidian;
                }

                currObs += obsBots[i - 1];
            }

            int estGeodes = R.Geodes;

            for (int i = 0; i < minutesLeft; i++)
            {
                estGeodes += geodeBots[i];
            }

            if (estGeodes <= MaxGeodes)
            {
                // Cutoff
                return;
            }

            if (CurrentBlueprint.CanBuyGeodeBot(R))
            {
                R.Ore -= CurrentBlueprint.GeodeBotCostOre;
                R.Obsidian -= CurrentBlueprint.GeodeBotCostObsidian;
                R.AddOutput(B);
                B.Geodes++;

                Dive(minutesLeft - 1);

                B.Geodes--;
                R.RemoveOutput(B);
                R.Obsidian += CurrentBlueprint.GeodeBotCostObsidian;
                R.Ore += CurrentBlueprint.GeodeBotCostOre;
            }

            if (CurrentBlueprint.CanBuyObsidianBot(R))
            {
                R.Ore -= CurrentBlueprint.ObsidianBotCostOre;
                R.Clay -= CurrentBlueprint.ObsidianBotCostClay;
                R.AddOutput(B);
                B.Obsidian++;

                Dive(minutesLeft - 1);

                B.Obsidian--;
                R.RemoveOutput(B);
                R.Clay += CurrentBlueprint.ObsidianBotCostClay;
                R.Ore += CurrentBlueprint.ObsidianBotCostOre;
            }

            if (CurrentBlueprint.CanBuyClayBot(R))
            {
                R.Ore -= CurrentBlueprint.ClayBotCost;
                R.AddOutput(B);
                B.Clay++;

                Dive(minutesLeft - 1);

                B.Clay--;
                R.RemoveOutput(B);
                R.Ore += CurrentBlueprint.ClayBotCost;
            }

            if (CurrentBlueprint.CanBuyOreBot(R))
            {
                R.Ore -= CurrentBlueprint.OreBotCost;
                R.AddOutput(B);
                B.Ore++;

                Dive(minutesLeft - 1);

                B.Ore--;
                R.RemoveOutput(B);
                R.Ore += CurrentBlueprint.OreBotCost;
            }

            // Final case, just go
            if (true)
            {
                R.AddOutput(B);
                Dive(minutesLeft - 1);
                R.RemoveOutput(B);
            }
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day19.txt");

            /*
            var data = new string[]
            {
                "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.",
                "Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.",
            };
            */

            ParseInput(data);

            int[] MaxGeodeCounts = new int[Blueprints.Length];

            foreach (var blueprint in Blueprints)
            {
                Console.WriteLine($"Checking blueprint {blueprint.BlueprintNumber}");

                CurrentBlueprint = blueprint;

                B = new Resources()
                {
                    Ore = 1
                };

                R = new Resources();

                MaxGeodes = 0;

                Nodes = 0;

                Dive(24);

                MaxGeodeCounts[blueprint.BlueprintNumber - 1] = MaxGeodes;

                Console.WriteLine($"Max geodes is {MaxGeodes} found in {Nodes} nodes");
            }

            int sum = 0;

            for (int i = 0; i < Blueprints.Length; i++)
            {
                sum += Blueprints[i].BlueprintNumber * MaxGeodeCounts[i];
            }

            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day19.txt");

            /*
            var data = new string[]
            {
                "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.",
                "Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.",
            };
            */

            ParseInput(data.Take(3).ToArray());

            int[] MaxGeodeCounts = new int[Blueprints.Length];

            for (int depth = 24; depth <= 32; depth++)
            {
                Console.WriteLine($"--- *** --- Depth {depth} --- *** ---");
                
                foreach (var blueprint in Blueprints)
                {
                    Console.WriteLine($"Checking blueprint {blueprint.BlueprintNumber}");

                    CurrentBlueprint = blueprint;

                    B = new Resources()
                    {
                        Ore = 1
                    };

                    R = new Resources();

                    // Start value to beat
                    MaxGeodes = MaxGeodeCounts[blueprint.BlueprintNumber - 1];

                    Nodes = 0;

                    Dive(depth);

                    MaxGeodeCounts[blueprint.BlueprintNumber - 1] = MaxGeodes;

                    Console.WriteLine($"Max geodes is {MaxGeodes} found in {Nodes} nodes");
                }

                int sum = 0;

                sum = MaxGeodeCounts.Aggregate(1, (a, b) => a * b);

                Console.WriteLine($"Answer is {sum}");
            }
        }


    }
}
