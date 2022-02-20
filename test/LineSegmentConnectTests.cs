using NUnit.Framework;
using PolygonDraw;
using System.Collections.Generic;

namespace PolygonDrawTests
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
    }
}