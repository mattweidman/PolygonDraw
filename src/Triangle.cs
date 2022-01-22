using System;
using System.Collections.Generic;
using System.Linq;

namespace PolygonDraw
{
    public class Triangle : Polygon
    {
        public Triangle(Vector2 p1, Vector2 p2, Vector2 p3) : base (new List<Vector2>() { p1, p2, p3 })
        {
        }

        /// <summary>
        /// Returns whether a point is contained inside the triangle.
        /// </summary>
        /// <param name="point">Point in question.</param>
        /// <param name="includeEdges">If true, return true if point is on an edge 
        /// of this triangle. Else, return false in that case.</param>
        public bool ContainsPoint(Vector2 point, bool includeEdges = false)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 t1 = this.vertices[i];
                Vector2 t2 = this.vertices[(i + 1) % 3];
                float angle = (t2 - t1).Angle(point - t1);

                if (includeEdges)
                {
                    if (FloatHelpers.Gt(angle, MathF.PI) || FloatHelpers.Lt(angle, 0))
                    {
                        return false;
                    }
                }
                else
                {
                    if (FloatHelpers.Gte(angle, MathF.PI) || FloatHelpers.Lte(angle, 0))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns whether another triangle is contained inside this triangle.
        /// </summary>
        /// <param name="other">Other triangle.</param>
        /// <param name="includeEdges">If true, return true if triangle's points are
        /// inside or on an edge. If false, return true only if points are inside.</param>
        public bool ContainsTriangle(Triangle other, bool includeEdges = false)
        {
            return other.vertices.All(v => this.ContainsPoint(v, includeEdges));
        }

        /// <summary>
        /// Cover this triangle by a mask triangle. Return polygons that cover the
        /// area of this triangle that are not covered by the mask.
        /// </summary>
        public List<Polygon> MaskToPolygons(Triangle mask)
        {
            // Special case where mask is entirely contained by base
            if (this.ContainsTriangle(mask))
            {
                return new List<Polygon>() { this.CreateInnerMaskPolygon(mask) };
            }

            List<IntersectionGraphNode> starterNodes = this.MaskToIntersectionGraph(mask);

            List<Polygon> polygons = new List<Polygon>();

            HashSet<IntersectionGraphNode> visited = new HashSet<IntersectionGraphNode>();

            foreach (IntersectionGraphNode starterNode in starterNodes)
            {
                // Skip if already visited this point
                if (visited.Contains(starterNode))
                {
                    continue;
                }

                // DFS to create polygon
                List<Vector2> vertices = new List<Vector2>() { starterNode.point };
                IntersectionGraphNode currentNode = starterNode.baseNext;
                bool onBaseTriangle = true;
                while (currentNode != starterNode)
                {
                    // Add point to polygon
                    vertices.Add(currentNode.point);

                    // Keep track of starter nodes we have visited
                    if (currentNode.isStarter)
                    {
                        visited.Add(currentNode);
                    }

                    // If we hit an intersection point, swap between base/mask
                    if (currentNode.IsSwappable())
                    {
                        onBaseTriangle = !onBaseTriangle;
                    }

                    // Jump to next vertex in graph
                    currentNode = onBaseTriangle
                        ? currentNode.baseNext
                        : currentNode.maskNext;
                }

                polygons.Add(new Polygon(vertices));
            }

            return polygons;
        }

        /// <summary>
        /// Construct a graph describing for each point of interest (vertices and intersection
        /// points) what point would come next clockwise if part of a polygon. Used for
        /// MaskToPolygon().
        /// </summary>
        private List<IntersectionGraphNode> MaskToIntersectionGraph(Triangle mask)
        {
            List<IntersectionGraphNode> allCorners = new List<IntersectionGraphNode>();
            allCorners.AddRange(this.vertices.Select(v => new IntersectionGraphNode(v, true)));
            allCorners.AddRange(mask.vertices.Select(v => new IntersectionGraphNode(v)));

            // Deal with vertex-vertex intersections.
            for (int i = 0; i < 3; i++)
            {
                // Don't start on base vertices covered by the mask.
                if (mask.ContainsPoint(this.vertices[i], true))
                {
                    allCorners[i].isStarter = false;
                }

                for (int j = 0; j < 3; j++)
                {
                    if (this.vertices[i].Equals(mask.vertices[j]))
                    {
                        // Vertices should be the same instance if one of them is an
                        // intersection point. The vertex is an intersection point if
                        // the two triangles overlap at that point.
                        if (AnEdgeIsBetween(this.vertices[i], this.vertices[(i + 1) % 3],
                            this.vertices[(i + 2) % 3], mask.vertices[(j + 1) % 3],
                            mask.vertices[(j + 2) % 3]) || 
                            AnEdgeIsBetween(this.vertices[i], mask.vertices[(j + 1) % 3],
                            mask.vertices[(j + 2) % 3], this.vertices[(i + 1) % 3],
                            this.vertices[(i + 2) % 3]))
                        {
                            allCorners[i] = allCorners[j + 3];
                        }
                    }
                }
            }

            Dictionary<(int, int), IntersectionGraphNode> edgeEdgeIntersections =
                this.GetEdgeEdgeIntersections(mask);

            // Find intersections with base edges.
            for (int i = 0; i < 3; i++)
            {
                List<IntersectionGraphNode> edgeNodes = mask.GetIntersectionsOnLine(
                    allCorners[i],
                    allCorners[(i + 1) % 3],
                    j => allCorners[j + 3],
                    j => edgeEdgeIntersections.GetValueOrDefault((i, j)));

                for (int k = 0; k < edgeNodes.Count - 1; k++)
                {
                    edgeNodes[k].baseNext = edgeNodes[k + 1];
                }
            }

            // Find intersections with mask edges.
            for (int j = 0; j < 3; j++)
            {
                List<IntersectionGraphNode> edgeNodes = this.GetIntersectionsOnLine(
                    allCorners[j + 3],
                    allCorners[(j + 1) % 3 + 3],
                    i => allCorners[i],
                    i => edgeEdgeIntersections.GetValueOrDefault((i, j)));

                for (int k = 1; k < edgeNodes.Count; k++)
                {
                    edgeNodes[k].maskNext = edgeNodes[k - 1];
                }
            }

            return allCorners.Where(v => v.isStarter).ToList();
        }

        /// <summary>
        /// Compute a (this edge, mask edge) -> intersection node mapping.
        /// </summary>
        private Dictionary<(int, int), IntersectionGraphNode> GetEdgeEdgeIntersections(Triangle mask)
        {
            var nodes = new Dictionary<(int, int), IntersectionGraphNode>();

            for (int i = 0; i < 3; i++)
            {
                LineSegment thisLineSegment =
                    new LineSegment(this.vertices[i], this.vertices[(i + 1) % 3]);
                
                for (int j = 0; j < 3; j++)
                {
                    LineSegment maskLineSegment =
                        new LineSegment(mask.vertices[j], mask.vertices[(j + 1) % 3]);
                    
                    Vector2 intersection = thisLineSegment.GetIntersection(maskLineSegment);
                    if (intersection != null)
                    {
                        nodes[(i, j)] = new IntersectionGraphNode(intersection);
                    }
                }
            }

            return nodes;
        }

        /// <summary>
        /// Find points on this triangle that intersect a line on another triangle.
        /// </summary>
        private List<IntersectionGraphNode> GetIntersectionsOnLine(
            IntersectionGraphNode endpoint1,
            IntersectionGraphNode endpoint2,
            Func<int, IntersectionGraphNode> getNode,
            Func<int, IntersectionGraphNode> getExistingIntersection)
        {
            List<IntersectionGraphNode> edgeNodes = new List<IntersectionGraphNode>()
            {
                endpoint1,
                endpoint2
            };

            LineSegment otherLineSegment = new LineSegment(endpoint1.point, endpoint2.point);

            for (int j = 0; j < 3; j++)
            {
                // Edge-edge intersection
                IntersectionGraphNode existingIntersection = getExistingIntersection(j);
                if (existingIntersection != null)
                {
                    edgeNodes.Add(existingIntersection);
                }

                // Edge-vertex intersection
                if (otherLineSegment.IntersectsPoint(this.vertices[j]))
                {
                    // Only intersect if one of the edges is bounded by the other triangle.
                    if (AnEdgeIsBetween(this.vertices[j], endpoint2.point, endpoint1.point,
                        this.vertices[(j + 1) % 3], this.vertices[(j + 2) % 3]))
                    {
                        IntersectionGraphNode otherNode = getNode(j);
                        otherNode.isStarter = false;
                        edgeNodes.Add(otherNode);
                    }
                }
            }

            return edgeNodes.OrderBy(n => (n.point - endpoint1.point).Magnitude()).ToList();
        }

        /// <summary>
        /// Whether (target1 - center) or (target2 - center) is between the clockwise
        /// bounds of (bound1 - center) and (bound2 - center).
        /// </summary>
        private bool AnEdgeIsBetween(Vector2 center, Vector2 bound1, Vector2 bound2,
            Vector2 target1, Vector2 target2)
        {
            Vector2 boundDir1 = bound1 - center;
            Vector2 boundDir2 = bound2 - center;
            Vector2 targetDir1 = target1 - center;
            Vector2 targetDir2 = target2 - center;

            return targetDir1.IsBetween(boundDir1, boundDir2) ||
                targetDir2.IsBetween(boundDir1, boundDir2);
        }

        /// <summary>
        /// If mask is entirely contained inside base triangle, combine them into one polygon.
        /// </summary>
        private Polygon CreateInnerMaskPolygon(Triangle mask)
        {
            // Find which vertex of mask is closest to first vertex in this triangle.
            int closestMaskIndex = mask.vertices
                .Select((v, i) => (v, i))
                .OrderBy(pair => (pair.v - this.vertices[0]).Magnitude())
                .First().i;
            
            // Construct list of vertices.
            List<Vector2> polygonVertices = new List<Vector2>();
            polygonVertices.AddRange(this.vertices);
            polygonVertices.Add(this.vertices[0]);

            for (int i = 0; i < 3; i++)
            {
                polygonVertices.Add(mask.vertices[(3 - i + closestMaskIndex) % 3]);
            }
            polygonVertices.Add(mask.vertices[closestMaskIndex]);

            return new Polygon(polygonVertices);
        }

        /// <summary>
        /// Describes a node in a graph produced by intersecting two triangles.
        /// </summary>
        public class IntersectionGraphNode
        {
            public Vector2 point;

            public bool isStarter = false;

            public IntersectionGraphNode baseNext = null;

            public IntersectionGraphNode maskNext = null;

            public IntersectionGraphNode(Vector2 point, bool isStarter = false)
            {
                this.point = point;
                this.isStarter = isStarter;
            }

            public bool IsSwappable()
            {
                return baseNext != null && maskNext != null;
            }
        }
    }
}