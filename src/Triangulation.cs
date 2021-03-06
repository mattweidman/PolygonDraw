using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        /// Divide a polygon into triangles.
        /// </summary>
        /// <param name="polygons">List of polygons to fill.</param>
        public static ImmutableList<Triangle> Triangulate(Polygon polygon)
        {
            return Triangulate(new List<Polygon> { polygon }, new List<Polygon>());
        }

        /// <summary>
        /// Divide a polygon into triangles.
        /// </summary>
        /// <param name="arrangement">Arrangement of polygons and holes.</param>
        public static ImmutableList<Triangle> Triangulate(PolygonArrangement arrangement)
        {
            return Triangulate(arrangement.polygons, arrangement.holes);
        }

        /// <summary>
        /// Divide polygons and holes into triangles.
        /// </summary>
        /// <param name="polygons">List of polygons to fill.</param>
        /// <param name="holes">Empty spaces within polygons that should
        /// not be filled. It is assumed holes are contained entirely inside
        /// polygons.</param>
        public static ImmutableList<Triangle> Triangulate(
            IEnumerable<Polygon> polygons, IEnumerable<Polygon> holes)
        {
            return GetYMonotonePolygons(polygons, holes)
                .SelectMany(polygon => polygon.MonotoneTriangulate())
                .ToImmutableList();
        }

        /// <summary>
        /// Divide a polygon into y-monotone polygons.
        /// </summary>
        /// <param name="polygons">List of polygons to fill.</param>
        internal static List<Polygon> GetYMonotonePolygons(Polygon polygon)
        {
            return GetYMonotonePolygons(new List<Polygon> { polygon }, new List<Polygon>());
        }

        /// <summary>
        /// Divide polygons and holes into y-monotone polygons.
        /// </summary>
        /// <param name="polygons">List of polygons to fill.</param>
        /// <param name="holes">Empty spaces within polygons that should
        /// not be filled. It is assumed holes are contained entirely inside
        /// polygons.</param>
        internal static List<Polygon> GetYMonotonePolygons(
            IEnumerable<Polygon> polygons, IEnumerable<Polygon> holes)
        {
            List<PolygonEdge> divisions = GetYMonotonePolygonDivisions(polygons, holes);
            List<PolygonVertex> allVertices = PolygonsToPolygonVertices(polygons, false)
                .Concat(PolygonsToPolygonVertices(holes, true))
                .ToList();
            Dictionary<PolygonVertex, List<PolygonVertex>> adjMap = GetDivisionsMap(divisions, allVertices);
            HashSet<PolygonVertex> verticesVisited = new HashSet<PolygonVertex>();
            List<Polygon> yMonotonePolygons = new List<Polygon>();

            // Iterate over each unvisited vertex
            foreach (PolygonVertex startPv in allVertices)
            {
                if (verticesVisited.Contains(startPv))
                {
                    continue;
                }

                verticesVisited.Add(startPv);

                HashSet<PolygonEdge> edgesVisited = new HashSet<PolygonEdge>();
                
                // Initialize queue with edges from start vertex
                Queue<PolygonEdge> edgesToTraverse = new Queue<PolygonEdge>();
                foreach (PolygonVertex nextPv in adjMap[startPv])
                {
                    edgesToTraverse.Enqueue(new PolygonEdge(startPv, nextPv));
                }

                // BFS
                while (edgesToTraverse.Any())
                {
                    PolygonEdge initialEdge = edgesToTraverse.Dequeue();

                    if (edgesVisited.Contains(initialEdge))
                    {
                        continue;
                    }

                    List<PolygonVertex> yMonotoneVertices = new List<PolygonVertex>()
                    {
                        initialEdge.vertex1,
                    };

                    // Loop to create the smallest-possible polygon and add edges to queue
                    PolygonEdge currentEdge = initialEdge;
                    while (!currentEdge.vertex2.Equals(initialEdge.vertex1))
                    {
                        List<PolygonVertex> nextPvs = adjMap[currentEdge.vertex2]
                            .Where(nextPv => !nextPv.Equals(currentEdge.vertex1))
                            .ToList();

                        // Add this vertex to the list
                        yMonotoneVertices.Add(currentEdge.vertex2);

                        // Visit the vertex
                        verticesVisited.Add(currentEdge.vertex2);
                        edgesVisited.Add(currentEdge);

                        // Find the edge that makes the smallest possible polygon
                        int rightmostPvIndex = nextPvs.ArgMax(pv => {
                            Vector2 dir1 = currentEdge.vertex1.vertex - currentEdge.vertex2.vertex;
                            Vector2 dir2 = pv.vertex - currentEdge.vertex2.vertex;
                            return dir1.Angle(dir2);
                        });

                        // Add the rest of the edges to the queue for later
                        foreach (PolygonVertex nextPv in nextPvs.Where((_, i) => i != rightmostPvIndex))
                        {
                            edgesToTraverse.Enqueue(new PolygonEdge(currentEdge.vertex2, nextPv));
                        }

                        // Move to the polygon-size-minimizing edge
                        currentEdge = new PolygonEdge(currentEdge.vertex2, nextPvs[rightmostPvIndex]);
                    }

                    edgesVisited.Add(currentEdge);
                    yMonotonePolygons.Add(new Polygon(yMonotoneVertices
                        .Select(pv => pv.vertex)
                        .Where((v, i) => {
                            // Remove vertices with a 180-degree angle
                            int prevIndex = (i + yMonotoneVertices.Count() - 1) % yMonotoneVertices.Count();
                            Vector2 prevDir = yMonotoneVertices[prevIndex].vertex - v;

                            int nextIndex = (i + 1) % yMonotoneVertices.Count();
                            Vector2 nextDir = yMonotoneVertices[nextIndex].vertex - v;

                            float angle = nextDir.Angle(prevDir);
                            return !FloatHelpers.Eq(angle, MathF.PI);
                        })
                        .ToList()));
                }
            }

            return yMonotonePolygons;
        }

        /// <summary>
        /// Create adjacency map for polygon edges.
        /// </summary>
        private static Dictionary<PolygonVertex, List<PolygonVertex>> GetDivisionsMap(
            IEnumerable<PolygonEdge> divisions, IEnumerable<PolygonVertex> allVertices)
        {
            var adjMap = new Dictionary<PolygonVertex, List<PolygonVertex>>();

            foreach (PolygonVertex pv in allVertices)
            {
                adjMap[pv] = new List<PolygonVertex>() { pv.nextPolygonVertex };
            }
            
            foreach (PolygonEdge edge in divisions)
            {
                adjMap[edge.vertex1].Add(edge.vertex2);
                adjMap[edge.vertex2].Add(edge.vertex1);
            }

            return adjMap;
        }

        /// <summary>
        /// Find how to divide a polygon into y-monotone polygons. Return
        /// a list of edges where polygons should be divided in order
        /// to produce y-monotone polygons.
        /// </summary>
        /// <param name="polygons">Polygon to fill.</param>
        internal static List<PolygonEdge> GetYMonotonePolygonDivisions(Polygon polygon)
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
        internal static List<PolygonEdge> GetYMonotonePolygonDivisions(
            IEnumerable<Polygon> polygons, IEnumerable<Polygon> holes)
        {
            List<PolygonVertex> allVertices = PolygonsToPolygonVertices(polygons, false)
                .Concat(PolygonsToPolygonVertices(holes, true))
                .ToList();
            
            // Sort by descending y, then increasing x
            float xDiff = allVertices.Max(v => v.x) - allVertices.Min(v => v.x);
            allVertices = allVertices.OrderBy(pv => - pv.y * xDiff + pv.x).ToList();
            
            // Tree to store edges. Each PolygonVertex stored as metadata is a
            // the first vertex of the edge. We need to store it so that we can
            // use it to look up helpers.
            var lineSegmentTree = new LineSegmentTree<PolygonVertex>();

            // Maps <first vertex of edge> -> <helper vertex>.
            var helperMap = new Dictionary<PolygonVertex, PolygonVertex>();

            // Output edges
            var divisions = new List<PolygonEdge>();

            // Track line segments to remove from the tree so that we can remove
            // horizontal line segments at the same y-coordinate at once.
            float previousY = allVertices[0].y + 1;
            List<PolygonVertex> lineSegmentsToRemove = new List<PolygonVertex>();

            foreach (PolygonVertex vertex in allVertices)
            {
                if (!FloatHelpers.Eq(vertex.y, previousY))
                {
                    foreach (PolygonVertex pv in lineSegmentsToRemove)
                    {
                        lineSegmentTree.Remove(new LineSegment(pv.vertex, pv.nextVertex));
                        helperMap.Remove(pv);
                    }

                    lineSegmentsToRemove.Clear();
                }

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
                        lineSegmentTree.Remove(nextLine);
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
                        lineSegmentTree.Insert(prevLine, vertex.prevPolygonVertex);
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

                        // Temporarily remove the line segment from the status so that we can
                        // search for vertex in the tree.
                        lineSegmentTree.Remove(nextLine);
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

                    if (!nextLine.IsHorizontal())
                    {
                        // Re-add the next edge to the status in case any vertices on the same
                        // y-coordinate need to connect to it. Remove it once we move on to other
                        // y-coordinates.
                        lineSegmentTree.Insert(nextLine, vertex);
                        lineSegmentsToRemove.Add(vertex);
                        helperMap[vertex] = vertex;
                    }
                }
                else if (vertexType == PolygonVertex.VertexType.EXTERIOR_LEFT)
                {
                    // Connect to helper if it is a merge vertex.
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
                    PolygonVertex helper = helperMap[lsData.metadata];
                    if (helper.GetVertexType() == PolygonVertex.VertexType.MERGE)
                    {
                        divisions.Add(new PolygonEdge(vertex, helper));
                    }

                    // Change the helper we used.
                    helperMap[lsData.metadata] = vertex;
                }

                previousY = vertex.y;
            }

            return divisions;
        }

        private static IEnumerable<PolygonVertex> PolygonsToPolygonVertices(
            IEnumerable<Polygon> polygons, bool isHole)
        {
            return polygons
                .SelectMany(polygon => polygon.vertices
                    .Select((vertex, i) => new PolygonVertex(polygon, i, isHole)));
        }
    }
}