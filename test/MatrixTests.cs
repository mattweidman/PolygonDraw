using NUnit.Framework;
using PolygonDraw;

namespace PolygonDrawTests
{
    public class MatrixTests
    {
        [Test]
        public void Determinant_Test()
        {
            Matrix2 matrix = new Matrix2(1, 2, -3, -4);
            Assert.AreEqual(2, matrix.Determinant());
        }

        [Test]
        public void Inverse_Possible()
        {
            Matrix2 matrix = new Matrix2(4, 7, 2, 6);
            Matrix2 expected = new Matrix2(0.6f, -0.7f, -0.2f, 0.4f);
            Matrix2 observed = matrix.Inverse();
            PolygonDrawAssert.AreEqual(expected, observed);
        }

        [Test]
        public void Inverse_NotPossible()
        {
            Matrix2 matrix = new Matrix2(3, 2, 6, 4);
            Matrix2 observed = matrix.Inverse();
            Assert.IsNull(observed);
        }

        [Test]
        public void Dot_Test()
        {
            Matrix2 matrix = new Matrix2(3, 2, 6, 4);
            Vector2 v = new Vector2(1, 2);
            Vector2 product = matrix.Dot(v);
            Vector2 expected = new Vector2(7, 14);

            PolygonDrawAssert.AreEqual(expected, product);
        }
    }
}