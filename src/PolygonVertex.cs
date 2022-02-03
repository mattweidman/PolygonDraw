using System;

namespace PolygonDraw
{
    /// <summary>
    /// Describes a reference to a vertex in a polygon to be used for triangulation. The
    /// raw state of a PolygonVertex only consists of 3 things: a reference to the polygon
    /// the vertex is in, the index of the vertex in the polygon, and whether the polygon
    /// is a hole. PolygonVertex objects are hashed and compared only using the polygon
    /// reference and the vertex index.
    /// </summary>
    public class PolygonVertex
    {
        public readonly Polygon polygon;

        public readonly int vertexIndex;

        public readonly bool isHole;

        public int prevVertexIndex => (this.vertexIndex + this.vertexCount - 1) % this.vertexCount;

        public int nextVertexIndex => (this.vertexIndex + 1) % this.vertexCount;

        public Vector2 vertex => this.polygon.vertices[this.vertexIndex];

        public Vector2 prevVertex => this.polygon.vertices[prevVertexIndex];
        
        public Vector2 nextVertex => this.polygon.vertices[nextVertexIndex];

        public PolygonVertex prevPolygonVertex =>
            new PolygonVertex(this.polygon, this.prevVertexIndex, this.isHole);

        public PolygonVertex nextPolygonVertex =>
            new PolygonVertex(this.polygon, this.nextVertexIndex, this.isHole);

        public float x => this.vertex.x;

        public float y => this.vertex.y;

        private int vertexCount => this.polygon.vertices.Count;

        public PolygonVertex(Polygon polygon, int vertexIndex, bool isHole)
        {
            this.polygon = polygon;
            this.vertexIndex = vertexIndex;
            this.isHole = isHole;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PolygonVertex other))
            {
                return false;
            }

            // Compare polygons by reference
            return this.polygon == other.polygon && this.vertexIndex == other.vertexIndex;
        }

        public override string ToString()
        {
            return this.isHole ? $"hole({this.vertex})" : $"polygon({this.vertex})";
        }

        public override int GetHashCode()
        {
            return (this.polygon.GetHashCode(), vertexIndex).GetHashCode();
        }

        public VertexType GetVertexType()
        {
            float thisY = this.vertex.y;
            float prevY = this.prevVertex.y;
            float nextY = this.nextVertex.y;

            if (FloatHelpers.Lt(prevY, thisY) && FloatHelpers.Lt(thisY, nextY))
            {
                return VertexType.EXTERIOR_LEFT;
            }

            if (FloatHelpers.Lt(nextY, thisY) && FloatHelpers.Lt(thisY, prevY))
            {
                return VertexType.EXTERIOR_RIGHT;
            }

            float interiorAngle = (this.nextVertex - this.vertex).Angle(this.prevVertex - this.vertex);

            if (FloatHelpers.Lte(prevY, thisY) && FloatHelpers.Lte(nextY, thisY))
            {
                return FloatHelpers.Lt(interiorAngle, MathF.PI) ? VertexType.START : VertexType.SPLIT;
            }
            else
            {
                return FloatHelpers.Lt(interiorAngle, MathF.PI) ? VertexType.END : VertexType.MERGE;
            }
        }

        public enum VertexType
        {
            START,
            END,
            SPLIT,
            MERGE,
            EXTERIOR_LEFT,
            EXTERIOR_RIGHT
        }
    }
}