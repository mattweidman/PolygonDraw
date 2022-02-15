using System;
using System.Collections.Generic;
using System.Linq;

namespace PolygonDraw
{
    public class Polygon
    {
        public List<Vector2> vertices;

        public Polygon(List<Vector2> vertices)
        {
            if (vertices.Count < 3)
            {
                throw new ArgumentException(
                    $"Cannot create a polygon [{string.Join(",", vertices)}] "
                    + "with less than 3 vertices.");
            }

            this.vertices = vertices;
        }

        #region Brute-Force Triangulation

        /// <summary>
        /// If any edge of this polygon blocks the straight line from 
        /// vertices[vIndex1] to vertices[vIndex2] return false. Else, return true.
        /// </summary>
        public bool InViewOf(int vIndex1, int vIndex2)
        {
            // Not in view if vertices are the same or their segment already forms an edge.
            if (this.AreAdjacent(vIndex1, vIndex2))
            {
                return false;
            }

            // Not in view if segment goes outside of polygon.
            if (this.BeginsOutside(vIndex1, this.vertices[vIndex2]) 
                || this.BeginsOutside(vIndex2, this.vertices[vIndex1]))
            {
                return false;
            }

            LineSegment segment = new LineSegment(vertices[vIndex1], vertices[vIndex2]);

            for (int i = 0; i < this.vertices.Count; i++)
            {
                int j = (i+1) % vertices.Count;

                // Don't count any edges that are connected to segment by an endpoint.
                if (i == vIndex1 || j == vIndex1 || i == vIndex2 || j == vIndex2)
                {
                    continue;
                }

                Vector2 vertex1 = this.vertices[i];
                Vector2 vertex2 = this.vertices[(i+1) % vertices.Count];
                LineSegment edge = new LineSegment(vertex1, vertex2);
                
                Vector2 intersection = segment.GetIntersection(edge, true);
                if (intersection != null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns whether the line from this.vertices[vIndex] to point begins
        /// outside of the polygon.
        /// </summary>
        public bool BeginsOutside(int vIndex, Vector2 point)
        {
            Vector2 vertex = this.vertices[vIndex];
            Vector2 pointDir = point - vertex;
            Vector2 d1 = this.vertices[(vIndex + 1) % this.vertices.Count] - vertex;
            Vector2 d2 = this.vertices[(vIndex + this.vertices.Count - 1) % this.vertices.Count] - vertex;

            return !pointDir.IsBetween(d1, d2);
        }

        /// <summary>
        /// Splits this polygon into two along the line from this.vertices[vIndex1]
        /// to this.vertices[vIndex2]. Precondition: vIndex1 and vIndex2 may not be
        /// next to each other.
        /// </summary>
        public (Polygon, Polygon) Split(int vIndex1, int vIndex2)
        {
            if (this.AreAdjacent(vIndex1, vIndex2))
            {
                throw new ArgumentException($"Split indices cannot be adjacent: {vIndex1}, {vIndex2}");
            }

            List<Vector2> poly1 = new List<Vector2>();
            List<Vector2> poly2 = new List<Vector2>();
            bool onPoly1 = true;

            for (int i = 0; i < this.vertices.Count; i++)
            {
                if (i == vIndex1 || i == vIndex2)
                {
                    onPoly1 = !onPoly1;
                    poly1.Add(this.vertices[i]);
                    poly2.Add(this.vertices[i]);
                }
                else if (onPoly1)
                {
                    poly1.Add(this.vertices[i]);
                }
                else
                {
                    poly2.Add(this.vertices[i]);
                }
            }

            return (new Polygon(poly1), new Polygon(poly2));
        }

        /// <summary>
        /// Divide this polygon into triangles that cover the same area.
        /// </summary>
        public List<Triangle> DivideIntoTriangles()
        {
            // If this polygon is a triangle, return it.
            if (this.vertices.Count == 3)
            {
                Triangle triangle = new Triangle(this.vertices[0], this.vertices[1], this.vertices[2]);
                return new List<Triangle>() { triangle };
            }

            // If not a triangle, find one way to split the polygon and recurse for each polygon.
            for (int i = 0; i < this.vertices.Count; i++)
            {
                int jMax = i == 0 ? this.vertices.Count - 1 : this.vertices.Count;
                for (int j = i + 2; j < jMax; j++)
                {
                    if (this.InViewOf(i, j))
                    {
                        (Polygon poly1, Polygon poly2) = this.Split(i, j);
                        
                        List<Triangle> triangles = new List<Triangle>();
                        triangles.AddRange(poly1.DivideIntoTriangles());
                        triangles.AddRange(poly2.DivideIntoTriangles());

                        return triangles;
                    }
                }
            }

            throw new ArgumentException("Polygon could not be divided into triangles.");
        }

        /// <summary>
        /// Whether the indices of two vertices in this polygon are next to each other.
        /// </summary>
        private bool AreAdjacent(int vIndex1, int vIndex2)
        {
            int absDiff = Math.Abs(vIndex1 - vIndex2);
            return absDiff <= 1 || absDiff == this.vertices.Count - 1;
        }

        #endregion

        public override bool Equals(object otherObj)
        {
            if (!(otherObj is Polygon other))
            {
                return false;
            }

            if (this.vertices.Count != other.vertices.Count)
            {
                return false;
            }

            // Find index of vertex in other that equals the first vertex in this
            int offset = 0;
            bool foundMatch = false;
            for (int i = 0; i < other.vertices.Count; i++)
            {
                if (other.vertices[i].Equals(this.vertices[0]))
                {
                    foundMatch = true;
                    offset = i;
                    break;
                }
            }
            if (!foundMatch)
            {
                return false;
            }

            for (int i = 1; i < this.vertices.Count; i++)
            {
                if (!this.vertices[i].Equals(other.vertices[(i + offset) % other.vertices.Count]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Polygon[{string.Join(",", this.vertices)}]";
        }

        #region Sweep-Line Triangulation

        /// <summary>
        /// Assuming this polygon is y-monotone, divide it into triangles that cover 
        /// the same area.
        /// </summary>
        public List<Triangle> MonotoneTriangulate()
        {
            int vertexMaxIndex = this.vertices.ArgMax(v => v.y);
            int vertexMinIndex = this.vertices.ArgMin(v => v.y);

            // Sort points by y-coordinate
            List<MonotonePolygonVertex> sortedPoints = new List<MonotonePolygonVertex>()
            {
                new MonotonePolygonVertex(this.vertices[vertexMaxIndex], MonotoneChain.Top)
            };
            int leftIndex = (vertexMaxIndex + this.vertices.Count - 1) % this.vertices.Count;
            int rightIndex = (vertexMaxIndex + 1) % this.vertices.Count;
            while (leftIndex != vertexMinIndex || rightIndex != vertexMinIndex)
            {
                // Add the higher one to the list each time
                if (FloatHelpers.Gt(this.vertices[leftIndex].y, this.vertices[rightIndex].y) ||
                    (FloatHelpers.Eq(this.vertices[leftIndex].y, this.vertices[rightIndex].y) &&
                    rightIndex == vertexMinIndex))
                {
                    sortedPoints.Add(new MonotonePolygonVertex(
                        this.vertices[leftIndex],
                        MonotoneChain.LeftChain));
                    leftIndex = (leftIndex + this.vertices.Count - 1) % this.vertices.Count;
                }
                else
                {
                    sortedPoints.Add(new MonotonePolygonVertex(
                        this.vertices[rightIndex],
                        MonotoneChain.RightChain));
                    rightIndex = (rightIndex + 1) % this.vertices.Count;
                }
            }
            sortedPoints.Add(new MonotonePolygonVertex(
                this.vertices[vertexMinIndex],
                MonotoneChain.Bottom));

            // Move from top to bottom, keeping a stack of vertices that still need triangles.
            List<Triangle> triangles = new List<Triangle>();
            Stack<MonotonePolygonVertex> stack = new Stack<MonotonePolygonVertex>();
            MonotoneChain lastPosition = MonotoneChain.Top;
            for (int i = 0; i < sortedPoints.Count; i++)
            {
                MonotonePolygonVertex currentVertex = sortedPoints[i];
                if (i < 2)
                {
                    // First two points can't be matched with anything.
                    stack.Push(currentVertex);
                }
                else if (currentVertex.position != lastPosition)
                {
                    // If we jumped across the polygon, connect vertex to everything in stack.
                    MonotonePolygonVertex previousVertex = stack.Pop();
                    MonotonePolygonVertex lastVertex = previousVertex;

                    while (stack.Count > 0)
                    {
                        MonotonePolygonVertex higherVertex = stack.Pop();
                        triangles.Add(GenerateTriangle(currentVertex, previousVertex, higherVertex));
                        previousVertex = higherVertex;
                    }

                    stack.Push(lastVertex);
                    stack.Push(currentVertex);
                }
                else
                {
                    // If we are on the same chain, connect to everything until we
                    // reach a vertex that isn't reachable.
                    MonotonePolygonVertex previousVertex = stack.Pop();

                    while (stack.Count > 0)
                    {
                        MonotonePolygonVertex higherVertex = stack.Peek();

                        // If higherVertex isn't in view, break.
                        if (!NewPointIsInView(currentVertex, previousVertex, higherVertex))
                        {
                            break;
                        }

                        Triangle newTriangle = GenerateTriangle(currentVertex, previousVertex, higherVertex);

                        // Don't add a triangle if its vertices are co-linear.
                        if (newTriangle.IsValidTriangle())
                        {
                            triangles.Add(newTriangle);
                        }

                        stack.Pop();
                        previousVertex = higherVertex;
                    }

                    stack.Push(previousVertex);
                    stack.Push(currentVertex);
                }

                lastPosition = currentVertex.position;
            }

            return triangles;
        }

        /// <summary>
        /// Takes 3 vertices and combines them into a triangle.
        /// </summary>
        private static Triangle GenerateTriangle(
            MonotonePolygonVertex p1,
            MonotonePolygonVertex p2,
            MonotonePolygonVertex p3)
        {
            Vector2 dir2 = p2.point - p1.point;
            Vector2 dir3 = p3.point - p1.point;
            float angle = dir2.Angle(dir3);

            if (FloatHelpers.Lt(angle, MathF.PI))
            {
                return new Triangle(p1.point, p2.point, p3.point);
            }
            else
            {
                return new Triangle(p1.point, p3.point, p2.point);
            }
        }

        /// <summary>
        /// Returns whether p3 is in view of p1, assuming there is already a
        /// triangle stretching from p1 to p2.
        /// </summary>
        private static bool NewPointIsInView(
            MonotonePolygonVertex p1,
            MonotonePolygonVertex p2,
            MonotonePolygonVertex p3)
        {
            Vector2 dir2 = p2.point - p1.point;
            Vector2 dir3 = p3.point - p1.point;
            float angle = dir2.Angle(dir3);

            if (p1.position == MonotoneChain.LeftChain)
            {
                return FloatHelpers.Lte(angle, MathF.PI);
            }
            else
            {
                return FloatHelpers.Gte(angle, MathF.PI);
            }
        }

        /// <summary>
        /// Helper class for MonotoneTriangulate.
        /// </summary>
        private class MonotonePolygonVertex
        {
            public Vector2 point;
            public MonotoneChain position;

            public MonotonePolygonVertex(Vector2 v, MonotoneChain mc)
            {
                this.point = v;
                this.position = mc;
            }
        }

        /// <summary>
        /// Describes where in a y-monotone polygon a particular vertex is.
        /// </summary>
        private enum MonotoneChain
        {
            Top,
            Bottom,
            LeftChain,
            RightChain
        }

        #endregion

        #region Clipping

        /// <summary>
        /// Find all intersections of this polygon with an infinite line. The infinite
        /// line is given as a line segment. A list of intersections from the first point
        /// in the line segment is returned.
        /// </summary>
        /// <param name="lineSegment">Line segment representing infinite line.</param>
        /// <param name="otherPolygon">Other polygon.
        /// Used for populating IntersectionData with indices from both polygons.</param>
        /// <param name="otherEdgeIndex">Index of polygon in which lineSegment appears.
        /// Used for populating IntersectionData with indices from both polygons.</param>
        private List<IntersectionData> GetIntersectionDatasForLine(
            LineSegment lineSegment, Polygon otherPolygon = null, int otherEdgeIndex = -1)
        {
            List<IntersectionData> intersectionDatas = new List<IntersectionData>();

            for (int i = 0; i < this.vertices.Count(); i++)
            {
                int nextVertexIndex = (i + 1) % this.vertices.Count();
                LineSegment polygonEdge = new LineSegment(this.vertices[i], this.vertices[nextVertexIndex]);
                (float, float)? distances = lineSegment.GetLineIntersectionDistances(polygonEdge);

                if (!distances.HasValue)
                {
                    if (lineSegment.Colinear(polygonEdge))
                    {
                        // If lines are parallel and colinear, still register an intersection
                        if (polygonEdge.IntersectsPoint(lineSegment.p1))
                        {
                            // If lineSegment.p1 is on polygonEdge, use lineSegment.p1
                            float dist = Vector2.ColinearLengths(
                                polygonEdge.p1, lineSegment.p1, polygonEdge.p2 - polygonEdge.p1);
                            distances = (0, dist);
                        }
                        else
                        {
                            // Otherwise, use polygonEdge.p1
                            float dist = Vector2.ColinearLengths(
                                lineSegment.p1, polygonEdge.p1, lineSegment.p2 - lineSegment.p1);
                            distances = (dist, 0);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                (float lineSegmentDist, float edgeDist) = distances.Value;

                // Only include intersections with polygon edge.
                if (FloatHelpers.Lt(edgeDist, 0) || FloatHelpers.Gte(edgeDist, 1))
                {
                    continue;
                }

                intersectionDatas.Add(new IntersectionData(
                    polygon1: otherPolygon,
                    poly1EdgeIndex: otherEdgeIndex,
                    poly1Dist: lineSegmentDist,
                    polygon2: this,
                    poly2EdgeIndex: i,
                    poly2Dist: edgeDist
                ));
            }

            return intersectionDatas;
        }

        /// <summary>
        /// Whether a point was inside this polygon.
        /// </summary>
        public ContainmentType ContainsPoint(Vector2 point)
        {
            List<IntersectionData> datas = this.GetIntersectionDatasForLine(
                new LineSegment(point, new Vector2(point.x + 1, point.y)));
            
            return this.ContainsPoint(point, datas);
        }

        /// <summary>
        /// Based on the intersectionDatas from a certain point, compute whether
        /// the point was inside this polygon.
        /// </summary>
        private ContainmentType ContainsPoint(Vector2 point, List<IntersectionData> intersectionDatas)
        {
            if (intersectionDatas.Any(data => FloatHelpers.Eq(data.poly1.distanceAlongEdge, 0)))
            {
                return ContainmentType.BOUNDARY;
            }

            IEnumerable<IntersectionData> relevantDatas = intersectionDatas
                .Where(data => FloatHelpers.Gte(data.poly1.distanceAlongEdge, 0))
                .Where(data => data.GetIntersectionType(point) == IntersectionType.OVERLAPPING);
            
            return relevantDatas.Count() % 2 == 0 ? ContainmentType.OUTSIDE : ContainmentType.INSIDE;
        }

        #endregion
    }
}