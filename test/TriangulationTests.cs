using NUnit.Framework;
using PolygonDraw;
using System.Collections.Generic;

namespace PolygonDrawTests
{
    public class TriangulationTests
    {
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
                new PolygonEdge(new PolygonVertex(polygon, 1), new PolygonVertex(polygon, 5)),
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
                new PolygonEdge(new PolygonVertex(polygon, 6), new PolygonVertex(polygon, 1)),
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
                new PolygonEdge(new PolygonVertex(polygon, 6), new PolygonVertex(polygon, 9)), // good
                new PolygonEdge(new PolygonVertex(polygon, 2), new PolygonVertex(polygon, 5)), // good
                new PolygonEdge(new PolygonVertex(polygon, 4), new PolygonVertex(polygon, 1)),
                new PolygonEdge(new PolygonVertex(polygon, 15), new PolygonVertex(polygon, 12)),
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
    }
}