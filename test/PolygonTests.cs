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

        [Test]
        public void Split_SquareAt0()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
            });

            Polygon expected1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(1, 1),
                new Vector2(1, 0),
            });
            Polygon expected2 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
            });

            (Polygon observed1, Polygon observed2) = polygon.Split(0, 2);

            PolygonDrawAssert.AreEqual(expected1, observed1);
            PolygonDrawAssert.AreEqual(expected2, observed2);
        }

        [Test]
        public void Split_SquareAt1()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
            });

            Polygon expected1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 0),
            });
            Polygon expected2 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
            });

            (Polygon observed1, Polygon observed2) = polygon.Split(1, 3);

            PolygonDrawAssert.AreEqual(expected1, observed1);
            PolygonDrawAssert.AreEqual(expected2, observed2);
        }

        [Test]
        public void Split_Pentagon()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 2),
                new Vector2(2, 1),
                new Vector2(2, 0),
            });

            Polygon expected1 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(2, 1),
                new Vector2(2, 0),
            });
            Polygon expected2 = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 1),
                new Vector2(1, 2),
                new Vector2(2, 1),
            });

            (Polygon observed1, Polygon observed2) = polygon.Split(1, 3);

            PolygonDrawAssert.AreEqual(expected1, observed1);
            PolygonDrawAssert.AreEqual(expected2, observed2);
        }

        #region DivideIntoTriangles

        [Test]
        public void DivideIntoTriangles_Square()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
            });

            List<Triangle> observed = polygon.DivideIntoTriangles();

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(1, 1), new Vector2(1, 0)),
                new Triangle(new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)),
            };

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void DivideIntoTriangles_Pentagon()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 2),
                new Vector2(2, 1),
                new Vector2(2, 0),
            });

            List<Triangle> observed = polygon.DivideIntoTriangles();

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(2, 1), new Vector2(2, 0)),
                new Triangle(new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 1)),
                new Triangle(new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 2)),
            };

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void DivideIntoTriangles_ConcavePentagon()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(1, 1),
                new Vector2(2, 2),
                new Vector2(2, 0),
            });

            List<Triangle> observed = polygon.DivideIntoTriangles();

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(1, 1), new Vector2(2, 0)),
                new Triangle(new Vector2(1, 1), new Vector2(2, 2), new Vector2(2, 0)),
                new Triangle(new Vector2(0, 0), new Vector2(0, 2), new Vector2(1, 1)),
            };

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void DivideIntoTriangles_ConcaveQuadrilateral()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(1, 2),
                new Vector2(2, 0),
                new Vector2(1, 1),
            });

            List<Triangle> observed = polygon.DivideIntoTriangles();

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(1, 2), new Vector2(1, 1)),
                new Triangle(new Vector2(1, 2), new Vector2(2, 0), new Vector2(1, 1)),
            };

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void DivideIntoTriangles_Claw()
        {
            Polygon polygon = CLAW;

            List<Triangle> observed = polygon.DivideIntoTriangles();

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(0, 5), new Vector2(2, 4)),
                new Triangle(new Vector2(0, 5), new Vector2(5, 5), new Vector2(2, 4)),
                new Triangle(new Vector2(0, 0), new Vector2(2, 4), new Vector2(2, 2)),
                new Triangle(new Vector2(0, 0), new Vector2(2, 2), new Vector2(3, 1)),
                new Triangle(new Vector2(0, 0), new Vector2(3, 1), new Vector2(5, 0)),
                new Triangle(new Vector2(3, 1), new Vector2(5, 2), new Vector2(5, 0)),
            };

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void DivideIntoTriangles_HoleTouchesOneEdge()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(4, 4), new Vector2(8, 0), new Vector2(4, 0),
                new Vector2(5, 2), new Vector2(3, 2), new Vector2(4, 0)
            });

            List<Triangle> observed = polygon.DivideIntoTriangles();

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(3, 2), new Vector2(4, 0)),
                new Triangle(new Vector2(4, 4), new Vector2(8, 0), new Vector2(5, 2)),
                new Triangle(new Vector2(8, 0), new Vector2(4, 0), new Vector2(5, 2)),
                new Triangle(new Vector2(0, 0), new Vector2(4, 4), new Vector2(3, 2)),
                new Triangle(new Vector2(4, 4), new Vector2(5, 2), new Vector2(3, 2)),
            };

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void DivideIntoTriangles_HoleTouchesOneVertex()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 4), new Vector2(4, 0), new Vector2(0, 0), new Vector2(2, 1),
                new Vector2(1, 2), new Vector2(0, 0)
            });

            List<Triangle> observed = polygon.DivideIntoTriangles();

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 4), new Vector2(4, 0), new Vector2(2, 1)),
                new Triangle(new Vector2(0, 4), new Vector2(2, 1), new Vector2(1, 2)),
                new Triangle(new Vector2(0, 4), new Vector2(1, 2), new Vector2(0, 0)),
                new Triangle(new Vector2(4, 0), new Vector2(0, 0), new Vector2(2, 1)),
            };

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void DivideIntoTriangles_HoleTouchesFirstVertex()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0), new Vector2(0, 0),
                new Vector2(2, 1), new Vector2(1, 2)
            });

            List<Triangle> observed = polygon.DivideIntoTriangles();

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 4), new Vector2(4, 0), new Vector2(2, 1)),
                new Triangle(new Vector2(0, 4), new Vector2(2, 1), new Vector2(1, 2)),
                new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(1, 2)),
                new Triangle(new Vector2(4, 0), new Vector2(0, 0), new Vector2(2, 1)),
            };

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void DivideIntoTriangles_InnerRightTriangle()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0), new Vector2(0, 0),
                new Vector2(1, 1), new Vector2(2, 1), new Vector2(1, 2), new Vector2(1, 1)
            });

            List<Triangle> observed = polygon.DivideIntoTriangles();

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(1, 2), new Vector2(1, 1)),
                new Triangle(new Vector2(4, 0), new Vector2(1, 1), new Vector2(2, 1)),
                new Triangle(new Vector2(0, 4), new Vector2(2, 1), new Vector2(1, 2)),
                new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(1, 2)),
                new Triangle(new Vector2(0, 4), new Vector2(4, 0), new Vector2(2, 1)),
                new Triangle(new Vector2(4, 0), new Vector2(0, 0), new Vector2(1, 1)),
            };

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void DivideIntoTriangles_AlignedVerticesShouldBlock()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(4, 4), // Should not connect to (0, 0)
                new Vector2(4, 3),
                new Vector2(3, 3), // Should not connect to (0, 0)
                new Vector2(1, 1), // Should connect to (0, 0)
                new Vector2(1, 0)
            });

            List<Triangle> observed = polygon.DivideIntoTriangles();

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(1, 1), new Vector2(1, 0)),
                new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(1, 1)),
                new Triangle(new Vector2(0, 4), new Vector2(3, 3), new Vector2(1, 1)),
                new Triangle(new Vector2(0, 4), new Vector2(4, 3), new Vector2(3, 3)),
                new Triangle(new Vector2(0, 4), new Vector2(4, 4), new Vector2(4, 3)),
            };

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        #endregion
    }
}