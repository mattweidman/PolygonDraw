using System;

namespace PolygonDraw
{
    /// <summary>
    /// Red-black tree of line segments. Stores line segments so they are sorted
    /// horizontally. Assumes no lines intersecting.
    /// </summary>
    public class LineSegmentTree
    {
        private Node root;

        public void Insert(LineSegment lineSegment)
        {
            if (this.root == null)
            {
                this.root = new Node(lineSegment, false);
                return;
            }

            // TODO
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
            if (node.left != null && !PointIsLeftOfLineSegment(node.left.GetHighPoint(), node.lineSegment))
            {
                throw new InvariantFailedException(
                    $"Node {node} has a left child {node.left} that is not to its left.");
            }

            // Right should be higher.
            if (node.right != null && PointIsLeftOfLineSegment(node.right.GetHighPoint(), node.lineSegment))
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

            public Vector2 GetHighPoint()
            {
                return lineSegment.p1.y > lineSegment.p2.y ? lineSegment.p1 : lineSegment.p2;
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