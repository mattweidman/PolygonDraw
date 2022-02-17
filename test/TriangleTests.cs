using NUnit.Framework;
using PolygonDraw;
using System.Collections.Generic;

namespace PolygonDrawTests
{
    public class TriangleTests
    {
        #region ContainsPoint

        [Test]
        public void ContainsPoint_RightTriangle_BottomLeftCorner()
        {
            Triangle t = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));

            Assert.IsTrue(t.ContainsPoint(new Vector2(1, 1)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(1, 1), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(3, 3)));
            Assert.IsFalse(t.ContainsPoint(new Vector2(-1, -1), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(0, 2)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(0, 2), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(3, 1)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(3, 1), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(0, 5)));
            Assert.IsFalse(t.ContainsPoint(new Vector2(0, 5), true));
        }

        [Test]
        public void ContainsPoint_RightTriangle_TopRightCorner()
        {
            Triangle t = new Triangle(new Vector2(0, 4), new Vector2(4, 4), new Vector2(4, 0));

            Assert.IsTrue(t.ContainsPoint(new Vector2(3, 3)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(3, 3), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(5, -1)));
            Assert.IsFalse(t.ContainsPoint(new Vector2(5, 2), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(4, 4)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(4, 4), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(2, 2)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(2, 2), true));
        }

        [Test]
        public void ContainsPoint_NonRightTriangle()
        {
            Triangle t = new Triangle(new Vector2(0, 1), new Vector2(3, 0), new Vector2(-2, -2));

            Assert.IsTrue(t.ContainsPoint(new Vector2(0, 0)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(1, 0), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(1, -2)));
            Assert.IsFalse(t.ContainsPoint(new Vector2(-1, 0), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(3, 0)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(0, 1), true));
            Assert.IsFalse(t.ContainsPoint(new Vector2(0, -1.2f)));
            Assert.IsTrue(t.ContainsPoint(new Vector2(0, -1.2f), true));
        }

        #endregion

        #region ContainsTriangle

        [Test]
        public void ContainsTriangle()
        {
            Triangle t = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));

            // All inside
            Assert.IsTrue(t.ContainsTriangle(
                new Triangle(new Vector2(1, 1), new Vector2(1, 2), new Vector2(2, 1))));

            // All or some outside
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(4, 4), new Vector2(4, 3), new Vector2(3, 4))));
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(1, 1), new Vector2(1, 4), new Vector2(2, 1))));
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(1, 3), new Vector2(4, 4), new Vector2(2, 2))));
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(1, 3), new Vector2(4, 4), new Vector2(2, 2)), true));
            
            // On vertex
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 1))));
            Assert.IsTrue(t.ContainsTriangle(
                new Triangle(new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 1)), true));
            
            // On edge - don't include edges
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(0, 1), new Vector2(1, 2), new Vector2(2, 1))));
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(1, 0), new Vector2(0, 1), new Vector2(2, 2))));
            Assert.IsFalse(t.ContainsTriangle(
                new Triangle(new Vector2(1, 1), new Vector2(1, 3), new Vector2(2, 2))));
            
            // On edge - include edges
            Assert.IsTrue(t.ContainsTriangle(
                new Triangle(new Vector2(0, 1), new Vector2(1, 2), new Vector2(2, 1)), true));
            Assert.IsTrue(t.ContainsTriangle(
                new Triangle(new Vector2(1, 0), new Vector2(0, 1), new Vector2(2, 2)), true));
            Assert.IsTrue(t.ContainsTriangle(
                new Triangle(new Vector2(1, 1), new Vector2(1, 3), new Vector2(2, 2)), true));
        }

        #endregion

        #region ClipToPolygons

        [Test]
        public void ClipToPolygons_NoIntersections()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(5, 0), new Vector2(5, 4), new Vector2(9, 0));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>() {new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0)})
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_TwoIntersections()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(1, 1), new Vector2(5, 5), new Vector2(5, 1));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(0, 4),
                    new Vector2(2, 2),
                    new Vector2(1, 1),
                    new Vector2(3, 1),
                    new Vector2(4, 0),
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_CoverOneVertex()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(2, 2), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(0, 3), new Vector2(4, 3), new Vector2(2, 1));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(1.5f, 1.5f),
                    new Vector2(2, 1),
                    new Vector2(2.5f, 1.5f),
                    new Vector2(4, 0),
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_FourIntersections()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(6, 4), new Vector2(1, -1), new Vector2(1, 4));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(0, 4),
                    new Vector2(1, 3),
                    new Vector2(1, 0),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 0),
                    new Vector2(2, 0),
                    new Vector2(3, 1),
                }),
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_SixIntersections()
        {
            Triangle t1 = new Triangle(new Vector2(0, 1), new Vector2(1, 3), new Vector2(2, 1));
            Triangle t2 = new Triangle(new Vector2(0, 2), new Vector2(2, 2), new Vector2(1, 0));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(1, 3),
                    new Vector2(1.5f, 2),
                    new Vector2(0.5f, 2),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(2, 1),
                    new Vector2(1.5f, 1),
                    new Vector2(1.75f, 1.5f),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 1),
                    new Vector2(0.25f, 1.5f),
                    new Vector2(0.5f, 1),
                }),
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_OneIntersectionOnEdge()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(2, 2), new Vector2(3, 3), new Vector2(3, 2));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_OneIntersectionOnCorner()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(4, 0), new Vector2(5, 1), new Vector2(5, 0));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_ThreeIntersections()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(2, 0), new Vector2(2, 4), new Vector2(6, 4));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 4), new Vector2(2, 2), new Vector2(2, 0)
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 0), new Vector2(2, 0), new Vector2(3, 1)
                }),
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_SharedEdge()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(0, 0), new Vector2(4, 4), new Vector2(4, 0));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 4), new Vector2(2, 2), new Vector2(0, 0)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_PartialSharedEdge()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(2, 0), new Vector2(6, 4), new Vector2(6, 0));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 4), new Vector2(3, 1), new Vector2(2, 0)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_SubjectCovered()
        {
            Triangle t1 = new Triangle(new Vector2(1, 1), new Vector2(1, 2), new Vector2(2, 1));
            Triangle t2 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));

            List<Polygon> expected = new List<Polygon>() {};

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_IdenticalTriangles()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));

            List<Polygon> expected = new List<Polygon>() {};

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_ScaledTriangles()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(0, 0), new Vector2(0, 2), new Vector2(2, 0));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 4), new Vector2(4, 0), new Vector2(2, 0), new Vector2(0, 2)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_EdgesTouchButNoOverlap()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(0, 0), new Vector2(-4, 0), new Vector2(0, 4));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 0), new Vector2(0, 0), new Vector2(0, 4)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_TouchingCorners()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(4, 0), new Vector2(8, 4), new Vector2(8, 0));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_OppositeQuadrants()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(0, 0), new Vector2(0, -4), new Vector2(-4, 0));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 4), new Vector2(4, 0), new Vector2(0, 0)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_Cut45()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(0, 0), new Vector2(-4, 4), new Vector2(4, 4));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 0), new Vector2(0, 0), new Vector2(2, 2)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_HoleTouchesEachEdge()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(2, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(1, 2), new Vector2(3, 2), new Vector2(2, 0));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 0)
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(2, 4), new Vector2(3, 2), new Vector2(1, 2)
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 0), new Vector2(2, 0), new Vector2(3, 2)
                }),
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_HoleTouchesOneEdge()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(4, 4), new Vector2(8, 0));
            Triangle t2 = new Triangle(new Vector2(3, 2), new Vector2(5, 2), new Vector2(4, 0));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(4, 4), new Vector2(8, 0), new Vector2(4, 0),
                    new Vector2(5, 2), new Vector2(3, 2), new Vector2(4, 0)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_HoleTouchesOneVertex()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 1));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 4), new Vector2(4, 0), new Vector2(0, 0), new Vector2(2, 1),
                    new Vector2(1, 2), new Vector2(0, 0)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_SmallClipOnOneEdge()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(1, 0), new Vector2(2, 1), new Vector2(3, 0));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0), new Vector2(3, 0),
                    new Vector2(2, 1), new Vector2(1, 0)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_SmallClipOpposingOneEdge()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(1, 3), new Vector2(4, 4), new Vector2(3, 1));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_SmallClipStraddlingOneEdge()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(1, 3), new Vector2(4, 4), new Vector2(1, 1));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 4), new Vector2(1, 3), new Vector2(1, 1),
                    new Vector2(2, 2), new Vector2(4, 0)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_RightTriangleContainsClip()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(1, 1), new Vector2(1, 2), new Vector2(2, 1));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0), new Vector2(0, 0),
                    new Vector2(1, 1), new Vector2(2, 1), new Vector2(1, 2), new Vector2(1, 1)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_InnerTriangleFirstVertexHidden()
        {
            Triangle t1 = new Triangle(new Vector2(4, 4), new Vector2(8, 0), new Vector2(0, 0));
            Triangle t2 = new Triangle(new Vector2(4, 1), new Vector2(3, 2), new Vector2(5, 2));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 4), new Vector2(8, 0), new Vector2(0, 0), new Vector2(4, 4),
                    new Vector2(3, 2), new Vector2(4, 1), new Vector2(5, 2), new Vector2(3, 2)
                })
            };

            List<Polygon> observed = t1.ClipToPolygons(t2);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        #endregion

        #region IsValidTriangle

        [Test]
        public void IsValidTriangle_Valid()
        {
            Triangle tri = new Triangle(new Vector2(0, 0), new Vector2(1, 0.01f), new Vector2(2, 0));
            Assert.IsTrue(tri.IsValidTriangle());
        }

        [Test]
        public void IsValidTriangle_Invalid_StartLeft()
        {
            Triangle tri = new Triangle(new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0));
            Assert.IsFalse(tri.IsValidTriangle());
        }

        [Test]
        public void IsValidTriangle_Invalid_StartInMiddle()
        {
            Triangle tri = new Triangle(new Vector2(1, 0), new Vector2(0, 0), new Vector2(2, 0));
            Assert.IsFalse(tri.IsValidTriangle());
        }

        [Test]
        public void IsValidTriangle_Invalid_StartRight()
        {
            Triangle tri = new Triangle(new Vector2(2, 0), new Vector2(0, 0), new Vector2(1, 0));
            Assert.IsFalse(tri.IsValidTriangle());
        }

        #endregion
    }
}