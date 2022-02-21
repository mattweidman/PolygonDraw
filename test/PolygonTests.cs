using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace PolygonDraw.Tests
{
    public class PolygonTests
    {
        [Test]
        public void ContainsPoint_Square()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(0, 2), new Vector2(2, 2), new Vector2(2, 0),
            });

            // Right
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(3, 1)));
            // Left
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(-1, 1)));
            // Inside
            Assert.AreEqual(ContainmentType.INSIDE, polygon.ContainsPoint(new Vector2(1, 1)));
            // To the left aligned with bottom edge
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(-1, 0)));
            // To the left aligned with top edge
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(-1, 2)));
            // Above
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(0, 3)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(1, 3)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(2, 3)));
            // Below
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(0, -1)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(1, -1)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(2, -1)));

            // Edges
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(1, 0)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(2, 1)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(0, 1)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(1, 2)));

            // Corners
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(0, 0)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(0, 2)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(2, 2)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(2, 0)));
        }

        [Test]
        public void ContainsPoint_Diamond()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0), new Vector2(0, 2), new Vector2(2, 4), new Vector2(4, 2),
            });

            // Right
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(3, 0)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(4, 1)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(5, 2)));
            // Left
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(-1, 2)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(0, 1)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(0.5f, 1)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(1, 0)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(1.5f, 0)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(-5, 2)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(-5, 3)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(-5, 4)));
            // Inside
            Assert.AreEqual(ContainmentType.INSIDE, polygon.ContainsPoint(new Vector2(2, 2)));
            Assert.AreEqual(ContainmentType.INSIDE, polygon.ContainsPoint(new Vector2(2, 1)));
            Assert.AreEqual(ContainmentType.INSIDE, polygon.ContainsPoint(new Vector2(1, 2)));
            Assert.AreEqual(ContainmentType.INSIDE, polygon.ContainsPoint(new Vector2(3, 2)));
            Assert.AreEqual(ContainmentType.INSIDE, polygon.ContainsPoint(new Vector2(3.5f, 2)));
            Assert.AreEqual(ContainmentType.INSIDE, polygon.ContainsPoint(new Vector2(2.5f, 1)));
            // Above/below
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(2, 6)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(2, -5)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(1, 6)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(1, -5)));

            // Edges
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(1, 3)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(3, 3)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(3, 1)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(1, 1)));

            // Corners
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(2, 0)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(0, 2)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(2, 4)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(4, 2)));
        }

        [Test]
        public void ContainsPoint_Spikes()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 1),
                new Vector2(1, 2),
                new Vector2(2, 1),
                new Vector2(3, 2),
                new Vector2(4, 1),
                new Vector2(5, 1),
                new Vector2(6, 2),

                new Vector2(6, 1),
                new Vector2(5, 0),
                new Vector2(4, 0),
                new Vector2(3, 1),
                new Vector2(2, 0),
                new Vector2(1, 1),
                new Vector2(0, 0),
            });

            // Height 1.5
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(0, 1.5f)));
            Assert.AreEqual(ContainmentType.INSIDE, polygon.ContainsPoint(new Vector2(1, 1.5f)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(2, 1.5f)));
            Assert.AreEqual(ContainmentType.INSIDE, polygon.ContainsPoint(new Vector2(3, 1.5f)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(4, 1.5f)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(5, 1.5f)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(6, 1.5f)));

            // Height 1
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(-0.5f, 1)));
            Assert.AreEqual(ContainmentType.INSIDE, polygon.ContainsPoint(new Vector2(0.5f, 1)));
            Assert.AreEqual(ContainmentType.INSIDE, polygon.ContainsPoint(new Vector2(1.5f, 1)));
            Assert.AreEqual(ContainmentType.INSIDE, polygon.ContainsPoint(new Vector2(2.5f, 1)));
            Assert.AreEqual(ContainmentType.INSIDE, polygon.ContainsPoint(new Vector2(3.5f, 1)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(4.5f, 1)));
            Assert.AreEqual(ContainmentType.INSIDE, polygon.ContainsPoint(new Vector2(5.5f, 1)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(6.5f, 1)));

            // Height 0
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(-0.5f, 0)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(0.5f, 0)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(1.5f, 0)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(2.5f, 0)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(3.5f, 0)));
            Assert.AreEqual(ContainmentType.BOUNDARY, polygon.ContainsPoint(new Vector2(4.5f, 0)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(5.5f, 0)));
            Assert.AreEqual(ContainmentType.OUTSIDE, polygon.ContainsPoint(new Vector2(6.5f, 0)));
        }

        [Test]
        public void IsClockwise_Triangle()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(2, 2), new Vector2(4, 0),
            });
            TestIsClockwise(polygon);
        }

        [Test]
        public void IsClockwise_Square()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(0, 2), new Vector2(2, 2), new Vector2(2, 0),
            });
            TestIsClockwise(polygon);
        }

        [Test]
        public void IsClockwise_BisectCrossesLine()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(0, 2), new Vector2(2, 2), new Vector2(2, 0),
                new Vector2(1, 1), new Vector2(1, 0),
            });
            TestIsClockwise(polygon);
        }

        private void TestIsClockwise(Polygon clockwisePolygon)
        {
            Assert.IsTrue(clockwisePolygon.IsClockwise());
            Polygon counterclockwisePolygon = new Polygon(
                Enumerable.Reverse(clockwisePolygon.vertices).ToList());
            Assert.IsFalse(counterclockwisePolygon.IsClockwise());
        }
    }
}