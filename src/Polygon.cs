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
    }
}