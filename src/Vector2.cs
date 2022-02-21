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

        public Vector2 ToUnit()
        {
            return this / this.Magnitude();
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
        /// Returns the unit vector that bisects this vector and another one, assuming
        /// other comes after this in the clockwise direction.
        /// </summary>
        public Vector2 Bisect(Vector2 other)
        {
            float angle = this.Angle(other) / 2;

            Matrix2 rotationMatrix = new Matrix2(
                MathF.Cos(angle),
                MathF.Sin(angle),
                -MathF.Sin(angle),
                MathF.Cos(angle));
            
            return rotationMatrix.Dot(this).ToUnit();
        }

        /// <summary>
        /// Whether all points are on the same line.
        /// </summary>
        public static bool Colinear(params Vector2[] points)
        {
            Vector2 lineDir = (points[1] - points[0]);
            Vector2 unitDir = lineDir.ToUnit();

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

        /// <summary>
        /// Assume v and u are colinear with (v+d). Solve for t in v + td = u.
        /// Can return negative if d and (u-v) are opposite directions.
        /// </summary>
        /// <returns>The number of lengths of d it takes to get from v to u.</returns>
        public static float ColinearLengths(Vector2 v, Vector2 u, Vector2 d)
        {
            if (FloatHelpers.Gt(MathF.Abs(d.x), MathF.Abs(d.y)))
            {
                return (u.x - v.x) / d.x;
            }
            else
            {
                return (u.y - v.y) / d.y;
            }
        }
    }
}