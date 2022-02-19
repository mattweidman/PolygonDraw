using System;
using System.Collections.Generic;
using System.Linq;

namespace PolygonDraw
{
    public class Polygon
    {
        public List<Vector2> vertices;

        public Polygon(List<Vector2> vertices)
        {
            if (vertices.Count < 3)
            {
                throw new ArgumentException(
                    $"Cannot create a polygon [{string.Join(",", vertices)}] "
                    + "with less than 3 vertices.");
            }

            this.vertices = vertices;
        }

        public override bool Equals(object otherObj)
        {
            if (!(otherObj is Polygon other))
            {
                return false;
            }

            if (this.vertices.Count != other.vertices.Count)
            {
                return false;
            }

            // Find index of vertex in other that equals the first vertex in this
            int offset = 0;
            bool foundMatch = false;
            for (int i = 0; i < other.vertices.Count; i++)
            {
                if (other.vertices[i].Equals(this.vertices[0]))
                {
                    foundMatch = true;
                    offset = i;
                    break;
                }
            }
            if (!foundMatch)
            {
                return false;
            }

            for (int i = 1; i < this.vertices.Count; i++)
            {
                if (!this.vertices[i].Equals(other.vertices[(i + offset) % other.vertices.Count]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Polygon[{string.Join(",", this.vertices)}]";
        }

        #region Sweep-Line Triangulation

        /// <summary>
        /// Assuming this polygon is y-monotone, divide it into triangles that cover 
        /// the same area.
        /// </summary>
        public List<Triangle> MonotoneTriangulate()
        {
            int vertexMaxIndex = this.vertices.ArgMax(v => v.y);
            int vertexMinIndex = this.vertices.ArgMin(v => v.y);

            // Sort points by y-coordinate
            List<MonotonePolygonVertex> sortedPoints = new List<MonotonePolygonVertex>()
            {
                new MonotonePolygonVertex(this.vertices[vertexMaxIndex], MonotoneChain.Top)
            };
            int leftIndex = (vertexMaxIndex + this.vertices.Count - 1) % this.vertices.Count;
            int rightIndex = (vertexMaxIndex + 1) % this.vertices.Count;
            while (leftIndex != vertexMinIndex || rightIndex != vertexMinIndex)
            {
                // Add the higher one to the list each time
                if (FloatHelpers.Gt(this.vertices[leftIndex].y, this.vertices[rightIndex].y) ||
                    (FloatHelpers.Eq(this.vertices[leftIndex].y, this.vertices[rightIndex].y) &&
                    rightIndex == vertexMinIndex))
                {
                    sortedPoints.Add(new MonotonePolygonVertex(
                        this.vertices[leftIndex],
                        MonotoneChain.LeftChain));
                    leftIndex = (leftIndex + this.vertices.Count - 1) % this.vertices.Count;
                }
                else
                {
                    sortedPoints.Add(new MonotonePolygonVertex(
                        this.vertices[rightIndex],
                        MonotoneChain.RightChain));
                    rightIndex = (rightIndex + 1) % this.vertices.Count;
                }
            }
            sortedPoints.Add(new MonotonePolygonVertex(
                this.vertices[vertexMinIndex],
                MonotoneChain.Bottom));

            // Move from top to bottom, keeping a stack of vertices that still need triangles.
            List<Triangle> triangles = new List<Triangle>();
            Stack<MonotonePolygonVertex> stack = new Stack<MonotonePolygonVertex>();
            MonotoneChain lastPosition = MonotoneChain.Top;
            for (int i = 0; i < sortedPoints.Count; i++)
            {
                MonotonePolygonVertex currentVertex = sortedPoints[i];
                if (i < 2)
                {
                    // First two points can't be matched with anything.
                    stack.Push(currentVertex);
                }
                else if (currentVertex.position != lastPosition)
                {
                    // If we jumped across the polygon, connect vertex to everything in stack.
                    MonotonePolygonVertex previousVertex = stack.Pop();
                    MonotonePolygonVertex lastVertex = previousVertex;

                    while (stack.Count > 0)
                    {
                        MonotonePolygonVertex higherVertex = stack.Pop();
                        triangles.Add(GenerateTriangle(currentVertex, previousVertex, higherVertex));
                        previousVertex = higherVertex;
                    }

                    stack.Push(lastVertex);
                    stack.Push(currentVertex);
                }
                else
                {
                    // If we are on the same chain, connect to everything until we
                    // reach a vertex that isn't reachable.
                    MonotonePolygonVertex previousVertex = stack.Pop();

                    while (stack.Count > 0)
                    {
                        MonotonePolygonVertex higherVertex = stack.Peek();

                        // If higherVertex isn't in view, break.
                        if (!NewPointIsInView(currentVertex, previousVertex, higherVertex))
                        {
                            break;
                        }

                        // Don't add a triangle if its vertices are co-linear.
                        if (!Vector2.Colinear(
                            currentVertex.point, previousVertex.point, higherVertex.point))
                        {
                            triangles.Add(GenerateTriangle(currentVertex, previousVertex, higherVertex));
                        }

                        stack.Pop();
                        previousVertex = higherVertex;
                    }

                    stack.Push(previousVertex);
                    stack.Push(currentVertex);
                }

                lastPosition = currentVertex.position;
            }

            return triangles;
        }

        /// <summary>
        /// Takes 3 vertices and combines them into a triangle.
        /// </summary>
        private static Triangle GenerateTriangle(
            MonotonePolygonVertex p1,
            MonotonePolygonVertex p2,
            MonotonePolygonVertex p3)
        {
            Vector2 dir2 = p2.point - p1.point;
            Vector2 dir3 = p3.point - p1.point;
            float angle = dir2.Angle(dir3);

            if (FloatHelpers.Lt(angle, MathF.PI))
            {
                return new Triangle(p1.point, p2.point, p3.point);
            }
            else
            {
                return new Triangle(p1.point, p3.point, p2.point);
            }
        }

        /// <summary>
        /// Returns whether p3 is in view of p1, assuming there is already a
        /// triangle stretching from p1 to p2.
        /// </summary>
        private static bool NewPointIsInView(
            MonotonePolygonVertex p1,
            MonotonePolygonVertex p2,
            MonotonePolygonVertex p3)
        {
            Vector2 dir2 = p2.point - p1.point;
            Vector2 dir3 = p3.point - p1.point;
            float angle = dir2.Angle(dir3);

            if (p1.position == MonotoneChain.LeftChain)
            {
                return FloatHelpers.Lte(angle, MathF.PI);
            }
            else
            {
                return FloatHelpers.Gte(angle, MathF.PI);
            }
        }

        /// <summary>
        /// Helper class for MonotoneTriangulate.
        /// </summary>
        private class MonotonePolygonVertex
        {
            public Vector2 point;
            public MonotoneChain position;

            public MonotonePolygonVertex(Vector2 v, MonotoneChain mc)
            {
                this.point = v;
                this.position = mc;
            }
        }

        /// <summary>
        /// Describes where in a y-monotone polygon a particular vertex is.
        /// </summary>
        private enum MonotoneChain
        {
            Top,
            Bottom,
            LeftChain,
            RightChain
        }

        #endregion

        #region Clipping

        /// <summary>
        /// Cover this polygon by a clip polygon. Return polygons that cover the
        /// area of this polygon that are not covered by the clip.
        /// </summary>
        public PolygonArrangement ClipToPolygons(Polygon clip)
        {
            List<IntersectionOrVertexNode> starterNodes = this.ClipToIntersectionGraph(
                clip, out PolygonOverlapType overlapType);

            // Return early if no intersections
            if (overlapType == PolygonOverlapType.NO_OVERLAP)
            {
                return new PolygonArrangement(new List<Polygon>() {this});
            }
            else if (overlapType == PolygonOverlapType.SUBJECT_CONTAINS_HOLE)
            {
                return new PolygonArrangement(new List<Polygon>() {this}, new List<Polygon> {clip});
            }
            else if (overlapType == PolygonOverlapType.HOLE_CONTAINS_SUBJECT)
            {
                return new PolygonArrangement();
            }

            List<Polygon> polygons = new List<Polygon>();

            int maxIterations = (clip.vertices.Count + this.vertices.Count) * 2;
            int iterations = 0;

            foreach (IntersectionOrVertexNode starterNode in starterNodes)
            {
                // Skip if already visited this point
                if (!starterNode.CanStart())
                {
                    continue;
                }

                // DFS to create polygon
                List<Vector2> vertices = new List<Vector2>();

                IntersectionOrVertexNode currentNode = starterNode.NextNode();
                IntersectionOrVertexNode prevNode = starterNode;
                while (currentNode.VisitableFrom(prevNode))
                {
                    vertices.Add(currentNode.point);

                    currentNode.Visit(prevNode);

                    IntersectionOrVertexNode temp = currentNode;
                    currentNode = currentNode.NextNode(prevNode);
                    prevNode = temp;

                    iterations++;
                    if (iterations > maxIterations)
                    {
                        throw new InvalidOperationException("Iterated in ClipToPolygons too many times.");
                    }
                }

                polygons.Add(new Polygon(vertices));
            }

            return new PolygonArrangement(polygons);
        }

        /// <summary>
        /// Construct a graph describing for each point of interest (vertices and intersection
        /// points) what point would come next clockwise if part of a polygon. Used for
        /// ClipToPolygons().
        /// </summary>
        /// <param name="clip">Clipping polygon.</param>
        /// <param name="overlapType">Output of the type of overlap that was found.</param>
        private List<IntersectionOrVertexNode> ClipToIntersectionGraph(
            Polygon clip, out PolygonOverlapType overlapType)
        {
            List<IntersectionOrVertexNode> vertexNodes = new List<IntersectionOrVertexNode>();
            List<IntersectionOrVertexNode> intersectNodes = new List<IntersectionOrVertexNode>();

            for (int i = 0; i < this.vertices.Count; i++)
            {
                int j = (i + 1) % this.vertices.Count;
                LineSegment lineSegment = new LineSegment(this.vertices[i], this.vertices[j]);

                List<IntersectionData> intersectionDatas =
                    clip.GetIntersectionDatasForLine(lineSegment, this, i);
                
                // Add vertex if it isn't an intersection point (avoiding duplicates)
                if (intersectionDatas.All(data => !FloatHelpers.Eq(data.poly1.distanceAlongEdge, 0)))
                {
                    ContainmentType containmentType = this.ContainsPoint(
                        this.vertices[i],
                        intersectionDatas);
                    IntersectionOrVertexNode vertexNode = new IntersectionOrVertexNode(
                        new PolygonVertex(this, i),
                        containmentType != ContainmentType.OUTSIDE);
                    vertexNodes.Add(vertexNode);
                }
                
                IEnumerable<IntersectionOrVertexNode> newIntersectNodes = intersectionDatas
                    .Where(data => FloatHelpers.Lt(data.poly1.distanceAlongEdge, 1) &&
                        FloatHelpers.Gte(data.poly1.distanceAlongEdge, 0) &&
                        !data.IsRemovable())
                    .Select(data => new IntersectionOrVertexNode(data));

                // Add intersection points along edge
                intersectNodes.AddRange(newIntersectNodes);
            }

            // Choose overlap type based on intersections
            if (intersectNodes.Count > 0)
            {
                overlapType = PolygonOverlapType.INTERSECTING;
            }
            else if (vertexNodes[0].isHidden)
            {
                overlapType = PolygonOverlapType.HOLE_CONTAINS_SUBJECT;
                return new List<IntersectionOrVertexNode>();
            }
            else if (this.ContainsPoint(clip.vertices[0]) == ContainmentType.INSIDE)
            {
                overlapType = PolygonOverlapType.SUBJECT_CONTAINS_HOLE;
                return vertexNodes;
            }
            else
            {
                overlapType = PolygonOverlapType.NO_OVERLAP;
                return vertexNodes;
            }

            List<IntersectionOrVertexNode> subjectNodes = intersectNodes
                .Concat(vertexNodes)
                .OrderBy(data => data.subjectPriority)
                .ToList();
            
            // Create edges around subject polygon
            for (int i = 0; i < subjectNodes.Count; i++)
            {
                subjectNodes[i].AddSubjectEdge(subjectNodes[(i + 1) % subjectNodes.Count]);
            }

            List<IntersectionOrVertexNode> clipNodesPreliminary = intersectNodes
                .Concat(clip.vertices.Select((v, i) =>
                    new IntersectionOrVertexNode(new PolygonVertex(clip, i, true), false)))
                .OrderBy(data => data.clipPriority)
                .ToList();
            
            // Remove duplicate vertices
            List<IntersectionOrVertexNode> clipNodes = new List<IntersectionOrVertexNode>();
            for (int i = 0; i < clipNodesPreliminary.Count; i++)
            {
                IntersectionOrVertexNode node = clipNodesPreliminary[i];
                if (clipNodesPreliminary[i].isIntersection)
                {
                    clipNodes.Add(node);
                }
                else
                {
                    // Only include vertex nodes if they don't match the next or previous node.
                    int prevI = (i + clipNodesPreliminary.Count - 1) % clipNodesPreliminary.Count;
                    int nextI = (i + 1) % clipNodesPreliminary.Count;
                    if (!(FloatHelpers.Eq(clipNodesPreliminary[prevI].clipPriority, node.clipPriority) ||
                        FloatHelpers.Eq(clipNodesPreliminary[nextI].clipPriority, node.clipPriority)))
                    {
                        clipNodes.Add(node);
                    }
                }
            }
            
            // Create edges around clip polygon
            for (int i = 0; i < clipNodes.Count; i++)
            {
                clipNodes[i].AddClipEdge(clipNodes[(i + clipNodes.Count - 1) % clipNodes.Count]);
            }
            
            return clipNodes.Concat(vertexNodes).Where(node => node.isStarter).ToList();
        }

        /// <summary>
        /// Find all intersections of this polygon with an infinite line. The infinite
        /// line is given as a line segment. A list of intersections from the first point
        /// in the line segment is returned.
        /// </summary>
        /// <param name="lineSegment">Line segment representing infinite line.</param>
        /// <param name="otherPolygon">Other polygon.
        /// Used for populating IntersectionData with indices from both polygons.</param>
        /// <param name="otherEdgeIndex">Index of polygon in which lineSegment appears.
        /// Used for populating IntersectionData with indices from both polygons.</param>
        private List<IntersectionData> GetIntersectionDatasForLine(
            LineSegment lineSegment, Polygon otherPolygon = null, int otherEdgeIndex = -1)
        {
            List<IntersectionData> intersectionDatas = new List<IntersectionData>();

            for (int i = 0; i < this.vertices.Count(); i++)
            {
                int nextVertexIndex = (i + 1) % this.vertices.Count();
                LineSegment polygonEdge = new LineSegment(this.vertices[i], this.vertices[nextVertexIndex]);
                (float, float)? distances = lineSegment.GetLineIntersectionDistances(polygonEdge);

                if (!distances.HasValue)
                {
                    if (lineSegment.Colinear(polygonEdge))
                    {
                        // If lines are parallel and colinear, still register an intersection
                        if (polygonEdge.IntersectsPoint(lineSegment.p1))
                        {
                            // If lineSegment.p1 is on polygonEdge, use lineSegment.p1
                            float dist = Vector2.ColinearLengths(
                                polygonEdge.p1, lineSegment.p1, polygonEdge.p2 - polygonEdge.p1);
                            distances = (0, dist);
                        }
                        else
                        {
                            // Otherwise, use polygonEdge.p1
                            float dist = Vector2.ColinearLengths(
                                lineSegment.p1, polygonEdge.p1, lineSegment.p2 - lineSegment.p1);
                            distances = (dist, 0);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                (float lineSegmentDist, float edgeDist) = distances.Value;

                // Only include intersections with polygon edge.
                if (FloatHelpers.Lt(edgeDist, 0) || FloatHelpers.Gte(edgeDist, 1))
                {
                    continue;
                }

                intersectionDatas.Add(new IntersectionData(
                    polygon1: otherPolygon,
                    poly1EdgeIndex: otherEdgeIndex,
                    poly1Dist: lineSegmentDist,
                    polygon2: this,
                    poly2EdgeIndex: i,
                    poly2Dist: edgeDist
                ));
            }

            return intersectionDatas;
        }

        /// <summary>
        /// Whether a point was inside this polygon.
        /// </summary>
        public ContainmentType ContainsPoint(Vector2 point)
        {
            List<IntersectionData> datas = this.GetIntersectionDatasForLine(
                new LineSegment(point, new Vector2(point.x + 1, point.y)));
            
            return this.ContainsPoint(point, datas);
        }

        /// <summary>
        /// Based on the intersectionDatas from a certain point, compute whether
        /// the point was inside this polygon.
        /// </summary>
        private ContainmentType ContainsPoint(Vector2 point, List<IntersectionData> intersectionDatas)
        {
            if (intersectionDatas.Any(data => FloatHelpers.Eq(data.poly1.distanceAlongEdge, 0)))
            {
                return ContainmentType.BOUNDARY;
            }

            IEnumerable<IntersectionData> relevantDatas = intersectionDatas
                .Where(data => FloatHelpers.Gte(data.poly1.distanceAlongEdge, 0))
                .Where(data => data.GetIntersectionType(point) == IntersectionType.OVERLAPPING);
            
            return relevantDatas.Count() % 2 == 0 ? ContainmentType.OUTSIDE : ContainmentType.INSIDE;
        }

        private enum PolygonOverlapType
        {
            INTERSECTING,
            NO_OVERLAP,
            SUBJECT_CONTAINS_HOLE,
            HOLE_CONTAINS_SUBJECT,
        }

        #endregion
    }
}