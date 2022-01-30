using NUnit.Framework;
using PolygonDraw;

namespace PolygonDrawTests
{
    public class LineSegmentTreeTests
    {
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
    }
}