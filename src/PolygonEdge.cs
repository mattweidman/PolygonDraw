namespace PolygonDraw
{
    /// <summary>
    /// Describes an edge between vertices in polygons.
    /// </summary>
    public class PolygonEdge
    {
        public readonly PolygonVertex vertex1, vertex2;

        public PolygonEdge(PolygonVertex vertex1, PolygonVertex vertex2)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
        }
    }
}