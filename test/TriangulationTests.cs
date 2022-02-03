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
            List<PolygonEdge> observed = Triangulation.GetYMonotonePolygonDivisions(
                new List<Polygon>() { polygon },
                new List<Polygon>());
            
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
    }
}