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
    }
}