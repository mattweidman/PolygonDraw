using System;

namespace PolygonDraw
{
    /// <summary>
    /// Red-black tree of line segments. Stores line segments so they are sorted
    /// horizontally. Assumes no lines intersecting.
    /// Following https://www.cs.cornell.edu/courses/cs3110/2009sp/lectures/lec11.html
    /// </summary>
    public class LineSegmentTree
    {
        private Node root;
        
        /// <summary>
        /// Insert a new line segment into the tree.
        /// </summary>
        public void Insert(LineSegment lineSegment)
        {
            Vector2 higherPoint = lineSegment.GetHigherPoint();

            Node bubbledUpNode = this.InsertRecursive(higherPoint, lineSegment, this.root);

            // Root must be black.
            bubbledUpNode.isRed = false;
            this.root = bubbledUpNode;
        }

        private Node InsertRecursive(Vector2 higherPoint, LineSegment newLineSegment, Node node)
        {
            if (node == null)
            {
                return new Node(newLineSegment, true);
            }

            if (PointIsLeftOfLineSegment(higherPoint, node.lineSegment))
            {
                node.left = this.InsertRecursive(higherPoint, newLineSegment, node.left);

                if (node.left.isRed && (node.left.left != null && node.left.left.isRed))
                {
                    Node newRoot = node.left;
                    Node newLeft = node.left.left;
                    Node newRight = node;

                    newRight.left = newRoot.right;
                    newRoot.right = newRight;

                    newLeft.isRed = false;

                    return newRoot;
                }
                else if (node.left.isRed && (node.left.right != null && node.left.right.isRed))
                {
                    Node newRoot = node.left.right;
                    Node newLeft = node.left;
                    Node newRight = node;

                    newLeft.right = newRoot.left;
                    newRight.left = newRoot.right;
                    newRoot.left = newLeft;
                    newRoot.right = newRight;

                    newLeft.isRed = false;

                    return newRoot;
                }
            }
            else
            {
                node.right = this.InsertRecursive(higherPoint, newLineSegment, node.right);

                if (node.right.isRed && (node.right.left != null && node.right.left.isRed))
                {
                    Node newRoot = node.right.left;
                    Node newLeft = node;
                    Node newRight = node.right;

                    newRight.left = newRoot.right;
                    newLeft.right = newRoot.left;
                    newRoot.left = newLeft;
                    newRoot.right = newRight;

                    newRight.isRed = false;

                    return newRoot;
                }
                else if (node.right.isRed && (node.right.right != null && node.right.right.isRed))
                {
                    Node newRoot = node.right;
                    Node newLeft = node;
                    Node newRight = node.right.right;

                    newLeft.right = newRoot.left;
                    newRoot.left = newLeft;

                    newRight.isRed = false;

                    return newRoot;
                }
            }

            return node;
        }

        /// <summary>
        /// Get the line segment to the left of point. If point is to the left
        /// of all line segments in this tree, return null.
        /// </summary>
        public LineSegment GetLineSegmentToTheLeft(Vector2 point)
        {
            return GetLineSegmentToTheLeftRecursive(point, this.root);
        }

        private LineSegment GetLineSegmentToTheLeftRecursive(Vector2 point, Node node)
        {
            if (node == null)
            {
                return null;
            }

            LineSegment leftmostSegment = null;
            if (PointIsLeftOfLineSegment(point, node.lineSegment))
            {
                leftmostSegment = GetLineSegmentToTheLeftRecursive(point, node.left);
            }
            else
            {
                leftmostSegment = GetLineSegmentToTheLeftRecursive(point, node.right);

                if (leftmostSegment == null)
                {
                    leftmostSegment = node.lineSegment;
                }
            }

            return leftmostSegment;
        }

        public void CheckInvariants()
        {
            if (this.root == null)
            {
                return;
            }

            if (this.root.isRed)
            {
                throw new InvariantFailedException("Root must be black.");
            }

            this.CheckInvariantsRecursive(this.root);
        }

