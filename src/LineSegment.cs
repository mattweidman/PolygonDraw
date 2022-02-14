using System;

namespace PolygonDraw
{
    public class LineSegment
    {
        public Vector2 p1;
        public Vector2 p2;

        public LineSegment(Vector2 p1, Vector2 p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        /// <summary>
        /// Find the intersection of two line segments. If the line segments do
        /// not overlap, return null. If includeEndpoints is true, will return an 
        /// intersection if it is at an endpoint.
        /// </summary>
        public Vector2 GetIntersection(LineSegment otherLine, bool includeEndpoints = false)
        {
            (float, float)? distances = this.GetLineIntersectionDistances(otherLine);
            
            if (!distances.HasValue)
            {
                return null;
            }

            (float t1, float t2) = distances.Value;

            bool intersects;
            if (includeEndpoints)
            {
                intersects = FloatHelpers.Gte(t1, 0) && FloatHelpers.Lte(t1, 1)
                    && FloatHelpers.Gte(t2, 0) && FloatHelpers.Lte(t2, 1);
            }
            else
            {
                intersects = FloatHelpers.Gt(t1, 0) && FloatHelpers.Lt(t1, 1)
                    && FloatHelpers.Gt(t2, 0) && FloatHelpers.Lt(t2, 1);
            }

            return intersects ? (1 - t1) * this.p1 + t1 * this.p2 : null;
        }

        /// <summary>
        /// Find the intersection of two infinite lines. If there is
        /// no intersection (parallel), return null. If includeEndpoints is true,
        /// return an intersection if it is at an endpoint. Returns a (float, float)
        /// tuple, where the first element is the number of this LineSegment's lengths
        /// away the intersection is from this.p1. The second element is the number
        /// of otherLine's lengths away from otherLine.p1.
        /// </summary>
        /// <param name="otherLine">Other line.</param>
        /// <returns>A (float, float) tuple, where item1 is the number of this LineSegment's
        /// lengths away the intersection is from this.p1, and item2 is the number of otherLine
        /// lengths the intersection is from otherLine.p1. Return null if parallel.</returns>
        public (float, float)? GetLineIntersectionDistances(LineSegment otherLine)
        {
            Vector2 d1 = this.p2 - this.p1;
            Vector2 d2 = otherLine.p2 - otherLine.p1;
            Matrix2 dMat = new Matrix2(d1.x, -d2.x, d1.y, -d2.y);

            float determinant = dMat.Determinant();
            if (FloatHelpers.Eq(determinant, 0))
            {
                return null;
            }

            Vector2 pDiff = otherLine.p1 - this.p1;
            Vector2 tParam = dMat.Inverse().Dot(pDiff);
            float t1 = tParam.x;
            float t2 = tParam.y;

            return (tParam.x, tParam.y);
        }

        /// <summary>
        /// Whether a point should be considered "on" this line segment.
        /// </summary>
        public bool IntersectsPoint(Vector2 point, bool includeEndpoints = false)
        {
            Vector2 lineDir = (this.p2 - this.p1);
            Vector2 unitDir = lineDir / lineDir.Magnitude();
            float projectionLength = (point - p1).Dot(unitDir);

            // If the projection of point on line is not within bounds, no intersection.
            if (includeEndpoints)
            {
                if (FloatHelpers.Lt(projectionLength, 0) || 
                    FloatHelpers.Gt(projectionLength, (p2 - p1).Magnitude()))
                {
                    return false;
                }
            }
            else
            {
                if (FloatHelpers.Lte(projectionLength, 0) || 
                    FloatHelpers.Gte(projectionLength, (p2 - p1).Magnitude()))
                {
                    return false;
                }
            }

            // If projection is within bounds, depends on perpendicular distance.
            Vector2 projectionPoint = this.p1 + projectionLength * unitDir;
            float perpendicularDist = (point - projectionPoint).Magnitude();

            return FloatHelpers.Eq(0, perpendicularDist);
        }

        public override string ToString()
        {
            return $"{this.p1}-{this.p2}";
        }

        /// <summary>
        /// Return whether a point can be considered left, right, or on a line.
        /// If point is within the y-range of this line segment, return left, right, or
        /// intersecting depending on its x-corodinate.
        /// If this line segment is horizontal, return whether point is left/right
        /// of the line if it has the same x-coordinate as the line segment.
        /// If point is not within the y-range of this line segment, return not aligned.
        /// </summary>
        public HorizontalPosition GetRelativeHorizontalPosition(Vector2 point)
        {
            if (point.Equals(this.p1) || point.Equals(this.p2))
            {
                return HorizontalPosition.INTERSECTING_AT_ENDPOINT;
            }

            Vector2 pLow = this.GetLowerPoint();
            Vector2 pHigh = this.GetHigherPoint();

            if (FloatHelpers.Lt(point.y, pLow.y) || FloatHelpers.Gt(point.y, pHigh.y))
            {
                return HorizontalPosition.NOT_ALIGNED;
            }

            if (this.IsHorizontal())
            {
                if (FloatHelpers.Lt(point.x, this.p1.x) && FloatHelpers.Lt(point.x, this.p2.x))
                {
                    return HorizontalPosition.LEFT;
                }
                else if (FloatHelpers.Gt(point.x, this.p1.x) && FloatHelpers.Gt(point.x, this.p2.x))
                {
                    return HorizontalPosition.RIGHT;
                }
                else
                {
                    return HorizontalPosition.INTERSECTING;
                }
            }

            float lhs = (point.x - pLow.x) * (pHigh.y - pLow.y);
            float rhs = (point.y - pLow.y) * (pHigh.x - pLow.x);

            if (FloatHelpers.Lt(lhs, rhs))
            {
                return HorizontalPosition.LEFT;
            }
            else if (FloatHelpers.Gt(lhs, rhs))
            {
                return HorizontalPosition.RIGHT;
            }
            else
            {
                return HorizontalPosition.INTERSECTING;
            }
        }

        public enum HorizontalPosition
        {
            LEFT,
            RIGHT,
            INTERSECTING,
            INTERSECTING_AT_ENDPOINT,
            NOT_ALIGNED
        }

        /// <summary>
        /// The point with higher y-coordinate.
        /// </summary>
        public Vector2 GetHigherPoint()
        {
            return FloatHelpers.Gt(this.p1.y, this.p2.y) ? this.p1 : this.p2;
        }

        /// <summary>
        /// The point with lower y-coordinate.
        /// </summary>
        public Vector2 GetLowerPoint()
        {
            return FloatHelpers.Gt(this.p1.y, this.p2.y) ? this.p2 : this.p1;
        }

        /// <summary>
        /// The point with lower x-coordinate.
        /// </summary>
        public Vector2 GetLeftPoint()
        {
            return FloatHelpers.Gt(this.p1.x, this.p2.x) ? this.p2 : this.p1;
        }

        /// <summary>
        /// The point with higher x-coordinate.
        /// </summary>
        public Vector2 GetRightPoint()
        {
            return FloatHelpers.Gt(this.p1.x, this.p2.x) ? this.p1 : this.p2;
        }

        /// <summary>
        /// The point that intersects this line at a certain y-coordinate.
        /// Null if y is not in range.
        /// </summary>
        public Vector2 GetPointAtY(float y)
        {
            float minX = MathF.Min(this.p1.x, this.p2.x) - 1;
            float maxX = MathF.Max(this.p1.x, this.p2.x) + 1;
            LineSegment compareSegment = new LineSegment(new Vector2(minX, y), new Vector2(maxX, y));

            return this.GetIntersection(compareSegment, true);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LineSegment other))
            {
                return false;
            }

            return this.p1.Equals(other.p1) && this.p2.Equals(other.p2);
        }

        public override int GetHashCode()
        {
            return (this.p1, this.p2).GetHashCode();
        }

        public bool IsHorizontal()
        {
            return FloatHelpers.Eq(this.p1.y, this.p2.y);
        }

        public bool Colinear(LineSegment other)
        {
            return Vector2.Colinear(this.p1, this.p2, other.p1, other.p2);
        }
    }
}