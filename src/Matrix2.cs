namespace PolygonDraw
{
    public class Matrix2
    {
        public float f1, f2, f3, f4;

        /// <summary>
        /// Matrix looks like this:
        /// [f1 f2]
        /// [f3 f4]
        /// </summary>
        public Matrix2(float f1, float f2, float f3, float f4)
        {
            this.f1 = f1;
            this.f2 = f2;
            this.f3 = f3;
            this.f4 = f4;
        }

        public static Matrix2 operator /(Matrix2 m, float f)
        {
            return new Matrix2(m.f1 / f, m.f2 / f, m.f3 / f, m.f4 / f);
        }

        public float Determinant()
        {
            return f1 * f4 - f2 * f3;
        }

        /// <summary>
        /// Inverse of matrix. Null if inverse cannot be found.
        /// </summary>
        public Matrix2 Inverse()
        {
            float det = this.Determinant();

            if (FloatHelpers.FloatEquals(det, 0))
            {
                return null;
            }

            return new Matrix2(this.f4, -this.f2, -this.f3, f1) / det;
        }

        public Vector2 Dot(Vector2 v)
        {
            return new Vector2(this.f1 * v.x + this.f2 * v.y, this.f3 * v.x + this.f4 * v.y);
        }

        public override bool Equals(object otherObj)
        {
            if (!(otherObj is Matrix2 other))
            {
                return false;
            }

            return FloatHelpers.FloatEquals(this.f1, other.f1) && FloatHelpers.FloatEquals(this.f2, other.f2)
                && FloatHelpers.FloatEquals(this.f3, other.f3) && FloatHelpers.FloatEquals(this.f4, other.f4);
        }

        public override string ToString()
        {
            return $"[{this.f1}, {this.f2}; {this.f3}, {this.f4}]";
        }
    }
}