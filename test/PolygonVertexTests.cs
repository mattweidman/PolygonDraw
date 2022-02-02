using NUnit.Framework;
using PolygonDraw;
using System.Collections.Generic;

namespace PolygonDrawTests
{
    public class PolygonVertexTests
    {
        [Test]
        public void GetVertexType_Diamond()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0),
                new Vector2(0, 2),
                new Vector2(2, 4),
                new Vector2(4, 2),
            });

            PolygonVertex.VertexType[] expectedTypes = new PolygonVertex.VertexType[]
            {
                PolygonVertex.VertexType.END,
                PolygonVertex.VertexType.EXTERIOR_LEFT,
                PolygonVertex.VertexType.START,
                PolygonVertex.VertexType.EXTERIOR_RIGHT,
            };

            for (int i = 0; i < expectedTypes.Length; i++)
            {
                Assert.AreEqual(
                    expectedTypes[i],
                    new PolygonVertex(polygon, i, false).GetVertexType());
            }
        }

        [Test]
        public void GetVertexType_Star()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(1, 2),
                new Vector2(0, 4),
                new Vector2(3, 3),
                new Vector2(6, 4),
                new Vector2(5, 2),
                new Vector2(6, 0),
                new Vector2(3, 1),
            });

            PolygonVertex.VertexType[] expectedTypes = new PolygonVertex.VertexType[]
            {
                PolygonVertex.VertexType.END,
                PolygonVertex.VertexType.EXTERIOR_LEFT,
                PolygonVertex.VertexType.START,
                PolygonVertex.VertexType.MERGE,
                PolygonVertex.VertexType.START,
                PolygonVertex.VertexType.EXTERIOR_RIGHT,
                PolygonVertex.VertexType.END,
                PolygonVertex.VertexType.SPLIT,
            };

            for (int i = 0; i < expectedTypes.Length; i++)
            {
                Assert.AreEqual(
                    expectedTypes[i],
                    new PolygonVertex(polygon, i, false).GetVertexType());
            }
        }

        [Test]
        public void GetVertexType_HorizontalTop()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(2, 0),
                new Vector2(0, 3),
                new Vector2(2, 2),
                new Vector2(4, 2),
            });

            PolygonVertex.VertexType[] expectedTypes = new PolygonVertex.VertexType[]
            {
                PolygonVertex.VertexType.END,
                PolygonVertex.VertexType.START,
                PolygonVertex.VertexType.MERGE,
                PolygonVertex.VertexType.START,
            };

            for (int i = 0; i < expectedTypes.Length; i++)
            {
                Assert.AreEqual(
                    expectedTypes[i],
                    new PolygonVertex(polygon, i, false).GetVertexType());
            }
        }

        [Test]
        public void GetVertexType_HorizontalBottom()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, -2),
                new Vector2(1, 2),
                new Vector2(4, 0),
                new Vector2(2, 0),
            });

            PolygonVertex.VertexType[] expectedTypes = new PolygonVertex.VertexType[]
            {
                PolygonVertex.VertexType.END,
                PolygonVertex.VertexType.START,
                PolygonVertex.VertexType.END,
                PolygonVertex.VertexType.SPLIT,
            };

            for (int i = 0; i < expectedTypes.Length; i++)
            {
                Assert.AreEqual(
                    expectedTypes[i],
                    new PolygonVertex(polygon, i, false).GetVertexType());
            }
        }

        [Test]
        public void GetVertexType_Square()
        {
            Polygon polygon = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(2, 2),
                new Vector2(2, 0),
            });

            PolygonVertex.VertexType[] expectedTypes = new PolygonVertex.VertexType[]
            {
                PolygonVertex.VertexType.END,
                PolygonVertex.VertexType.START,
                PolygonVertex.VertexType.START,
                PolygonVertex.VertexType.END,
            };

            for (int i = 0; i < expectedTypes.Length; i++)
            {
                Assert.AreEqual(
                    expectedTypes[i],
                    new PolygonVertex(polygon, i, false).GetVertexType());
            }
        }
    }
}