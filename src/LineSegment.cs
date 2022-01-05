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
            if (FloatHelpers.FloatEquals(determinant, 0))
            {
                return null;
            }

            Vector2 pDiff = otherLine.p1 - this.p1;
            Vector2 tParam = dMat.Inverse().Dot(pDiff);
            float t1 = tParam.x;
            float t2 = tParam.y;

            bool anEndpointOverlaps = FloatHelpers.FloatEquals(t1, 0) || FloatHelpers.FloatEquals(t1, 1) 
                || FloatHelpers.FloatEquals(t2, 0) || FloatHelpers.FloatEquals(t2, 1);
            
            if ((!anEndpointOverlaps && t1 > 0 && t1 < 1 && t2 > 0 && t2 < 1) ||
                (includeEndpoints && anEndpointOverlaps))
            {
                return (1 - t1) * this.p1 + t1 * this.p2;
            }

            return null;
        }
    }
}