using System;

namespace PolygonDraw
{
    public class Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 p1, Vector2 p2)
        {
            return new Vector2(p1.x + p2.x, p1.y + p2.y);
        }

        public static Vector2 operator -(Vector2 p1, Vector2 p2)
        {
            return new Vector2(p1.x - p2.x, p1.y - p2.y);
        }

        public static Vector2 operator *(Vector2 p, float f)
        {
            return new Vector2(p.x * f, p.y * f);
        }

        public static Vector2 operator /(Vector2 p, float f)
        {
            return new Vector2(p.x / f, p.y / f);
        }

        public static Vector2 operator *(float f, Vector2 p)
        {
            return new Vector2(p.x * f, p.y * f);
        }

        public static Vector2 operator /(float f, Vector2 p)
        {
            return new Vector2(p.x / f, p.y / f);
        }

        public float Magnitude()
        {
            return MathF.Sqrt(this.x * this.x + this.y * this.y);
        }

        public override bool Equals(object otherObj)
        {
            if (!(otherObj is Vector2 other))
            {
                return false;
            }

            return FloatHelpers.Eq(this.x, other.x) && FloatHelpers.Eq(this.y, other.y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(this.x, this.y).GetHashCode();
        }

        public override string ToString()
        {
            return $"<{this.x}, {this.y}>";
        }

        /// <summary>
        /// Let d1, d2, and this be direction vectors. Returns whether this
        /// vector is between the angle formed from d1 to d2 in the clockwise
        /// direction.
        /// </summary>
        public bool IsBetween(Vector2 d1, Vector2 d2)
        {
            float thisAngle = d1.Angle(this);
            return FloatHelpers.Gt(d1.Angle(d2), thisAngle) && FloatHelpers.Gt(thisAngle, 0);
        }

        /// <summary>
        /// Returns the clockwise angle from this vector to another vector.
        /// </summary>
        public float Angle(Vector2 other)
        {
            float dot = this.x * other.x + this.y * other.y;
            float det = this.y * other.x - this.x * other.y;
            float angle = MathF.Atan2(det, dot);

            if (angle < 0)
            {
                angle += MathF.PI * 2;
            }

            return angle;
        }

        public float Dot(Vector2 other)
        {
            return this.x * other.x + this.y * other.y;
        }

        /// <summary>
        /// Whether all points are on the same line.
        /// </summary>
        public static bool Colinear(params Vector2[] points)
        {
            Vector2 lineDir = (points[1] - points[0]);
            Vector2 unitDir = lineDir / lineDir.Magnitude();

            for (int i = 2; i < points.Length; i++)
            {
                float projectionLength = (points[i] - points[0]).Dot(unitDir);
                Vector2 projectionPoint = points[0] + projectionLength * unitDir;
                float perpendicularDist = (points[i] - projectionPoint).Magnitude();

                if (!FloatHelpers.Eq(perpendicularDist, 0))
                {
                    return false;
                }
            }

            return true;
        }
    }
}