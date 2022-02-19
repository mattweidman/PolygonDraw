using NUnit.Framework;
using PolygonDraw;
using System.Collections.Generic;

namespace PolygonDrawTests
{
    public class TriangleTests
    {
        #region ContainsPoint

        [Test]
        public void ContainsPoint_RightTriangle_BottomLeftCorner()
        {
            Triangle t = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));

            Assert.IsTrue(t.ContainsPoint(new Vector2(1, 1)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(1, 1), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(3, 3)));
            Assert.IsFalse(t.ContainsPoint(new Vector2(-1, -1), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(0, 2)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(0, 2), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(3, 1)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(3, 1), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(0, 5)));
            Assert.IsFalse(t.ContainsPoint(new Vector2(0, 5), true));
        }

        [Test]
        public void ContainsPoint_RightTriangle_TopRightCorner()
        {
            Triangle t = new Triangle(new Vector2(0, 4), new Vector2(4, 4), new Vector2(4, 0));

            Assert.IsTrue(t.ContainsPoint(new Vector2(3, 3)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(3, 3), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(5, -1)));
            Assert.IsFalse(t.ContainsPoint(new Vector2(5, 2), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(4, 4)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(4, 4), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(2, 2)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(2, 2), true));
        }

        [Test]
        public void ContainsPoint_NonRightTriangle()
        {
            Triangle t = new Triangle(new Vector2(0, 1), new Vector2(3, 0), new Vector2(-2, -2));

            Assert.IsTrue(t.ContainsPoint(new Vector2(0, 0)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(1, 0), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(1, -2)));
            Assert.IsFalse(t.ContainsPoint(new Vector2(-1, 0), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(3, 0)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(0, 1), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(0, -1.2f)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(0, -1.2f), true));
        }

        #endregion

        #region ContainsTriangle

        [Test]
        public void ContainsTriangle()
        {
            Triangle t = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));

            // All inside
            Assert.IsTrue(t.ContainsTriangle(
                new Triangle(new Vector2(1, 1), new Vector2(1, 2), new Vector2(2, 1))));

            // All or some outside
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(4, 4), new Vector2(4, 3), new Vector2(3, 4))));
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(1, 1), new Vector2(1, 4), new Vector2(2, 1))));
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(1, 3), new Vector2(4, 4), new Vector2(2, 2))));
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(1, 3), new Vector2(4, 4), new Vector2(2, 2)), true));
            
            // On vertex
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 1))));
            Assert.IsTrue(t.ContainsTriangle(
                new Triangle(new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 1)), true));
            
            // On edge - don't include edges
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(0, 1), new Vector2(1, 2), new Vector2(2, 1))));
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(1, 0), new Vector2(0, 1), new Vector2(2, 2))));
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(1, 1), new Vector2(1, 3), new Vector2(2, 2))));
            
            // On edge - include edges
            Assert.IsTrue(t.ContainsTriangle(
                new Triangle(new Vector2(0, 1), new Vector2(1, 2), new Vector2(2, 1)), true));
            Assert.IsTrue(t.ContainsTriangle(
                new Triangle(new Vector2(1, 0), new Vector2(0, 1), new Vector2(2, 2)), true));
            Assert.IsTrue(t.ContainsTriangle(
                new Triangle(new Vector2(1, 1), new Vector2(1, 3), new Vector2(2, 2)), true));
        }

        #endregion

        #region IsValidTriangle

        [Test]
        public void IsValidTriangle_Valid()
        {
            Triangle tri = new Triangle(new Vector2(0, 0), new Vector2(1, 0.01f), new Vector2(2, 0));
            Assert.IsTrue(tri.IsValidTriangle());
        }

        [Test]
        public void IsValidTriangle_Invalid_StartLeft()
        {
            Triangle tri = new Triangle(new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0));
            Assert.IsFalse(tri.IsValidTriangle());
        }

        [Test]
        public void IsValidTriangle_Invalid_StartInMiddle()
        {
            Triangle tri = new Triangle(new Vector2(1, 0), new Vector2(0, 0), new Vector2(2, 0));
            Assert.IsFalse(tri.IsValidTriangle());
        }

        [Test]
        public void IsValidTriangle_Invalid_StartRight()
        {
            Triangle tri = new Triangle(new Vector2(2, 0), new Vector2(0, 0), new Vector2(1, 0));
            Assert.IsFalse(tri.IsValidTriangle());
        }

        #endregion
    }
}