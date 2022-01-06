using PolygonDraw;
using NUnit.Framework;

namespace PolygonDrawTests
{
    public class LineSegmentTests
    {
        [Test]
        public void GetIntersection_X()
        {
            LineSegment segment1 = new LineSegment(new Vector2(0, 0), new Vector2(2, 2));
            LineSegment segment2 = new LineSegment(new Vector2(0, 2), new Vector2(2, 0));
            Vector2 intersection = segment1.GetIntersection(segment2);

            PolygonDrawAssert.AreEqual(new Vector2(1, 1), intersection);
        }

        [Test]
        public void GetIntersection_Plus()
        {
            LineSegment segment1 = new LineSegment(new Vector2(1, 0), new Vector2(1, 2));
            LineSegment segment2 = new LineSegment(new Vector2(2, 1), new Vector2(0, 1));
            Vector2 intersection = segment1.GetIntersection(segment2);

            PolygonDrawAssert.AreEqual(new Vector2(1, 1), intersection);
        }

        [Test]
        public void GetIntersection_Parallel()
        {
            LineSegment segment1 = new LineSegment(new Vector2(0, 0), new Vector2(1, 1));
            LineSegment segment2 = new LineSegment(new Vector2(1, 0), new Vector2(2, 1));
            Vector2 intersection = segment1.GetIntersection(segment2);

            Assert.IsNull(intersection);
        }

        [Test]
        public void GetIntersection_EndpointIntersects()
        {
            LineSegment segment1 = new LineSegment(new Vector2(-5, -2), new Vector2(3, -2));
            LineSegment segment2 = new LineSegment(new Vector2(2, -2), new Vector2(-1, 1));
            Vector2 intersection = segment1.GetIntersection(segment2, true);

            PolygonDrawAssert.AreEqual(new Vector2(2, -2), intersection);
        }

        [Test]
        public void GetIntersection_EndpointDoesntIntersect()
        {
            LineSegment segment1 = new LineSegment(new Vector2(-5, -2), new Vector2(3, -2));
            LineSegment segment2 = new LineSegment(new Vector2(2, -2), new Vector2(-1, 1));
            Vector2 intersection = segment1.GetIntersection(segment2);

            Assert.IsNull(intersection);
        }

        [Test]
        public void GetIntersection_NotTouchingAbove()
        {
            LineSegment segment1 = new LineSegment(new Vector2(-1, 0), new Vector2(1, 0));
            LineSegment segment2 = new LineSegment(new Vector2(0, 1), new Vector2(0, 2));
            Vector2 intersection = segment1.GetIntersection(segment2);

            Assert.IsNull(intersection);
        }

        [Test]
        public void GetIntersection_NotTouchingBelow()
        {
            LineSegment segment1 = new LineSegment(new Vector2(-1, 0), new Vector2(1, 0));
            LineSegment segment2 = new LineSegment(new Vector2(0, -2), new Vector2(0, -1));
            Vector2 intersection = segment1.GetIntersection(segment2);

            Assert.IsNull(intersection);
        }

        [Test]
        public void GetIntersection_NotTouchingLeft()
        {
            LineSegment segment1 = new LineSegment(new Vector2(-1, 0), new Vector2(1, 0));
            LineSegment segment2 = new LineSegment(new Vector2(-3, 0), new Vector2(-2, 0));
            Vector2 intersection = segment1.GetIntersection(segment2);

            Assert.IsNull(intersection);
        }

        [Test]
        public void GetIntersection_NotTouchingRight()
        {
            LineSegment segment1 = new LineSegment(new Vector2(-1, 0), new Vector2(1, 0));
            LineSegment segment2 = new LineSegment(new Vector2(2, 0), new Vector2(3, 0));
            Vector2 intersection = segment1.GetIntersection(segment2);

            Assert.IsNull(intersection);
        }

        [Test]
        public void GetIntersection_EndpointsTouch()
        {
            LineSegment segment1 = new LineSegment(new Vector2(-1, 0), new Vector2(1, 0));
            LineSegment segment2 = new LineSegment(new Vector2(1, 0), new Vector2(0, 1));
            Vector2 intersection = segment1.GetIntersection(segment2, true);

            PolygonDrawAssert.AreEqual(new Vector2(1, 0), intersection);
        }
    }
}