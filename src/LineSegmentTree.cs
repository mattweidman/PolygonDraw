using System;

namespace PolygonDraw
{
    /// <summary>
    /// Red-black tree of line segments. Stores line segments so they are sorted
    /// horizontally. Assumes no lines intersecting.
    /// Following https://www.cs.cornell.edu/courses/cs3110/2009sp/lectures/lec11.html
    /// and https://www.cs.cornell.edu/courses/cs312/2004fa/lectures/lecture11.htm#deletion
    /// </summary>
    /// <typeparam name="TMetadata">Type of metadata to associate with line segments.</typeparam>
    public class LineSegmentTree<TMetadata>
    {
        private Node root;
        
        /// <summary>
        /// Insert a new line segment into the tree.
        /// </summary>
        public void Insert(LineSegment lineSegment, TMetadata metadata)
        {
            Node bubbledUpNode = this.InsertRecursive(lineSegment, this.root, metadata);

            // Root must be black.
            bubbledUpNode.isRed = false;
            this.root = bubbledUpNode;
        }

        private Node InsertRecursive(LineSegment newLineSegment, Node node, TMetadata metadata)
        {
            if (node == null)
            {
                return new Node(newLineSegment, metadata, true);
            }

            if (LineIsLeftOfLineSegment(newLineSegment, node.lineSegment))
            {
                node.left = this.InsertRecursive(newLineSegment, node.left, metadata);

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
                node.right = this.InsertRecursive(newLineSegment, node.right, metadata);

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

        #region Remove

        /// <summary>
        /// Remove a line segment from the tree.
        /// </summary>
        public void Remove(LineSegment lineSegment)
        {
            // Can't remove from empty tree
            if (this.root == null)
            {
                throw new ArgumentException($"Can't remove {lineSegment} from empty tree.");
            }

            Vector2 lowerPoint = lineSegment.GetLowerPoint();
            Node newRoot = this.RemoveRecursive(lineSegment, this.root);

            if (newRoot == null || newRoot.lineSegment == null)
            {
                this.root = null;
            }
            else
            {
                if (newRoot.isDoubleBlack)
                {
                    newRoot.color = NodeColor.BLACK;
                }

                this.root = newRoot;
            }
        }

        private Node RemoveRecursive(
            LineSegment lineSegment, Node node, Node nodeToSwap = null)
        {
            if (node == null)
            {
                throw new ArgumentException($"Line segment {lineSegment} not found in tree.");
            }

            // Stop when we get to the bottom and we have found a match
            if ((node.left == null || node.right == null) && 
                ((nodeToSwap != null && node.left == null) || lineSegment.Equals(node.lineSegment)))
            {
                if (nodeToSwap != null)
                {
                    // Swap values if found
                    node.SwapValuesWith(nodeToSwap);
                }

                // Remove this node
                Node nodeToReturn;
                if (node.left != null)
                {
                    nodeToReturn = node.left;
                }
                else if (node.right != null)
                {
                    nodeToReturn = node.right;
                }
                else
                {
                    nodeToReturn = null;
                }

                if (!node.isRed)
                {
                    // Convert to black or double-black to maintain black path length
                    if (nodeToReturn == null)
                    {
                        return new Node(null, NodeColor.DOUBLE_BLACK);
                    }
                    else if (nodeToReturn.isRed)
                    {
                        nodeToReturn.color = NodeColor.BLACK;
                        return nodeToReturn;
                    }
                    else
                    {
                        nodeToReturn.color = NodeColor.DOUBLE_BLACK;
                        return nodeToReturn;
                    }
                }
                else
                {
                    return nodeToReturn;
                }
            }

            // Choose whether to search left or right
            bool goLeft;
            if (nodeToSwap != null)
            {
                // If we found a node to swap already, find the leftmost node because
                // it will be the lowest node to the right of the swapped node.
                goLeft = true;
            }
            else if (lineSegment.Equals(node.lineSegment))
            {
                // If this node is the one we want to swap, go right once
                // before traversing left the rest of the path.
                nodeToSwap = node;
                goLeft = false;
            }
            else
            {
                // If we haven't found a node to swap with, keep searching.
                goLeft = LineIsLeftOfLineSegment(lineSegment, node.lineSegment, false);
            }

            // Recurse
            if (goLeft)
            {
                node.left = RemoveRecursive(lineSegment, node.left, nodeToSwap);
            }
            else
            {
                node.right = RemoveRecursive(lineSegment, node.right, nodeToSwap);
            }

            return RotateForDoubleBlack(node);
        }

        // See https://www.cs.cornell.edu/courses/cs312/2004fa/lectures/lecture11.htm#deletion
        private static Node RotateForDoubleBlack(Node node)
        {
            if (node.left != null && node.left.isDoubleBlack)
            {
                if (node.right == null)
                {
                    throw new InvariantFailedException(
                        $"Invariant failed: node.left={node.left}, but node.right=null.");
                }
                else if (node.right.isRed)
                {
                    // Case 1
                    Node newRoot = node.right;
                    newRoot.isRed = false;
                    Node newRight = node.right.right;

                    // Use other cases to construct the left
                    Node preliminaryNewLeft = node;
                    preliminaryNewLeft.isRed = true;
                    Node newLeftLeft = node.left;
                    Node newLeftRight = node.right.left;
                    preliminaryNewLeft.left = newLeftLeft;
                    preliminaryNewLeft.right = newLeftRight;
                    Node newLeft = RotateForDoubleBlack(preliminaryNewLeft);

                    newRoot.left = newLeft;
                    newRoot.right = newRight;
                    return newRoot;
                }
                else if (Node.IsNullOrBlack(node.right.left) && Node.IsNullOrBlack(node.right.right))
                {
                    // Case 2
                    node.color = node.isRed ? NodeColor.BLACK : NodeColor.DOUBLE_BLACK;
                    node.right.color = NodeColor.RED;

                    if (node.left.lineSegment == null)
                    {
                        // Make node.left null if empty
                        node.left = null;
                    }
                    else
                    {
                        node.left.color = NodeColor.BLACK;
                    }

                    return node;
                }
                else if (!Node.IsNullOrBlack(node.right.left) && Node.IsNullOrBlack(node.right.right))
                {
                    // Case 3
                    Node newRight = node.right.left;
                    newRight.isRed = false;
                    Node newRightRight = node.right;
                    newRightRight.isRed = true;
                    Node newRightRightRight = node.right.right; // could be null
                    Node newRightRightLeft = node.right.left.right; // could be null

                    node.right = newRight;
                    newRight.right = newRightRight;
                    newRightRight.left = newRightRightLeft;
                    newRightRight.right = newRightRightRight;

                    return RotateForDoubleBlack(node);
                }
                else if (!Node.IsNullOrBlack(node.right.right))
                {
                    // Case 4
                    Node newRoot = node.right;
                    newRoot.color = node.color;
                    Node newLeft = node;
                    newLeft.color = NodeColor.BLACK;
                    Node newRight = node.right.right;
                    newRight.color = NodeColor.BLACK;
                    Node newLeftRight = node.right.left;

                    Node newLeftLeft;
                    if (node.left.lineSegment == null)
                    {
                        // Make (what was) node.left null if empty
                        newLeftLeft = null;
                    }
                    else
                    {
                        newLeftLeft = node.left;
                        newLeftLeft.color = NodeColor.BLACK;
                    }

                    newRoot.left = newLeft;
                    newRoot.right = newRight;
                    newLeft.left = newLeftLeft;
                    newLeft.right = newLeftRight;

                    return newRoot;
                }
                else
                {
                    throw new InvariantFailedException(
                        $"Invalid structure while deleting at {node}."
                        + $"node.left={node.left}, node.right={node.right}.");
                }
            }
            else if (node.right != null && node.right.isDoubleBlack)
            {
                if (node.left == null)
                {
                    throw new InvariantFailedException(
                        $"Invariant failed: node.right={node.right}, but node.left=null.");
                }
                else if (node.left.isRed)
                {
                    // Case 1
                    Node newRoot = node.left;
                    newRoot.isRed = false;
                    Node newLeft = node.left.left;

                    // Use other cases to construct the left
                    Node preliminaryNewRight = node;
                    preliminaryNewRight.isRed = true;
                    Node newRightLeft = node.left.right;
                    Node newRightRight = node.right;
                    preliminaryNewRight.left = newRightLeft;
                    preliminaryNewRight.right = newRightRight;
                    Node newRight = RotateForDoubleBlack(preliminaryNewRight);

                    newRoot.left = newLeft;
                    newRoot.right = newRight;
                    return newRoot;
                }
                else if (Node.IsNullOrBlack(node.left.left) && Node.IsNullOrBlack(node.left.right))
                {
                    // Case 2
                    node.color = node.isRed ? NodeColor.BLACK : NodeColor.DOUBLE_BLACK;
                    node.left.color = NodeColor.RED;

                    if (node.right.lineSegment == null)
                    {
                        node.right = null;
                    }
                    else
                    {
                        node.right.color = NodeColor.BLACK;
                    }

                    return node;
                }
                else if (Node.IsNullOrBlack(node.left.left) && !Node.IsNullOrBlack(node.left.right))
                {
                    // Case 3
                    Node newLeft = node.left.right;
                    newLeft.isRed = false;
                    Node newLeftLeft = node.left;
                    newLeftLeft.isRed = true;
                    Node newLeftLeftLeft = node.left.left; // could be null
                    Node newLeftLeftRight = node.left.right.left; // could be null

                    node.left = newLeft;
                    newLeft.left = newLeftLeft;
                    newLeftLeft.left = newLeftLeftLeft;
                    newLeftLeft.right = newLeftLeftRight;

                    return RotateForDoubleBlack(node);
                }
                else if (!Node.IsNullOrBlack(node.left.left))
                {
                    // Case 4
                    Node newRoot = node.left;
                    newRoot.color = node.color;
                    Node newLeft = node.left.left;
                    newLeft.color = NodeColor.BLACK;
                    Node newRight = node;
                    newRoot.color = NodeColor.BLACK;
                    Node newRightLeft = node.left.right;
                    
                    Node newRightRight = node.right;
                    if (node.right.lineSegment == null)
                    {
                        newRightRight = null;
                    }
                    else
                    {
                        newRightRight = node.right;
                        newRightRight.color = NodeColor.BLACK;
                    }

                    newRoot.left = newLeft;
                    newRoot.right = newRight;
                    newRight.left = newRightLeft;
                    newRight.right = newRightRight;
                    
                    return newRoot;
                }
                else
                {
                    throw new InvariantFailedException(
                        $"Invalid structure while deleting at {node}."
                        + $"node.left={node.left}, node.right={node.right}.");
                }
            }
            else
            {
                return node;
            }
        }

        #endregion

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

            // No double-black nodes.
            if (node.color == NodeColor.DOUBLE_BLACK)
            {
                throw new InvariantFailedException($"{node} is double-black.");
            }

            // No null line segments.
            if (node.lineSegment == null)
            {
                throw new InvariantFailedException($"{node} has a null line segment.");
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
            else if (pos == LineSegment.HorizontalPosition.INTERSECTING_AT_ENDPOINT)
            {
                throw new ArgumentException($"Point {point} cannot be an endpoint of {lineSegment}.");
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
        /// <param name="compareTops">If true, compares at the y-coordinate of the top of the
        /// lower segment. If false, compares at the bottom of the higher segment.</param>
        private static bool LineIsLeftOfLineSegment(LineSegment ls1, LineSegment ls2, bool compareTops = true)
        {
            float compareLevel = compareTops
                ? MathF.Min(ls1.GetHigherPoint().y, ls2.GetHigherPoint().y)
                : MathF.Max(ls1.GetLowerPoint().y, ls2.GetLowerPoint().y);

            Vector2 ls1Point = FloatHelpers.Eq(ls1.p1.y, ls1.p2.y)
                ? ls1.p1 : ls1.GetPointAtY(compareLevel);

            if (ls1Point == null)
            {
                throw new InvalidOperationException($"Line {ls1} does not have a point at y={compareLevel}.");
            }

            LineSegment.HorizontalPosition pos = ls2.GetRelativeHorizontalPosition(ls1Point);

            if (pos == LineSegment.HorizontalPosition.LEFT)
            {
                return true;
            }
            else if (pos == LineSegment.HorizontalPosition.RIGHT)
            {
                return false;
            }
            else if (pos == LineSegment.HorizontalPosition.INTERSECTING_AT_ENDPOINT)
            {
                Vector2 other1 = ls1Point.Equals(ls1.p1) ? ls1.p2 : ls1.p1;
                Vector2 other2 = ls1Point.Equals(ls2.p1) ? ls2.p2 : ls2.p1;

                if (other1.Equals(other2))
                {
                    throw new ArgumentException($"Segments {ls1} and {ls2} are equal.");
                }

                bool other1AboveHori = FloatHelpers.Gt(other1.y, ls1Point.y);
                bool other2AboveHori = FloatHelpers.Gt(other2.y, ls1Point.y);

                if (other1AboveHori != other2AboveHori)
                {
                    LineSegment higherSegment = other1AboveHori ? ls1 : ls2;
                    LineSegment lowerSegment = other1AboveHori ? ls2 : ls1;
                    throw new ArgumentException(
                        $"Segment {higherSegment} needs to be removed before {lowerSegment} is added.");
                }

                float other1Angle = new Vector2(-1, 0).Angle(other1 - ls1Point);
                float other2Angle = new Vector2(-1, 0).Angle(other2 - ls1Point);

                return other1AboveHori
                    ? FloatHelpers.Lt(other1Angle, other2Angle)
                    : FloatHelpers.Lt(other2Angle, other1Angle);
            }
            else if (pos == LineSegment.HorizontalPosition.INTERSECTING)
            {
                throw new ArgumentException($"Segments {ls1} and {ls2} intersect at {ls1Point}.");
            }
            else
            {
                throw new ArgumentException($"Segments {ls1} and {ls2} do not share vertical range.");
            }
        }

        private class Node
        {
            public LineSegment lineSegment;

            public NodeColor color;

            public bool isRed
            {
                get { return color == NodeColor.RED; }
                set { this.color = value ? NodeColor.RED : NodeColor.BLACK; }
            }

            public bool isDoubleBlack => this.color == NodeColor.DOUBLE_BLACK;

            public Node left, right;

            public TMetadata metadata;

            public Node(LineSegment lineSegment, TMetadata metadata, bool isRed)
            {
                this.lineSegment = lineSegment;
                this.metadata = metadata;
                this.isRed = isRed;
            }

            public Node(LineSegment lineSegment, NodeColor color)
            {
                this.lineSegment = lineSegment;
                this.color = color;
            }

            public override string ToString()
            {
                return $"Node({this.lineSegment}, color={color})";
            }

            public void SwapValuesWith(Node other)
            {
                LineSegment temp = this.lineSegment;
                this.lineSegment = other.lineSegment;
                other.lineSegment = temp;
            }

            /// <summary>
            /// Convenience method that returns true if a node is null or colored black.
            /// </summary>
            public static bool IsNullOrBlack(Node node)
            {
                return node == null || !node.isRed;
            }

            /// <summary>
            /// Swap positions of left and right.
            /// </summary>
            public void ReverseNextRow()
            {
                Node temp = this.left;
                this.left = this.right;
                this.right = temp;
            }

            /// <summary>
            /// Reverse order of nodes in next row and the one below it.
            /// </summary>
            public void ReverseNextTwoRows()
            {
                this.ReverseNextRow();

                if (this.left != null)
                {
                    this.left.ReverseNextRow();
                }

                if (this.right != null)
                {
                    this.right.ReverseNextRow();
                }
            }
        }

        private enum NodeColor
        {
            RED,
            BLACK,
            DOUBLE_BLACK
        }

        public class InvariantFailedException : Exception
        {
            public InvariantFailedException(String message) : base(message) {}
        }
    }
}