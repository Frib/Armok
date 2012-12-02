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
            var world = Execute("+>>m<<d", "");
            Assert.AreEqual(0, world.Dwarves.First().Rocks);
            Assert.AreEqual(63, world.Cave[4].Rocks);
        }

        [TestMethod]
        public void OutputTraderTest()
        {
            var world = Execute("+>>mwmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm>mm<w<<<", "");
            Assert.AreEqual("A", world.OutputInText);
        }

        [TestMethod]
        public void InputTraderTest()
        {
            var world = Execute("+>>m<<wwwwwwwwwwwwwwwwwwwwwwwww<", "Hello world!");
            Assert.AreEqual("Hello world!", world.OutputInText);
        }

        [TestMethod]
        public void InfiniteInOutTest()
        {
            var world = Execute("+>>mwmm<w>mmdd<w->ww<w", "Hello world!");
            Assert.AreEqual("Hello world!", world.OutputInText);
        }

        [TestMethod]
        public void BrokerTest()
        {
            var world = Execute("+>>mmm<w>mmmdddm<<w>>mmmmmmmm<wwwwwwww<w<<", "");
            Assert.AreEqual(3, world.OutputInNumbers.FirstOrDefault());
        }

        private World Execute(string script, string input)
        {
            Language l = new Language();
            l.ShortParse(script);
            return l.Execute(input);
        }
    }
}
