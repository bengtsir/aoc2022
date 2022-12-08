using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2022
{
    internal class Day7
    {
        internal class FileEntry
        {
            public string Name { get; set; }
            public int Size { get; set; }
        }

        internal class DirEntry
        {
            public string Name { get; set; }
            public DirEntry Parent { get; set; }
            public List<DirEntry> ChildDirs { get; } = new List<DirEntry>();
            public List<FileEntry> Files { get; } = new List<FileEntry>();
            public int Size => Files.Select(f => f.Size).Sum() + ChildDirs.Select(d => d.Size).Sum();

            public string Path()
            {
                string path = Name;
                var p = Parent;
                
                if (p != null)
                {
                    path = p.Path() + "/" + path;
                }

                return path;
            }

            public int Part1()
            {
                int size = 0;
                if (Size <= 100000)
                {
                    size += Size;
                }

                foreach (var dir in ChildDirs)
                {
                    size += dir.Part1();
                }
                return size;
            }

            public int Part2(List<FileEntry> allDirs)
            {
                var cumSize = 0;

                foreach (var dir in ChildDirs)
                {
                    cumSize += dir.Part2(allDirs);
                }

                cumSize += Files.Select(f => f.Size).Sum();

                allDirs.Add(new FileEntry(){Name = Path(), Size = cumSize});
                return cumSize;
            }
        }

        internal DirEntry RootDir = new DirEntry()
        {
            Name = "/"
        };

        internal void Parse(string[][] values)
        {
            int pos = 0;
            DirEntry curDir = RootDir;

            while (pos < values.Length)
            {
                switch (values[pos][1])
                {
                    case "ls":
                        pos++;
                        while (pos < values.Length && values[pos][0] != "$")
                        {
                            if (values[pos][0] == "dir")
                            {
                                curDir.ChildDirs.Add(new DirEntry() {Parent = curDir, Name = values[pos][1]});
                            }
                            else
                            {
                                curDir.Files.Add(new FileEntry(){Name = values[pos][1], Size = Int32.Parse(values[pos][0])});
                            }

                            pos++;
                        }
                        break;
                    case "cd":
                        if (values[pos][2] == "/")
                        {
                            curDir = RootDir;
                        }
                        else if (values[pos][2] == "..")
                        {
                            curDir = curDir.Parent;
                        }
                        else
                        {
                            curDir = curDir.ChildDirs.Find(d => d.Name == values[pos][2]);
                        }

                        pos++;
                        break;
                }
            }
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day7.txt");

            var values = data.Select(r => r.Split(' ')).ToArray();

            Parse(values);

            Console.WriteLine($"Answer is {RootDir.Part1()}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day7.txt");

            var values = data.Select(r => r.Split(' ')).ToArray();

            Parse(values);

            var dirs = new List<FileEntry>();

            var totOccupiedSpace = RootDir.Part2(dirs);

            var spaceToRelease = 30000000 - (70000000 - totOccupiedSpace);

            var dd = dirs.OrderBy(d => d.Size).ToList();
            foreach (var d in dd)
            {
                Console.WriteLine($"{d.Size}       {d.Name}");
            }

            var d2 = dirs.OrderBy(d => d.Size).First(d => d.Size >= spaceToRelease);


            Console.WriteLine($"Answer is {d2.Name} with size {d2.Size}");
        }

    }
}
