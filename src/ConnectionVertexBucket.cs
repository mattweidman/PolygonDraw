using System;
using System.Collections.Generic;
using System.Linq;

namespace PolygonDraw
{
    /// <summary>
    /// Bucket of ConnectionVertex objects.
    /// </summary>
    public class ConnectionVertexBucket
    {
        public float minX {
            get;
            private set;
        }

        public float maxX {
            get;
            private set;
        }

        private float maxSeparation;

        private List<ConnectionVertex> vertices;

        /// <param name="firstVertex">First vertex added to this bucket.</param>
        /// <param name="maxSeparation">Max distance between connected points of different
        /// line segments.</param>
        public ConnectionVertexBucket(ConnectionVertex firstVertex, float maxSeparation)
        {
            this.maxSeparation = maxSeparation;
            this.vertices = new List<ConnectionVertex>() { firstVertex };
            this.minX = firstVertex.point.x - maxSeparation;
            this.maxX = firstVertex.point.x + maxSeparation;
        }

        /// <summary>
        /// Determine if a vertex is within range of this bucket.
        /// </summary>
        /// <param name="vertex">New vertex.</param>
        /// <returns>0 if vertex is within range; -1 if vertex x-coordinate is too low;
        /// 1 if vertex x-coordinate is too high.</returns>
        public int WithinRange(ConnectionVertex vertex)
        {
            // We don't use FloatHelpers here because maxSeparation could potentially
            // be different from FloatHelpers.EPSILON.
            if (vertex.point.x < minX)
            {
                return -1;
            }
            else if (vertex.point.x > maxX)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Add a new vertex to the bucket. We assume proper checking has been done
        /// so that the vertex is within range.
        /// </summary>
        /// <param name="vertex">New vertex.</param>
        public void Add(ConnectionVertex vertex)
        {
            this.vertices.Add(vertex);
            this.minX = MathF.Min(this.minX, vertex.point.x - this.maxSeparation);
            this.maxX = MathF.Max(this.maxX, vertex.point.x + this.maxSeparation);
        }

        /// <summary>
        /// Sort by y-coordinate.
        /// </summary>
        public void Sort()
        {
            this.vertices = this.vertices.OrderBy(v => v.point.y).ToList();
        }
    }
}