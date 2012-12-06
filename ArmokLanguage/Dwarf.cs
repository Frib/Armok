using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ArmokLanguage
{
    public class Dwarf
    {
        public List<Instruction> Instructions;

        public Dwarf(string name, List<Instruction> startInstructions)
        {
            this.Name = name;
            this.Instructions = startInstructions;
            this.Position = 1;
        }
        public int Rocks { get; set; }
        public int Position { get; set; }

        public bool Dead { get; set; }

        internal void Turn(World world)
        {
            if (SkipTurn || Dead)
            {
                SkipTurn = false;
                Instructions.RemoveAt(0);
                return;
            }

            if (Instructions.Any())
            {
                var instruction = Instructions.First();
                Instructions.Remove(instruction);
                if (!instruction.Execute(this, world))
                {
                    Dead = true;
                }

            }
            else
            {
                Trace.WriteLine(Name + " was stricken by melancholy!");
                Dead = true;
            }
        }

        public bool SkipTurn { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            string result = "\"" + Name + "\" ";

            if (Dead)
            {
                result += "DEAD ";
            }

            result += "pos:" + Position + " r:" + Rocks + " ";
            if (Instructions.Any())
            {
                result += Instructions.First().ToString();
            }

            return result;
        }
    }
}
