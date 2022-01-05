using System;

namespace PolygonDraw
{
    public static class FloatHelpers
    {
        public static readonly float EPSILON = 0.00001f;

        public static bool FloatEquals(float a, float b)
        {
            return Math.Abs(a - b) < EPSILON;
        }
    }
}