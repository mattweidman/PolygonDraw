using System;
using System.Collections.Generic;
using System.Linq;

namespace PolygonDraw
{
    public class Triangle : Polygon
    {
        public Triangle(Vector2 p1, Vector2 p2, Vector2 p3) : base (new List<Vector2>() { p1, p2, p3 })
        {
        }

        /// <summary>
        /// Returns whether a point is contained inside the triangle.
        /// </summary>
        /// <param name="point">Point in question.</param>
        /// <param name="includeEdges">If true, return true if point is on an edge 
        /// of this triangle. Else, return false in that case.</param>
        public bool ContainsPoint(Vector2 point, bool includeEdges = false)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 t1 = this.vertices[i];
                Vector2 t2 = this.vertices[(i + 1) % 3];
                float angle = (t2 - t1).Angle(point - t1);

                if (includeEdges)
                {
                    if (FloatHelpers.Gt(angle, MathF.PI) || FloatHelpers.Lt(angle, 0))
                    {
                        return false;
                    }
                }
                else
                {
                    if (FloatHelpers.Gte(angle, MathF.PI) || FloatHelpers.Lte(angle, 0))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Finds intersection points of the sides of two triangles. Returns 2D array mat
        /// where mat[i][j], if non-null, is an intersection between the side (i -> i+1) 
        /// of this triangle and the side (j -> j+1) of the other triangle.
        /// </summary>
        public Vector2[][] GetIntersections(Triangle other)
        {
            Vector2[][] intersectionPoints = new Vector2[3][];

            for (int i = 0; i < 3; i ++)
            {
                intersectionPoints[i] = new Vector2[3];

                LineSegment thisLineSegment = new LineSegment(
                    this.vertices[i],
                    this.vertices[(i + 1) % 3]);

                for (int j = 0; j < 3; j++)
                {
                    LineSegment otherLineSegment = new LineSegment(
                        other.vertices[j],
                        other.vertices[(j + 1) % 3]);
                    
                    Vector2 intersection = thisLineSegment.GetIntersection(otherLineSegment);
                    if (intersection != null)
                    {
                        intersectionPoints[i][j] = intersection;
                    }
                }
            }

            return intersectionPoints;
        }

        /// <summary>
        /// Construct a graph describing for each point of interest (vertices and intersection
        /// points) what point would come next clockwise if part of a polygon. Used for
        /// MaskToPolygon().
        /// </summary>
        public TriangleIntersectionGraph MaskToIntersectionGraph(Triangle mask)
        {
            // Construct a mapping of int -> point so that making an adjacency list is easier
            List<Vector2> pointsOfInterest = new List<Vector2>();
            pointsOfInterest.AddRange(this.vertices);
            pointsOfInterest.AddRange(mask.vertices);

            // Find intersection points between the triangles.
            Vector2[][] allIntersectionPoints = this.GetIntersections(mask);

            // Put all intersections into the points of interest list and record their indices.
            int?[][] intersectionIndices = new int?[allIntersectionPoints.Length][];
            for (int i = 0; i < allIntersectionPoints.Length; i++)
            {
                intersectionIndices[i] = new int?[allIntersectionPoints[i].Length];
                for (int j = 0; j < allIntersectionPoints[i].Length; j++)
                {
                    if (allIntersectionPoints[i][j] != null)
                    {
                        intersectionIndices[i][j] = pointsOfInterest.Count;
                        pointsOfInterest.Add(allIntersectionPoints[i][j]);
                    }
                }
            }

            // These two arrays make up the adjacency list describing the graph.
            // baseEdges[i] = j or maskEdges[i] = j indicate an edge from i to j.
            // There are two arrays because intersection points have two outgoing edges
            // and because we want to switch between the two types of edges.
            int?[] baseEdges = new int?[pointsOfInterest.Count];
            int?[] maskEdges = new int?[pointsOfInterest.Count];

            // Create base edges
            for (int i = 0; i < 3; i ++)
            {
                IEnumerable<int> sideIntersections = allIntersectionPoints[i]
                    // Pair with indices in pointsOfInterest
                    .Select((ip, j) => (ip, intersectionIndices[i][j]))
                    // Remove null elements
                    .Where(pair => pair.Item1 != null)
                    // Sort by proximity to first vertex
                    .OrderBy(pair => (pair.Item1 - this.vertices[i]).Magnitude())
                    // Extract just the indices
                    .Select(pair => pair.Item2.Value);

                // Created ordered list of vertices on this side
                List<int> sidePoints = new List<int>() { i };
                sidePoints.AddRange(sideIntersections);
                sidePoints.Add((i + 1) % 3);

                // Add vertices to adjacency list
                for (int sideInd = 0; sideInd < sidePoints.Count - 1; sideInd++)
                {
                    int start = sidePoints[sideInd];
                    int next = sidePoints[sideInd + 1];
                    baseEdges[start] = next;
                }
            }

            // Create mask edges
            for (int j = 0; j < 3; j++)
            {
                IEnumerable<int> sideIntersections = allIntersectionPoints
                    // Extract column j
                    .Select(row => row[j])
                    // Pair with indices in pointsOfInterest
                    .Select((ip, i) => (ip, intersectionIndices[i][j]))
                    // Remove null elements
                    .Where(pair => pair.Item1 != null)
                    // Sort by proximity to first vertex
                    .OrderByDescending(pair => (pair.Item1 - mask.vertices[j]).Magnitude())
                    // Extract just the indices
                    .Select(pair => pair.Item2.Value);

                // Create ordered list of vertices on this side
                List<int> sidePoints = new List<int>() { (j + 1) % 3 + 3 };
                sidePoints.AddRange(sideIntersections);
                sidePoints.Add(j + 3);

                // Add vertices to adjacency list
                for (int sideInd = 0; sideInd < sidePoints.Count - 1; sideInd++)
                {
                    int start = sidePoints[sideInd];
                    int next = sidePoints[sideInd + 1];
                    maskEdges[start] = next;
                }
            }

            return new TriangleIntersectionGraph(pointsOfInterest.ToArray(), baseEdges, maskEdges);
        }

        /// <summary>
        /// Cover this triangle by a mask triangle. Return polygons that cover the
        /// area of this triangle that are not covered by the mask.
        /// </summary>
        public List<Polygon> MaskToPolygons(Triangle mask)
        {
            TriangleIntersectionGraph graph = this.MaskToIntersectionGraph(mask);

            List<Polygon> polygons = new List<Polygon>();

            bool[] baseVerticesVisited = new bool[3];

            for (int i = 0; i < 3; i++)
            {
                // Skip if covered by mask
                if (mask.ContainsPoint(this.vertices[i]))
                {
                    continue;
                }

                // Skip if already visited this point
                if (baseVerticesVisited[i])
                {
                    continue;
                }

                List<Vector2> vertices = new List<Vector2>() { graph.allPoints[i] };
                
                // DFS to create polygon
                int vIndex = graph.baseEdges[i].Value;
                bool onBaseTriangle = true;
                while (vIndex != i)
                {
                    vertices.Add(graph.allPoints[vIndex]);

                    // Keep track of base vertices we have visited
                    if (vIndex < 3)
                    {
                        baseVerticesVisited[vIndex] = true;
                    }

                    // If we hit an intersection point, swap between base/mask
                    if (vIndex >= 6)
                    {
                        onBaseTriangle = !onBaseTriangle;
                    }

                    // Jump to next vertex in graph
                    vIndex = onBaseTriangle
                        ? graph.baseEdges[vIndex].Value
                        : graph.maskEdges[vIndex].Value;
                }

                polygons.Add(new Polygon(vertices));
            }

            return polygons;
        }
    }
}