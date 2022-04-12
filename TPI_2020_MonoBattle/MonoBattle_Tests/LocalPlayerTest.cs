using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using TPI_2020_MonoBattle;
using static TPI_2020_MonoBattle.Constants;

namespace MonoBattle_Tests
{
    [TestClass]
    public class LocalPlayerTest
    {
        [TestMethod]
        public void TestLocalPlayer()
        {
            Grid expectedGridResult = new Grid(new Point(10, 10));
            List<Ship> shipsTest = new List<Ship>() { new Ship(new Point(1, 1), 2) };
            LocalPlayer player = new LocalPlayer(expectedGridResult, shipsTest);

            Assert.AreEqual(expectedGridResult, player.PlayerGrid);
        }
        [TestMethod]
        public void TestReceiveShootMiss()
        {
            string expectedResult = MISS_MESSAGE;
            Grid gridTest = new Grid(new Point(0, 0));
            List<Ship> shipsTest = new List<Ship>() { new Ship(new Point(0, 0), 2) };
            LocalPlayer player = new LocalPlayer(gridTest, shipsTest);

            Assert.AreEqual(expectedResult, player.ReceiveShot(new Point(5, 5)));
        }
        [TestMethod]
        public void TestReceiveShootHit()
        {
            string expectedResult = HIT_MESSAGE;
            Grid gridTest = new Grid(new Point(0, 0));
            List<Ship> shipsTest = new List<Ship>() { new Ship(new Point(0, 0), 2) };
            LocalPlayer player = new LocalPlayer(gridTest, shipsTest);
            player.ValidateShips(true);

            Assert.AreEqual(expectedResult, player.ReceiveShot(new Point(0, 0)));
        }
        [TestMethod]
        public void TestReceiveShootSink()
        {
            string expectedResult = SINK_MESSAGE;
            Grid gridTest = new Grid(new Point(0, 0));
            List<Ship> shipsTest = new List<Ship>() { new Ship(new Point(0, 0), 1) , new Ship(new Point(50, 50), 1) };
            LocalPlayer player = new LocalPlayer(gridTest, shipsTest);
            player.ValidateShips(true);

            Assert.IsTrue(player.ReceiveShot(new Point(0, 0)).Contains(expectedResult));
        }
        [TestMethod]
        public void TestReceiveShootFinish()
        {
            string expectedResult = GAME_FINISHED_MESSAGE;
            Grid gridTest = new Grid(new Point(0, 0));
            List<Ship> shipsTest = new List<Ship>() { new Ship(new Point(0, 0), 1)};
            LocalPlayer player = new LocalPlayer(gridTest, shipsTest);
            player.ValidateShips(true);

            Assert.AreEqual(expectedResult, player.ReceiveShot(new Point(0, 0)));
        }
        [TestMethod]
        public void TestReceiveShootError()
        {
            string expectedResult = ERROR_MESSAGE;
            Grid gridTest = new Grid(new Point(0, 0));
            List<Ship> shipsTest = new List<Ship>() { new Ship(new Point(0, 0), 2) };
            LocalPlayer player = new LocalPlayer(gridTest, shipsTest);
            player.ValidateShips(true);

            Assert.AreEqual(expectedResult, player.ReceiveShot(new Point(50, 50)));
        }
        [TestMethod]
        public void TestValidateShipFalse()
        {
            bool expectedResult = false;
            Grid gridTest = new Grid(new Point(0, 0));
            List<Ship> shipsTest = new List<Ship>() { new Ship(new Point(500, 500), 2) };
            LocalPlayer player = new LocalPlayer(gridTest, shipsTest);
           

            Assert.AreEqual(expectedResult, player.ValidateShips(true));
        }
        [TestMethod]
        public void TestValidateShipTrue()
        {
            bool expectedResult = true;
            Grid gridTest = new Grid(new Point(0, 0));
            List<Ship> shipsTest = new List<Ship>() { new Ship(new Point(0, 0), 2) };
            LocalPlayer player = new LocalPlayer(gridTest, shipsTest);


            Assert.AreEqual(expectedResult, player.ValidateShips(true));
        }
    }
}
