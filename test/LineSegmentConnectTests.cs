using NUnit.Framework;
using PolygonDraw;
using System.Collections.Generic;

namespace PolygonDraw.Tests
{
    public class LineSegmentConnectTests
    {
        [Test]
        public void ConnectLineSegments_SimpleTriangle()
        {
            float maxSeparation = 0.01f;

            List<LineSegment> lineSegments = new List<LineSegment>()
            {
                new LineSegment(new Vector2(0, 0), new Vector2(2, 2)),
                new LineSegment(new Vector2(2, 2), new Vector2(4, 0)),
                new LineSegment(new Vector2(4, 0), new Vector2(0, 0)),
            };

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(2, 2), new Vector2(4, 0),
                }),
            };

            List<Polygon> observed =
                LineSegmentConnect.ConnectLineSegments(lineSegments, maxSeparation).polygons;
            
            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ConnectLineSegments_TwoSquares()
        {
            float maxSeparation = 0.01f;

            List<LineSegment> lineSegments = new List<LineSegment>()
            {
                new LineSegment(new Vector2(0, 0), new Vector2(0, 2)),
                new LineSegment(new Vector2(5, 0), new Vector2(5, 3)),
                new LineSegment(new Vector2(0, 2), new Vector2(2, 2)),
                new LineSegment(new Vector2(5, 3), new Vector2(8, 3)),
                new LineSegment(new Vector2(2, 2), new Vector2(2, 0)),
                new LineSegment(new Vector2(8, 3), new Vector2(8, 0)),
                new LineSegment(new Vector2(2, 0), new Vector2(0, 0)),
                new LineSegment(new Vector2(8, 0), new Vector2(5, 0)),
            };

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 2), new Vector2(2, 2), new Vector2(2, 0),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(5, 0), new Vector2(5, 3), new Vector2(8, 3), new Vector2(8, 0),
                }),
            };

            List<Polygon> observed =
                LineSegmentConnect.ConnectLineSegments(lineSegments, maxSeparation).polygons;
            
            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ConnectLineSegments_EdgesWithSameXCoords()
        {
            float maxSeparation = 0.01f;

            List<LineSegment> lineSegments = new List<LineSegment>()
            {
                new LineSegment(new Vector2(0, 0), new Vector2(0, 2)),
                new LineSegment(new Vector2(0, 2), new Vector2(1, 1)),
                new LineSegment(new Vector2(1, 1), new Vector2(0, 0)),
                new LineSegment(new Vector2(0, 3), new Vector2(0, 5)),
                new LineSegment(new Vector2(0, 5), new Vector2(1, 4)),
                new LineSegment(new Vector2(1, 4), new Vector2(0, 3)),
            };

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 2), new Vector2(1, 1),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 3), new Vector2(0, 5), new Vector2(1, 4),
                }),
            };

            List<Polygon> observed =
                LineSegmentConnect.ConnectLineSegments(lineSegments, maxSeparation).polygons;
            
            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ConnectLineSegments_Hole()
        {
            float maxSeparation = 0.01f;

            List<LineSegment> lineSegments = new List<LineSegment>()
            {
                new LineSegment(new Vector2(0, 0), new Vector2(0, 3)),
                new LineSegment(new Vector2(0, 3), new Vector2(3, 3)),
                new LineSegment(new Vector2(3, 3), new Vector2(3, 0)),
                new LineSegment(new Vector2(3, 0), new Vector2(0, 0)),

                new LineSegment(new Vector2(1, 1), new Vector2(2, 1)),
                new LineSegment(new Vector2(2, 1), new Vector2(2, 2)),
                new LineSegment(new Vector2(2, 2), new Vector2(1, 2)),
                new LineSegment(new Vector2(1, 2), new Vector2(1, 1)),
            };

            List<Polygon> expectedPolygons = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 3), new Vector2(3, 3), new Vector2(3, 0),
                }),
            };
            List<Polygon> expectedHoles = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(1, 1), new Vector2(1, 2), new Vector2(2, 2), new Vector2(2, 1),
                }),
            };

            PolygonArrangement observed =
                LineSegmentConnect.ConnectLineSegments(lineSegments, maxSeparation);
            PolygonDrawAssert.ListsContainSame(expectedPolygons, observed.polygons);
            PolygonDrawAssert.ListsContainSame(expectedHoles, observed.holes);
        }

        [Test]
        public void ConnectLineSegments_EdgesSlightlyOff()
        {
            float maxSeparation = 0.01f;

            List<LineSegment> lineSegments = new List<LineSegment>()
            {
                new LineSegment(new Vector2(0, 0), new Vector2(0, 1.991f)),
                new LineSegment(new Vector2(0, 2), new Vector2(1.1991f, 2)),
                new LineSegment(new Vector2(2, 2), new Vector2(0.009f, 0.009f)),
            };

            List<Polygon> expectedPolygons = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 2), new Vector2(2, 2),
                }),
            };

            PolygonArrangement observed =
                LineSegmentConnect.ConnectLineSegments(lineSegments, maxSeparation);
            PolygonDrawAssert.ListsContainSame(expectedPolygons, observed.polygons);
        }

        [Test]
        public void ConnectLineSegments_DriftingBucket()
        {
            float maxSeparation = 0.01f;

            List<LineSegment> lineSegments = new List<LineSegment>()
            {
                new LineSegment(new Vector2(2, 0), new Vector2(0, 0)),
                new LineSegment(new Vector2(0, 0), new Vector2(1, 2)),
                new LineSegment(new Vector2(1.016f, 1), new Vector2(2, 0)),
                new LineSegment(new Vector2(1, 1.991f), new Vector2(1.009f, 1)),
            };

            List<Polygon> expectedPolygons = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(1, 1.991f), new Vector2(1.016f, 1), new Vector2(2, 0),
                }),
            };

            PolygonArrangement observed =
                LineSegmentConnect.ConnectLineSegments(lineSegments, maxSeparation);
            PolygonDrawAssert.ListsContainSame(expectedPolygons, observed.polygons);
        }
    }
}