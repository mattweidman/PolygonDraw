using NUnit.Framework;

namespace PolygonDraw.Tests
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

        [Test]
        public void GetIntersection_HoriAngleSkew()
        {
            LineSegment segment1 = new LineSegment(new Vector2(0, 3), new Vector2(2, 3));
            LineSegment segment2 = new LineSegment(new Vector2(3, 5), new Vector2(4, 3));
            Vector2 intersection = segment1.GetIntersection(segment2, true);

            Assert.IsNull(intersection);
        }

        [Test]
        public void IntersectsPoint_Horizontal()
        {
            LineSegment ls = new LineSegment(new Vector2(-1, 1), new Vector2(3, 1));

            Assert.IsTrue(ls.IntersectsPoint(new Vector2(0, 1)));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(-1, 1)));
            Assert.IsTrue(ls.IntersectsPoint(new Vector2(-1, 1), true));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(3, 1)));
            Assert.IsTrue(ls.IntersectsPoint(new Vector2(3, 1), true));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(-1, 1)));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(4, 1)));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(0, 0)));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(0, 4)));
        }

        [Test]
        public void IntersectsPoint_Vertical()
        {
            LineSegment ls = new LineSegment(new Vector2(-1, 1), new Vector2(-1, 5));

            Assert.IsTrue(ls.IntersectsPoint(new Vector2(-1, 3)));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(-1, 1)));
            Assert.IsTrue(ls.IntersectsPoint(new Vector2(-1, 1), true));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(-1, 5)));
            Assert.IsTrue(ls.IntersectsPoint(new Vector2(-1, 5), true));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(-1, 0)));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(-1, 6)));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(2, 4)));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(-3, -5)));
        }

        [Test]
        public void IntersectsPoint_Diagonal()
        {
            LineSegment ls = new LineSegment(new Vector2(0, 0), new Vector2(2, 2));

            Assert.IsTrue(ls.IntersectsPoint(new Vector2(1, 1)));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(0, 0)));
            Assert.IsTrue(ls.IntersectsPoint(new Vector2(0, 0), true));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(2, 2)));
            Assert.IsTrue(ls.IntersectsPoint(new Vector2(2, 2), true));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(-1, -1)));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(5, 5)));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(3, 1)));
            Assert.IsFalse(ls.IntersectsPoint(new Vector2(4, -2)));
        }

        #region GetRelativeHorizontalPosition

        [Test]
        public void GetRelativeHorizontalPosition_UpRight()
        {
            LineSegment ls = new LineSegment(new Vector2(0, 0), new Vector2(2, 2));

            Assert.AreEqual(
                LineSegment.HorizontalPosition.LEFT,
                ls.GetRelativeHorizontalPosition(new Vector2(0.5f, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.LEFT,
                ls.GetRelativeHorizontalPosition(new Vector2(1, 1.5f)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.LEFT,
                ls.GetRelativeHorizontalPosition(new Vector2(-1, 0)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.RIGHT,
                ls.GetRelativeHorizontalPosition(new Vector2(1.5f, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.RIGHT,
                ls.GetRelativeHorizontalPosition(new Vector2(1, 0.5f)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.RIGHT,
                ls.GetRelativeHorizontalPosition(new Vector2(3, 2)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING,
                ls.GetRelativeHorizontalPosition(new Vector2(1, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING_AT_ENDPOINT,
                ls.GetRelativeHorizontalPosition(new Vector2(0, 0)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING_AT_ENDPOINT,
                ls.GetRelativeHorizontalPosition(new Vector2(2, 2)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(5, 3)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(-4, -1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(-1, -1)));
        }

        [Test]
        public void GetRelativeHorizontalPosition_UpLeft()
        {
            LineSegment ls = new LineSegment(new Vector2(2, 0), new Vector2(0, 2));

            Assert.AreEqual(
                LineSegment.HorizontalPosition.LEFT,
                ls.GetRelativeHorizontalPosition(new Vector2(0.5f, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.LEFT,
                ls.GetRelativeHorizontalPosition(new Vector2(1, 0.5f)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.LEFT,
                ls.GetRelativeHorizontalPosition(new Vector2(-1, 0)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.RIGHT,
                ls.GetRelativeHorizontalPosition(new Vector2(1.5f, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.RIGHT,
                ls.GetRelativeHorizontalPosition(new Vector2(1, 1.5f)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.RIGHT,
                ls.GetRelativeHorizontalPosition(new Vector2(3, 2)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING,
                ls.GetRelativeHorizontalPosition(new Vector2(1, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING_AT_ENDPOINT,
                ls.GetRelativeHorizontalPosition(new Vector2(2, 0)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING_AT_ENDPOINT,
                ls.GetRelativeHorizontalPosition(new Vector2(0, 2)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(5, 3)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(-4, -1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(3, -1)));
        }

        [Test]
        public void GetRelativeHorizontalPosition_DownRight()
        {
            LineSegment ls = new LineSegment(new Vector2(0, 2), new Vector2(2, 0));

            Assert.AreEqual(
                LineSegment.HorizontalPosition.LEFT,
                ls.GetRelativeHorizontalPosition(new Vector2(0.5f, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.LEFT,
                ls.GetRelativeHorizontalPosition(new Vector2(1, 0.5f)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.LEFT,
                ls.GetRelativeHorizontalPosition(new Vector2(-1, 0)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.RIGHT,
                ls.GetRelativeHorizontalPosition(new Vector2(1.5f, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.RIGHT,
                ls.GetRelativeHorizontalPosition(new Vector2(1, 1.5f)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.RIGHT,
                ls.GetRelativeHorizontalPosition(new Vector2(3, 2)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING,
                ls.GetRelativeHorizontalPosition(new Vector2(1, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING_AT_ENDPOINT,
                ls.GetRelativeHorizontalPosition(new Vector2(2, 0)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING_AT_ENDPOINT,
                ls.GetRelativeHorizontalPosition(new Vector2(0, 2)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(5, 3)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(-4, -1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(3, -1)));
        }

        [Test]
        public void GetRelativeHorizontalPosition_DownLeft()
        {
            LineSegment ls = new LineSegment(new Vector2(2, 2), new Vector2(0, 0));

            Assert.AreEqual(
                LineSegment.HorizontalPosition.LEFT,
                ls.GetRelativeHorizontalPosition(new Vector2(0.5f, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.LEFT,
                ls.GetRelativeHorizontalPosition(new Vector2(1, 1.5f)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.LEFT,
                ls.GetRelativeHorizontalPosition(new Vector2(-1, 0)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.RIGHT,
                ls.GetRelativeHorizontalPosition(new Vector2(1.5f, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.RIGHT,
                ls.GetRelativeHorizontalPosition(new Vector2(1, 0.5f)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.RIGHT,
                ls.GetRelativeHorizontalPosition(new Vector2(3, 2)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING,
                ls.GetRelativeHorizontalPosition(new Vector2(1, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING_AT_ENDPOINT,
                ls.GetRelativeHorizontalPosition(new Vector2(0, 0)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING_AT_ENDPOINT,
                ls.GetRelativeHorizontalPosition(new Vector2(2, 2)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(5, 3)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(-4, -1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(-1, -1)));
        }

        [Test]
        public void GetRelativeHorizontalPosition_Horizontal()
        {
            LineSegment ls = new LineSegment(new Vector2(-1, 1), new Vector2(1, 1));

            Assert.AreEqual(
                LineSegment.HorizontalPosition.LEFT,
                ls.GetRelativeHorizontalPosition(new Vector2(-2, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING_AT_ENDPOINT,
                ls.GetRelativeHorizontalPosition(new Vector2(-1, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING,
                ls.GetRelativeHorizontalPosition(new Vector2(0, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING_AT_ENDPOINT,
                ls.GetRelativeHorizontalPosition(new Vector2(1, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.RIGHT,
                ls.GetRelativeHorizontalPosition(new Vector2(2, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(0, 2)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(0, 0)));
        }

        [Test]
        public void GetRelativeHorizontalPosition_Vertical()
        {
            LineSegment ls = new LineSegment(new Vector2(0, -1), new Vector2(0, 1));

            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(0, -2)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING_AT_ENDPOINT,
                ls.GetRelativeHorizontalPosition(new Vector2(0, -1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING,
                ls.GetRelativeHorizontalPosition(new Vector2(0, 0)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.INTERSECTING_AT_ENDPOINT,
                ls.GetRelativeHorizontalPosition(new Vector2(0, 1)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.NOT_ALIGNED,
                ls.GetRelativeHorizontalPosition(new Vector2(0, 2)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.LEFT,
                ls.GetRelativeHorizontalPosition(new Vector2(-1, 0)));
            Assert.AreEqual(
                LineSegment.HorizontalPosition.RIGHT,
                ls.GetRelativeHorizontalPosition(new Vector2(1, 0)));
        }

        #endregion

        [Test]
        public void GetLineIntersectionDistances_Negatives()
        {
            LineSegment l1 = new LineSegment(new Vector2(4, 0), new Vector2(6, 0));
            LineSegment l2 = new LineSegment(new Vector2(0, 4), new Vector2(0, 6));
            (float, float)? expected = (-2, -2);
            Assert.AreEqual(expected, l1.GetLineIntersectionDistances(l2));
            Assert.AreEqual(expected, l2.GetLineIntersectionDistances(l1));
        }

        [Test]
        public void GetLineIntersectionDistances_HighPositives()
        {
            LineSegment l1 = new LineSegment(new Vector2(-2, 2), new Vector2(-1, 1));
            LineSegment l2 = new LineSegment(new Vector2(2, 2), new Vector2(1, 1));
            (float, float)? expected = (2, 2);
            Assert.AreEqual(expected, l1.GetLineIntersectionDistances(l2));
            Assert.AreEqual(expected, l2.GetLineIntersectionDistances(l1));
        }

        [Test]
        public void GetLineIntersectionDistances_Intersecting()
        {
            LineSegment l1 = new LineSegment(new Vector2(0, 0), new Vector2(4, 2));
            LineSegment l2 = new LineSegment(new Vector2(0, 2), new Vector2(4, 0));
            (float, float)? expected = (0.5f, 0.5f);
            Assert.AreEqual(expected, l1.GetLineIntersectionDistances(l2));
            Assert.AreEqual(expected, l2.GetLineIntersectionDistances(l1));
        }

        [Test]
        public void GetLineIntersectionDistances_Connected()
        {
            LineSegment l1 = new LineSegment(new Vector2(0, 0), new Vector2(1, 0));
            LineSegment l2 = new LineSegment(new Vector2(1, 0), new Vector2(-1, 3));
            Assert.AreEqual((1, 0), l1.GetLineIntersectionDistances(l2));
            Assert.AreEqual((0, 1), l2.GetLineIntersectionDistances(l1));
        }

        [Test]
        public void GetLineIntersectionDistances_Parallel()
        {
            LineSegment l1 = new LineSegment(new Vector2(0, 0), new Vector2(1, 0));
            LineSegment l2 = new LineSegment(new Vector2(1, 1), new Vector2(0, 1));
            Assert.IsNull(l1.GetLineIntersectionDistances(l2));
            Assert.IsNull(l2.GetLineIntersectionDistances(l1));
        }
    }
}