using System.Collections.Generic;
using System.Linq;

namespace PolygonDraw
{
    /// <summary>
    /// Static class for connecting line segments to form polygons.
    /// </summary>
    public static class LineSegmentConnect
    {
        /// <summary>
        /// Connect line segments to create polygons.
        /// </summary>
        /// <param name="lineSegments">List of line segments.</param>
        /// <param name="maxSeparation">Max distance between connected points of different
        /// line segments.</param>
        public static PolygonArrangement ConnectLineSegments(
            List<LineSegment> lineSegments, float maxSeparation)
        {
            // Create ConnectionVertex nodes
            List<(ConnectionVertex, ConnectionVertex)> connectionTuples = lineSegments
                .Select(ls => ConnectionVertex.FromLineSegment(ls))
                .ToList();
            List<ConnectionVertex> startVertices = connectionTuples.Select(cv => cv.Item1).ToList();
            List<ConnectionVertex> endVertices = connectionTuples.Select(cv => cv.Item2).ToList();

            // Place start vertices into buckets.
            startVertices = startVertices.OrderBy(cv => cv.point.x).ToList();
            List<ConnectionVertexBucket> buckets = new List<ConnectionVertexBucket>();
            foreach (ConnectionVertex vertex in startVertices)
            {
                if (buckets.Any() && buckets[buckets.Count - 1].WithinRange(vertex) == 0)
                {
                    buckets[buckets.Count - 1].Add(vertex);
                }
                else
                {
                    buckets.Add(new ConnectionVertexBucket(vertex, maxSeparation));
                }
            }

            // Sort by y-coordinate within each bucket.
            buckets.ForEach(bucket => bucket.Sort());

            // TODO

            return new PolygonArrangement();
        }
    }
}