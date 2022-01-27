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
            if (node.left != null && !this.PointIsLeftOfLineSegment(node.left.GetHighPoint(), node.lineSegment))
            {
                throw new InvariantFailedException(
                    $"Node {node} has a left child {node.left} that is not to its left.");
            }

            // Right should be higher.
            if (node.right != null && this.PointIsLeftOfLineSegment(node.right.GetHighPoint(), node.lineSegment))
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
        private bool PointIsLeftOfLineSegment(Vector2 point, LineSegment lineSegment)
        {
            // Vector2 epLow, epHigh;
            // if (FloatHelpers.Lt(lineSegment.p1.y, lineSegment.p2.y))
            // {
            //     epLow = lineSegment.p1;
            //     epHigh = lineSegment.p2;
            // }
            // else
            // {
            //     epLow = lineSegment.p2;
            //     epHigh = lineSegment.p1;
            // }

            // if (FloatHelpers.Lt(point.y, epLow.y) || FloatHelpers.Gt(point.y, epHigh.y))
            // {
            //     throw new ArgumentException(
            //         $"Point {point} is not within the y-coordinate range of line segment {lineSegment}.");
            // }

            // if (lineSegment.IntersectsPoint(point))
            // {
            //     throw new ArgumentException($"Point {point} can not be on line segment {lineSegment}.");
            // }

            // if (FloatHelpers.Lt(point.x, epLow.x) && FloatHelpers.Lt(point.x, epHigh.x))
            // {
            //     return true;
            // }

            // if (FloatHelpers.Gt(point.x, epLow.x) && FloatHelpers.Gt(point.x, epHigh.x))
            // {
            //     return false;
            // }

            if (FloatHelpers.Eq(lineSegment.p1.x, lineSegment.p2.x))
            {
                return FloatHelpers.Lt(point.x, MathF.Min(lineSegment.p1.x, lineSegment.p2.x));
            }

            return FloatHelpers.Lt(
                (point.x - lineSegment.p1.x) * (lineSegment.p2.y - lineSegment.p1.y),
                (point.y - lineSegment.p1.y) * (lineSegment.p2.x - lineSegment.p1.x));
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