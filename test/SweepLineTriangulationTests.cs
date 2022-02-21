using NUnit.Framework;
using System.Collections.Generic;

namespace PolygonDraw.Tests
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

        [Test]
        public void MonotoneTriangulate_TwoPointsGoingRight()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(2, 3),
                new Vector2(1, 2),
                new Vector2(2, 1),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(1, 2)),
                new Triangle(new Vector2(1, 2), new Vector2(0, 4), new Vector2(2, 3)),
                new Triangle(new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 1)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_TwoPointsGoingLeft()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0),
                new Vector2(0, 1),
                new Vector2(1, 2),
                new Vector2(0, 3),
                new Vector2(2, 4),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(2, 0), new Vector2(1, 2), new Vector2(2, 4)),
                new Triangle(new Vector2(1, 2), new Vector2(0, 3), new Vector2(2, 4)),
                new Triangle(new Vector2(2, 0), new Vector2(0, 1), new Vector2(1, 2)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_TwoPointsOnEachSide()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(-1, 4),
                new Vector2(-2, 6),
                new Vector2(0, 7),
                new Vector2(2, 5),
                new Vector2(1, 3),
                new Vector2(2, 1),
                new Vector2(0, 0),
                new Vector2(-2, 2)
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(-2, 2), new Vector2(2, 1)),
                new Triangle(new Vector2(2, 1), new Vector2(-2, 2), new Vector2(1, 3)),
                new Triangle(new Vector2(-2, 2), new Vector2(-1, 4), new Vector2(1, 3)),
                new Triangle(new Vector2(1, 3), new Vector2(-1, 4), new Vector2(2, 5)),
                new Triangle(new Vector2(-1, 4), new Vector2(-2, 6), new Vector2(2, 5)),
                new Triangle(new Vector2(2, 5), new Vector2(-2, 6), new Vector2(0, 7)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_AxisAlignedPointPairs()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(1, 0),
                new Vector2(-1, 0),
                new Vector2(-2, 1),
                new Vector2(-1, 2),
                new Vector2(-2, 3),
                new Vector2(-1, 4),
                new Vector2(1, 4),
                new Vector2(2, 3),
                new Vector2(1, 2),
                new Vector2(2, 1),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(2, 3), new Vector2(-1, 4), new Vector2(1, 4)),
                new Triangle(new Vector2(-2, 3), new Vector2(-1, 4), new Vector2(2, 3)),
                new Triangle(new Vector2(1, 2), new Vector2(-2, 3), new Vector2(2, 3)),
                new Triangle(new Vector2(-1, 2), new Vector2(-2, 3), new Vector2(1, 2)),
                new Triangle(new Vector2(2, 1), new Vector2(-1, 2), new Vector2(1, 2)),
                new Triangle(new Vector2(-2, 1), new Vector2(-1, 2), new Vector2(2, 1)),
                new Triangle(new Vector2(-1, 0), new Vector2(-2, 1), new Vector2(2, 1)),
                new Triangle(new Vector2(1, 0), new Vector2(-1, 0), new Vector2(2, 1)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_NeedleRight()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 8),
                new Vector2(1, 6),
                new Vector2(3, 5),
                new Vector2(6, 4),
                new Vector2(3, 3),
                new Vector2(1, 2),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(3, 3), new Vector2(3, 5), new Vector2(6, 4)),
                new Triangle(new Vector2(3, 3), new Vector2(1, 6), new Vector2(3, 5)),
                new Triangle(new Vector2(1, 2), new Vector2(1, 6), new Vector2(3, 3)),
                new Triangle(new Vector2(1, 2), new Vector2(0, 8), new Vector2(1, 6)),
                new Triangle(new Vector2(0, 0), new Vector2(0, 8), new Vector2(1, 2)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_NeedleLeft()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(-6, 4),
                new Vector2(-3, 5),
                new Vector2(-1, 6),
                new Vector2(0, 8),
                new Vector2(0, 0),
                new Vector2(-1, 2),
                new Vector2(-3, 3),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(-3, 3), new Vector2(-6, 4), new Vector2(-3, 5)),
                new Triangle(new Vector2(-3, 3), new Vector2(-3, 5), new Vector2(-1, 6)),
                new Triangle(new Vector2(-1, 2), new Vector2(-3, 3), new Vector2(-1, 6)),
                new Triangle(new Vector2(-1, 2), new Vector2(-1, 6), new Vector2(0, 8)),
                new Triangle(new Vector2(0, 0), new Vector2(-1, 2), new Vector2(0, 8)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_AngledC()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 6),
                new Vector2(2, 8),
                new Vector2(3, 7),
                new Vector2(1, 5),
                new Vector2(1, 3),
                new Vector2(3, 1),
                new Vector2(2, 0),
                new Vector2(0, 2),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 6), new Vector2(2, 8), new Vector2(3, 7)),
                new Triangle(new Vector2(1, 5), new Vector2(0, 6), new Vector2(3, 7)),
                new Triangle(new Vector2(1, 3), new Vector2(0, 6), new Vector2(1, 5)),
                new Triangle(new Vector2(0, 2), new Vector2(0, 6), new Vector2(1, 3)),
                new Triangle(new Vector2(3, 1), new Vector2(0, 2), new Vector2(1, 3)),
                new Triangle(new Vector2(2, 0), new Vector2(0, 2), new Vector2(3, 1)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_AxisAlignedC()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(3, 6),
                new Vector2(3, 4),
                new Vector2(1, 4),
                new Vector2(1, 2),
                new Vector2(3, 2),
                new Vector2(3, 0),
                new Vector2(0, 0),
                new Vector2(0, 6),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(3, 4), new Vector2(0, 6), new Vector2(3, 6)),
                new Triangle(new Vector2(1, 4), new Vector2(0, 6), new Vector2(3, 4)),
                new Triangle(new Vector2(1, 2), new Vector2(0, 6), new Vector2(1, 4)),
                new Triangle(new Vector2(3, 0), new Vector2(0, 0), new Vector2(3, 2)),
                new Triangle(new Vector2(0, 0), new Vector2(1, 2), new Vector2(3, 2)),
                new Triangle(new Vector2(0, 0), new Vector2(0, 6), new Vector2(1, 2)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_Comb()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(1, 0.5f),
                new Vector2(0, 3),
                new Vector2(2, 3),
                new Vector2(4, 2.5f),
                new Vector2(2, 2.5f),
                new Vector2(4, 2),
                new Vector2(2, 2),
                new Vector2(4, 1.5f),
                new Vector2(2, 1.5f),
                new Vector2(4, 1),
                new Vector2(2, 1),
                new Vector2(4, 0.5f),
                new Vector2(2, 0.5f),
                new Vector2(4, 0),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(4, 2.5f), new Vector2(0, 3), new Vector2(2, 3)),
                new Triangle(new Vector2(2, 2.5f), new Vector2(0, 3), new Vector2(4, 2.5f)),
                new Triangle(new Vector2(2, 2), new Vector2(2, 2.5f), new Vector2(4, 2)),
                new Triangle(new Vector2(2, 1.5f), new Vector2(2, 2), new Vector2(4, 1.5f)),
                new Triangle(new Vector2(2, 1), new Vector2(2, 1.5f), new Vector2(4, 1)),
                new Triangle(new Vector2(2, 0.5f), new Vector2(2, 1), new Vector2(4, 0.5f)),
                new Triangle(new Vector2(2, 2), new Vector2(0, 3), new Vector2(2, 2.5f)),
                new Triangle(new Vector2(2, 1.5f), new Vector2(0, 3), new Vector2(2, 2)),
                new Triangle(new Vector2(2, 1), new Vector2(0, 3), new Vector2(2, 1.5f)),
                new Triangle(new Vector2(2, 0.5f), new Vector2(0, 3), new Vector2(2, 1)),
                new Triangle(new Vector2(1, 0.5f), new Vector2(0, 3), new Vector2(2, 0.5f)),
                new Triangle(new Vector2(4, 0), new Vector2(1, 0.5f), new Vector2(2, 0.5f)),
                new Triangle(new Vector2(0, 0), new Vector2(1, 0.5f), new Vector2(4, 0)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_AxisAlignedTwoPointsRight()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(2, 2),
                new Vector2(1, 1),
                new Vector2(2, 0),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(1, 1), new Vector2(0, 2), new Vector2(2, 2)),
                new Triangle(new Vector2(0, 0), new Vector2(0, 2), new Vector2(1, 1)),
                new Triangle(new Vector2(0, 0), new Vector2(1, 1), new Vector2(2, 0)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void MonotoneTriangulate_AxisAlignedTwoPointsLeft()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(1, 1),
                new Vector2(0, 2),
                new Vector2(2, 2),
                new Vector2(2, 0),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(1, 1), new Vector2(0, 2), new Vector2(2, 2)),
                new Triangle(new Vector2(2, 0), new Vector2(1, 1), new Vector2(2, 2)),
                new Triangle(new Vector2(0, 0), new Vector2(1, 1), new Vector2(2, 0)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void DivideIntoTriangles_Plateau1()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 2),
                new Vector2(5, 2),
                new Vector2(5, 1),
                new Vector2(4, 0),
                new Vector2(4, 1),
                new Vector2(1, 1),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 2), new Vector2(5, 1), new Vector2(1, 1)),
                new Triangle(new Vector2(0, 2), new Vector2(5, 2), new Vector2(5, 1)),
                new Triangle(new Vector2(4, 0), new Vector2(4, 1), new Vector2(5, 1)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void DivideIntoTriangles_Plateau2()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 2),
                new Vector2(5, 2),
                new Vector2(5, 0.5f),
                new Vector2(4, 0),
                new Vector2(4, 1),
                new Vector2(1, 1),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 2), new Vector2(5, 2), new Vector2(1, 1)),
                new Triangle(new Vector2(4, 1), new Vector2(1, 1), new Vector2(5, 2)),
                new Triangle(new Vector2(5, 0.5f), new Vector2(4, 1), new Vector2(5, 2)),
                new Triangle(new Vector2(5, 0.5f), new Vector2(4, 0), new Vector2(4, 1)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void DivideIntoTriangles_Plateau3()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 2),
                new Vector2(5, 2),
                new Vector2(5, 1.5f),
                new Vector2(4, 0),
                new Vector2(4, 1),
                new Vector2(1, 1),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 2), new Vector2(5, 2), new Vector2(5, 1.5f)),
                new Triangle(new Vector2(1, 1), new Vector2(0, 2), new Vector2(5, 1.5f)),
                new Triangle(new Vector2(1, 1), new Vector2(5, 1.5f), new Vector2(4, 1)),
                new Triangle(new Vector2(4, 1), new Vector2(5, 1.5f), new Vector2(4, 0)),
            };

            List<Triangle> observed = polygon.MonotoneTriangulate();

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        #endregion
    }
}