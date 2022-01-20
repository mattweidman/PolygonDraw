using System;
using System.Linq;

namespace PolygonDraw
{
    /// <summary>
    /// This class encloses data used to describe a graph of points created
    /// when intersecting a triangle with a triangle mask. This graph can be
    /// used to calculate polygons left over after intersecting the triangle mask.
    /// See Triangle.MaskToIntersectionGraph() to see how this is created.
    /// </summary>
    public class TriangleIntersectionGraph
    {
        public Vector2[] allPoints;
        public int?[] baseEdges;
        public int?[] maskEdges;

        /// <summary>
        /// Construct a new triangle intersection graph.
        /// </summary>
        /// <param name="allPoints">List of all points. The indices of this list are used
        /// in baseEdges and maskEdges.</param>
        /// <param name="baseEdges">Adjacency list of clockwise node -> node relationships
        /// on the base triangle.</param>
        /// <param name="maskEdges">Adjacency list of counterclockwise node -> node relationships
        /// on the mask triangle.</param>
        public TriangleIntersectionGraph(Vector2[] allPoints, int?[] baseEdges, int?[] maskEdges)
        {
            if (allPoints.Length != baseEdges.Length || allPoints.Length != maskEdges.Length)
            {
                throw new ArgumentException("All arrays must have equal length in triangle intersection graph.");
            }

            this.allPoints = allPoints;
            this.baseEdges = baseEdges;
            this.maskEdges = maskEdges;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TriangleIntersectionGraph other))
            {
                return false;
            }

            if (this.allPoints.Length != other.allPoints.Length)
            {
                return false;
            }

            for (int i = 0; i < this.allPoints.Length; i++)
            {
                if (!this.allPoints[i].Equals(other.allPoints[i])) return false;
                if (!this.baseEdges[i].Equals(other.baseEdges[i])) return false;
                if (!this.maskEdges[i].Equals(other.maskEdges[i])) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            String allPointsStr = string.Join(",", this.allPoints.ToList());
            String baseEdgesStr = string.Join(",", this.baseEdges.ToList());
            String maskEdgesStr = string.Join(",", this.maskEdges.ToList());

            return $"TriangleIntersectionGraph(allPoints=[{allPointsStr}], "
                + $"baseEdges=[{baseEdgesStr}], maskEdges=[{maskEdgesStr}])";
        }
    }
}