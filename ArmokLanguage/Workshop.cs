using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ArmokLanguage
{
    public abstract class Workshop
    {
        public abstract void Execute(Dwarf d, World w);
    }

    public class Trader : Workshop
    {
        public override void Execute(Dwarf d, World w)
        {
            if (d.Rocks > 0)
            {
                w.WriteChar((char)d.Rocks);
                d.Rocks = 0;
            }
            else
            {
                d.Rocks = w.ReadChar();
                if (d.Rocks == 0)
                {
                    Trace.WriteLine(d.Name + " was killed by the traders!");
                    d.Dead = true;
                }
            }
        }
    }

    public class ManagerOffice : Workshop
    {
        public override void Execute(Dwarf d, World w)
        {
            int rocks = w.Cave[d.Position].Rocks;
            if (rocks > 0)
            {
                List<Instruction> toInsert = w.Routines[rocks.ToString()];
                d.Instructions.InsertRange(0, toInsert);
            }
            else
            {
                w.Cave[d.Position].Workshop = null;
            }
        }
    }

    public class Appraiser : Workshop
    {
        public override void Execute(Dwarf d, World w)
        {
            int rocks = w.Cave[d.Position].Rocks;
            if (d.Rocks > rocks)
            {
                new Dump().Execute(d, w);
            }
        }
    }
}
