using NUnit.Framework;
using PolygonDraw;

namespace PolygonDrawTests
{
    public class TriangleTests
    {
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
    }
}