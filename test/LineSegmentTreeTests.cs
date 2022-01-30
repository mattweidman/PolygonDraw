using NUnit.Framework;
using PolygonDraw;
using System.Collections.Generic;

namespace PolygonDrawTests
{
    public class LineSegmentTreeTests
    {
        #region Insert

        [Test]
        public void Insert_Decreasing()
        {
            LineSegmentTree lsTree = new LineSegmentTree();

            for (int i = 10; i >= 1; i--)
            {
                lsTree.Insert(new LineSegment(new Vector2(i, i), new Vector2(i, 0)));
                lsTree.CheckInvariants();
            }
        }

        [Test]
        public void Insert_Increasing()
        {
            LineSegmentTree lsTree = new LineSegmentTree();

            for (int i = 10; i >= 1; i--)
            {
                lsTree.Insert(new LineSegment(new Vector2(10 - i, i), new Vector2(10 - i, 0)));
                lsTree.CheckInvariants();
            }
        }

        [Test]
        public void Insert_Middles()
        {
            LineSegmentTree lsTree = new LineSegmentTree();

            int[] xCoordsToAdd = new int[] {10, 1, 5, 3, 2, 7, 8, 9, 4};

            for (int i = 0; i < xCoordsToAdd.Length; i++)
            {
                int xCoord = xCoordsToAdd[i];
                lsTree.Insert(new LineSegment(
                    new Vector2(xCoord, xCoordsToAdd.Length + 1 - i),
                    new Vector2(xCoord, 0)));
                lsTree.CheckInvariants();
            }
        }

        [Test]
        public void Insert_Horizontal()
        {
            LineSegmentTree lsTree = new LineSegmentTree();

            lsTree.Insert(new LineSegment(new Vector2(2, 0), new Vector2(3, 0)));
            lsTree.CheckInvariants();

            lsTree.Insert(new LineSegment(new Vector2(0, 0), new Vector2(1, 0)));
            lsTree.CheckInvariants();

            lsTree.Insert(new LineSegment(new Vector2(4, 0), new Vector2(5, 0)));
            lsTree.CheckInvariants();

            lsTree.Insert(new LineSegment(new Vector2(6, 0), new Vector2(7, 0)));
            lsTree.CheckInvariants();

            lsTree.Insert(new LineSegment(new Vector2(-1, 0), new Vector2(-1, -1)));
            lsTree.CheckInvariants();

            lsTree.Insert(new LineSegment(new Vector2(-3, 0), new Vector2(-2, 0)));
            lsTree.CheckInvariants();
        }

        [Test]
        public void Insert_Diagonals()
        {
            LineSegmentTree lsTree = new LineSegmentTree();

            LineSegment[] linesToAdd = new LineSegment[]
            {
                new LineSegment(new Vector2(0, 0), new Vector2(4, 4)),
                new LineSegment(new Vector2(2, 3), new Vector2(-2, -1)),
                new LineSegment(new Vector2(3, 2), new Vector2(1, 0)),
                new LineSegment(new Vector2(3, 1), new Vector2(4, 0)),
                new LineSegment(new Vector2(-2, 0), new Vector2(-3, 1)),
            };

            foreach (LineSegment line in linesToAdd)
            {
                lsTree.Insert(line);
                lsTree.CheckInvariants();
            }
        }

        #endregion

        #region GetLineSegmentToTheLeft

        [Test]
        public void GetLineSegmentToTheLeft_VerticalLines()
        {
            LineSegment[] lineSegments = new LineSegment[]
            {
                new LineSegment(new Vector2(0, 10), new Vector2(0, 0)),
                new LineSegment(new Vector2(2, 10), new Vector2(2, 0)),
                new LineSegment(new Vector2(4, 10), new Vector2(4, 0)),
            };
            
            LineSegmentTree lsTree = new LineSegmentTree();
            foreach (LineSegment ls in lineSegments)
            {
                lsTree.Insert(ls);
            }

            Assert.IsNull(lsTree.GetLineSegmentToTheLeft(new Vector2(-1, 8)));
            Assert.AreEqual(lineSegments[0], lsTree.GetLineSegmentToTheLeft(new Vector2(1, 10)));
            Assert.AreEqual(lineSegments[1], lsTree.GetLineSegmentToTheLeft(new Vector2(3, 0)));
            Assert.AreEqual(lineSegments[2], lsTree.GetLineSegmentToTheLeft(new Vector2(10, 1)));
        }

