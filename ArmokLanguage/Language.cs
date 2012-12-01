﻿using System;
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
                else if (c == '>')
                {
                    routines[currentRoutine].Add(new MoveRight());
                }
                else if (c == '<')
                {
                    routines[currentRoutine].Add(new MoveLeft());
                }
                else if (c == 'm')
                {
                    routines[currentRoutine].Add(new Mine());
                }
                else if (c == 'd')
                {
                    routines[currentRoutine].Add(new Dump());
                }
                else if (c == 'w')
                {
                    routines[currentRoutine].Add(new Work());
                }
            }

            foreach (var d in dwarves)
            {
                d.Instructions = d.Instructions.ToList();
            }
        }

        private void CreateRoutine(string name)
        {
            routines.Add(name, new List<Instruction>());
        }

        public World Execute(string consoleInput)
        {
            World world = new World(dwarves, routines);

            world.Run(consoleInput);

            return world;
        }
    }
}
