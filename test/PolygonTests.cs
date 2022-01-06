using NUnit.Framework;
using PolygonDraw;
using System.Collections.Generic;

namespace PolygonDrawTests
{
    public class PolygonTests
    {
        [Test]
        public void InViewOf_SquareDiagonal()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 5),
                new Vector2(5, 5),
                new Vector2(5, 0),
            });

            Assert.IsTrue(polygon.InViewOf(0, 2));
            Assert.IsTrue(polygon.InViewOf(3, 1));
        }

        [Test]
        public void InViewOf_SquareSide()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 5),
                new Vector2(5, 5),
                new Vector2(5, 0),
            });

            Assert.IsFalse(polygon.InViewOf(1, 0));
            Assert.IsFalse(polygon.InViewOf(0, 3));
        }

        [Test]
        public void InViewOf_SpikeTouchesSegment()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 3),
                new Vector2(1, 5),
                new Vector2(2, 3),
                new Vector2(3, 5),
                new Vector2(4, 3),
                new Vector2(4, 0),
                new Vector2(0, 0),
            });

            Assert.IsFalse(polygon.InViewOf(0, 4));
            Assert.IsTrue(polygon.InViewOf(0, 2));
            Assert.IsTrue(polygon.InViewOf(4, 2));
        }

        [Test]
        public void InViewOf_SegmentOutsideOfPolygon()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 3),
                new Vector2(1, 5),
                new Vector2(2, 3),
                new Vector2(3, 5),
                new Vector2(4, 3),
                new Vector2(4, 0),
                new Vector2(0, 0),
            });

            Assert.IsFalse(polygon.InViewOf(1, 3));
        }
    }
}