        [Test]
        public void GetLineSegmentToTheLeft_VariousLines()
        {
            LineSegment[] lineSegments = new LineSegment[]
            {
                new LineSegment(new Vector2(0, 8), new Vector2(8, 0)),
                new LineSegment(new Vector2(5, 6), new Vector2(4, 8)),
                new LineSegment(new Vector2(5, 5), new Vector2(8, 8)),
            };
            
            LineSegmentTree lsTree = new LineSegmentTree();
            foreach (LineSegment ls in lineSegments)
            {
                lsTree.Insert(ls);
            }

            Assert.IsNull(lsTree.GetLineSegmentToTheLeft(new Vector2(1, 6)));
            Assert.AreEqual(lineSegments[0], lsTree.GetLineSegmentToTheLeft(new Vector2(3, 8)));
            Assert.AreEqual(lineSegments[1], lsTree.GetLineSegmentToTheLeft(new Vector2(6, 7)));
            Assert.AreEqual(lineSegments[2], lsTree.GetLineSegmentToTheLeft(new Vector2(7, 6)));
        }

        [Test]
        public void GetLineSegmentToTheLeft_ParallelDiagonals()
        {
            LineSegment[] lineSegments = new LineSegment[]
            {
                new LineSegment(new Vector2(0, 8), new Vector2(8, 0)),
                new LineSegment(new Vector2(2, 8), new Vector2(10, 0)),
            };
            
            LineSegmentTree lsTree = new LineSegmentTree();
            foreach (LineSegment ls in lineSegments)
            {
                lsTree.Insert(ls);
            }

            Assert.IsNull(lsTree.GetLineSegmentToTheLeft(new Vector2(0, 7)));
            Assert.IsNull(lsTree.GetLineSegmentToTheLeft(new Vector2(7, 0)));
            Assert.AreEqual(lineSegments[0], lsTree.GetLineSegmentToTheLeft(new Vector2(1, 8)));
            Assert.AreEqual(lineSegments[0], lsTree.GetLineSegmentToTheLeft(new Vector2(9, 0)));
            Assert.AreEqual(lineSegments[1], lsTree.GetLineSegmentToTheLeft(new Vector2(3, 8)));
            Assert.AreEqual(lineSegments[1], lsTree.GetLineSegmentToTheLeft(new Vector2(11, 0)));
        }

        [Test]
        public void GetLineSegmentToTheLeft_DeepTree()
        {
            int numSegments = 15;
            LineSegmentTree lsTree = new LineSegmentTree();
            List<LineSegment> segments = new List<LineSegment>();
            for (int i = 0; i <= numSegments; i++)
            {
                LineSegment ls = new LineSegment(new Vector2(i, 0), new Vector2(i, 10));
                segments.Add(ls);
                lsTree.Insert(ls);
            }

            Assert.IsNull(lsTree.GetLineSegmentToTheLeft(new Vector2(-1, 5)));
            for (int i = 0; i <= numSegments; i++)
            {
                Assert.AreEqual(segments[i], lsTree.GetLineSegmentToTheLeft(new Vector2(i + 0.5f, 5)));
            }
        }

        [Test]
        public void GetLineSegmentToTheLeft_HorizontalLines()
        {
            LineSegment[] lineSegments = new LineSegment[]
            {
                new LineSegment(new Vector2(0, 0), new Vector2(1, 0)),
                new LineSegment(new Vector2(3, 0), new Vector2(4, 0)),
            };
            
            LineSegmentTree lsTree = new LineSegmentTree();
            foreach (LineSegment ls in lineSegments)
            {
                lsTree.Insert(ls);
            }

            Assert.IsNull(lsTree.GetLineSegmentToTheLeft(new Vector2(-1, 0)));
            Assert.AreEqual(lineSegments[0], lsTree.GetLineSegmentToTheLeft(new Vector2(2, 0)));
            Assert.AreEqual(lineSegments[1], lsTree.GetLineSegmentToTheLeft(new Vector2(5, 0)));
        }

        #endregion
    }
}