using NUnit.Framework;
using System.Collections.Generic;

namespace PolygonDraw.Tests
{
    public class ClippingTests
    {
        #region Triangle clipping

        [Test]
        public void ClipToPolygons_NoIntersections()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(5, 0), new Vector2(5, 4), new Vector2(9, 0));

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>() {new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0)})
            };

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
        }

        [Test]
        public void ClipToPolygons_SubjectCovered()
        {
            Triangle t1 = new Triangle(new Vector2(1, 1), new Vector2(1, 2), new Vector2(2, 1));
            Triangle t2 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));

            List<Polygon> expected = new List<Polygon>() {};

            TestClipToPolygonsSimple(t1, t2, expected);
        }

        [Test]
        public void ClipToPolygons_IdenticalTriangles()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));

            List<Polygon> expected = new List<Polygon>() {};

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
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

            TestClipToPolygonsSimple(t1, t2, expected);
        }

        [Test]
        public void ClipToPolygons_RightTriangleContainsClip()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(1, 1), new Vector2(1, 2), new Vector2(2, 1));

            List<Polygon> expectedPolygons = new List<Polygon>() { t1 };
            List<Polygon> expectedHoles = new List<Polygon>() { t2 };

            TestClipToPolygonsWithHoles(t1, t2, expectedPolygons, expectedHoles);
        }

        [Test]
        public void ClipToPolygons_InnerTriangleFirstVertexHidden()
        {
            Triangle t1 = new Triangle(new Vector2(4, 4), new Vector2(8, 0), new Vector2(0, 0));
            Triangle t2 = new Triangle(new Vector2(4, 1), new Vector2(3, 2), new Vector2(5, 2));

            List<Polygon> expectedPolygons = new List<Polygon>() { t1 };
            List<Polygon> expectedHoles = new List<Polygon>() { t2 };

            TestClipToPolygonsWithHoles(t1, t2, expectedPolygons, expectedHoles);
        }

        #endregion

        [Test]
        public void ClipToPolygons_SquaresOverlap()
        {
            Polygon subject = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(0, 2), new Vector2(2, 2), new Vector2(2, 0),
            });
            Polygon clip = new Polygon(new List<Vector2>()
            {
                new Vector2(1, 1), new Vector2(1, 3), new Vector2(3, 3), new Vector2(3, 1),
            });
            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 2), new Vector2(1, 2), new Vector2(1, 1),
                    new Vector2(2, 1), new Vector2(2, 0),
                }),
            };
            List<Polygon> observed = subject.ClipToPolygons(clip).polygons;
            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_UCross()
        {
            Polygon subject = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(0, 8), new Vector2(2, 8), new Vector2(2, 2),
                new Vector2(4, 2), new Vector2(4, 8), new Vector2(6, 8), new Vector2(6, 0),
            });
            Polygon clip = new Polygon(new List<Vector2>()
            {
                new Vector2(-1, 3), new Vector2(-1, 5), new Vector2(7, 5), new Vector2(7, 7),
                new Vector2(-1, 7), new Vector2(-1, 9), new Vector2(7, 9), new Vector2(7, 3),
            });
            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 3), new Vector2(2, 3), new Vector2(2, 2),
                    new Vector2(4, 2), new Vector2(4, 3), new Vector2(6, 3), new Vector2(6, 0),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 5), new Vector2(0, 7), new Vector2(2, 7), new Vector2(2, 5),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 5), new Vector2(4, 7), new Vector2(6, 7), new Vector2(6, 5),
                }),
            };
            List<Polygon> observed = subject.ClipToPolygons(clip).polygons;
            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_SplitVertices()
        {
            Polygon subject = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(0, 8), new Vector2(8, 8), new Vector2(4, 4),
                new Vector2(8, 0),
            });
            Polygon clip = new Polygon(new List<Vector2>()
            {
                new Vector2(4, 4), new Vector2(2, 6), new Vector2(6, 6), new Vector2(6, 2),
                new Vector2(2, 2),
            });
            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 8), new Vector2(8, 8), new Vector2(6, 6),
                    new Vector2(2, 6), new Vector2(4, 4), new Vector2(2, 2), new Vector2(6, 2),
                    new Vector2(8, 0),
                }),
            };
            List<Polygon> observed = subject.ClipToPolygons(clip).polygons;
            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_AllCornersCovered()
        {
            Polygon subject = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(4, 4), new Vector2(8, 0),
            });
            Polygon clip = new Polygon(new List<Vector2>()
            {
                new Vector2(5, 3), new Vector2(2, 3), new Vector2(2, -1), new Vector2(7, -1),
                new Vector2(7, 3), new Vector2(8, 3), new Vector2(8, -2), new Vector2(-1, -2),
                new Vector2(-1, 6), new Vector2(5, 6),
            });
            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(5, 3), new Vector2(7, 1), new Vector2(7, 0), new Vector2(2, 0),
                    new Vector2(2, 2), new Vector2(3, 3),
                }),
            };
            List<Polygon> observed = subject.ClipToPolygons(clip).polygons;
            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_AllContainsVertices()
        {
            Polygon subject = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(4, 4), new Vector2(8, 0),
            });
            Polygon clip = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(3, 2), new Vector2(4, 4), new Vector2(5, 2),
                new Vector2(8, 0), new Vector2(4, 1),
            });
            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(4, 4), new Vector2(3, 2),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 4), new Vector2(8, 0), new Vector2(5, 2),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(8, 0), new Vector2(0, 0), new Vector2(4, 1)
                }),
            };
            List<Polygon> observed = subject.ClipToPolygons(clip).polygons;
            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_ConcaveConnection()
        {
            Polygon subject = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(2, 2), new Vector2(0, 4), new Vector2(4, 4),
                new Vector2(4, 0),
            });
            Polygon clip = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 2), new Vector2(4, 4), new Vector2(4, 0),
            });
            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(2, 2), new Vector2(4, 0),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 4), new Vector2(4, 4), new Vector2(2, 2),
                }),
            };
            List<Polygon> observed = subject.ClipToPolygons(clip).polygons;
            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_AlignedSquares()
        {
            Polygon subject = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 4), new Vector2(4, 0)
            });
            Polygon clip = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 2), new Vector2(2, 4), new Vector2(4, 4), new Vector2(4, 2),
            });
            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0), new Vector2(0, 4), new Vector2(2, 4), new Vector2(2, 2),
                    new Vector2(4, 2), new Vector2(4, 0),
                }),
            };
            List<Polygon> observed = subject.ClipToPolygons(clip).polygons;
            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void ClipToPolygons_ShrinkSquare()
        {
            Polygon subject = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 4), new Vector2(4, 0)
            });
            Polygon clip = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(0, 4), new Vector2(2, 4), new Vector2(2, 2),
                new Vector2(4, 2), new Vector2(4, 0),
            });
            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(2, 2), new Vector2(2, 4), new Vector2(4, 4), new Vector2(4, 2),
                }),
            };
            List<Polygon> observed = subject.ClipToPolygons(clip).polygons;
            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        private void TestClipToPolygonsSimple(Polygon subject, Polygon clip, List<Polygon> expected)
        {
            PolygonArrangement observedArrangement = subject.ClipToPolygons(clip);
            PolygonDrawAssert.ListsContainSame(expected, observedArrangement.polygons);
        }

        private void TestClipToPolygonsWithHoles(
            Polygon subject, Polygon clip, List<Polygon> expectedPolygons, List<Polygon> expectedHoles)
        {
            PolygonArrangement observedArrangement = subject.ClipToPolygons(clip);
            PolygonDrawAssert.ListsContainSame(expectedPolygons, observedArrangement.polygons);
            PolygonDrawAssert.ListsContainSame(expectedHoles, observedArrangement.holes);
        }
    }
}