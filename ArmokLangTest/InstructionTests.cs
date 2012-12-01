using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArmokLanguage;

namespace ArmokLangTest
{
    [TestClass]
    public class InstructionTests
    {
        [TestMethod]
        public void MoveLeftTest()
        {
            Dictionary<string, List<Instruction>> routines = new Dictionary<string, List<Instruction>>();
            routines.Add("main", new List<Instruction>() { new MoveLeft() });
            World w = new World(new List<Dwarf>() { new Dwarf("Urist", routines["main"]) }, routines);
            w.Dwarves[0].Position = 2;

            Assert.AreEqual(2, w.Dwarves[0].Position);
            w.Run("");
            Assert.AreEqual(1, w.Dwarves[0].Position);
        }

        [TestMethod]
        public void MoveRightTest()
        {
            Dictionary<string, List<Instruction>> routines = new Dictionary<string, List<Instruction>>();
            routines.Add("main", new List<Instruction>() { new MoveRight() });
            World w = new World(new List<Dwarf>() { new Dwarf("Urist", routines["main"]) }, routines);
            w.Dwarves[0].Position = 2;

            Assert.AreEqual(2, w.Dwarves[0].Position);
            w.Run("");
            Assert.AreEqual(3, w.Dwarves[0].Position);
        }

        [TestMethod]
        public void MoveRightIntoWallTest()
        {
            Dictionary<string, List<Instruction>> routines = new Dictionary<string, List<Instruction>>();
            routines.Add("main", new List<Instruction>() { new MoveRight() });
            World w = new World(new List<Dwarf>() { new Dwarf("Urist", routines["main"]) }, routines);
            w.Dwarves[0].Position = 3;

            Assert.AreEqual(3, w.Dwarves[0].Position);
            w.Run("");
            Assert.AreEqual(3, w.Dwarves[0].Position);
        }

        [TestMethod]
        public void MoveRightLeftTest()
        {
            Dictionary<string, List<Instruction>> routines = new Dictionary<string, List<Instruction>>();
            routines.Add("main", new List<Instruction>() { new MoveRight(), new MoveLeft() });
            World w = new World(new List<Dwarf>() { new Dwarf("Urist", routines["main"]) }, routines);
            w.Dwarves[0].Position = 2;

            w.Run("");
            Assert.AreEqual(2, w.Dwarves[0].Position);
        }

        [TestMethod]
        public void MoveLeftIntoMagmaTest()
        {
            Dictionary<string, List<Instruction>> routines = new Dictionary<string, List<Instruction>>();
            routines.Add("main", new List<Instruction>() { new MoveLeft(), new MoveRight() });
            World w = new World(new List<Dwarf>() { new Dwarf("Urist", routines["main"]) }, routines);
            w.Dwarves[0].Position = 1;

            Assert.AreEqual(1, w.Dwarves[0].Position);
            w.Run("");
            Assert.AreEqual(0, w.Dwarves[0].Position);
        }

        [TestMethod]
        public void MineTest()
        {
            Dictionary<string, List<Instruction>> routines = new Dictionary<string, List<Instruction>>();
            routines.Add("main", new List<Instruction>() { new Mine() });
            World w = new World(new List<Dwarf>() { new Dwarf("Urist", routines["main"]) }, routines);
            w.Dwarves[0].Position = 3;

            Assert.AreEqual(0, w.Dwarves[0].Rocks);
            Assert.AreEqual(-1, w.Cave[4].Rocks);
            Assert.AreEqual(5, w.Cave.Count);
            w.Run("");
            Assert.AreEqual(1, w.Dwarves[0].Rocks);
            Assert.AreEqual(63, w.Cave[4].Rocks);
            Assert.AreEqual(-1, w.Cave[5].Rocks);
            Assert.AreEqual(6, w.Cave.Count);
        }

        [TestMethod]
        public void DumpTest()
        {
            Dictionary<string, List<Instruction>> routines = new Dictionary<string, List<Instruction>>();
            routines.Add("main", new List<Instruction>() { new Dump() });
            World w = new World(new List<Dwarf>() { new Dwarf("Urist", routines["main"]) }, routines);
            w.Dwarves[0].Position = 3;
            w.Dwarves[0].Rocks = 1;

            w.Run("");
            Assert.AreEqual(0, w.Dwarves[0].Rocks);
            Assert.AreEqual(1, w.Cave[2].Rocks);
        }
    }
}
