using NUnit.Framework;
using PolygonDraw;
using System.Collections.Generic;

namespace PolygonDrawTests
{
    public class IntersectionDataTests
    {
        [Test]
        public void GetIntersectionType_EdgeCrossLow()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0),
                new Vector2(0, 2),
                new Vector2(2, 4),
            });

            IntersectionData intersect = new IntersectionData(poly1, 2, 0.5f, poly2, 0, 0.5f);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.OVERLAPPING, intersect.GetIntersectionType());
            Assert.IsFalse(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_EdgeCrossHigh()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0),
                new Vector2(0, 2),
                new Vector2(2, 4),
            });

            IntersectionData intersect = new IntersectionData(poly1, 1, 0.5f, poly2, 1, 0.5f);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.OVERLAPPING, intersect.GetIntersectionType());
            Assert.IsTrue(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_OuterVertices()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(4, 0),
                new Vector2(2, 2),
                new Vector2(4, 4),
            });

            IntersectionData intersect = new IntersectionData(poly1, 2, 0, poly2, 1, 0);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.OUTER, intersect.GetIntersectionType());
            Assert.IsTrue(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_OuterEdge_ClipVertex()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(-2, 0),
                new Vector2(-2, 4),
                new Vector2(0, 2),
            });

            IntersectionData intersect = new IntersectionData(poly1, 0, 0.5f, poly2, 2, 0);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.OUTER, intersect.GetIntersectionType());
            Assert.IsFalse(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_OuterEdge_SubjectVertex()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0),
                new Vector2(2, 4),
                new Vector2(4, 2),
            });

            IntersectionData intersect = new IntersectionData(poly1, 2, 0, poly2, 0, 0.5f);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.OUTER, intersect.GetIntersectionType());
            Assert.IsTrue(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_P1VertexContainsP2Vertex()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 8),
                new Vector2(4, 4),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 3),
                new Vector2(2, 5),
                new Vector2(4, 4),
            });

            IntersectionData intersect = new IntersectionData(poly1, 2, 0, poly2, 2, 0);

            AssertContainsIntersectionType(poly1, poly2, intersect);
        }

        [Test]
        public void GetIntersectionType_P1EdgeContainsP2Vertex()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 8),
                new Vector2(4, 4),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 3),
                new Vector2(0, 4),
                new Vector2(2, 5),
            });

            IntersectionData intersect = new IntersectionData(poly1, 0, 0.5f, poly2, 1, 0);

            AssertContainsIntersectionType(poly1, poly2, intersect);
        }

        [Test]
        public void GetIntersectionType_P1VertexContainsP2Edge()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 8),
                new Vector2(8, 8),
                new Vector2(4, 4),
                new Vector2(8, 0),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 4),
                new Vector2(4, 5),
                new Vector2(4, 3),
            });

            IntersectionData intersect = new IntersectionData(poly1, 3, 0, poly2, 1, 0.5f);

            AssertContainsIntersectionType(poly1, poly2, intersect);
        }

        [Test]
        public void GetIntersectionType_SplitVertices()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 8),
                new Vector2(8, 8),
                new Vector2(4, 4),
                new Vector2(8, 0),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(4, 4),
                new Vector2(2, 6),
                new Vector2(6, 6),
                new Vector2(6, 2),
                new Vector2(2, 2),
            });

            IntersectionData intersect = new IntersectionData(poly1, 3, 0, poly2, 0, 0);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.SPLIT, intersect.GetIntersectionType());
            Assert.IsFalse(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_SplitVertexAndEdge()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 8),
                new Vector2(8, 8),
                new Vector2(4, 4),
                new Vector2(8, 0),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(4, 4),
                new Vector2(2, 6),
                new Vector2(6, 6),
                new Vector2(6, 2),
                new Vector2(2, 2),
            });

            IntersectionData intersect = new IntersectionData(poly1, 3, 0, poly2, 0, 0);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.SPLIT, intersect.GetIntersectionType());
            Assert.IsFalse(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_OverlappingVertices()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 2),
                new Vector2(1, 2),
                new Vector2(2, 4),
            });

            IntersectionData intersect = new IntersectionData(poly1, 2, 0, poly2, 0, 0);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.OVERLAPPING, intersect.GetIntersectionType());
            Assert.IsFalse(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_OverlappingVertexAndEdge()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(1, 2),
                new Vector2(2, 4),
                new Vector2(3, 2),
            });

            IntersectionData intersect = new IntersectionData(poly1, 2, 0, poly2, 2, 0.5f);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.OVERLAPPING, intersect.GetIntersectionType());
            Assert.IsFalse(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_SharedEdge_VertexVertex_Outer()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(-2, 2),
                new Vector2(0, 4),
            });

            IntersectionData intersect = new IntersectionData(poly1, 0, 0, poly2, 0, 0);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.OUTER, intersect.GetIntersectionType());
            Assert.IsTrue(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_SharedEdge_VertexEdge_Outer()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(-2, 2),
                new Vector2(0, 2),
            });

            IntersectionData intersect = new IntersectionData(poly1, 0, 0.5f, poly2, 2, 0);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.OUTER, intersect.GetIntersectionType());
            Assert.IsFalse(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_SharedEdge_VertexVertex_TopCovered1()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 2),
                new Vector2(0, 2),
                new Vector2(0, 4),
            });

            IntersectionData intersect = new IntersectionData(poly1, 2, 0, poly2, 0, 0);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.OVERLAPPING, intersect.GetIntersectionType());
            Assert.IsFalse(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_SharedEdge_VertexEdge_TopCovered2()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 2),
                new Vector2(0, 2),
                new Vector2(0, 4),
            });

            IntersectionData intersect = new IntersectionData(poly1, 0, 0.5f, poly2, 1, 0);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.OVERLAPPING, intersect.GetIntersectionType());
            Assert.IsTrue(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_SharedEdge_VertexVertex_BottomCovered()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 2),
                new Vector2(0, 0),
                new Vector2(0, 2),
            });

            IntersectionData intersect = new IntersectionData(poly1, 2, 0, poly2, 0, 0);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.OVERLAPPING, intersect.GetIntersectionType());
            Assert.IsTrue(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_SharedEdge_VertexVertex_Overlapping()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(2, -2),
                new Vector2(-2, -2),
                new Vector2(-2, 2),
                new Vector2(0, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(2, 2),
                new Vector2(2, -2),
                new Vector2(-2, -2),
            });

            IntersectionData intersect = new IntersectionData(poly1, 0, 0, poly2, 0, 0);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.OVERLAPPING, intersect.GetIntersectionType());
            Assert.IsTrue(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_SameVertex()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(1, 1),
                new Vector2(1, 3),
                new Vector2(2, 2),
            });

            IntersectionData intersect = new IntersectionData(poly1, 2, 0, poly2, 2, 0);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.POLY2_CONTAINS_POLY1, intersect.GetIntersectionType());
            Assert.IsFalse(intersect.IsStarter());
        }

        [Test]
        public void GetIntersectionType_ReversedVertex()
        {
            Polygon poly1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            Polygon poly2 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(2, 2),
                new Vector2(0, 4),
                new Vector2(3, 4),
                new Vector2(0, 4),
            });

            IntersectionData intersect = new IntersectionData(poly1, 2, 0, poly2, 1, 0);

            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.OUTER, intersect.GetIntersectionType());
            Assert.IsTrue(intersect.IsStarter());
        }

        private void AssertIntersectionPointsMatch(IntersectionData intersect)
        {
            PolygonDrawAssert.AreEqual(intersect.poly1.intersectionPoint, intersect.poly2.intersectionPoint);
        }

        private void AssertContainsIntersectionType(Polygon poly1, Polygon poly2, IntersectionData intersect)
        {
            AssertIntersectionPointsMatch(intersect);
            PolygonDrawAssert.AreEqual(
                IntersectionType.POLY1_CONTAINS_POLY2, intersect.GetIntersectionType());
            Assert.IsTrue(intersect.IsStarter());
            
            IntersectionData intersect2 = new IntersectionData(
                intersect.poly2.polygon,
                intersect.poly2.edgeIndex,
                intersect.poly2.distanceAlongEdge,
                intersect.poly1.polygon,
                intersect.poly1.edgeIndex,
                intersect.poly1.distanceAlongEdge);

            PolygonDrawAssert.AreEqual(
                IntersectionType.POLY2_CONTAINS_POLY1, intersect2.GetIntersectionType());
            Assert.IsFalse(intersect2.IsStarter());
        }
    }
}