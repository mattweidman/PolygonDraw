using NUnit.Framework;
using PolygonDraw;
using System.Collections.Generic;

namespace PolygonDraw.Tests
{
    public class FloatHelpersTests
    {
        [Test]
        public void ArgMax_Vectors()
        {
            List<Vector2> vectors = new List<Vector2>()
            {
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(-1, -1),
                new Vector2(-2, -1),
                new Vector2(10, 10),
                new Vector2(100, 1),
            };

            int observed = vectors.ArgMax(v => v.y);

            Assert.AreEqual(4, observed);
        }

        [Test]
        public void ArgMax_FirstVecotr()
        {
            List<Vector2> vectors = new List<Vector2>()
            {
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(-1, -1),
                new Vector2(-2, -1),
            };

            int observed = vectors.ArgMax(v => v.y);
            
            Assert.AreEqual(0, observed);
        }

        [Test]
        public void ArgMin_Vector()
        {
            List<Vector2> vectors = new List<Vector2>()
            {
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(-2, -1),
                new Vector2(-1, -2),
                new Vector2(10, 10),
                new Vector2(100, 1),
            };

            int observed = vectors.ArgMin(v => v.y);

            Assert.AreEqual(3, observed);
        }
    }
}