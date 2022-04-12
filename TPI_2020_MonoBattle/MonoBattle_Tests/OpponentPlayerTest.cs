using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using TPI_2020_MonoBattle;
using static TPI_2020_MonoBattle.Constants;

namespace MonoBattle_Tests
{
    [TestClass]
    public class OpponentPlayerTest
    {
        [TestMethod]
        public void TestOpponentPlayer()
        {
            Grid expectedGridResult = new Grid(new Point(10, 10));
            OpponentPlayer player = new OpponentPlayer(expectedGridResult);

            Assert.AreEqual(expectedGridResult, player.PlayerGrid);
        }
        [TestMethod]
        public void TestReceiveShotMiss()
        {
            Grid.SquareType expectedResult = Grid.SquareType.miss;
            Grid gridTest = new Grid(new Point(10, 10));
            OpponentPlayer player = new OpponentPlayer(gridTest);

            player.ReceiveShot(new Point(0, 0), MISS_MESSAGE);
            Assert.AreEqual(expectedResult, player.PlayerGrid.SquareGrid[0,0]);
        }
        [TestMethod]
        public void TestReceiveShotHit()
        {
            Grid.SquareType expectedResult = Grid.SquareType.hit;
            Grid gridTest = new Grid(new Point(10, 10));
            OpponentPlayer player = new OpponentPlayer(gridTest);

            player.ReceiveShot(new Point(0, 0), HIT_MESSAGE);
            Assert.AreEqual(expectedResult, player.PlayerGrid.SquareGrid[0, 0]);
        }
        [TestMethod]
        public void TestReceiveShotSink()
        {
            Grid.SquareType expectedResult = Grid.SquareType.sunk;
            Grid gridTest = new Grid(new Point(10, 10));
            OpponentPlayer player = new OpponentPlayer(gridTest);

            player.ReceiveShot(new Point(0, 0), SINK_MESSAGE + TYPE_SEPARATOR + ";0,0;0,1");
            Assert.AreEqual(expectedResult, player.PlayerGrid.SquareGrid[0, 0]);
            Assert.AreEqual(expectedResult, player.PlayerGrid.SquareGrid[0, 1]);
        }
    }
}
