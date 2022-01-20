using System.Collections.Generic;

namespace PolygonDraw
{
    public class Triangle : Polygon
    {
        public Triangle(Vector2 p1, Vector2 p2, Vector2 p3) : base (new List<Vector2>() { p1, p2, p3 })
        {
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

        // /// <summary>
        // /// Cover this triangle by a mask triangle. Return polygons that cover the
        // /// area of this triangle that are not covered by the mask.
        // /// </summary>
        // public List<Polygon> MaskToPolygons(Triangle mask)
        // {
        //     // Construct a mapping of int -> point so that making an adjacency list is easier
        //     List<Vector2> pointsOfInterest = new List<Vector2>();
        //     pointsOfInterest.AddRange(this.vertices);
        //     pointsOfInterest.AddRange(mask.vertices);

        //     // These two arrays make up the adjacency list describing the graph.
        //     // baseEdges[i] = j, or maskEdges[i] = j, indicates an edge from i to j.
        //     // There are two arrays because intersection points have two outgoing edges
        //     // and because we may want to switch between the two types of edges.
        //     List<int?> baseEdges = new List<int?>() { null, null, null };
        //     List<int?> maskEdges = new List<int?>() { null, null, null };

        //     // Find intersection points and construct edges in the same loops.

        //     // Create base edges
        //     for (int baseI = 0; baseI < 3; baseI++)
        //     {
        //         // The list of vertices on this side of the base triangle.
        //         List<int> verticesOnSide = new List<int>() { baseI };

        //         // Find intersection points.
        //         for (int maskI = 0; maskI < 3; maskI++)
        //         {

        //         }
        //     }
        // }
    }
}