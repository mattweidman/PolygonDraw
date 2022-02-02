using System.Collections.Generic;
using System.Linq;

namespace PolygonDraw
{
    /// <summary>
    /// Static class to help with triangulating polygons. Follows sweep-line triangulation.
    /// See https://www.cs.umd.edu/class/spring2020/cmsc754/Lects/lect05-triangulate.pdf
    /// and https://cs.gmu.edu/~jmlien/teaching/09_fall_cs633/uploads/Main/lecture03.pdf
    /// </summary>
    public static class Triangulation
    {   
        /// <summary>
        /// Find how to divide polygons into y-monotone polygons. Return
        /// a list of edges where polygons should be divided in order
        /// to produce y-monotone polygons. Can also take holes in polygons,
        /// assuming holes are contained entirely inside other polygons.
        /// </summary>
        /// <param name="polygons">List of polygons to fill.</param>
        /// <param name="holes">Empty spaces within polygons that should
        /// not be filled. It is assumed holes are contained entirely inside
        /// polygons.</param>
        public static List<PolygonEdge> GetYMonotonePolygonDivisions(
            List<Polygon> polygons,
            List<Polygon> holes)
        {
            IEnumerable<PolygonVertex> polygonVertices = polygons
                .SelectMany(polygon => polygon.vertices
                    .Select((vertex, i) => new PolygonVertex(polygon, i, false)));
            
            IEnumerable<PolygonVertex> holeVertices = holes
                .SelectMany(polygon => polygon.vertices
                    // convert to enumerable so we can use Reverse()
                    .AsEnumerable()
                    // holes should have opposite direction from polygons
                    .Reverse()
                    .Select((vertex, i) => new PolygonVertex(polygon, i, true)));
            
            List<PolygonVertex> allVertices = polygonVertices
                .Concat(holeVertices)
                .OrderByDescending(pv => pv.y)
                .ToList();
            
            // Tree to store edges. Each PolygonVertex stored as metadata is a
            // the first vertex of the edge. We need to store it so that we can
            // use it to look up helpers.
            var lineSegmentTree = new LineSegmentTree<PolygonVertex>();

            // Maps <first vertex of edge> -> <helper vertex>.
            var helperMap = new Dictionary<PolygonVertex, PolygonVertex>();

            // Output edges
            var divisions = new List<PolygonEdge>();

            foreach (PolygonVertex vertex in allVertices)
            {
                PolygonVertex.VertexType vertexType = vertex.GetVertexType();
                // TODO
            }

            return divisions;
        }
    }
}