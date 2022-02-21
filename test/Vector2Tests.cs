using NUnit.Framework;
using PolygonDraw;
using System;

namespace PolygonDraw.Tests
{
    public class Vector2Tests
    {
        [Test]
        public void Angle_Hori_Right()
        {
            PolygonDrawAssert.AreEqual(0, new Vector2(1, 0).Angle(new Vector2(1, 0)));
        }

        [Test]
        public void Angle_Hori_Q1()
        {
            PolygonDrawAssert.AreEqual(MathF.PI * 7 / 4, new Vector2(1, 0).Angle(new Vector2(1, 1)));
        }

        [Test]
        public void Angle_Hori_Up()
        {
            PolygonDrawAssert.AreEqual(MathF.PI * 3 / 2, new Vector2(1, 0).Angle(new Vector2(0, 1)));
        }

        [Test]
        public void Angle_Hori_Q2()
        {
            PolygonDrawAssert.AreEqual(MathF.PI * 5 / 4, new Vector2(1, 0).Angle(new Vector2(-1, 1)));
        }

        [Test]
        public void Angle_Hori_180()
        {
            PolygonDrawAssert.AreEqual(MathF.PI, new Vector2(1, 0).Angle(new Vector2(-1, 0)));
        }

        [Test]
        public void Angle_Hori_Q3()
        {
            PolygonDrawAssert.AreEqual(MathF.PI * 3 / 4, new Vector2(1, 0).Angle(new Vector2(-1, -1)));
        }

        [Test]
        public void Angle_Hori_Down()
        {
            PolygonDrawAssert.AreEqual(MathF.PI / 2, new Vector2(1, 0).Angle(new Vector2(0, -1)));
        }

        [Test]
        public void Angle_Hori_Q4()
        {
            PolygonDrawAssert.AreEqual(MathF.PI / 4, new Vector2(1, 0).Angle(new Vector2(1, -1)));
        }

        [Test]
        public void Angle_Q2_Q3()
        {
            PolygonDrawAssert.AreEqual(MathF.PI * 3 / 2, new Vector2(-5, 5).Angle(new Vector2(-0.5f, -0.5f)));
        }

        [Test]
        public void IsBetween_Q1()
        {
            Vector2 d1 = new Vector2(5, 5);
            Vector2 d2 = new Vector2(3, 0);

            Assert.IsTrue(new Vector2(2, 1).IsBetween(d1, d2));
            Assert.IsTrue(new Vector2(6, 3).IsBetween(d1, d2));
            Assert.IsFalse(new Vector2(1, 3).IsBetween(d1, d2));
            Assert.IsFalse(new Vector2(-2, 2).IsBetween(d1, d2));
            Assert.IsFalse(new Vector2(-1, -1).IsBetween(d1, d2));
            Assert.IsFalse(new Vector2(1, -1).IsBetween(d1, d2));
        }

        [Test]
        public void IsBetween_SmallAngle()
        {
            Vector2 d1 = new Vector2(-1, -0.5f);
            Vector2 d2 = new Vector2(-1, 0.5f);

            Assert.IsTrue(new Vector2(-1, 0).IsBetween(d1, d2));
            Assert.IsTrue(new Vector2(-0.01f, 0).IsBetween(d1, d2));
            Assert.IsFalse(new Vector2(-1, 1).IsBetween(d1, d2));
            Assert.IsFalse(new Vector2(-1, 100).IsBetween(d1, d2));
            Assert.IsFalse(new Vector2(1, 100).IsBetween(d1, d2));
        }

        [Test]
        public void Colinear_Diagonal()
        {
            Assert.IsTrue(Vector2.Colinear(
                new Vector2(-1, 1), new Vector2(5, 4), new Vector2(-2, 0.5f)));
        }

        [Test]
        public void Colinear_Horizontal()
        {
            Assert.IsTrue(Vector2.Colinear(
                new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-10, 0), new Vector2(3, 0)));
        }

        [Test]
        public void Colinear_Vertical()
        {
            Assert.IsTrue(Vector2.Colinear(
                new Vector2(1, 2), new Vector2(1, 4), new Vector2(1, 3), new Vector2(1, 0)));
        }

        [Test]
        public void Colinear_False_3Vertices()
        {
            Assert.IsFalse(Vector2.Colinear(
                new Vector2(1, 2), new Vector2(2, 3), new Vector2(2, 4)));
        }

        [Test]
        public void Colinear_False_4Vertices()
        {
            Assert.IsFalse(Vector2.Colinear(
                new Vector2(5, 5), new Vector2(3, 3), new Vector2(4, 4), new Vector2(1, 0)));
        }

        [Test]
        public void ColinearLengths_Horizontal()
        {
            Assert.AreEqual(3, Vector2.ColinearLengths(
                new Vector2(1, 0), new Vector2(7, 0), new Vector2(2, 0)));
        }

        [Test]
        public void ColinearLengths_Vertical()
        {
            Assert.AreEqual(12, Vector2.ColinearLengths(
                new Vector2(0, -1), new Vector2(0, 5), new Vector2(0, 0.5f)));
        }

        [Test]
        public void ColinearLengths_Diagonal()
        {
            Assert.AreEqual(0.5f, Vector2.ColinearLengths(
                new Vector2(1, 1), new Vector2(3, 3), new Vector2(4, 4)));
        }

        [Test]
        public void Bisect_RightAngle()
        {
            Vector2 v1 = new Vector2(0, 5);
            Vector2 v2 = new Vector2(2, 0);
            Vector2 expected = new Vector2(1/MathF.Sqrt(2), 1/MathF.Sqrt(2));
            PolygonDrawAssert.AreEqual(expected, v1.Bisect(v2));
        }

        [Test]
        public void Bisect_RightAngle_Wide()
        {
            Vector2 v1 = new Vector2(2, 0);
            Vector2 v2 = new Vector2(0, 5);
            Vector2 expected = new Vector2(-1/MathF.Sqrt(2), -1/MathF.Sqrt(2));
            PolygonDrawAssert.AreEqual(expected, v1.Bisect(v2));
        }

        [Test]
        public void Bisect_AroundXAxis()
        {
            Vector2 v1 = new Vector2(2, 1);
            Vector2 v2 = new Vector2(4, -2);
            Vector2 expected = new Vector2(1, 0);
            PolygonDrawAssert.AreEqual(expected, v1.Bisect(v2));
        }

        [Test]
        public void Bisect_AroundXAxis_Wide()
        {
            Vector2 v1 = new Vector2(4, -2);
            Vector2 v2 = new Vector2(2, 1);
            Vector2 expected = new Vector2(-1, 0);
            PolygonDrawAssert.AreEqual(expected, v1.Bisect(v2));
        }
    }
}