using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Voxel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int SidesShowing { get; set; } = 6;

        public Voxel(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Voxel(string s)
        {
            var values = s.Split(',').Select(Int32.Parse).ToArray();

            X = values[0];
            Y = values[1];
            Z = values[2];
        }

        public bool IsAdjacent(Voxel other)
        {
            if (other.Z == Z &&
                other.Y == Y &&
                (other.X == X - 1 ||
                 other.X == X + 1))
            {
                return true;
            }
            if (other.Y == Y &&
                other.X == X &&
                (other.Z == Z - 1 ||
                 other.Z == Z + 1))
            {
                return true;
            }
            if (other.X == X &&
                other.Z == Z &&
                (other.Y == Y - 1 ||
                 other.Y == Y + 1))
            {
                return true;
            }

            return false;
        }
    }

    internal class Day18
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day18.txt");

            /*
            var data = new string[]
            {
                "2,2,2",
                "1,2,2",
                "3,2,2",
                "2,1,2",
                "2,3,2",
                "2,2,1",
                "2,2,3",
                "2,2,4",
                "2,2,6",
                "1,2,5",
                "3,2,5",
                "2,1,5",
                "2,3,5",
            };
            */

            var values = data.Select(r => new Voxel(r)).ToArray();

            for (int i = 0; i < values.Length - 1; i++)
            {
                for (int j = i+1; j < values.Length; j++)
                {
                    if (values[i].IsAdjacent(values[j]))
                    {
                        values[i].SidesShowing--;
                        values[j].SidesShowing--;
                    }
                }
            }

            var answer = values.Sum(v => v.SidesShowing);

            Console.WriteLine($"Answer is {answer}");
        }

        private const int MaxX = 25;
        private int[,,] Space;

        public void FloodFill()
        {
            int mm = MaxX - 1;
            // Fill all borders
            for (int i = 0; i < MaxX; i++)
            {
                Space[i, 0, 0] = 100;
                Space[0, i, 0] = 100;
                Space[0, 0, i] = 100;

                Space[i, mm, 0] = 100;
                Space[mm, i, 0] = 100;
                Space[mm, 0, i] = 100;

                Space[i, 0, mm] = 100;
                Space[0, i, mm] = 100;
                Space[0, mm, i] = 100;

                Space[i, mm, mm] = 100;
                Space[mm, i, mm] = 100;
                Space[mm, mm, i] = 100;
            }

            bool found = true;

            while (found)
            {
                found = false;

                for (int x = 0; x < MaxX; x++)
                {
                    for (int y = 0; y < MaxX; y++)
                    {
                        for (int z = 0; z < MaxX; z++)
                        {
                            if (Space[x,y,z] == 0)
                            {
                                if ((x > 0  && Space[x - 1, y, z] == 100) ||
                                    (x < mm && Space[x + 1, y, z] == 100) ||
                                    (y > 0  && Space[x, y - 1, z] == 100) ||
                                    (y < mm && Space[x, y + 1, z] == 100) ||
                                    (z > 0  && Space[x, y, z - 1] == 100) ||
                                    (z < mm && Space[x, y, z + 1] == 100) )
                                {
                                    found = true;
                                    Space[x, y, z] = 100;
                                }
                            }
                        }
                    }

                }
            }
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day18.txt");

            /*
            var data = new string[]
            {
                "2,2,2",
                "1,2,2",
                "3,2,2",
                "2,1,2",
                "2,3,2",
                "2,2,1",
                "2,2,3",
                "2,2,4",
                "2,2,6",
                "1,2,5",
                "3,2,5",
                "2,1,5",
                "2,3,5",
            };
            */

            var values = data.Select(r => new Voxel(r)).ToArray();

            // Translate away from zero
            foreach (var v in values)
            {
                v.X += 1;
                v.Y += 1;
                v.Z += 1;
            }

            Space = new int[MaxX, MaxX, MaxX];

            foreach (var t in values)
            {
                Space[t.X, t.Y, t.Z] = 1;
            }

            FloodFill();

            int voxels = Space.Cast<int>().Count(v => v == 1);
            int emptyspace = Space.Cast<int>().Count(v => v == 100);
            int intspace = Space.Cast<int>().Count(v => v == 0);

            // Only count faces facing space
            foreach (var v in values)
            {
                if (Space[v.X - 1, v.Y, v.Z] != 100)
                {
                    v.SidesShowing--;
                }
                if (Space[v.X + 1, v.Y, v.Z] != 100)
                {
                    v.SidesShowing--;
                }
                if (Space[v.X, v.Y - 1, v.Z] != 100)
                {
                    v.SidesShowing--;
                }
                if (Space[v.X, v.Y + 1, v.Z] != 100)
                {
                    v.SidesShowing--;
                }
                if (Space[v.X, v.Y, v.Z - 1] != 100)
                {
                    v.SidesShowing--;
                }
                if (Space[v.X, v.Y, v.Z + 1] != 100)
                {
                    v.SidesShowing--;
                }
            }


            Console.WriteLine($"Voxels: {voxels} Emptyspace: {emptyspace} Intspace: {intspace}");

            var answer = values.Sum(v => v.SidesShowing);

            Console.WriteLine($"Answer is {answer}");
        }

    }
}
