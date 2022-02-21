using System;
using System.Collections.Generic;

namespace PolygonDraw.Sample
{
    class PolygonDrawSample
    {
        static void Main(string[] args)
        {
            // Define loosely-connected line segments.
            float maxSeparation = 0.01f;

            List<LineSegment> lineSegments = new List<LineSegment>()
            {
                new LineSegment(new Vector2(2, 0), new Vector2(0, 0)),
                new LineSegment(new Vector2(0, 0), new Vector2(1, 2)),
                new LineSegment(new Vector2(1.016f, 1), new Vector2(2, 0)),
                new LineSegment(new Vector2(1, 1.991f), new Vector2(1.009f, 1)),
            };

            // Create polygons from line segments.
            PolygonArrangement arrangement =
                LineSegmentConnect.ConnectLineSegments(lineSegments, maxSeparation);
            
            Console.WriteLine($"Constructed from line segments: {arrangement}");

            // Create polygons.
            Polygon bigSquare = new Polygon(new List<Vector2>()
            {
                new Vector2(-2, -2), new Vector2(-2, 2), new Vector2(2, 2), new Vector2(2, -2),
            });
            Polygon smallSquare = new Polygon(new List<Vector2>()
            {
                new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1),
            });

            // Divide polygons into triangles.
            List<Triangle> triangles = Triangulation.Triangulate(
                polygons: new List<Polygon>() { bigSquare },
                holes: new List<Polygon>() { smallSquare });
            
            Console.WriteLine($"Triangles: {string.Join(",", triangles)}");

            Polygon overlappingSquare = new Polygon(new List<Vector2>()
            {
                new Vector2(0, 0), new Vector2(0, 3), new Vector2(3, 3), new Vector2(3, 0),
            });

            // Clip polygons with other polygons to produce smaller pieces.
            PolygonArrangement clipArrangement = bigSquare.ClipToPolygons(overlappingSquare);
            Console.WriteLine($"Clipped polygon: {clipArrangement}");
        }
    }
}