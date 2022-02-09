using NUnit.Framework;
using PolygonDraw;
using System.Collections.Generic;

namespace PolygonDrawTests
{
    public class TriangulationTests
    {
        #region GetYMonotonePolygonDivisions

        [Test]
        public void GetYMonotonePolygonDivisions_Diamond()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0),
                new Vector2(0, 2),
                new Vector2(2, 4),
                new Vector2(4, 2),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>();
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);
            
            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_Square()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(2, 2),
                new Vector2(2, 0),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>();
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(
                new List<Polygon>() { polygon },
                new List<Polygon>());
            
            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_MergeVertex()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0),
                new Vector2(0, 4),
                new Vector2(1, 3),
                new Vector2(2, 4),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 0), new PolygonVertex(polygon, 2)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_SplitVertex()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(1, 4),
                new Vector2(2, 0),
                new Vector2(1, 1),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 3), new PolygonVertex(polygon, 1)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_MergeAndSplit()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(1, 3),
                new Vector2(2, 4),
                new Vector2(2, 0),
                new Vector2(1, 1),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 5), new PolygonVertex(polygon, 2)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_MergeLeftAndRight()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0),
                new Vector2(0, 2),
                new Vector2(1, 4),
                new Vector2(2, 3),
                new Vector2(3, 4),
                new Vector2(4, 2),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 1), new PolygonVertex(polygon, 3)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_MergeRight()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0),
                new Vector2(0, 4),
                new Vector2(2, 3),
                new Vector2(3, 4),
                new Vector2(4, 2),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 4), new PolygonVertex(polygon, 2)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_TwoMerges()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(3, 0),
                new Vector2(0, 2),
                new Vector2(1, 5),
                new Vector2(2, 4),
                new Vector2(3, 5),
                new Vector2(4, 3),
                new Vector2(5, 5),
                new Vector2(6, 2),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 5), new PolygonVertex(polygon, 3)),
                new PolygonEdge(new PolygonVertex(polygon, 1), new PolygonVertex(polygon, 5)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_TwoMergesAligned()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(3, 0),
                new Vector2(0, 2),
                new Vector2(1, 5),
                new Vector2(2, 4),
                new Vector2(3, 5),
                new Vector2(4, 4),
                new Vector2(5, 5),
                new Vector2(6, 2),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 5), new PolygonVertex(polygon, 3)),
                new PolygonEdge(new PolygonVertex(polygon, 1), new PolygonVertex(polygon, 3)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_TwoSplitsAligned()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 3),
                new Vector2(3, 5),
                new Vector2(6, 3),
                new Vector2(5, 0),
                new Vector2(4, 1),
                new Vector2(3, 0),
                new Vector2(2, 1),
                new Vector2(1, 0),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 6), new PolygonVertex(polygon, 2)),
                new PolygonEdge(new PolygonVertex(polygon, 4), new PolygonVertex(polygon, 6)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_NegativeTurnPoints()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 2),
                new Vector2(1, 1),
                new Vector2(2, 2),
                new Vector2(2, -2),
                new Vector2(1, -1),
                new Vector2(0, -2),
                new Vector2(-1, -1),
                new Vector2(-2, -2),
                new Vector2(-2, 2),
                new Vector2(-1, 1),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 1), new PolygonVertex(polygon, 9)),
                new PolygonEdge(new PolygonVertex(polygon, 6), new PolygonVertex(polygon, 9)),
                new PolygonEdge(new PolygonVertex(polygon, 4), new PolygonVertex(polygon, 6)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_TopCrenel()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(1, 2),
                new Vector2(1, 1),
                new Vector2(2, 1),
                new Vector2(2, 2),
                new Vector2(3, 2),
                new Vector2(3, 0),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 0), new PolygonVertex(polygon, 3)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_BottomCrenel()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(3, 2),
                new Vector2(3, 0),
                new Vector2(2, 0),
                new Vector2(2, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 6), new PolygonVertex(polygon, 1)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_SpikedTop()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(6, 2),
                new Vector2(6, 0),
                new Vector2(5, 1),
                new Vector2(4, 0),
                new Vector2(3, 1),
                new Vector2(2, 0),
                new Vector2(1, 1),
                new Vector2(0, 0),
                new Vector2(0, 2),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 6), new PolygonVertex(polygon, 8)),
                new PolygonEdge(new PolygonVertex(polygon, 4), new PolygonVertex(polygon, 6)),
                new PolygonEdge(new PolygonVertex(polygon, 2), new PolygonVertex(polygon, 4)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_BottomAndTopCrenels()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(4, 0), // 0
                new Vector2(4, 1),
                new Vector2(3, 1),
                new Vector2(3, 0),
                new Vector2(2, 0),
                new Vector2(2, 1), // 5
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(5, 2), // 10
                new Vector2(5, 1),
                new Vector2(6, 1),
                new Vector2(6, 2),
                new Vector2(7, 2),
                new Vector2(7, 1), // 15
                new Vector2(8, 1),
                new Vector2(8, 2),
                new Vector2(9, 2),
                new Vector2(9, 0), // 19
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 6), new PolygonVertex(polygon, 9)),
                new PolygonEdge(new PolygonVertex(polygon, 2), new PolygonVertex(polygon, 5)),
                new PolygonEdge(new PolygonVertex(polygon, 15), new PolygonVertex(polygon, 12)),
                new PolygonEdge(new PolygonVertex(polygon, 0), new PolygonVertex(polygon, 11)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_TopAndBottomCrenels()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), // 0
                new Vector2(0, 2),
                new Vector2(1, 2),
                new Vector2(1, 1),
                new Vector2(2, 1),
                new Vector2(2, 2), // 5
                new Vector2(3, 2),
                new Vector2(3, 1),
                new Vector2(4, 1),
                new Vector2(4, 2),
                new Vector2(9, 2), // 10
                new Vector2(9, 0),
                new Vector2(8, 0),
                new Vector2(8, 1),
                new Vector2(7, 1),
                new Vector2(7, 0), // 15
                new Vector2(6, 0),
                new Vector2(6, 1),
                new Vector2(5, 1),
                new Vector2(5, 0), // 19
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 0), new PolygonVertex(polygon, 3)),
                new PolygonEdge(new PolygonVertex(polygon, 7), new PolygonVertex(polygon, 4)),
                new PolygonEdge(new PolygonVertex(polygon, 18), new PolygonVertex(polygon, 8)),
                new PolygonEdge(new PolygonVertex(polygon, 14), new PolygonVertex(polygon, 17)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_Tetris()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 2),
                new Vector2(2, 2),
                new Vector2(2, 1),
                new Vector2(3, 1),
                new Vector2(3, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 5), new PolygonVertex(polygon, 2)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_Snake()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), // 0
                new Vector2(0, 2),
                new Vector2(3, 2),
                new Vector2(3, 1),
                new Vector2(4, 1),
                new Vector2(4, 2), // 5
                new Vector2(5, 2),
                new Vector2(5, 0),
                new Vector2(2, 0),
                new Vector2(2, 1),
                new Vector2(1, 1), // 10
                new Vector2(1, 0),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 10), new PolygonVertex(polygon, 1)),
                new PolygonEdge(new PolygonVertex(polygon, 8), new PolygonVertex(polygon, 3)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_TwoTopCrenels()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), // 0
                new Vector2(0, 2),
                new Vector2(1, 2),
                new Vector2(1, 1),
                new Vector2(2, 1),
                new Vector2(2, 2), // 5
                new Vector2(3, 2),
                new Vector2(3, 1),
                new Vector2(4, 1),
                new Vector2(4, 2),
                new Vector2(5, 2), // 10
                new Vector2(5, 0),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 7), new PolygonVertex(polygon, 4)),
                new PolygonEdge(new PolygonVertex(polygon, 0), new PolygonVertex(polygon, 3)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_TwoBottomCrenels()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), // 0
                new Vector2(0, 2),
                new Vector2(5, 2),
                new Vector2(5, 0),
                new Vector2(4, 0),
                new Vector2(4, 1), // 5
                new Vector2(3, 1),
                new Vector2(3, 0),
                new Vector2(2, 0),
                new Vector2(2, 1),
                new Vector2(1, 1), // 10
                new Vector2(1, 0),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 10), new PolygonVertex(polygon, 1)),
                new PolygonEdge(new PolygonVertex(polygon, 6), new PolygonVertex(polygon, 9)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_TwoTopCrenels_OneChipped()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), // 0
                new Vector2(0, 2),
                new Vector2(1, 2),
                new Vector2(1, 1.5f),
                new Vector2(2, 1),
                new Vector2(2, 2), // 5
                new Vector2(3, 2),
                new Vector2(3, 1),
                new Vector2(4, 1),
                new Vector2(4, 2),
                new Vector2(5, 2), // 10
                new Vector2(5, 0),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 7), new PolygonVertex(polygon, 4)),
                new PolygonEdge(new PolygonVertex(polygon, 0), new PolygonVertex(polygon, 4)),
            };
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_DiamondHole()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0),
                new Vector2(0, 2),
                new Vector2(2, 4),
                new Vector2(4, 2),
            });

            Polygon hole = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 1),
                new Vector2(1, 2),
                new Vector2(2, 3),
                new Vector2(3, 2),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(hole, 2), new PolygonVertex(polygon, 2)),
                new PolygonEdge(new PolygonVertex(polygon, 0), new PolygonVertex(hole, 0)),
            };

            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(
                new List<Polygon>() { polygon },
                new List<Polygon>() { hole });

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_SquareHole()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 3),
                new Vector2(3, 3),
                new Vector2(3, 0),
            });

            Polygon hole = new Polygon(new List<Vector2>()
            {
                new Vector2(1, 1),
                new Vector2(1, 2),
                new Vector2(2, 2),
                new Vector2(2, 1),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(hole, 1), new PolygonVertex(polygon, 1)),
                new PolygonEdge(new PolygonVertex(polygon, 0), new PolygonVertex(hole, 0)),
            };

            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(
                new List<Polygon>() { polygon },
                new List<Polygon>() { hole });

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_JackOLantern()
        {
            List<Polygon> polygons = new List<Polygon>() {new Polygon(new List<Vector2>()
            {
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(0, 5),
                new Vector2(1, 6),
                new Vector2(8, 6),
                new Vector2(9, 5),
                new Vector2(9, 1),
                new Vector2(8, 0),
            })};

            List<Polygon> holes = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(2, 1),
                    new Vector2(2, 3),
                    new Vector2(3, 3),
                    new Vector2(3, 2),
                    new Vector2(4, 2),
                    new Vector2(4, 3),
                    new Vector2(5, 3),
                    new Vector2(5, 2),
                    new Vector2(6, 2),
                    new Vector2(6, 3),
                    new Vector2(7, 3),
                    new Vector2(7, 1),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(3, 4),
                    new Vector2(3.5f, 5),
                    new Vector2(4, 4),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(5, 4),
                    new Vector2(5.5f, 5),
                    new Vector2(6, 4),
                })
            };

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(holes[1], 1), new PolygonVertex(polygons[0], 2)),
                new PolygonEdge(new PolygonVertex(holes[2], 1), new PolygonVertex(holes[1], 1)),
                new PolygonEdge(new PolygonVertex(holes[2], 0), new PolygonVertex(holes[1], 2)),
                new PolygonEdge(new PolygonVertex(holes[0], 1), new PolygonVertex(holes[1], 0)),
                new PolygonEdge(new PolygonVertex(holes[0], 5), new PolygonVertex(holes[0], 2)),
                new PolygonEdge(new PolygonVertex(holes[0], 9), new PolygonVertex(holes[0], 6)),
                new PolygonEdge(new PolygonVertex(polygons[0], 6), new PolygonVertex(holes[0], 11)),
                new PolygonEdge(new PolygonVertex(polygons[0], 0), new PolygonVertex(holes[0], 0)),
            };

            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygons, holes);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_SeparateTriangles()
        {
            List<Polygon> polygons = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(2, 0),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 1),
                    new Vector2(3, 0),
                    new Vector2(2, 1),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 2),
                    new Vector2(2, 3),
                    new Vector2(4, 2),
                }),
            };

            List<PolygonEdge> expected = new List<PolygonEdge>();

            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(
                polygons, new List<Polygon>());

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_IslandWithinAnIsland()
        {
            List<Polygon> polygons = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(0, 9),
                    new Vector2(9, 9),
                    new Vector2(9, 0),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(2, 2),
                    new Vector2(2, 7),
                    new Vector2(7, 7),
                    new Vector2(7, 2),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 4),
                    new Vector2(4, 5),
                    new Vector2(5, 5),
                    new Vector2(5, 4),
                }),
            };

            List<Polygon> holes = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(1, 1),
                    new Vector2(1, 8),
                    new Vector2(8, 8),
                    new Vector2(8, 1),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(3, 3),
                    new Vector2(3, 6),
                    new Vector2(6, 6),
                    new Vector2(6, 3),
                }),
            };

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(holes[0], 1), new PolygonVertex(polygons[0], 1)),
                new PolygonEdge(new PolygonVertex(holes[1], 1), new PolygonVertex(polygons[1], 1)),
                new PolygonEdge(new PolygonVertex(polygons[0], 0), new PolygonVertex(holes[0], 0)),
                new PolygonEdge(new PolygonVertex(polygons[1], 0), new PolygonVertex(holes[1], 0)),
            };

            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygons, holes);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygonDivisions_VertexWithMultipleDivisions()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), // 0
                new Vector2(0, 5),
                new Vector2(1, 4),
                new Vector2(2, 5),
                new Vector2(3, 3),
                new Vector2(4, 5), // 5
                new Vector2(5, 4),
                new Vector2(6, 5),
                new Vector2(6, 0),
                new Vector2(4, 2),
                new Vector2(3, 0), // 10
                new Vector2(2, 1),
            });

            List<PolygonEdge> expected = new List<PolygonEdge>()
            {
                new PolygonEdge(new PolygonVertex(polygon, 4), new PolygonVertex(polygon, 2)),
                new PolygonEdge(new PolygonVertex(polygon, 4), new PolygonVertex(polygon, 6)),
                new PolygonEdge(new PolygonVertex(polygon, 9), new PolygonVertex(polygon, 4)),
                new PolygonEdge(new PolygonVertex(polygon, 11), new PolygonVertex(polygon, 9)),
            };

            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        #endregion

        #region GetYMonotonePolygons

        [Test]
        public void GetYMonotonePolygons_Triangle()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(4, 4),
            });

            List<Polygon> expected = new List<Polygon>() { polygon };
            List<Polygon> observed = Triangulation.GetYMonotonePolygons(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygons_MergeVertex()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0),
                new Vector2(0, 4),
                new Vector2(1, 3),
                new Vector2(2, 4),
            });

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(2, 0),
                    new Vector2(0, 4),
                    new Vector2(1, 3),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(2, 0),
                    new Vector2(1, 3),
                    new Vector2(2, 4),
                }),
            };
            List<Polygon> observed = Triangulation.GetYMonotonePolygons(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygons_SplitVertex()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(1, 4),
                new Vector2(2, 0),
                new Vector2(1, 1),
            });

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(1, 4),
                    new Vector2(1, 1),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(2, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 4),
                }),
            };
            List<Polygon> observed = Triangulation.GetYMonotonePolygons(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygons_VertexWithMultipleDivisions()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 5),
                new Vector2(1, 4),
                new Vector2(2, 5),
                new Vector2(3, 3),
                new Vector2(4, 5),
                new Vector2(5, 4),
                new Vector2(6, 5),
                new Vector2(6, 0),
                new Vector2(4, 2),
                new Vector2(3, 0),
                new Vector2(2, 1),
            });

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(0, 5),
                    new Vector2(1, 4),
                    new Vector2(3, 3),
                    new Vector2(4, 2),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(1, 4),
                    new Vector2(2, 5),
                    new Vector2(3, 3),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 2),
                    new Vector2(3, 0),
                    new Vector2(2, 1),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(3, 3),
                    new Vector2(4, 5),
                    new Vector2(5, 4),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(3, 3),
                    new Vector2(5, 4),
                    new Vector2(6, 5),
                    new Vector2(6, 0),
                }),
            };
            List<Polygon> observed = Triangulation.GetYMonotonePolygons(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygons_BottomAndTopCrenels()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(4, 0), // 0
                new Vector2(4, 1),
                new Vector2(3, 1),
                new Vector2(3, 0),
                new Vector2(2, 0),
                new Vector2(2, 1), // 5
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(5, 2), // 10
                new Vector2(5, 1),
                new Vector2(6, 1),
                new Vector2(6, 2),
                new Vector2(7, 2),
                new Vector2(7, 1), // 15
                new Vector2(8, 1),
                new Vector2(8, 2),
                new Vector2(9, 2),
                new Vector2(9, 0), // 19
            });

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    new Vector2(0, 0),
                    new Vector2(0, 2),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(3, 1),
                    new Vector2(3, 0),
                    new Vector2(2, 0),
                    new Vector2(2, 1),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(6, 1),
                    new Vector2(6, 2),
                    new Vector2(7, 2),
                    new Vector2(7, 1),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 0),
                    new Vector2(4, 1),
                    new Vector2(1, 1),
                    new Vector2(0, 2),
                    new Vector2(5, 2),
                    new Vector2(5, 1),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 0),
                    new Vector2(5, 1),
                    new Vector2(8, 1),
                    new Vector2(8, 2),
                    new Vector2(9, 2),
                    new Vector2(9, 0),
                }),
            };
            List<Polygon> observed = Triangulation.GetYMonotonePolygons(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygons_JackOLantern()
        {
            List<Polygon> polygons = new List<Polygon>() {new Polygon(new List<Vector2>()
            {
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(0, 5),
                new Vector2(1, 6),
                new Vector2(8, 6),
                new Vector2(9, 5),
                new Vector2(9, 1),
                new Vector2(8, 0),
            })};

            List<Polygon> holes = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(2, 1),
                    new Vector2(2, 3),
                    new Vector2(3, 3),
                    new Vector2(3, 2),
                    new Vector2(4, 2),
                    new Vector2(4, 3),
                    new Vector2(5, 3),
                    new Vector2(5, 2),
                    new Vector2(6, 2),
                    new Vector2(6, 3),
                    new Vector2(7, 3),
                    new Vector2(7, 1),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(3, 4),
                    new Vector2(3.5f, 5),
                    new Vector2(4, 4),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(5, 4),
                    new Vector2(5.5f, 5),
                    new Vector2(6, 4),
                })
            };

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>() // good
                {
                    new Vector2(1, 0),
                    new Vector2(0, 1),
                    new Vector2(0, 5),
                    new Vector2(3.5f, 5),
                    new Vector2(3, 4),
                    new Vector2(6, 4),
                    new Vector2(2, 3),
                    new Vector2(2, 1),
                    new Vector2(9, 1),
                    new Vector2(8, 0),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 5),
                    new Vector2(1, 6),
                    new Vector2(8, 6),
                    new Vector2(9, 5),
                    new Vector2(9, 1),
                    new Vector2(7, 1),
                    new Vector2(7, 3),
                    new Vector2(2, 3),
                    new Vector2(6, 4),
                    new Vector2(5.5f, 5),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 3),
                    new Vector2(4, 2),
                    new Vector2(3, 2),
                    new Vector2(3, 3),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(6, 3),
                    new Vector2(6, 2),
                    new Vector2(5, 2),
                    new Vector2(5, 3),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 4),
                    new Vector2(3.5f, 5),
                    new Vector2(5.5f, 5),
                    new Vector2(5, 4),
                }),
            };
            List<Polygon> observed = Triangulation.GetYMonotonePolygons(polygons, holes);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygons_SeparateTriangles()
        {
            List<Polygon> polygons = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(2, 0),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 1),
                    new Vector2(3, 0),
                    new Vector2(2, 1),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 2),
                    new Vector2(2, 3),
                    new Vector2(4, 2),
                }),
            };

            List<Polygon> expected = polygons;
            List<Polygon> observed = Triangulation.GetYMonotonePolygons(polygons, new List<Polygon>());

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void GetYMonotonePolygons_IslandWithinAnIsland()
        {
            List<Polygon> polygons = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(0, 9),
                    new Vector2(9, 9),
                    new Vector2(9, 0),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(2, 2),
                    new Vector2(2, 7),
                    new Vector2(7, 7),
                    new Vector2(7, 2),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 4),
                    new Vector2(4, 5),
                    new Vector2(5, 5),
                    new Vector2(5, 4),
                }),
            };

            List<Polygon> holes = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(1, 1),
                    new Vector2(1, 8),
                    new Vector2(8, 8),
                    new Vector2(8, 1),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(3, 3),
                    new Vector2(3, 6),
                    new Vector2(6, 6),
                    new Vector2(6, 3),
                }),
            };

            List<Polygon> expected = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(0, 9),
                    new Vector2(1, 8),
                    new Vector2(1, 1),
                    new Vector2(8, 1),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(8, 1),
                    new Vector2(8, 8),
                    new Vector2(1, 8),
                    new Vector2(0, 9),
                    new Vector2(9, 9),
                    new Vector2(9, 0),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(2, 2),
                    new Vector2(2, 7),
                    new Vector2(3, 6),
                    new Vector2(3, 3),
                    new Vector2(6, 3),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(2, 2),
                    new Vector2(6, 3),
                    new Vector2(6, 6),
                    new Vector2(3, 6),
                    new Vector2(2, 7),
                    new Vector2(7, 7),
                    new Vector2(7, 2),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 4),
                    new Vector2(4, 5),
                    new Vector2(5, 5),
                    new Vector2(5, 4),
                }),
            };
            List<Polygon> observed = Triangulation.GetYMonotonePolygons(polygons, holes);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        #endregion

        #region Triangulate

        [Test]
        public void Triangulate_Triangle()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 4),
                new Vector2(4, 4),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 4))
            };
            List<Triangle> observed = Triangulation.Triangulate(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void Triangulate_SeparateTriangles()
        {
            List<Polygon> polygons = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(2, 0),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(4, 1),
                    new Vector2(3, 0),
                    new Vector2(2, 1),
                }),
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 2),
                    new Vector2(2, 3),
                    new Vector2(4, 2),
                }),
            };

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(1, 1), new Vector2(2, 0)),
                new Triangle(new Vector2(4, 1), new Vector2(3, 0), new Vector2(2, 1)),
                new Triangle(new Vector2(0, 2), new Vector2(2, 3), new Vector2(4, 2)),
            };
            List<Triangle> observed = Triangulation.Triangulate(polygons, new List<Polygon>());

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void Triangulate_Square()
        {
            List<Polygon> polygons = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(0, 2),
                    new Vector2(2, 2),
                    new Vector2(2, 0),
                }),
            };

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(0, 2), new Vector2(2, 0)),
                new Triangle(new Vector2(2, 2), new Vector2(2, 0), new Vector2(0, 2)),
            };
            List<Triangle> observed = Triangulation.Triangulate(polygons, new List<Polygon>());

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void Triangulate_MergeVertex()
        {
            List<Polygon> polygons = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(2, 0),
                    new Vector2(0, 4),
                    new Vector2(1, 3),
                    new Vector2(2, 4),
                }),
            };

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(2, 0), new Vector2(0, 4), new Vector2(1, 3)),
                new Triangle(new Vector2(2, 0), new Vector2(1, 3), new Vector2(2, 4)),
            };
            List<Triangle> observed = Triangulation.Triangulate(polygons, new List<Polygon>());

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void Triangulate_SplitVertex()
        {
            List<Polygon> polygons = new List<Polygon>()
            {
                new Polygon(new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(1, 4),
                    new Vector2(2, 0),
                    new Vector2(1, 1),
                }),
            };

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(1, 4), new Vector2(1, 1)),
                new Triangle(new Vector2(2, 0), new Vector2(1, 1), new Vector2(1, 4)),
            };
            List<Triangle> observed = Triangulation.Triangulate(polygons, new List<Polygon>());

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void Triangulate_VertexWithMultipleDivisions()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), // 0
                new Vector2(0, 5),
                new Vector2(1, 4),
                new Vector2(2, 5),
                new Vector2(3, 3),
                new Vector2(4, 5), // 5
                new Vector2(5, 4),
                new Vector2(6, 5),
                new Vector2(6, 0),
                new Vector2(4, 2),
                new Vector2(3, 0), // 10
                new Vector2(2, 1),
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(0, 5), new Vector2(1, 4)),
                new Triangle(new Vector2(0, 0), new Vector2(1, 4), new Vector2(4, 2)),
                new Triangle(new Vector2(1, 4), new Vector2(3, 3), new Vector2(4, 2)),
                new Triangle(new Vector2(1, 4), new Vector2(2, 5), new Vector2(3, 3)),
                new Triangle(new Vector2(2, 1), new Vector2(4, 2), new Vector2(3, 0)),
                new Triangle(new Vector2(3, 3), new Vector2(4, 5), new Vector2(5, 4)),
                new Triangle(new Vector2(3, 3), new Vector2(5, 4), new Vector2(6, 0)),
                new Triangle(new Vector2(5, 4), new Vector2(6, 5), new Vector2(6, 0)),
            };
            List<Triangle> observed = Triangulation.Triangulate(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        [Test]
        public void Triangulate_BottomAndTopCrenels()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(4, 0), // 0
                new Vector2(4, 1),
                new Vector2(3, 1),
                new Vector2(3, 0),
                new Vector2(2, 0),
                new Vector2(2, 1), // 5
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(5, 2), // 10
                new Vector2(5, 1),
                new Vector2(6, 1),
                new Vector2(6, 2),
                new Vector2(7, 2),
                new Vector2(7, 1), // 15
                new Vector2(8, 1),
                new Vector2(8, 2),
                new Vector2(9, 2),
                new Vector2(9, 0), // 19
            });

            List<Triangle> expected = new List<Triangle>()
            {
                new Triangle(new Vector2(0, 0), new Vector2(1, 1), new Vector2(1, 0)),
                new Triangle(new Vector2(0, 0), new Vector2(0, 2), new Vector2(1, 1)),
                new Triangle(new Vector2(0, 2), new Vector2(5, 2), new Vector2(1, 1)),
                new Triangle(new Vector2(1, 1), new Vector2(5, 2), new Vector2(5, 1)),
                new Triangle(new Vector2(2, 0), new Vector2(2, 1), new Vector2(3, 1)),
                new Triangle(new Vector2(2, 0), new Vector2(3, 1), new Vector2(3, 0)),
                new Triangle(new Vector2(4, 0), new Vector2(4, 1), new Vector2(5, 1)),
                new Triangle(new Vector2(6, 1), new Vector2(6, 2), new Vector2(7, 1)),
                new Triangle(new Vector2(6, 2), new Vector2(7, 2), new Vector2(7, 1)),
                new Triangle(new Vector2(4, 0), new Vector2(5, 1), new Vector2(8, 1)),
                new Triangle(new Vector2(4, 0), new Vector2(8, 1), new Vector2(9, 0)),
                new Triangle(new Vector2(9, 0), new Vector2(8, 1), new Vector2(9, 2)),
                new Triangle(new Vector2(8, 1), new Vector2(8, 2), new Vector2(9, 2)),
            };
            List<Triangle> observed = Triangulation.Triangulate(polygon);

            PolygonDrawAssert.ListsContainSame(expected, observed);
        }

        #endregion
    }
}