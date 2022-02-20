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

            // Traverse graph to create polygons
            List<Polygon> allPolygons = GetPolygonsFromConnections(startVertices);

            // TODO: separate out into polygons and holes

            return new PolygonArrangement(allPolygons);
        }

        private static List<Polygon> GetPolygonsFromConnections(List<ConnectionVertex> startVertices)
        {
            List<Polygon> allPolygons = new List<Polygon>();
            HashSet<ConnectionVertex> visited = new HashSet<ConnectionVertex>();
            foreach (ConnectionVertex startVertex in startVertices)
            {
                if (visited.Contains(startVertex))
                {
                    continue;
                }

                List<Vector2> vertices = new List<Vector2>() { startVertex.point };
                ConnectionVertex currentVertex = startVertex.otherEndOfLineSegment;
                while (currentVertex != startVertex)
                {
                    ConnectionVertex nextVertex;
                    if (currentVertex.isFirstVertex)
                    {
                        vertices.Add(currentVertex.point);
                        visited.Add(currentVertex);
                        nextVertex = currentVertex.otherEndOfLineSegment;
                    }
                    else
                    {
                        nextVertex = currentVertex.vertexOnOtherLineSegment;
                    }

                    if (nextVertex == null)
                    {
                        throw new InvalidOperationException(
                            $"Vertex {currentVertex} wasn't connected to another vertex.");
                    }

                    if (visited.Contains(nextVertex))
                    {
                        throw new InvalidOperationException(
                            $"More than one incoming edge found for {nextVertex}.");
                    }

                    currentVertex = nextVertex;
                }

                allPolygons.Add(new Polygon(vertices));
            }

            return allPolygons;
        }

        /// <summary>
        /// Find the closest vertex from a list of buckets.
        /// </summary>
        /// <param name="bucketDivisions">Pre-computed float value for each bucket indicating
        /// what float value should be used to compare the search value with.</param>
        /// <param name="buckets">List of buckets.</param>
        /// <param name="vertex">Vertex we are searching for.</param>
        private static ConnectionVertex FindClosestVertex(
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
        private static int FindBucket(
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