using PolygonDraw;
using NUnit.Framework;

namespace PolygonDrawTests
{
    public class MatrixTests
    {
        [Test]
        public void Determinant()
        {
            Matrix2 matrix = new Matrix2(1, 2, -3, -4);
            Assert.AreEqual(2, matrix.Determinant());
        }
    }
}