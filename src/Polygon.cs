using System;
using System.Collections.Generic;

namespace PolygonDraw
{
    public class Polygon
    {
        public List<Vector2> vertices;

        public Polygon(List<Vector2> vertices)
        {
            this.vertices = vertices;
        }

        /// <summary>
        /// If any edge of this polygon blocks the straight line from 
        /// vertices[vIndex1] to vertices[vIndex2] return false. Else, return true.
        /// </summary>
        public bool InViewOf(int vIndex1, int vIndex2)
        {
            // Not in view if vertices are the same or their segment already forms an edge.
            int absDiff = Math.Abs(vIndex1 - vIndex2);
            if (absDiff <= 1 || absDiff == this.vertices.Count - 1)
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
    }
}