using System;
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
            List<ConnectionVertex> startVertices = connectionTuples
                .Select(pair => pair.Item1)
                .OrderBy(cv => cv.point.x)
                .ToList();
            List<ConnectionVertex> endVertices = connectionTuples.Select(pair => pair.Item2).ToList();

            // Place start vertices into buckets.
            List<ConnectionVertexBucket> buckets = new List<ConnectionVertexBucket>();
            foreach (ConnectionVertex startVertex in startVertices)
            {
                if (buckets.Any() && buckets[buckets.Count - 1].WithinRange(startVertex) == 0)
                {
                    buckets[buckets.Count - 1].Add(startVertex);
                }
                else
                {
                    buckets.Add(ConnectionVertexBucket.FromFirstVertex(startVertex, maxSeparation));
                }
            }

            // Sort by y-coordinate within each bucket.
            buckets.ForEach(bucket => bucket.Sort());
            List<float> bucketDivisions = buckets.Select(bucket => bucket.maxX).ToList();

            // Find a match for each end vertex.
            foreach (ConnectionVertex endVertex in endVertices)
            {
                ConnectionVertex startVertex = FindClosestVertex(bucketDivisions, buckets, endVertex);
                startVertex.ConnectToVertexOnOtherLineSegment(endVertex);
            }

            // TODO: Traverse graph to create polygons

            return new PolygonArrangement();
        }

        /// <summary>
        /// Find the closest vertex from a list of buckets.
        /// </summary>
        /// <param name="bucketDivisions">Pre-computed float value for each bucket indicating
        /// what float value should be used to compare the search value with.</param>
        /// <param name="buckets">List of buckets.</param>
        /// <param name="vertex">Vertex we are searching for.</param>
        public static ConnectionVertex FindClosestVertex(
            List<float> bucketDivisions, List<ConnectionVertexBucket> buckets, ConnectionVertex vertex)
        {
            int startIndex = FindBucket(bucketDivisions, buckets, vertex);
            ConnectionVertex closestVertex = null;

            int index = SearchHelpers.FindClosestValidIndex(
                list: buckets,
                startIndex,
                isValid: bucket => {
                    closestVertex = bucket.FindClosest(vertex);
                    return closestVertex != null;
                },
                distance: bucket => {
                    int rangeIndicator = bucket.WithinRange(vertex);
                    if (rangeIndicator == 0)
                    {
                        return 0;
                    }
                    else if (rangeIndicator < 0)
                    {
                        return bucket.minX - vertex.point.x;
                    }
                    else
                    {
                        return vertex.point.x - bucket.maxX;
                    }
                });
            
            return index == -1 ? null : closestVertex;
        }

        /// <summary>
        /// Find the bucket that a vertex belongs in.
        /// </summary>
        /// <param name="bucketDivisions">Pre-computed float value for each bucket indicating
        /// what float value should be used to compare the search value with.</param>
        /// <param name="buckets">List of buckets.</param>
        /// <param name="vertex">Vertex we are searching for.</param>
        public static int FindBucket(
            List<float> bucketDivisions, List<ConnectionVertexBucket> buckets, ConnectionVertex vertex)
        {
            return SearchHelpers.BinarySearchClosest(
                list: bucketDivisions,
                searchVal: vertex.point.x,
                minInRange: i => buckets[i].minX,
                maxInRange: i => buckets[i].maxX);
        }
    }
}