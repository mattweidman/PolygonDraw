namespace PolygonDraw
{
    /// <summary>
    /// Represents a location where two polygons can intersect. Rather than storing
    /// coordinates, IntersectionData stores the indices of the edges that intersect
    /// and the proportion along those edges where the intersection happens.
    /// </summary>
    public class IntersectionData
    {
        public PolygonData poly1;

        public PolygonData poly2;

        public IntersectionData(Polygon polygon1, int poly1EdgeIndex, float poly1Dist,
            Polygon polygon2, int poly2EdgeIndex, float poly2Dist)
        {
            this.poly1 = new PolygonData(polygon1, poly1EdgeIndex, poly1Dist);
            this.poly2 = new PolygonData(polygon2, poly2EdgeIndex, poly2Dist);
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
            bool poly1IsVertex = FloatHelpers.Eq(this.poly1.distanceAlongEdge, 0);
            bool poly2IsVertex = FloatHelpers.Eq(this.poly2.distanceAlongEdge, 0);
            
            // Edge-edge intersections are always overlapping.
            if (!poly1IsVertex && !poly2IsVertex)
            {
                return IntersectionType.OVERLAPPING;
            }

            return this.GetIntersectionTypeInternal(
                center: this.GetIntersectionPoint(),
                p11: poly1IsVertex ? this.poly1.pPrev : this.poly1.p1,
                p12: this.poly1.p2,
                p21: poly2IsVertex ? this.poly2.pPrev : this.poly2.p1,
                p22: this.poly2.p2);
        }

        /// <summary>
        /// Categorize intersection based on positions of edges connected to intersection.
        /// </summary>
        /// <param name="poly1SubIn">If poly1SubIn is provided, rather than use poly1 to
        /// compute 2 of the vectors of the intersection, use a vector from poly1SubIn to
        /// the intersection point and a vector opposite it at the intersection point.</param>
        public IntersectionType GetIntersectionType(Vector2 poly1SubIn)
        {
            bool poly1IsVertex = FloatHelpers.Eq(this.poly1.distanceAlongEdge, 0);
            bool poly2IsVertex = FloatHelpers.Eq(this.poly2.distanceAlongEdge, 0);
            
            // Edge-edge intersections are always overlapping.
            if (!poly1IsVertex && !poly2IsVertex)
            {
                return IntersectionType.OVERLAPPING;
            }

            Vector2 center = this.GetIntersectionPoint();
            return this.GetIntersectionTypeInternal(
                center,
                p11: poly1SubIn,
                p12: poly1SubIn + (center - poly1SubIn) * 2,
                p21: poly2IsVertex ? this.poly2.pPrev : this.poly2.p1,
                p22: this.poly2.p2);
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
            Vector2 poly2Dir1 = p21 - center;
            Vector2 poly2Dir2 = p22 - center;

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

            /// <summary>First endpoint of edge.</summary>
            public Vector2 p1 => this.polygon.vertices[this.edgeIndex];

            /// <summary>Second endpoint of edge.</summary>
            public Vector2 p2 => this.polygon.vertices[
                (this.edgeIndex + 1) % this.polygon.vertices.Count];

            /// <summary>Point that appears before p1.</summary>
            public Vector2 pPrev => this.polygon.vertices[
                (this.edgeIndex + this.polygon.vertices.Count - 1) % this.polygon.vertices.Count];

            /// <summary>Location of intersection point.</summary>
            public Vector2 intersectionPoint => this.p1 + (this.p2 - this.p1) * this.distanceAlongEdge;

            public PolygonData(Polygon polygon, int edgeIndex, float distanceAlongEdge)
            {
                this.polygon = polygon;
                this.edgeIndex = edgeIndex;
                this.distanceAlongEdge = distanceAlongEdge;
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