using System.Collections.Generic;

namespace PolygonDraw
{
    /// <summary>
    /// An arrangement of polygons and polygon-shaped holes.
    /// </summary>
    public class PolygonArrangement
    {
        public List<Polygon> polygons;

        public List<Polygon> holes;

        public PolygonArrangement(List<Polygon> polygons, List<Polygon> holes)
        {
            this.polygons = polygons;
            this.holes = holes;
        }

        public PolygonArrangement(List<Polygon> polygons) : this(polygons, new List<Polygon>())
        {}

        public PolygonArrangement() : this(new List<Polygon>())
        {}
    }
}