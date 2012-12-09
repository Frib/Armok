using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ArmokLanguage
{
    public class Language
    {
        private Dictionary<string, List<Instruction>> routines;
        private List<Dwarf> dwarves;

        public void ShortParse(string s)
        {
            routines = new Dictionary<string, List<Instruction>>();
            dwarves = new List<Dwarf>();

            var currentRoutine = "";
            int name = 1;

            foreach (char c in s.ToLowerInvariant())
            {
                if (c == '+')
                {
                    string newname = name.ToString();
                    List<Instruction> instructions = new List<Instruction>();
                    Dwarf d = new Dwarf(newname, instructions);
                    dwarves.Add(d);
                    routines.Add(newname, instructions);
                    currentRoutine = newname;
                    name++;
                }
                else if (c == '-')
                {
                    string newname = name.ToString();
                    List<Instruction> instructions = new List<Instruction>();
                    routines.Add(newname, instructions);
                    currentRoutine = newname;
                    name++;
                }
                else
                {
                    var order = ParseChar(c);
                    if (order != null)
                    {
                        routines[currentRoutine].Add(order);
                    }
                }
            }

            foreach (var d in dwarves)
            {
                d.Instructions = d.Instructions.ToList();
            }
        }

        public static Instruction ParseChar(char c)
        {
            if (c == '>')
            {
                return new MoveRight();
            }
            else if (c == '<')
            {
                return new MoveLeft();
            }
            else if (c == 'm')
            {
                return new Mine();
            }
            else if (c == 'd')
            {
                return new Dump();
            }
            else if (c == 'w')
            {
                return new Work();
            }
            return null;
        }

        private void CreateRoutine(string name)
        {
            routines.Add(name, new List<Instruction>());
        }

        public World Execute(string consoleInput, ParallelType pType = ParallelType.Sequential)
        {
            World world = new World(dwarves, routines);

            world.Run(consoleInput, pType);

            return world;
        }

        public World Debug(ParallelType pType = ParallelType.Sequential)
        {
            World world = new World(dwarves, routines);

            return world;
        }

        public static void Main (string[] args)
        {
            var language = new Language();
            var code = args.Any() ? args.Aggregate((x, y) => x + y) : Console.ReadLine();
            while (true)
            {
                language.ShortParse(code);
                Console.WriteLine(language.Execute(Console.ReadLine()).Output);
            }
        }
    }
}
