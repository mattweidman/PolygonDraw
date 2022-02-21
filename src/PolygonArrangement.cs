using System.Collections.Generic;
using System.Collections.Immutable;

namespace PolygonDraw
{
    /// <summary>
    /// An arrangement of polygons and polygon-shaped holes.
    /// </summary>
    public class PolygonArrangement
    {
        public readonly ImmutableList<Polygon> polygons;

        public readonly ImmutableList<Polygon> holes;

        public PolygonArrangement(IEnumerable<Polygon> polygons, IEnumerable<Polygon> holes)
        {
            this.polygons = polygons.ToImmutableList();
            this.holes = holes.ToImmutableList();
        }

        public PolygonArrangement(IEnumerable<Polygon> polygons) : this(polygons, new List<Polygon>())
        {}

        public PolygonArrangement() : this(new List<Polygon>())
        {}

        public override string ToString()
        {
            string polygonsString = string.Join(",", this.polygons);
            string holesString = string.Join(",", this.holes);
            return $"{{polygons:[{polygonsString}], holes:[{holesString}]}}";
        }
    }
}