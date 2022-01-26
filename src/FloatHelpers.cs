using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Finds the index of the value in a sequence that maps to the highest value
        /// according to a mapping function.
        /// </summary>
        public static int ArgMax<T>(this IList<T> list, Func<T, float> map)
        {
            if (list.Count == 0)
            {
                throw new ArgumentException("Cannot find the minimum in a list of length 0.");
            }

            int maxIndex = 0;
            float maxValue = map(list[0]);

            for (int i = 1; i < list.Count; i++)
            {
                T key = list[i];
                float value = map(key);

                if (FloatHelpers.Gt(value, maxValue))
                {
                    maxIndex = i;
                    maxValue = value;
                }
            }

            return maxIndex;
        }
    }
}