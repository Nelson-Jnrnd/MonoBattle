using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using TPI_2020_MonoBattle;
using static TPI_2020_MonoBattle.Constants;

namespace MonoBattle_Tests
{
    [TestClass]
    public class ShipTest
    {
        [TestMethod]
        public void TestShip()
        {
            int expectedLength = 3;
            Point expectedLocation = new Point(10, 10);
            Ship shipTest = new Ship(new Point(10, 10), 3);

            Assert.AreEqual(expectedLength, shipTest.Length);
            Assert.AreEqual(expectedLocation, shipTest.Position);
        }
        [TestMethod]
        public void TestShip2()
        {
            Color expectedInnerColor = Color.White;
            int expectedLength = 3;
            Point expectedPosition = new Point(10, 10);
            bool expectedSinkStatus = false;
            Ship shipTest = new Ship(new Rectangle(expectedPosition.X, expectedPosition.Y, 100, 100), 2, expectedInnerColor, Color.Black, expectedLength);

            Assert.AreEqual(expectedPosition, shipTest.Position);
            Assert.AreEqual(expectedLength, shipTest.Length);
            Assert.AreEqual(expectedInnerColor, shipTest.InnerColor);
            Assert.AreEqual(expectedSinkStatus, shipTest.IsSunk);
        }
        [TestMethod]
        public void TestIntersectWithTrue()
        {
            bool expectedResult = true;
            Point inputPoint = new Point(50, 50);
            Ship shipTest = new Ship(new Rectangle(0, 0, 100, 100), 2, Color.Black, Color.Red, 3);

            Assert.AreEqual(expectedResult, shipTest.IntersectWith(inputPoint));
        }

        [TestMethod]
        public void TestIntersectWithFalse()
        {
            bool expectedResult = false;
            Point inputPoint = new Point(500, 500);

            Ship shipTest = new Ship(new Rectangle(0, 0, 100, 100), 2, Color.Black, Color.Red, 3);

            Assert.AreEqual(expectedResult, shipTest.IntersectWith(inputPoint));
        }
        [TestMethod]
        public void TestRotate()
        {
            Ship shipTest = new Ship(new Point(10, 10), 3);
            bool expectedResult = !shipTest.IsVertical;

            shipTest.Rotate();
            Assert.AreEqual(expectedResult, shipTest.IsVertical);
        }
    }
}
