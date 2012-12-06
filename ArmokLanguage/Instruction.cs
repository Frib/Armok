using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ArmokLanguage
{
    public abstract class Instruction
    {
        public abstract bool Execute(Dwarf dwarf, World world);
    }

    public class MoveLeft : Instruction
    {
        public override bool Execute(Dwarf dwarf, World world)
        {
            dwarf.Position -= 1;
            if (dwarf.Position == 0)
            {
                Trace.WriteLine(dwarf.Name + " walked into magma and perished");
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return "<";    
        }
    }

    public class MoveRight : Instruction
    {
        public override bool Execute(Dwarf dwarf, World world)
        {
            var tile = world.Cave[dwarf.Position + 1];

            if (tile.Rocks != -1)
            {
                dwarf.Position += 1;
                return true;
            }

            Trace.WriteLine(dwarf.Name + " ran into a wall and perished");
            return false;
        }

        public override string ToString()
        {
            return ">";
        }
    }

    public class Mine : Instruction
    {
        public override bool Execute(Dwarf dwarf, World world)
        {
            int placeToMine = dwarf.Position + 1;
            var tile = world.Cave[placeToMine];

            lock (tile.rockLock)
            {
                if (world.Cave[placeToMine].Rocks != 0)
                {
                    if (world.Cave[placeToMine].Rocks == -1)
                    {
                        world.Cave[placeToMine].Rocks = 64;
                        world.Cave.Add(new Tile() { Rocks = -1 });
                    }

                    world.Cave[placeToMine].Rocks -= 1;
                    dwarf.Rocks += 1;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return "m";
        }
    }

    public class Dump : Instruction
    {
        public override bool Execute(Dwarf dwarf, World world)
        {
            if (dwarf.Rocks > 0)
            {
                int placeToDrop = dwarf.Position - 1;
                if (placeToDrop > 0)
                {
                    lock (world.Cave[placeToDrop].rockLock)
                    {
                        world.Cave[placeToDrop].Rocks += 1;
                    }
                }
                dwarf.Rocks -= 1;
            }

            return true;
        }

        public override string ToString()
        {
            return "d";
        }
    }

    public class Work : Instruction
    {
        public override bool Execute(Dwarf dwarf, World world)
        {
            lock (world.Cave[dwarf.Position].workLock)
            {
                if (world.Cave[dwarf.Position].Workshop == null)
                {
                    world.CreateWorkshop(dwarf);
                }
                else
                {
                    world.Cave[dwarf.Position].Workshop.Execute(dwarf, world);
                }
            }
            return true;
        }

        public override string ToString()
        {
            return "w";
        }
    }
}
