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
        /// Returns whether another triangle is contained inside this triangle.
        /// </summary>
        /// <param name="other">Other triangle.</param>
        /// <param name="includeEdges">If true, return true if triangle's points are
        /// inside or on an edge. If false, return true only if points are inside.</param>
        public bool ContainsTriangle(Triangle other, bool includeEdges = false)
        {
            return other.vertices.All(v => this.ContainsPoint(v, includeEdges));
        }

        /// <summary>
        /// Returns false if there is any vertex with a zero-degree angle.
        /// </summary>
        public bool IsValidTriangle()
        {
            Vector2 dir1 = this.vertices[1] - this.vertices[0];
            Vector2 dir2 = this.vertices[2] - this.vertices[0];
            float angle = dir1.Angle(dir2);

            return !(FloatHelpers.Eq(angle, 0) || FloatHelpers.Eq(angle, MathF.PI));
        }
    }
}