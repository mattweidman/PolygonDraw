using NUnit.Framework;
using PolygonDraw;
using System;

namespace PolygonDrawTests
{
    public static class PolygonDrawAssert
    {
        public static void AreEqual(Matrix2 expected, Matrix2 observed)
        {
            ObjectsAreEqual(expected, observed);
        }

        public static void AreEqual(Vector2 expected, Vector2 observed)
        {
            ObjectsAreEqual(expected, observed);
        }

        private static void ObjectsAreEqual(object expected, object observed)
        {
            if (!expected.Equals(observed))
            {
                Console.WriteLine($"FAIL: expected {expected}, found {observed}.");
                Assert.Fail();
            }
        }
    }
}