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

            for (int i = 0; i < this.vertices.Count; i++)
            {
                if (!this.vertices[i].Equals(other.vertices[i]))
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

        /// <summary>
        /// Whether the indices of two vertices in this polygon are next to each other.
        /// </summary>
        private bool AreAdjacent(int vIndex1, int vIndex2)
        {
            int absDiff = Math.Abs(vIndex1 - vIndex2);
            return absDiff <= 1 || absDiff == this.vertices.Count - 1;
        }
    }
}