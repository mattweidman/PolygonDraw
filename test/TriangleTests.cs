using NUnit.Framework;
using PolygonDraw;

namespace PolygonDrawTests
{
    public class TriangleTests
    {
        #region GetIntersections

        [Test]
        public void GetIntersections_NoIntersections()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(5, 0), new Vector2(5, 4), new Vector2(9, 0));

            Vector2[][] expected = new Vector2[][]
            {
                new Vector2[] { null, null, null },
                new Vector2[] { null, null, null },
                new Vector2[] { null, null, null },
            };

            Vector2[][] observed = t1.GetIntersections(t2);

            PolygonDrawAssert.Array2DsEqual(expected, observed);
        }

        [Test]
        public void GetIntersections_TwoIntersections()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(1, 1), new Vector2(5, 5), new Vector2(5, 1));

            Vector2[][] expected = new Vector2[][]
            {
                new Vector2[] { null, null, null },
                new Vector2[] { new Vector2(2, 2), null, new Vector2(3, 1) },
                new Vector2[] { null, null, null },
            };

            Vector2[][] observed = t1.GetIntersections(t2);

            PolygonDrawAssert.Array2DsEqual(expected, observed);
        }

        [Test]
        public void GetIntersections_FourIntersections()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(6, 4), new Vector2(1, -1), new Vector2(1, 4));

            Vector2[][] expected = new Vector2[][]
            {
                new Vector2[] { null, null, null },
                new Vector2[] { new Vector2(3, 1), new Vector2(1, 3), null },
                new Vector2[] { new Vector2(2, 0), new Vector2(1, 0), null },
            };

            Vector2[][] observed = t1.GetIntersections(t2);

            PolygonDrawAssert.Array2DsEqual(expected, observed);
        }

        [Test]
        public void GetIntersections_SixIntersections()
        {
            Triangle t1 = new Triangle(new Vector2(0, 1), new Vector2(1, 3), new Vector2(2, 1));
            Triangle t2 = new Triangle(new Vector2(0, 2), new Vector2(2, 2), new Vector2(1, 0));

            Vector2[][] expected = new Vector2[][]
            {
                new Vector2[] { new Vector2(0.5f, 2), null, new Vector2(0.25f, 1.5f) },
                new Vector2[] { new Vector2(1.5f ,2), new Vector2(1.75f, 1.5f), null },
                new Vector2[] { null, new Vector2(1.5f, 1), new Vector2(0.5f, 1) },
            };

            Vector2[][] observed = t1.GetIntersections(t2);

            PolygonDrawAssert.Array2DsEqual(expected, observed);
        }

        #endregion

        #region MaskToIntersectionGraph

        [Test]
        public void MaskToIntersectionGraph_NoIntersections()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(5, 0), new Vector2(5, 4), new Vector2(9, 0));

            TriangleIntersectionGraph expected = new TriangleIntersectionGraph(
                new Vector2[] {new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0),
                    new Vector2(5, 0), new Vector2(5, 4), new Vector2(9, 0)},
                new int?[] {1, 2, 0, null, null, null},
                new int?[] {null, null, null, 5, 3, 4}
            );

            TriangleIntersectionGraph observed = t1.MaskToIntersectionGraph(t2);

            PolygonDrawAssert.AreEqual(expected, observed);
        }

        [Test]
        public void MaskToIntersectionGraph_TwoIntersections()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(1, 1), new Vector2(5, 5), new Vector2(5, 1));

            TriangleIntersectionGraph expected = new TriangleIntersectionGraph(
                new Vector2[] {new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0),
                    new Vector2(1, 1), new Vector2(5, 5), new Vector2(5, 1),
                    new Vector2(2, 2), new Vector2(3, 1)},
                new int?[] {1, 6, 0, null, null, null, 7, 2},
                new int?[] {null, null, null, 7, 6, 4, 3, 5}
            );

            TriangleIntersectionGraph observed = t1.MaskToIntersectionGraph(t2);

            PolygonDrawAssert.AreEqual(expected, observed);
        }

        [Test]
        public void MaskToIntersectionGraph_FourIntersections()
        {
            Triangle t1 = new Triangle(new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0));
            Triangle t2 = new Triangle(new Vector2(6, 4), new Vector2(1, -1), new Vector2(1, 4));

            TriangleIntersectionGraph expected = new TriangleIntersectionGraph(
                new Vector2[] {new Vector2(0, 0), new Vector2(0, 4), new Vector2(4, 0),
                    new Vector2(6, 4), new Vector2(1, -1), new Vector2(1, 4),
                    new Vector2(3, 1), new Vector2(1, 3), new Vector2(2, 0), new Vector2(1, 0)},
                new int?[] {1, 7, 8, null, null, null, 2, 6, 9, 0},
                new int?[] {null, null, null, 5, 8, 7, 3, 9, 6, 4}
            );

            TriangleIntersectionGraph observed = t1.MaskToIntersectionGraph(t2);

            PolygonDrawAssert.AreEqual(expected, observed);
        }

        [Test]
        public void MaskToIntersectionGraph_SixIntersections()
        {
            Triangle t1 = new Triangle(new Vector2(0, 1), new Vector2(1, 3), new Vector2(2, 1));
            Triangle t2 = new Triangle(new Vector2(0, 2), new Vector2(2, 2), new Vector2(1, 0));

            TriangleIntersectionGraph expected = new TriangleIntersectionGraph(
                new Vector2[] {new Vector2(0, 1), new Vector2(1, 3), new Vector2(2, 1),
                    new Vector2(0, 2), new Vector2(2, 2), new Vector2(1, 0),
                    new Vector2(0.5f, 2), new Vector2(0.25f, 1.5f),
                    new Vector2(1.5f ,2), new Vector2(1.75f, 1.5f),
                    new Vector2(1.5f, 1), new Vector2(0.5f, 1)},
                new int?[] {7, 8, 10, null, null, null, 1, 6, 9, 2, 11, 0},
                new int?[] {null, null, null, 7, 8, 10, 3, 11, 6, 4, 9, 5}
            );

            TriangleIntersectionGraph observed = t1.MaskToIntersectionGraph(t2);

            PolygonDrawAssert.AreEqual(expected, observed);
        }

        #endregion
    }
}