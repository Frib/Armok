using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArmokLanguage;

namespace ArmokLangTest
{
    [TestClass]
    public class WorldTest
    {
        [TestMethod]
        public void InitialState()
        {
            Dictionary<string, List<Instruction>> routines = new Dictionary<string,List<Instruction>>();
            routines.Add("main", new List<Instruction>());
            World w = new World(new List<Dwarf>(){ new Dwarf("Urist", routines["main"])}, routines);

            Assert.AreEqual(0, w.Cave[0].Rocks);
            Assert.AreEqual(0, w.Cave[1].Rocks);
            Assert.AreEqual(0, w.Cave[2].Rocks);
            Assert.AreEqual(0, w.Cave[3].Rocks);
            Assert.AreEqual(-1, w.Cave[4].Rocks);
            Assert.AreEqual(1, w.Dwarves[0].Position);
        }

        [TestMethod]
        public void ShortParse()
        {
            Language l = new Language();
            l.ShortParse("+>>m<<d");
            var world = l.Execute("");
            Assert.AreEqual(0, world.Dwarves.First().Rocks);
            Assert.AreEqual(63, world.Cave[4].Rocks);
        }

        [TestMethod]
        public void OutputTraderTest()
        {
            string script = "+>>mwmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm>mm<w<<<";
            Language l = new Language();
            l.ShortParse(script);
            var world = l.Execute("");
            Assert.AreEqual("A", world.Output);
        }

        [TestMethod]
        public void InputTraderTest()
        {
            string script = "+>>m<<wwwwwwwwwwwwwwwwwwwwwwwww<";
            Language l = new Language();
            l.ShortParse(script);
            var world = l.Execute("Hello world!");
            Assert.AreEqual("Hello world!", world.Output);
        }

        [TestMethod]
        public void InfiniteInOutTest()
        {
            string script = "+>>mwmm<w>mmdd<w->ww<w";
            Language l = new Language();
            l.ShortParse(script);
            var world = l.Execute("Hello world!");
            Assert.AreEqual("Hello world!", world.Output);
        }
    }
}
