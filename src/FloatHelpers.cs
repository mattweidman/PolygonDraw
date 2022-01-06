using System;

namespace PolygonDraw
{
    public static class FloatHelpers
    {
        public static readonly float EPSILON = 0.00001f;

        /// <summary>
        /// Float equality within epsilon.
        /// </summary>
        public static bool Eq(float a, float b)
        {
            return Math.Abs(a - b) < EPSILON;
        }

        /// <summary>
        /// Greater than and not equal to.
        /// </summary>
        public static bool Gt(float a, float b)
        {
            return a > b && !Eq(a, b);
        }

        /// <summary>
        /// Greater than or equal to.
        /// </summary>
        public static bool Gte(float a, float b)
        {
            return a > b || Eq(a, b);
        }

        /// <summary>
        /// Less than and not equal to.
        /// </summary>
        public static bool Lt(float a, float b)
        {
            return a < b && !Eq(a, b);
        }

        /// <summary>
        /// Less than or equal to.
        /// </summary>
        public static bool Lte(float a, float b)
        {
            return a < b || Eq(a, b);
        }
    }
}