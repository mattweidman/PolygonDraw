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
        /// Find how to divide a polygon into y-monotone polygons. Return
        /// a list of edges where polygons should be divided in order
        /// to produce y-monotone polygons.
        /// </summary>
        /// <param name="polygons">Polygon to fill.</param>
        public static List<PolygonEdge> GetYMonotonePolygonDivisions(Polygon polygon)
        {
            return GetYMonotonePolygonDivisions(new List<Polygon>() { polygon }, new List<Polygon>());
        }

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
            
            IEnumerable<PolygonVertex> allVertices = polygonVertices
                .Concat(holeVertices)
                .ToList();
            
            float xDiff = allVertices.Max(v => v.x) - allVertices.Min(v => v.x);
            
            // Sort by descending y, then increasing x
            allVertices = allVertices
                .OrderBy(pv => - pv.y * xDiff + pv.x)
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
                if (vertexType == PolygonVertex.VertexType.START)
                {
                    LineSegment prevLine = new LineSegment(vertex.prevVertex, vertex.vertex);
                    if (!prevLine.IsHorizontal())
                    {
                        // Insert line segment into status and record its helper.
                        PolygonVertex prevPolygonVertex = vertex.prevPolygonVertex;
                        lineSegmentTree.Insert(prevLine, prevPolygonVertex);
                        helperMap[prevPolygonVertex] = vertex;
                    }
                }
                else if (vertexType == PolygonVertex.VertexType.END)
                {
                    LineSegment nextLine = new LineSegment(vertex.vertex, vertex.nextVertex);
                    if (!nextLine.IsHorizontal())
                    {
                        // Connect to merge vertex if needed.
                        PolygonVertex helper = helperMap[vertex];
                        if (helper.GetVertexType() == PolygonVertex.VertexType.MERGE)
                        {
                            divisions.Add(new PolygonEdge(vertex, helper));
                        }

                        // Remove line segment from status.
                        LineSegment lineSegment = new LineSegment(vertex.vertex, vertex.nextVertex);
                        lineSegmentTree.Remove(lineSegment);
                        helperMap.Remove(vertex);
                    }
                }
                else if (vertexType == PolygonVertex.VertexType.SPLIT)
                {
                    LineSegment nextLine = new LineSegment(vertex.vertex, vertex.nextVertex);
                    if (!nextLine.IsHorizontal())
                    {
                        // Add an edge to the helper of the edge to the left.
                        LineSegmentTree<PolygonVertex>.FetchedData lsData =
                            lineSegmentTree.GetLineSegmentDataToTheLeft(vertex.vertex);
                        PolygonVertex helper = helperMap[lsData.metadata];
                        divisions.Add(new PolygonEdge(vertex, helper));
                        helperMap[lsData.metadata] = vertex;
                    }

                    LineSegment prevLine = new LineSegment(vertex.prevVertex, vertex.vertex);
                    if (!prevLine.IsHorizontal())
                    {
                        // Insert into status and record helper.
                        LineSegment newLine = new LineSegment(vertex.prevVertex, vertex.vertex);
                        lineSegmentTree.Insert(newLine, vertex.prevPolygonVertex);
                        helperMap[vertex.prevPolygonVertex] = vertex;
                    }
                }
                else if (vertexType == PolygonVertex.VertexType.MERGE)
                {
                    LineSegment nextLine = new LineSegment(vertex.vertex, vertex.nextVertex);
                    if (!nextLine.IsHorizontal())
                    {
                        // If the helper of the next edge is a merge vertex, connect it.
                        PolygonVertex nextEdgeHelper = helperMap[vertex];
                        if (nextEdgeHelper.GetVertexType() == PolygonVertex.VertexType.MERGE)
                        {
                            divisions.Add(new PolygonEdge(vertex, nextEdgeHelper));
                        }

                        // Remove next edge from the status.
                        LineSegment nextEdge = new LineSegment(vertex.vertex, vertex.nextVertex);
                        lineSegmentTree.Remove(nextEdge);
                        helperMap.Remove(vertex);
                    }

                    LineSegment prevLine = new LineSegment(vertex.prevVertex, vertex.vertex);
                    if (!prevLine.IsHorizontal())
                    {
                        // Connect to the helper of the edge to the left if it's a merge vertex.
                        LineSegmentTree<PolygonVertex>.FetchedData lsData =
                            lineSegmentTree.GetLineSegmentDataToTheLeft(vertex.vertex);
                        PolygonVertex leftEdgeHelper = helperMap[lsData.metadata];
                        if (leftEdgeHelper.GetVertexType() == PolygonVertex.VertexType.MERGE)
                        {
                            divisions.Add(new PolygonEdge(vertex, leftEdgeHelper));
                        }

                        // Update the helper of the edge to the left.
                        helperMap[lsData.metadata] = vertex;
                    }
                }
                else if (vertexType == PolygonVertex.VertexType.EXTERIOR_LEFT)
                {
                    // Connect to helper if it is a merge vertex.
                    // TODO: helper could be not found for horizontal edges
                    PolygonVertex helper = helperMap[vertex];
                    if (helper.GetVertexType() == PolygonVertex.VertexType.MERGE)
                    {
                        divisions.Add(new PolygonEdge(vertex, helper));
                    }

                    // Remove an edge.
                    LineSegment topEdge = new LineSegment(vertex.vertex, vertex.nextVertex);
                    lineSegmentTree.Remove(topEdge);
                    helperMap.Remove(vertex);

                    // Add an edge.
                    LineSegment bottomEdge = new LineSegment(vertex.prevVertex, vertex.vertex);
                    lineSegmentTree.Insert(bottomEdge, vertex.prevPolygonVertex);
                    helperMap[vertex.prevPolygonVertex] = vertex;
                }
                else if (vertexType == PolygonVertex.VertexType.EXTERIOR_RIGHT)
                {
                    // Connect to the helper of the edge to the left if it is a merge vertex.
                    LineSegmentTree<PolygonVertex>.FetchedData lsData =
                        lineSegmentTree.GetLineSegmentDataToTheLeft(vertex.vertex);
                    // TODO: helper could be not found for horizontal edges
                    PolygonVertex helper = helperMap[lsData.metadata];
                    if (helper.GetVertexType() == PolygonVertex.VertexType.MERGE)
                    {
                        divisions.Add(new PolygonEdge(vertex, helper));
                    }

                    // Change the helper we used.
                    helperMap[lsData.metadata] = vertex;
                }
            }

            return divisions;
        }
    }
}