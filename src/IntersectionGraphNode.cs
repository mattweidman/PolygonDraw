namespace PolygonDraw
{
    /// <summary>
    /// Describes a node in a graph produced by intersecting two polygons.
    /// </summary>
    public class IntersectionGraphNode
    {
        public Vector2 point;

        public bool isStarter = false;

        public IntersectionGraphNode baseNext = null;

        public IntersectionGraphNode maskNext = null;

        public IntersectionGraphNode(Vector2 point, bool isStarter = false)
        {
            this.point = point;
            this.isStarter = isStarter;
        }

        public bool IsSwappable()
        {
            return baseNext != null && maskNext != null;
        }
    }
}