        /// <param name="node">Node traversed.</param>
        /// <returns>Number of black nodes from node to leaves.</returns>
        private int CheckInvariantsRecursive(Node node)
        {
            // Leaf
            if (node == null)
            {
                return 1;
            }

            // Red nodes cannot have red children.
            if (node.isRed)
            {
                if (node.left != null && node.left.isRed)
                {
                    throw new InvariantFailedException($"Red node {node} has a red child {node.left}.");
                }
                if (node.right != null && node.right.isRed)
                {
                    throw new InvariantFailedException($"Red node {node} has a red child {node.right}.");
                }
            }

            // Left should be lower.
            if (node.left != null && !LineIsLeftOfLineSegment(node.left.lineSegment, node.lineSegment))
            {
                throw new InvariantFailedException(
                    $"Node {node} has a left child {node.left} that is not to its left.");
            }

            // Right should be higher.
            if (node.right != null && LineIsLeftOfLineSegment(node.right.lineSegment, node.lineSegment))
            {
                throw new InvariantFailedException(
                    $"Node {node} has a right child {node.right} that is not to its right.");
            }

            int leftBlackNodes = this.CheckInvariantsRecursive(node.left);
            int rightBlackNodes = this.CheckInvariantsRecursive(node.right);

            // Compare number of black nodes on left and right.
            if (leftBlackNodes != rightBlackNodes)
            {
                throw new InvariantFailedException($"Unequal number of black nodes under {node}. "
                    + $"Left: {leftBlackNodes} nodes, Right: {rightBlackNodes} nodes.");
            }

            return node.isRed ? leftBlackNodes : leftBlackNodes + 1;
        }

        /// <summary>
        /// Whether a point can be considered "left" of a line.
        /// </summary>
        private static bool PointIsLeftOfLineSegment(Vector2 point, LineSegment lineSegment)
        {
            LineSegment.HorizontalPosition pos = lineSegment.GetRelativeHorizontalPosition(point);

            if (pos == LineSegment.HorizontalPosition.LEFT)
            {
                return true;
            }
            else if (pos == LineSegment.HorizontalPosition.RIGHT)
            {
                return false;
            }
            else if (pos == LineSegment.HorizontalPosition.INTERSECTING)
            {
                throw new ArgumentException($"Point {point} cannot be on line segment {lineSegment}.");
            }
            else
            {
                throw new ArgumentException(
                    $"Point {point} is not within the y-coordinate range of line segment {lineSegment}.");
            }
        }

        /// <summary>
        /// Whether ls1 should be considered left of ls2.
        /// </summary>
        private static bool LineIsLeftOfLineSegment(LineSegment ls1, LineSegment ls2)
        {
            Vector2 ls1Max = ls1.GetHigherPoint();
            Vector2 ls2Max = ls2.GetHigherPoint();
            float compareLevel = MathF.Min(ls1Max.y, ls2Max.y);

            Vector2 ls1Point = FloatHelpers.Eq(ls1.p1.y, ls1.p2.y)
                ? ls1.p1 : ls1.GetPointAtY(compareLevel);

            if (ls1Point == null)
            {
                throw new InvalidOperationException($"Line {ls1} does not have a point at y={compareLevel}.");
            }

            return PointIsLeftOfLineSegment(ls1Point, ls2);
        }

        private class Node
        {
            public LineSegment lineSegment;

            public bool isRed;

            public Node left, right;

            public Node(LineSegment lineSegment, bool isRed)
            {
                this.lineSegment = lineSegment;
                this.isRed = isRed;
            }

            public override string ToString()
            {
                return $"Node({this.lineSegment}, isRed={isRed})";
            }
        }

        public class InvariantFailedException : Exception
        {
            public InvariantFailedException(String message) : base(message) {}
        }
    }
}