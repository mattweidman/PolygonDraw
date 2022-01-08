using NUnit.Framework;
using PolygonDraw;
using System.Collections.Generic;

namespace PolygonDrawTests
{
    public class PolygonTests
    {
        private static readonly Polygon CLAW = new Polygon(new List<Vector2>()
        {
            new Vector2(0, 0),
            new Vector2(0, 5),
            new Vector2(5, 5),
            new Vector2(2, 4),
            new Vector2(2, 2),
            new Vector2(3, 1),
            new Vector2(5, 2),
            new Vector2(5, 0),
        });

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
            Assert.IsFalse(polygon.InViewOf(3, 1));
        }

        [Test]
        public void InViewOf_Claw()
        {
            Assert.IsFalse(CLAW.InViewOf(0, 0));
            Assert.IsFalse(CLAW.InViewOf(0, 1));
            Assert.IsFalse(CLAW.InViewOf(0, 2));
            Assert.IsTrue(CLAW.InViewOf(0, 3));
            Assert.IsTrue(CLAW.InViewOf(0, 4));
            Assert.IsTrue(CLAW.InViewOf(0, 5));
            Assert.IsFalse(CLAW.InViewOf(0, 6));
            Assert.IsFalse(CLAW.InViewOf(0, 7));

            Assert.IsFalse(CLAW.InViewOf(1, 7));
            Assert.IsFalse(CLAW.InViewOf(6, 2));
            Assert.IsFalse(CLAW.InViewOf(3, 4));
            Assert.IsFalse(CLAW.InViewOf(2, 7));

            Assert.IsTrue(CLAW.InViewOf(3, 1));
            Assert.IsTrue(CLAW.InViewOf(5, 7));
        }

        [Test]
        public void BeginsOutside_Inside()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(1, 1),
                new Vector2(2, 0),
            });

            Assert.IsFalse(polygon.BeginsOutside(1, new Vector2(1, 0)));
        }

        [Test]
        public void BeginsOutside_Outside()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(1, 1),
                new Vector2(2, 0),
            });

            Assert.IsTrue(polygon.BeginsOutside(1, new Vector2(1, 2)));
            Assert.IsTrue(polygon.BeginsOutside(1, new Vector2(0, 1)));
            Assert.IsTrue(polygon.BeginsOutside(1, new Vector2(2, 1)));
        }
    }
}