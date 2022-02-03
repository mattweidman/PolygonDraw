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

        public override bool Equals(object obj)
        {
            if (!(obj is PolygonEdge other))
            {
                return false;
            }

            return this.vertex1.Equals(other.vertex1) && this.vertex2.Equals(other.vertex2);
        }

        public override int GetHashCode()
        {
            return (this.vertex1, this.vertex2).GetHashCode();
        }

        public override string ToString()
        {
            return $"Edge({vertex1}, {vertex2})";
        }
    }
}