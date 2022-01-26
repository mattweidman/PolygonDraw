using NUnit.Framework;
using PolygonDraw;
using System.Collections.Generic;

namespace PolygonDrawTests
{
    public class SweepLineTriangulationTests
    {
        #region MonotoneTriangulate

        [Test]
        public void MonotoneTriangulate_TrianglePointingRight()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 2),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(2, 2)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_TrianglePointingLeft()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0),
                new Vector2(0, 2),
                new Vector2(2, 4),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(2, 0), new Vector2(0, 2), new Vector2(2, 4)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_TrianglePointingUp()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(2, 2),
                new Vector2(4, 0),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(2, 2), new Vector2(4, 0)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_TrianglePointingDown()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 2),
                new Vector2(2, 2),
                new Vector2(1, 0),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(1, 0), new Vector2(0, 2), new Vector2(2, 2)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_SimpleQuadrilateral()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 3),
                new Vector2(2, 2),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 1), new Vector2(1, 3), new Vector2(2, 2)),
                new Triangle(new Vector2(1, 0), new Vector2(0, 1), new Vector2(2, 2)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_Square1()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(2, 2),
                new Vector2(2, 0),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(2, 0), new Vector2(0, 2), new Vector2(2, 2)),
                new Triangle(new Vector2(0, 0), new Vector2(0, 2), new Vector2(2, 0)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_Square2()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 2),
                new Vector2(2, 2),
                new Vector2(2, 0),
                new Vector2(0, 0),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(0, 2), new Vector2(2, 2)),
                new Triangle(new Vector2(2, 0), new Vector2(0, 0), new Vector2(2, 2)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_Square3()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 2),
                new Vector2(2, 0),
                new Vector2(0, 0),
                new Vector2(0, 2),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(0, 2), new Vector2(2, 2)),
                new Triangle(new Vector2(2, 0), new Vector2(0, 0), new Vector2(2, 2)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_Square4()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0),
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(2, 2),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(0, 2), new Vector2(2, 2)),
                new Triangle(new Vector2(2, 0), new Vector2(0, 0), new Vector2(2, 2)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        #endregion
    }
}