using System.Collections.Generic;

namespace PolygonDraw
{
    /// <summary>
    /// Describes a node in a graph produced by intersecting two polygons.
    /// Can represent an intersection or a vertex.
    /// </summary>
    public class IntersectionOrVertexNode
    {
        private IntersectionData intersectionData;

        private PolygonVertex polygonVertex;

        private bool isHidden;

        // private int maxVisits => 
        //     (isIntersection &&
        //         intersectionData.GetIntersectionType() == IntersectionType.POLY1_CONTAINS_POLY2)
        //     ? 2 : 1;
        
        // private int visits = 0;

        // private int lastPolygonVisited = -1;

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
                    return prevNode == this.subjectPrev ? this.clipNext : this.subjectNext;
                }
                else
                {
                    return prevNode == this.clipPrev ? this.clipNext : this.subjectNext;
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

        public bool WasEverVisited()
        {
            return this.visitedViaSubject || this.visitedViaClip;
        }
    }
}