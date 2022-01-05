namespace PolygonDraw
{
    public class Triangle
    {
        public Vector2 p1;
        public Vector2 p2;
        public Vector2 p3;

        public Triangle(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public Vector2[] GetVertices()
        {
            return new Vector2[] { p1, p2, p3, };
        }

        public LineSegment[] GetEdges()
        {
            return new LineSegment[]
            {
                new LineSegment(p1, p2),
                new LineSegment(p2, p3),
                new LineSegment(p3, p1),
            };
        }
    }
}