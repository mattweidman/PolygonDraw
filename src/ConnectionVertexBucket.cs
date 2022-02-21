using System;
using System.Collections.Generic;
using System.Linq;

namespace PolygonDraw
{
    /// <summary>
    /// Bucket of ConnectionVertex objects.
    /// </summary>
    class ConnectionVertexBucket
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

        // Precomputed from vertices in Sort()
        private List<float> yCoords;

        /// <param name="maxSeparation">Max distance between connected points of different
        /// line segments.</param>
        /// <param name="minX">Minimum x-coordinate in range.</param>
        /// <param name="maxX">Maximum x-coordinate in range.</param>
        private ConnectionVertexBucket(float maxSeparation, float minX, float maxX)
        {
            this.maxSeparation = maxSeparation;
            this.minX = minX;
            this.maxX = maxX;
            this.vertices = new List<ConnectionVertex>();
        }

        /// <summary>
        /// Initialize a new connection vertex bucket using one vertex.
        /// </summary>
        /// <param name="firstVertex">First vertex added to this bucket.</param>
        /// <param name="maxSeparation">Max distance between connected points of different
        /// line segments.</param>
        public static ConnectionVertexBucket FromFirstVertex(
            ConnectionVertex firstVertex, float maxSeparation)
        {
            ConnectionVertexBucket bucket = new ConnectionVertexBucket(
                maxSeparation,
                firstVertex.point.x - maxSeparation,
                firstVertex.point.x + maxSeparation);
            
            bucket.vertices.Add(firstVertex);

            return bucket;
        }

        /// <summary>
        /// Initialize a new connection vertex bucket. Only expected to be used in testing.
        /// </summary>
        /// <param name="maxSeparation">Max distance between connected points of different
        /// line segments.</param>
        /// <param name="minX">Minimum x-coordinate in range.</param>
        /// <param name="maxX">Maximum x-coordinate in range.</param>
        public static ConnectionVertexBucket FromMinAndMax(float maxSeparation, float minX, float maxX)
        {
            return new ConnectionVertexBucket(maxSeparation, minX, maxX);
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
                return 1;
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
            this.yCoords = this.vertices.Select(v => v.point.y).ToList();
        }

        /// <summary>
        /// Find the closest vertex in this bucket. Return null if no valid vertices left.
        /// </summary>
        public ConnectionVertex FindClosest(ConnectionVertex searchVertex)
        {
            int startIndex = SearchHelpers.BinarySearchClosest(
                list: this.yCoords, searchVal: searchVertex.point.y);
            
            int index = SearchHelpers.FindClosestValidIndex(
                list: this.vertices,
                startIndex,
                isValid: v => v.vertexOnOtherLineSegment == null,
                distance: v => MathF.Abs(v.point.y - searchVertex.point.y));
            
            return index == -1 ? null : this.vertices[index];
        }
    }
}