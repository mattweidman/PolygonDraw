using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PolygonDraw.Tests
{
    public class SearchHelpersTests
    {
        [Test]
        public void BinarySearchClosest_ValueFound()
        {
            List<float> list = new List<float>() {-3, -2, 2, 3, 5, 7, 11};
            Assert.AreEqual(4, SearchHelpers.BinarySearchClosest(list, 5));
        }

        [Test]
        public void BinarySearchClosest_BetweenElements_High()
        {
            List<float> list = new List<float>() {-3, -2, 2, 3, 5, 7, 11};
            Assert.AreEqual(2, SearchHelpers.BinarySearchClosest(list, 1));
        }

        [Test]
        public void BinarySearchClosest_BetweenElements_Low()
        {
            List<float> list = new List<float>() {-3, -2, 2, 3, 5, 7, 11};
            Assert.AreEqual(1, SearchHelpers.BinarySearchClosest(list, -1));
        }

        [Test]
        public void BinarySearchClosest_BeforeFirst()
        {
            List<float> list = new List<float>() {-3, -2, 2, 3, 5, 7, 11};
            Assert.AreEqual(0, SearchHelpers.BinarySearchClosest(list, -4));
        }

        [Test]
        public void BinarySearchClosest_AfterLast()
        {
            List<float> list = new List<float>() {-3, -2, 2, 3, 5, 7, 11};
            Assert.AreEqual(6, SearchHelpers.BinarySearchClosest(list, 12));
        }

        [Test]
        public void BinarySearchClosest_Bucket_Found()
        {
            List<ConnectionVertexBucket> buckets = new List<ConnectionVertexBucket>()
            {
                ConnectionVertexBucket.FromMinAndMax(0.5f, 1, 2),
                ConnectionVertexBucket.FromMinAndMax(0.5f, 2.5f, 3.5f),
                ConnectionVertexBucket.FromMinAndMax(0.5f, 9.25f, 10.25f),
            };

            int observed = SearchHelpers.BinarySearchClosest(
                buckets.Select(bucket => bucket.maxX).ToList(),
                3.1f,
                i => buckets[i].minX,
                i => buckets[i].maxX);
            
            Assert.AreEqual(1, observed);
        }

        [Test]
        public void BinarySearchClosest_Bucket_BetweenElements_Low()
        {
            List<ConnectionVertexBucket> buckets = new List<ConnectionVertexBucket>()
            {
                ConnectionVertexBucket.FromMinAndMax(0.5f, 1, 2),
                ConnectionVertexBucket.FromMinAndMax(0.5f, 2.5f, 3.5f),
                ConnectionVertexBucket.FromMinAndMax(0.5f, 9.25f, 10.25f),
            };

            int observed = SearchHelpers.BinarySearchClosest(
                buckets.Select(bucket => bucket.maxX).ToList(),
                4.1f,
                i => buckets[i].minX,
                i => buckets[i].maxX);
            
            Assert.AreEqual(1, observed);
        }

        [Test]
        public void BinarySearchClosest_Bucket_BetweenElements_High()
        {
            List<ConnectionVertexBucket> buckets = new List<ConnectionVertexBucket>()
            {
                ConnectionVertexBucket.FromMinAndMax(0.5f, 1, 2),
                ConnectionVertexBucket.FromMinAndMax(0.5f, 2.5f, 3.5f),
                ConnectionVertexBucket.FromMinAndMax(0.5f, 9.25f, 10.25f),
            };

            int observed = SearchHelpers.BinarySearchClosest(
                buckets.Select(bucket => bucket.maxX).ToList(),
                8.8f,
                i => buckets[i].minX,
                i => buckets[i].maxX);
            
            Assert.AreEqual(2, observed);
        }

        [Test]
        public void BinarySearchClosest_Bucket_AfterLast()
        {
            List<ConnectionVertexBucket> buckets = new List<ConnectionVertexBucket>()
            {
                ConnectionVertexBucket.FromMinAndMax(0.5f, 1, 2),
                ConnectionVertexBucket.FromMinAndMax(0.5f, 2.5f, 3.5f),
                ConnectionVertexBucket.FromMinAndMax(0.5f, 9.25f, 10.25f),
            };

            int observed = SearchHelpers.BinarySearchClosest(
                buckets.Select(bucket => bucket.maxX).ToList(),
                10.26f,
                i => buckets[i].minX,
                i => buckets[i].maxX);
            
            Assert.AreEqual(2, observed);
        }

        [Test]
        public void BinarySearchClosest_Bucket_OnLast()
        {
            List<ConnectionVertexBucket> buckets = new List<ConnectionVertexBucket>()
            {
                ConnectionVertexBucket.FromMinAndMax(0.5f, 1, 2),
                ConnectionVertexBucket.FromMinAndMax(0.5f, 2.5f, 3.5f),
                ConnectionVertexBucket.FromMinAndMax(0.5f, 9.25f, 10.25f),
            };

            int observed = SearchHelpers.BinarySearchClosest(
                buckets.Select(bucket => bucket.maxX).ToList(),
                9.26f,
                i => buckets[i].minX,
                i => buckets[i].maxX);
            
            Assert.AreEqual(2, observed);
        }

        [Test]
        public void BinarySearchClosest_Bucket_BeforeFirst()
        {
            List<ConnectionVertexBucket> buckets = new List<ConnectionVertexBucket>()
            {
                ConnectionVertexBucket.FromMinAndMax(0.5f, 1, 2),
                ConnectionVertexBucket.FromMinAndMax(0.5f, 2.5f, 3.5f),
                ConnectionVertexBucket.FromMinAndMax(0.5f, 9.25f, 10.25f),
            };

            int observed = SearchHelpers.BinarySearchClosest(
                buckets.Select(bucket => bucket.maxX).ToList(),
                0,
                i => buckets[i].minX,
                i => buckets[i].maxX);
            
            Assert.AreEqual(0, observed);
        }

        [Test]
        public void BinarySearchClosest_Bucket_OnFirst()
        {
            List<ConnectionVertexBucket> buckets = new List<ConnectionVertexBucket>()
            {
                ConnectionVertexBucket.FromMinAndMax(0.5f, 1, 2),
                ConnectionVertexBucket.FromMinAndMax(0.5f, 2.5f, 3.5f),
                ConnectionVertexBucket.FromMinAndMax(0.5f, 9.25f, 10.25f),
            };

            int observed = SearchHelpers.BinarySearchClosest(
                buckets.Select(bucket => bucket.maxX).ToList(),
                2f,
                i => buckets[i].minX,
                i => buckets[i].maxX);
            
            Assert.AreEqual(0, observed);
        }

        [Test]
        public void FindClosestValidIndex_Even_FoundImmediately()
        {
            List<float> list = new List<float>() { 1, 3, 5, 6, 7, 9 };
            int observed = SearchHelpers.FindClosestValidIndex(
                list,
                startIndex: 3,
                isValid: x => x % 2 == 0,
                distance: x => MathF.Abs(x - 6));
            Assert.AreEqual(3, observed);
        }

        [Test]
        public void FindClosestValidIndex_Even_FoundRight()
        {
            List<float> list = new List<float>() { 1, 3, 5, 6, 7, 9 };
            int observed = SearchHelpers.FindClosestValidIndex(
                list,
                startIndex: 1,
                isValid: x => x % 2 == 0,
                distance: x => MathF.Abs(x - 6));
            Assert.AreEqual(3, observed);
        }

        [Test]
        public void FindClosestValidIndex_Even_FoundLeft()
        {
            List<float> list = new List<float>() { 1, 3, 5, 6, 7, 9 };
            int observed = SearchHelpers.FindClosestValidIndex(
                list,
                startIndex: 4,
                isValid: x => x % 2 == 0,
                distance: x => MathF.Abs(x - 6));
            Assert.AreEqual(3, observed);
        }

        [Test]
        public void FindClosestValidIndex_Even_StartOnFirst()
        {
            List<float> list = new List<float>() { 1, 3, 5, 6, 7, 9 };
            int observed = SearchHelpers.FindClosestValidIndex(
                list,
                startIndex: 0,
                isValid: x => x % 2 == 0,
                distance: x => MathF.Abs(x - 6));
            Assert.AreEqual(3, observed);
        }

        [Test]
        public void FindClosestValidIndex_Even_StartOnLast()
        {
            List<float> list = new List<float>() { 1, 3, 5, 6, 7, 9 };
            int observed = SearchHelpers.FindClosestValidIndex(
                list,
                startIndex: 5,
                isValid: x => x % 2 == 0,
                distance: x => MathF.Abs(x - 6));
            Assert.AreEqual(3, observed);
        }

        [Test]
        public void FindClosestValidIndex_Even_NotFound()
        {
            List<float> list = new List<float>() { 1, 3, 5, 7, 9 };
            int observed = SearchHelpers.FindClosestValidIndex(
                list,
                startIndex: 2,
                isValid: x => x % 2 == 0,
                distance: x => MathF.Abs(x - 6));
            Assert.AreEqual(-1, observed);
        }
    }
}