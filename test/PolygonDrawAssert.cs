using NUnit.Framework;
using PolygonDraw;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PolygonDrawTests
{
    public static class PolygonDrawAssert
    {
        public static void AreEqual(float expected, float observed)
        {
            if (!FloatHelpers.Eq(expected, observed))
            {
                Console.WriteLine($"FAIL: expected {expected}, found {observed}.");
                Assert.Fail();
            }
        }

        public static void AreEqual(object expected, object observed)
        {
            if (!expected.Equals(observed))
            {
                Console.WriteLine($"FAIL: expected {expected}, found {observed}.");
                Assert.Fail();
            }
        }

        public static void ListsAreEqual<T>(List<T> expected, List<T> observed)
        {
            bool areEqual = true;

            if (expected.Count != observed.Count)
            {
                areEqual = false;
            }
            else
            {
                for (int i = 0; i < expected.Count; i++)
                {
                    if (!expected[i].Equals(observed[i]))
                    {
                        areEqual = false;
                        break;
                    }
                }
            }

            if (!areEqual)
            {
                throw new AssertionException($"FAIL: expected {ListToString(expected)}, found {ListToString(observed)}");
            }
        }

        public static void ListsContainSame<T>(List<T> expected, List<T> observed)
        {
            bool areEqual = true;

            if (expected.Count != observed.Count)
            {
                areEqual = false;
            }
            else
            {
                foreach (T expectedObj in expected)
                {
                    bool found = observed.Any(obs => expectedObj.Equals(obs));

                    if (!found)
                    {
                        areEqual = false;
                        break;
                    }
                }
            }

            if (!areEqual)
            {
                throw new AssertionException($"FAIL: expected {ListToString(expected)}, found {ListToString(observed)}");
            }
        }

        private static string ListToString<T>(List<T> objs)
        {
            return string.Join(",", objs);
        }
    }
}