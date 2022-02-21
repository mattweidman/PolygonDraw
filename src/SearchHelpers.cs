using System;
using System.Collections.Generic;

namespace PolygonDraw
{
    static class SearchHelpers
    {
        /// <summary>
        /// Find the index of the closest value in a list.
        /// </summary>
        /// <param name="list">Sorted list.</param>
        /// <param name="searchVal">Value we are searching for.</param>
        /// <returns>The index of an element in the list whose range is closest to searchVal.</returns>
        public static int BinarySearchClosest(this List<float> list, float searchVal)
        {
            return BinarySearchClosest(list, searchVal, i => list[i], i => list[i]);
        }

        /// <summary>
        /// Find the index of the closest value in a list.
        /// </summary>
        /// <param name="list">Sorted list.</param>
        /// <param name="searchVal">Value we are searching for.</param>
        /// <param name="minInRange">Given an index in the list, returns the smallest value
        /// that is valid at that index.</param>
        /// <param name="minInRange">Given an index in the list, returns the highest value
        /// that is valid at that index.</param>
        /// <returns>The index of an element in the list whose range is closest to searchVal.</returns>
        public static int BinarySearchClosest(
            this List<float> list,
            float searchVal,
            Func<int, float> minInRange,
            Func<int, float> maxInRange)
        {
            int index = list.BinarySearch(searchVal);
            if (index < 0)
            {
                // BinarySearch returns the negative complement of the index of element
                // just above the target value if no exact match found.
                index = ~index;
            }
            if (index == list.Count)
            {
                // BinarySearch returns the negative complement of the list count if there
                // are no elements greater, so we want to return the last element.
                index = list.Count - 1;
            }
            else if (index != 0)
            {
                float prevDiff = MathF.Abs(maxInRange(index - 1) - searchVal);
                float currDiff = MathF.Abs(minInRange(index) - searchVal);
                if (prevDiff < currDiff)
                {
                    // If the previous element is closer, use it instead.
                    index--;
                }
            }
            
            return index;
        }

        /// <summary>
        /// Find the index of the closest valid element in a sorted list to a certain value,
        /// starting from a certain index. Returns -1 if no valid values found.
        /// </summary>
        /// <param name="list">List to look through.</param>
        /// <param name="startIndex">First index to look at.</param>
        /// <param name="isValid">Returns true if an element is considered "valid".
        /// Keep traversing if returns false.</param>
        /// <param name="distance">Returns the "distance" or "cost" of a particular
        /// object. Distances are expected to increase as we get farther from startIndex.
        /// This method aims to find the valid element with the lowest distance.</param>
        public static int FindClosestValidIndex<T>(
            List<T> list,
            int startIndex,
            Func<T, bool> isValid,
            Func<T, float> distance)
        {
            if (isValid(list[startIndex]))
            {
                return startIndex;
            }

            int rangeStart = startIndex - 1; // inclusive
            int rangeEnd = startIndex + 1; // inclusive

            while (rangeStart >= 0 || rangeEnd < list.Count)
            {
                bool readingFromStart;
                if (rangeStart < 0)
                {
                    readingFromStart = false;
                }
                else if (rangeEnd >= list.Count)
                {
                    readingFromStart = true;
                }
                else
                {
                    readingFromStart = distance(list[rangeStart]) < distance(list[rangeEnd]);
                }

                int readIndex = readingFromStart ? rangeStart : rangeEnd;
                if (isValid(list[readIndex]))
                {
                    return readIndex;
                }

                if (readingFromStart)
                {
                    rangeStart--;
                }
                else
                {
                    rangeEnd++;
                }
            }

            return -1;
        }
    }
}