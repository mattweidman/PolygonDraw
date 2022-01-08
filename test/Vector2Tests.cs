using NUnit.Framework;
using PolygonDraw;
using System;

namespace PolygonDrawTests
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
    }
}