using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PolygonDraw.Tests
{
    public static class PolygonDrawAssert
    {
        public static void AreEqual(float expected, float observed)
        {
            if (!FloatHelpers.Eq(expected, observed))
            {
                ThrowAssertion(expected, observed);
            }
        }

        public static void AreEqual(object expected, object observed)
        {
            if (!expected.Equals(observed))
            {
                ThrowAssertion(expected, observed);
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
                ThrowAssertion(ListToString(expected), ListToString(observed));
            }
        }

        public static void ListsContainSame<T>(IEnumerable<T> expected, IEnumerable<T> observed)
        {
            bool areEqual = true;

            if (expected.Count() != observed.Count())
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
                        Console.Error.WriteLine($"{expectedObj} not found in observed list.");
                        areEqual = false;
                        break;
                    }
                }
            }

            if (!areEqual)
            {
                ThrowAssertion(ListToString(expected), ListToString(observed));
            }
        }

        public static void Array2DsEqual<T>(T[][] expected, T[][] observed)
        {
            bool areEqual = true;

            if (expected.Length != observed.Length)
            {
                areEqual = false;
            }
            else
            {
                for (int i = 0; i < expected.Length; i++)
                {
                    if (expected[i].Length != observed[i].Length)
                    {
                        areEqual = false;
                        break;
                    }

                    for (int j = 0; j < expected[i].Length; j++)
                    {
                        T exp = expected[i][j];
                        T obs = observed[i][j];

                        if (exp == null && obs == null)
                        {
                            continue;
                        }

                        if (exp == null || obs == null || !exp.Equals(obs))
                        {
                            areEqual = false;
                            break;
                        }
                    }

                    if (!areEqual)
                    {
                        break;
                    }
                }
            }

            if (!areEqual)
            {
                ThrowAssertion(Array2DToString(expected), Array2DToString(observed));
            }
        }

        private static string ListToString<T>(IEnumerable<T> objs)
        {
            return $"[{string.Join(",", objs)}]";
        }

        private static string Array2DToString<T>(T[][] arr)
        {
            IEnumerable<string> rowStrings = arr.Select(row => $"[{string.Join(",", row)}]");
            return $"[{string.Join(",", rowStrings)}]";
        }

        private static string ExpectedFoundString(object expected, object observed)
        {
            return $"FAIL: expected\n{expected}\nfound\n{observed}";
        }

        private static void ThrowAssertion(object expected, object observed)
        {
            throw new AssertionException(ExpectedFoundString(expected, observed));
        }
    }
}