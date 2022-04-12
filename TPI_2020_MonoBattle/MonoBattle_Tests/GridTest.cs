using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using TPI_2020_MonoBattle;
using static TPI_2020_MonoBattle.Constants;

namespace MonoBattle_Tests
{
    [TestClass]
    public class GridTest
    {
        [TestMethod]
        public void TestGrid()
        {
            Point expectedResult = new Point(0, 0);
            Grid grid = new Grid(expectedResult);

            Assert.AreEqual(expectedResult, grid.AnchorPoint);
        }
        [TestMethod]
        public void TestInitializeGrid()
        {
            int expectedResult = NB_SQUARE_ROW;
            Grid grid = new Grid(new Point(0, 0));

            Assert.AreEqual(expectedResult, grid.SquareGrid.GetLength(0));
            Assert.AreEqual(expectedResult, grid.SquareGrid.GetLength(1));
        }
        [TestMethod]
        public void TestFindSquare00()
        {
            Point expectedResult = new Point(0, 0);
            Grid grid = new Grid(new Point(0, 0));

            Assert.AreEqual(expectedResult, grid.FindSquare(new Point(0, 0)));
        }
        [TestMethod]
        public void TestFindSquare11()
        {
            Point expectedResult = new Point(1, 1);
            Grid grid = new Grid(new Point(0, 0));

            Assert.AreEqual(expectedResult, grid.FindSquare(new Point(SQUARE_SIZE, SQUARE_SIZE)));
        }
        [TestMethod]
        public void TestFindSquareException()
        {
            Grid grid = new Grid(new Point(0, 0));

            try
            {
                grid.FindSquare(new Point(SQUARE_SIZE * NB_SQUARE_ROW + 1));
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void TestFindClosestSquareAnchor00()
        {
            Point expectedResult = new Point(0, 0);
            Grid grid = new Grid(new Point(0, 0));

            Assert.AreEqual(expectedResult, grid.FindClosestSquareAnchor(new Point(0, 0)));
        }
        [TestMethod]
        public void TestFindClosestSquareAnchor11()
        {
            Point expectedResult = new Point(SQUARE_SIZE, SQUARE_SIZE);
            Grid grid = new Grid(new Point(0, 0));

            Assert.AreEqual(expectedResult, grid.FindClosestSquareAnchor(new Point(SQUARE_SIZE, SQUARE_SIZE)));
        }
    }
}
