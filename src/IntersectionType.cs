namespace PolygonDraw
{
    /// <summary>
    /// Describes how two vertices/edges intersect. There are 4 edges involved in 
    /// an intersection: 2 from one polygon, and 2 from the other polygon. This enum
    /// categorizes intersections based on the positions of these 4 edges.
    /// </summary>
    enum IntersectionType
    {
        ///<summary>
        /// Most basic intersection type. Applies to all edge-edge intersections.
        /// Means that the edge from one polygon is between the two edges from the other polygon.
        ///</summary>
        OVERLAPPING,

        ///<summary>
        /// An outer intersection type means that there is no overlap in interiors of
        /// the two polygons.
        ///</summary>
        OUTER,

        ///<summary>
        /// Both edges from the second polygon are between the two edges from the first.
        ///</summary>
        POLY1_CONTAINS_POLY2,

        ///<summary>
        /// Both edges from the first polygon are between the two edges from the second.
        ///</summary>
        POLY2_CONTAINS_POLY1,

        ///<summary>
        /// Both pairs of edges contain each other. This is only possible at a concave
        /// vertex from at least one of the two polygons.
        ///</summary>
        SPLIT,
    }
}