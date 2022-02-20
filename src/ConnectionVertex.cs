namespace PolygonDraw
{
    /// <summary>
    /// Represents a vertex on a line segment where a connection to another
    /// line segment is being made.
    /// </summary>
    public class ConnectionVertex
    {
        public readonly Vector2 point;

        public readonly bool isFirstVertex;

        public ConnectionVertex otherEndOfLineSegment {
            get;
            private set;
        }

        public ConnectionVertex vertexOnOtherLineSegment;

        private ConnectionVertex(Vector2 point, bool isFirstVertex)
        {
            this.point = point;
            this.isFirstVertex = isFirstVertex;
        }

        public static (ConnectionVertex, ConnectionVertex) FromLineSegment(LineSegment ls)
        {
            ConnectionVertex startCv = new ConnectionVertex(ls.p1, true);
            ConnectionVertex endCv = new ConnectionVertex(ls.p2, false);
            startCv.otherEndOfLineSegment = endCv;
            endCv.otherEndOfLineSegment = startCv;
            return (startCv, endCv);
        }

        public void ConnectToVertexOnOtherLineSegment(ConnectionVertex other)
        {
            this.vertexOnOtherLineSegment = other;
            other.vertexOnOtherLineSegment = this;
        }
    }
}