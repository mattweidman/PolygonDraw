using System;

namespace PolygonDraw
{
    /// <summary>
    /// Describes a node in a graph produced by intersecting two polygons.
    /// Can represent an intersection or a vertex.
    /// </summary>
    class IntersectionOrVertexNode
    {
        private readonly IntersectionData intersectionData;

        private readonly PolygonVertex polygonVertex;

        public readonly bool isHidden;

        private bool visitedViaSubject = false, visitedViaClip = false;

        public IntersectionOrVertexNode subjectNext, subjectPrev, clipNext, clipPrev;

        public bool isIntersection => intersectionData != null;

        public Vector2 point => isIntersection
            ? intersectionData.GetIntersectionPoint() : polygonVertex.vertex;

        public bool isStarter {
            get {
                if (this.isIntersection)
                {
                    return intersectionData.IsStarter();
                }

                if (this.polygonVertex.isHole)
                {
                    return false;
                }

                return !this.isHidden;
            }
        }

        public float subjectPriority => isIntersection
            ? intersectionData.poly1.edgeIndex + intersectionData.poly1.distanceAlongEdge
            : polygonVertex.vertexIndex;

        public float clipPriority => isIntersection
            ? intersectionData.poly2.edgeIndex + intersectionData.poly2.distanceAlongEdge
            : polygonVertex.vertexIndex;

        public IntersectionOrVertexNode(IntersectionData intersectionData)
        {
            this.intersectionData = intersectionData;
        }

        public IntersectionOrVertexNode(PolygonVertex polygonVertex, bool isHidden)
        {
            this.polygonVertex = polygonVertex;
            this.isHidden = isHidden;
        }

        public void AddSubjectEdge(IntersectionOrVertexNode next)
        {
            this.subjectNext = next;
            next.subjectPrev = this;
        }

        public void AddClipEdge(IntersectionOrVertexNode next)
        {
            this.clipNext = next;
            next.clipPrev = this;
        }

        public IntersectionOrVertexNode NextNode(IntersectionOrVertexNode prevNode = null)
        {
            if (this.intersectionData != null)
            {
                IntersectionType intersectionType = this.intersectionData.GetIntersectionType();
                if (intersectionType == IntersectionType.OVERLAPPING)
                {
                    return this.intersectionData.NextPoly2EdgeInsidePoly1() ?
                        this.clipNext : this.subjectNext;
                }
                else if (intersectionType == IntersectionType.POLY1_CONTAINS_POLY2)
                {
                    if (prevNode == null)
                    {
                        // For type POLY1_CONTAINS_POLY2, subjectNext and subjectPrev are not connected.
                        // If we start on this node, and we already visited via subject, then choose the
                        // edge that hasn't been traversed yet.
                        return this.visitedViaSubject ? this.subjectNext : this.clipNext;
                    }
                    
                    return prevNode == this.subjectPrev ? this.clipNext : this.subjectNext;
                }
                else if (intersectionType == IntersectionType.OUTER)
                {
                    return this.subjectNext;
                }
                else if (intersectionType == IntersectionType.SPLIT)
                {
                    return this.clipNext;
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Not expected to traverse a vertex of type {intersectionType}.");
                }
            }
            else
            {
                return this.polygonVertex.isHole ? this.clipNext : this.subjectNext;
            }
        }

        public override string ToString()
        {
            return this.point.ToString();
        }

        public void Visit(IntersectionOrVertexNode previous)
        {
            if (previous == this.subjectPrev)
            {
                this.visitedViaSubject = true;
            }
            else
            {
                this.visitedViaClip = true;
            }
        }

        public bool VisitableFrom(IntersectionOrVertexNode previous)
        {
            if (previous == this.subjectPrev)
            {
                return !this.visitedViaSubject;
            }
            
            return !this.visitedViaClip;
        }

        public bool CanStart()
        {
            // POLY1_CONTAINS_POLY2 is a special case because the vertex has two outgoing edges
            if (this.isIntersection &&
                this.intersectionData.GetIntersectionType() == IntersectionType.POLY1_CONTAINS_POLY2)
            {
                return !(this.visitedViaSubject && this.visitedViaClip);
            }

            return !(this.visitedViaSubject || this.visitedViaClip);
        }
    }
}