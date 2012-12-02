using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ArmokLanguage
{
    public class World
    {
        public Dictionary<string, List<Instruction>> Routines;
        private StringBuilder sb;

        public List<Tile> Cave { get; set; }
        public List<Dwarf> Dwarves { get; set; }
        
        public World(List<Dwarf> dwarves, Dictionary<string, List<Instruction>> routines)
        {
            this.Routines = routines;
            this.Cave = new List<Tile>() { new Tile(), new Tile(), new Tile(), new Tile(), new Tile() { Rocks = -1} };
            this.Dwarves = dwarves;
        }
        
        public void Run(string consoleInput)
        {
            sb = new StringBuilder();
            input = consoleInput.ToArray();
            int turn = 0;
            while (Dwarves.Any(x => !x.Dead))
            {
                foreach (var d in Dwarves.Where(dorf => !dorf.Dead))
                {
                    d.Turn(this);
                }
                turn++;
            }
            Trace.WriteLine("All dwarves are dead and/or stopped working after " + turn + " turns");
        }

        internal void WriteChar(char p)
        {
            sb.Append(p);
        }

        internal int ReadChar()
        {
            var result = (int)input.FirstOrDefault();
            input = input.Skip(1).ToArray();
            return result;
        }
    
        private char[] input { get; set; }

        internal void CreateWorkshop(Dwarf dwarf)
        {
            switch(dwarf.Rocks)
            {
                case 1: Cave[dwarf.Position].Workshop = new Trader(); break;
                case 2: Cave[dwarf.Position].Workshop = new ManagerOffice(); break;
                case 3: Cave[dwarf.Position].Workshop = new Appraiser(); break;
                default: Trace.WriteLine(dwarf.Name + " failed to create a workshop with " + dwarf.Rocks + " rocks and went stark raving mad!"); dwarf.Dead = true; return;
            }

            dwarf.Rocks = 0;
        }

        public string OutputInText { get { return sb.ToString(); } }

        public List<int> OutputInNumbers { get { return sb.ToString().ToArray().Select(c => (int)c).ToList(); } }

        public string Output { get { return OutputInText + " (" + OutputInNumbers.Select(i => i.ToString()).Aggregate((x, y) => x + " " + y) + ")"; } }
    }
}
