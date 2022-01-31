using NUnit.Framework;
using PolygonDraw;
using System;
using System.Collections.Generic;
using System.Linq;

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

        #region Remove

        [Test]
        public void Remove_OneNode()
        {
            TestRemoveSimple(new int[] {0}, new int[] {0});
        }

        [Test]
        public void Remove_TwoNodes_RemoveRoot()
        {
            TestRemoveSimple(new int[] {0, 1}, new int[] {0, 1});
        }

        [Test]
        public void Remove_TwoNodes_RemoveLeft()
        {
            TestRemoveSimple(new int[] {0, -1}, new int[] {1, 0});
        }

        [Test]
        public void Remove_TwoNodes_RemoveRight()
        {
            TestRemoveSimple(new int[] {0, 1}, new int[] {1, 0});
        }

        [Test]
        public void Remove_ThreeNodes_RemoveLeftFirst()
        {
            TestRemoveSimple(new int[] {0, -1, 1}, new int[] {1, 2, 0});
        }

        [Test]
        public void Remove_ThreeNodes_RemoveRightFirst()
        {
            TestRemoveSimple(new int[] {0, -1, 1}, new int[] {2, 1, 0});
        }

        [Test]
        public void Remove_FourNodes_InsertLeftLeft_RemoveLeftLeft()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 1}, new int[] {3, 2, 1, 0});
        }

        [Test]
        public void Remove_FourNodes_InsertLeftLeft_RemoveLeft()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 1}, new int[] {2, 3, 1, 0});
        }

        [Test]
        public void Remove_FourNodes_InsertLeftLeft_RemoveRight()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 1}, new int[] {0, 1, 2, 3});
        }

        [Test]
        public void Remove_FourNodes_InsertLeftRight_RemoveLeftRight()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 4}, new int[] {3, 2, 0, 1});
        }

        [Test]
        public void Remove_FourNodes_InsertLeftRight_RemoveLeft()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 4}, new int[] {2, 3, 0, 1});
        }

        [Test]
        public void Remove_FourNodes_InsertLeftRight_RemoveRight()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 4}, new int[] {0, 1, 2, 3});
        }

        [Test]
        public void Remove_FourNodes_InsertRightLeft_RemoveRightLeft()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 6}, new int[] {3, 2, 1, 0});
        }

        [Test]
        public void Remove_FourNodes_InsertRightLeft_RemoveRight()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 6}, new int[] {0, 3, 2, 1});
        }

        [Test]
        public void Remove_FourNodes_InsertRightLeft_RemoveLeft()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 6}, new int[] {2, 0, 1, 3});
        }

        [Test]
        public void Remove_FourNodes_InsertRightRight_RemoveRightRight()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 8}, new int[] {3, 0, 2, 1});
        }

        [Test]
        public void Remove_FourNodes_InsertRightRight_RemoveRight()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 8}, new int[] {0, 3, 1, 2});
        }

        [Test]
        public void Remove_FourNodes_InsertRightRight_RemoveLeft()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 8}, new int[] {2, 1, 3, 0});
        }

        [Test]
        public void Remove_FiveNodes_LeftFilled_RemoveBottom()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 2, 4}, new int[] {3, 4});
        }

        [Test]
        public void Remove_FiveNodes_LeftFilled_RemoveRight()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 2, 4}, new int[] {0});
        }

        [Test]
        public void Remove_FiveNodes_RightFilled_RemoveBottom()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 6, 8}, new int[] {3, 4});
        }

        [Test]
        public void Remove_FiveNodes_RightFilled_RemoveLeft()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 6, 8}, new int[] {2});
        }

        [Test]
        public void Remove_FiveNodes_OuterFilled()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 2, 8}, new int[] {3, 4});
        }

        [Test]
        public void Remove_FiveNodes_InnerFilled()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 4, 6}, new int[] {3, 4});
        }

        [Test]
        public void Remove_FillThreeRows_RemoveRightThenRoot()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 2, 4, 6, 8}, new int[] {0, 1, 3, 5, 2, 4, 6});
        }

        [Test]
        public void Remove_FillThreeRows_RemoveRootThenRight()
        {
            TestRemoveSimple(new int[] {7, 5, 3, 2, 4, 6, 8}, new int[] {1, 0, 2, 3, 4, 5, 6});
        }

        [Test]
        public void Remove_15Nodes()
        {
            TestRemoveForNNodes(15, 7, 7);
        }

        [Test]
        public void Remove_50Nodes()
        {
            TestRemoveForNNodes(50, 7, 7);
        }

        [Test]
        public void Remove_100Nodes_RemoveIncreasing()
        {
            TestRemoveForNNodes(100, 0, 1);
        }

        [Test]
        public void Remove_100Nodes_RemoveDecreasing()
        {
            TestRemoveForNNodes(100, 99, 99);
        }

        private static void TestRemoveSimple(int[] xCoordinates, int[] removalOrder)
        {
            LineSegment[] lineSegments = xCoordinates
                .Select(x => new LineSegment(new Vector2(x, 0), new Vector2(x, 1)))
                .ToArray();

            LineSegmentTree lsTree = new LineSegmentTree();
            foreach (LineSegment ls in lineSegments)
            {
                lsTree.Insert(ls);
            }

            foreach (int idx in removalOrder)
            {
                try
                {
                    lsTree.Remove(lineSegments[idx]);
                    lsTree.CheckInvariants();
                }
                catch (Exception)
                {
                    Console.Error.WriteLine(
                        "TestRemoveSimple: Failed while removing "
                        + $"line segment {lineSegments[idx]}, idx={idx}.");
                    
                    throw;
                }
            }
        }

        private static void TestRemoveForNNodes(int n, int firstRemoved, int indexJump)
        {
            int[] xCoords = new int[n].Select((_, i) => i).ToArray();
            int[] removalOrder = xCoords.Select((_, i) => (firstRemoved + i * indexJump) % n).ToArray();
            TestRemoveSimple(xCoords, removalOrder);
        }

        #endregion

        [Test]
        public void InsertAndRemove_VariousHeights()
        {
            LineSegment[] lineSegments = new LineSegment[]
            {
                new LineSegment(new Vector2(0, 0), new Vector2(0, 5)),
                new LineSegment(new Vector2(1, 1), new Vector2(1, 4)),
                new LineSegment(new Vector2(2, -1), new Vector2(2, 3)),
            };

            LineSegmentTree lsTree = new LineSegmentTree();

            lsTree.Insert(lineSegments[0]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[1]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[2]);
            lsTree.CheckInvariants();

            lsTree.Remove(lineSegments[1]);
            lsTree.CheckInvariants();
            lsTree.Remove(lineSegments[0]);
            lsTree.CheckInvariants();
            lsTree.Remove(lineSegments[2]);
            lsTree.CheckInvariants();
        }

        [Test]
        public void InsertAndRemove_V()
        {
            LineSegment[] lineSegments = new LineSegment[]
            {
                new LineSegment(new Vector2(0, 0), new Vector2(3, 3)),
                new LineSegment(new Vector2(3, 3), new Vector2(6, 0)),
            };

            LineSegmentTree lsTree = new LineSegmentTree();

            lsTree.Insert(lineSegments[0]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[1]);
            lsTree.CheckInvariants();

            Assert.IsNull(lsTree.GetLineSegmentToTheLeft(new Vector2(0, 1)));
            Assert.AreEqual(lineSegments[0], lsTree.GetLineSegmentToTheLeft(new Vector2(2, 1)));
            Assert.AreEqual(lineSegments[0], lsTree.GetLineSegmentToTheLeft(new Vector2(3, 1)));
            Assert.AreEqual(lineSegments[0], lsTree.GetLineSegmentToTheLeft(new Vector2(4, 1)));
            Assert.AreEqual(lineSegments[1], lsTree.GetLineSegmentToTheLeft(new Vector2(6, 1)));

            lsTree.Remove(lineSegments[0]);
            lsTree.CheckInvariants();
            lsTree.Remove(lineSegments[1]);
            lsTree.CheckInvariants();

            Assert.IsNull(lsTree.GetLineSegmentToTheLeft(new Vector2(6, 1)));
        }

        [Test]
        public void InsertAndRemove_A()
        {
            LineSegment[] lineSegments = new LineSegment[]
            {
                new LineSegment(new Vector2(0, 3), new Vector2(3, 0)),
                new LineSegment(new Vector2(3, 0), new Vector2(6, 3)),
            };

            LineSegmentTree lsTree = new LineSegmentTree();

            lsTree.Insert(lineSegments[0]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[1]);
            lsTree.CheckInvariants();

            Assert.IsNull(lsTree.GetLineSegmentToTheLeft(new Vector2(0, 2)));
            Assert.AreEqual(lineSegments[0], lsTree.GetLineSegmentToTheLeft(new Vector2(2, 2)));
            Assert.AreEqual(lineSegments[0], lsTree.GetLineSegmentToTheLeft(new Vector2(3, 2)));
            Assert.AreEqual(lineSegments[0], lsTree.GetLineSegmentToTheLeft(new Vector2(4, 2)));
            Assert.AreEqual(lineSegments[1], lsTree.GetLineSegmentToTheLeft(new Vector2(6, 2)));

            lsTree.Remove(lineSegments[0]);
            lsTree.CheckInvariants();
            lsTree.Remove(lineSegments[1]);
            lsTree.CheckInvariants();

            Assert.IsNull(lsTree.GetLineSegmentToTheLeft(new Vector2(6, 1)));
        }

        [Test]
        public void InsertAndRemove_Shape()
        {
            LineSegment[] lineSegments = new LineSegment[]
            {
                new LineSegment(new Vector2(2, 4), new Vector2(3, 2)),
                new LineSegment(new Vector2(3, 2), new Vector2(4, 3)),
                new LineSegment(new Vector2(4, 3), new Vector2(6, 4)),
                new LineSegment(new Vector2(6, 4), new Vector2(7, 3)),
                new LineSegment(new Vector2(7, 3), new Vector2(5, 2)),
                new LineSegment(new Vector2(5, 2), new Vector2(6, 1)),
                new LineSegment(new Vector2(6, 1), new Vector2(5, 0)),
                new LineSegment(new Vector2(5, 0), new Vector2(3, 1)),
                new LineSegment(new Vector2(3, 1), new Vector2(1, 0)),
                new LineSegment(new Vector2(1, 0), new Vector2(0, 1)),
                new LineSegment(new Vector2(0, 1), new Vector2(0, 2)),
                new LineSegment(new Vector2(0, 2), new Vector2(2, 4)),
            };

            LineSegmentTree lsTree = new LineSegmentTree();

            // y=4
            lsTree.Insert(lineSegments[0]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[2]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[11]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[3]);
            lsTree.CheckInvariants();

            // y=3
            lsTree.Remove(lineSegments[2]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[1]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[4]);
            lsTree.CheckInvariants();

            // y=2
            lsTree.Remove(lineSegments[0]);
            lsTree.CheckInvariants();
            lsTree.Remove(lineSegments[1]);
            lsTree.CheckInvariants();
            lsTree.Remove(lineSegments[11]);
            lsTree.CheckInvariants();
            lsTree.Remove(lineSegments[3]);
            lsTree.CheckInvariants();
            lsTree.Remove(lineSegments[4]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[10]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[5]);
            lsTree.CheckInvariants();

            // y=1
            lsTree.Remove(lineSegments[10]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[9]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[8]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[7]);
            lsTree.CheckInvariants();
            lsTree.Insert(lineSegments[6]);
            lsTree.CheckInvariants();

            // y=0
            lsTree.Remove(lineSegments[9]);
            lsTree.CheckInvariants();
            lsTree.Remove(lineSegments[8]);
            lsTree.CheckInvariants();
            lsTree.Remove(lineSegments[7]);
            lsTree.CheckInvariants();
            lsTree.Remove(lineSegments[6]);
            lsTree.CheckInvariants();
        }
    }
}