namespace PolygonDraw
{
    public class LineSegment
    {
        public Vector2 p1;
        public Vector2 p2;

        public LineSegment(Vector2 p1, Vector2 p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        /// <summary>
        /// Find the intersection of two line segments. If the line segments do
        /// not overlap, return null.  If includeEndpoints is true, will return an 
        /// intersection if it is at an endpoint.
        /// </summary>
        public Vector2 GetIntersection(LineSegment otherLine, bool includeEndpoints = false)
        {
            Vector2 d1 = this.p2 - this.p1;
            Vector2 d2 = otherLine.p2 - otherLine.p1;
            Matrix2 dMat = new Matrix2(d1.x, -d2.x, d1.y, -d2.y);

            float determinant = dMat.Determinant();
            if (FloatHelpers.Eq(determinant, 0))
            {
                return null;
            }

            Vector2 pDiff = otherLine.p1 - this.p1;
            Vector2 tParam = dMat.Inverse().Dot(pDiff);
            float t1 = tParam.x;
            float t2 = tParam.y;

            bool intersects;
            if (includeEndpoints)
            {
                intersects = FloatHelpers.Gte(t1, 0) && FloatHelpers.Lte(t1, 1)
                    && FloatHelpers.Gte(t2, 0) && FloatHelpers.Lte(t2, 1);
            }
            else
            {
                intersects = FloatHelpers.Gt(t1, 0) && FloatHelpers.Lt(t1, 1)
                    && FloatHelpers.Gt(t2, 0) && FloatHelpers.Lt(t2, 1);
            }

            return intersects ? (1 - t1) * this.p1 + t1 * this.p2 : null;
        }

        public override string ToString()
        {
            return $"{this.p1}-{this.p2}";
        }
    }
}