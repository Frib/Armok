using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArmokLanguage
{
    public class Tile
    {
        public readonly object rockLock = new object();
        public readonly object workLock = new object();

        public int Rocks { get; set; }
        public Workshop Workshop { get; set; }
    }
}
