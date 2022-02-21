namespace PolygonDraw
{
    /// <summary>
    /// Represents a location where two polygons can intersect. Rather than storing
    /// coordinates, IntersectionData stores the indices of the edges that intersect
    /// and the proportion along those edges where the intersection happens.
    /// </summary>
    class IntersectionData
    {
        public PolygonData poly1;

        public PolygonData poly2;

        private IntersectionType? cachedIntersectionType;

        public IntersectionData(Polygon polygon1, int poly1EdgeIndex, float poly1Dist,
            Polygon polygon2, int poly2EdgeIndex, float poly2Dist)
        {
            this.poly1 = new PolygonData(polygon1, poly1EdgeIndex, poly1Dist, false);
            this.poly2 = new PolygonData(polygon2, poly2EdgeIndex, poly2Dist, true);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IntersectionData other))
            {
                return false;
            }

            return this.poly1.Equals(other.poly1) && this.poly2.Equals(other.poly2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Coordinates of intersection.
        /// </summary>
        public Vector2 GetIntersectionPoint()
        {
            return this.poly1.polygon != null 
                ? this.poly1.intersectionPoint
                : this.poly2.intersectionPoint;
        }

        /// <summary>
        /// Categorize intersection based on positions of edges connected to intersection.
        /// </summary>
        public IntersectionType GetIntersectionType()
        {
            if (this.cachedIntersectionType.HasValue)
            {
                return this.cachedIntersectionType.Value;
            }
            
            // Edge-edge intersections are always overlapping.
            if (!this.poly1.isVertex && !this.poly2.isVertex)
            {
                this.cachedIntersectionType = IntersectionType.OVERLAPPING;
                return this.cachedIntersectionType.Value;
            }

            this.cachedIntersectionType = this.GetIntersectionTypeInternal(
                center: this.GetIntersectionPoint(),
                p11: this.poly1.prevPoint,
                p12: this.poly1.nextPoint,
                p21: this.poly2.prevPoint,
                p22: this.poly2.nextPoint);
            return this.cachedIntersectionType.Value;
        }

        /// <summary>
        /// Categorize intersection based on positions of edges connected to intersection.
        /// </summary>
        /// <param name="poly1SubIn">If poly1SubIn is provided, rather than use poly1 to
        /// compute 2 of the vectors of the intersection, use a vector from poly1SubIn to
        /// the intersection point and a vector opposite it at the intersection point.</param>
        public IntersectionType GetIntersectionType(Vector2 poly1SubIn)
        {
            // Edge-edge intersections are always overlapping.
            if (!this.poly1.isVertex && !this.poly2.isVertex)
            {
                this.cachedIntersectionType = IntersectionType.OVERLAPPING;
                return this.cachedIntersectionType.Value;
            }

            Vector2 center = this.GetIntersectionPoint();
            this.cachedIntersectionType = this.GetIntersectionTypeInternal(
                center,
                p11: poly1SubIn,
                p12: poly1SubIn + (center - poly1SubIn) * 2,
                p21: this.poly2.prevPoint,
                p22: this.poly2.nextPoint);
            return this.cachedIntersectionType.Value;
        }

        /// <summary>
        /// Computes the intersection type for an intersection of 4 vectors.
        /// Intersection types are based on how two pairs of vectors interact.
        /// </summary>
        /// <param name="center">Center of intersection.</param>
        /// <param name="p11">First point of first pair.</param>
        /// <param name="p12">Second point of first pair.</param>
        /// <param name="p12">First point of second pair.</param>
        /// <param name="p12">Second point of second pair.</param>
        private IntersectionType GetIntersectionTypeInternal(
            Vector2 center, Vector2 p11, Vector2 p12, Vector2 p21, Vector2 p22)
        {
            Vector2 poly1Dir1 = p11 - center;
            Vector2 poly1Dir2 = p12 - center;

            // Flip edges on clip polygon because its vertices are in reverse order
            Vector2 poly2Dir1 = p22 - center;
            Vector2 poly2Dir2 = p21 - center;

            if (FloatHelpers.Eq(poly1Dir1.Angle(poly2Dir1), 0) &&
                FloatHelpers.Eq(poly1Dir2.Angle(poly2Dir2), 0))
            {
                return IntersectionType.POLY2_CONTAINS_POLY1;
            }

            if (FloatHelpers.Eq(poly1Dir1.Angle(poly2Dir1), 0) ||
                FloatHelpers.Eq(poly1Dir2.Angle(poly2Dir2), 0))
            {
                return IntersectionType.OVERLAPPING;
            }

            bool p1D1BetweenP2 = poly1Dir1.IsBetween(poly2Dir2, poly2Dir1);
            bool p1D2BetweenP2 = poly1Dir2.IsBetween(poly2Dir2, poly2Dir1);
            bool p2D1BetweenP1 = poly2Dir1.IsBetween(poly1Dir2, poly1Dir1);
            bool p2D2BetweenP1 = poly2Dir2.IsBetween(poly1Dir2, poly1Dir1);

            if (!p1D1BetweenP2 && !p1D2BetweenP2 && !p2D1BetweenP1 && !p2D2BetweenP1)
            {
                return IntersectionType.OUTER;
            }

            if (!p1D1BetweenP2 && !p1D2BetweenP2)
            {
                return IntersectionType.POLY1_CONTAINS_POLY2;
            }

            if (!p2D1BetweenP1 && !p2D2BetweenP1)
            {
                return IntersectionType.POLY2_CONTAINS_POLY1;
            }

            if (p1D1BetweenP2 && p1D2BetweenP2 && p2D1BetweenP1 && p2D2BetweenP1)
            {
                return IntersectionType.SPLIT;
            }

            return IntersectionType.OVERLAPPING;
        }

        /// <summary>
        /// Whether this intersection could be a starting node in a new polygon.
        /// We assume poly1 is the subject polygon and poly2 is the clip polygon.
        /// </summary>
        public bool IsStarter()
        {
            IntersectionType intersectionType = this.GetIntersectionType();

            if (intersectionType == IntersectionType.POLY2_CONTAINS_POLY1)
            {
                // Subject completely hidden. Don't start a polygon here.
                return false;
            }
            else if (intersectionType == IntersectionType.POLY1_CONTAINS_POLY2)
            {
                // Clip is inside the subject. Can start a polygon here.
                return true;
            }
            else if (intersectionType == IntersectionType.SPLIT)
            {
                // If there is a split vertex, there must be at least two overlapping
                // vertices on the same poygon, so let one of them be the starter instead.
                return false;
            }
            else if (intersectionType == IntersectionType.OUTER)
            {
                // For outer, return true if the vertex is part of the subject.
                return FloatHelpers.Eq(this.poly1.distanceAlongEdge, 0);
            }

            // Overlapping vertices are starters if the edge from poly2 is entering poly1.
            Vector2 center = this.GetIntersectionPoint();
            Vector2 dir11 = this.poly1.prevPoint - center;
            Vector2 dir12 = this.poly1.nextPoint - center;
            Vector2 dir22 = this.poly2.nextPoint - center;

            return dir22.IsBetween(dir12, dir11);
        }

        /// <summary>
        /// Returns true if the next edge in polygon 2 goes within the bounds of polygon 1.
        /// </summary>
        public bool NextPoly2EdgeInsidePoly1()
        {
            Vector2 center = this.GetIntersectionPoint();
            Vector2 poly2Next = this.poly2.nextPoint - center;
            Vector2 poly1Prev = this.poly1.prevPoint - center;
            Vector2 poly1Next = this.poly1.nextPoint - center;
            return poly2Next.IsBetween(poly1Next, poly1Prev);
        }

        public override string ToString()
        {
            return this.GetIntersectionPoint().ToString();
        }

        public bool IsRemovable()
        {
            // We can remove outer-type intersections that aren't on a subject vertex
            return this.GetIntersectionType() == IntersectionType.OUTER &&
                !FloatHelpers.Eq(this.poly1.distanceAlongEdge, 0);
        }

        /// <summary>
        /// Intersection data for one polygon.
        /// </summary>
        public class PolygonData
        {
            /// <summary>Reference to polygon.</summary>
            public Polygon polygon;

            /// <summary>Index of edge in polygon where intersection happens.</summary>
            public int edgeIndex;

            /// <summary>
            /// Proportion across the edge from polygon where intersection happens.
            /// In the range [0, 1) if between the endpoints of edge. 0 if intersection is at
            /// first endpoint in segment. 1 if intersection is at second endpoint.
            /// </summary>
            public float distanceAlongEdge;

            private bool isReversed;

            /// <summary>First endpoint of edge.</summary>
            private Vector2 p1 => this.polygon.vertices[this.edgeIndex];

            /// <summary>Second endpoint of edge.</summary>
            private Vector2 p2 => this.polygon.vertices[
                (this.edgeIndex + 1) % this.polygon.vertices.Count];

            /// <summary>Point that appears before p1.</summary>
            private Vector2 pPrev => this.polygon.vertices[
                (this.edgeIndex + this.polygon.vertices.Count - 1) % this.polygon.vertices.Count];

            /// <summary>Location of intersection point.</summary>
            public Vector2 intersectionPoint => this.p1 + (this.p2 - this.p1) * this.distanceAlongEdge;

            /// <summary>Whether this point is on a vertex.</summary>
            public bool isVertex => FloatHelpers.Eq(this.distanceAlongEdge, 0);

            /// <summary>The next point in the polygon.</summary>
            public Vector2 nextPoint {
                get {
                    if (this.isReversed)
                    {
                        return this.isVertex ? this.pPrev : this.p1;
                    }

                    return this.p2;
                }
            }

            /// <summary>The previous point in the polygon.</summary>
            public Vector2 prevPoint {
                get {
                    if (this.isReversed)
                    {
                        return this.p2;
                    }

                    return this.isVertex ? this.pPrev : this.p1;
                }
            }

            public PolygonData(Polygon polygon, int edgeIndex, float distanceAlongEdge, bool isReversed)
            {
                this.polygon = polygon;
                this.edgeIndex = edgeIndex;
                this.distanceAlongEdge = distanceAlongEdge;
                this.isReversed = isReversed;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is PolygonData other))
                {
                    return false;
                }

                return this.polygon.Equals(other.polygon) &&
                    this.edgeIndex == other.edgeIndex &&
                    FloatHelpers.Eq(this.distanceAlongEdge, other.distanceAlongEdge);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}