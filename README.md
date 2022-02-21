# Polygon Draw

This library primarily lets you do three things:
1. Triangulation: Take any arbitrary set of polygons and completely-contained holes in polygons, and return a set of triangles that cover the entire area of polygons not covered by holes.
2. Clipping: Take two arbitrary polygons, a subject polygon and a clip polygon, and return a set of polygons that cover the entire area of the subject polygon that is not covered by the clip polygon.
3. Polygon construction: Take a set of line segments that approximately connect at their endpoints, and use them to construct polygons and holes.

## Triangulation

You should be able to triangulate any polygons with the following assumptions:
* Polygon vertices are represented in clockwise order.
* No two edges of a single polygon intersect.
* Any hole in a polygon is completely contained inside some other polygon.
* Polygons do not intersect or overlap.

The following scenarios are supported:
* Convex and concave polygons
* Holes in polygons

### Example usage
```
using PolygonDraw;
using System.Collections.Generic;
using System.Collections.Immutable;

// ...

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
ImmutableList<Triangle> triangles = Triangulation.Triangulate(
    polygons: new List<Polygon>() { bigSquare },
    holes: new List<Polygon>() { smallSquare });
```

## Clipping

You should be able to clip any polygons with the following assumptions:
* Polygon vertices are represented in clockwise order.
* No two edges of a single polygon intersect.

The following scenarios are supported:
* Convex and concave polygons
* Vertices and edges of separate polygons can intersect or overlap.

### Example usage
```
using PolygonDraw;
using System.Collections.Generic;
using System.Collections.Immutable;

// ...

// Create polygons.
Polygon bigSquare = new Polygon(new List<Vector2>()
{
    new Vector2(-2, -2), new Vector2(-2, 2), new Vector2(2, 2), new Vector2(2, -2),
});

Polygon overlappingSquare = new Polygon(new List<Vector2>()
{
    new Vector2(0, 0), new Vector2(0, 3), new Vector2(3, 3), new Vector2(3, 0),
});

// Clip polygons with other polygons to produce smaller pieces.
PolygonArrangement arrangement = bigSquare.ClipToPolygons(overlappingSquare);
ImmutableList<Polygon> polygonsCreated = arrangement.polygons;
ImmutableList<Polygon> holesCreated = arrangement.holes;
```

## Polygon construction from line segments

When constructing polygons from line segments, we make the following assumptions:
* For every line segment, if you stand on the first vertex and look toward the second vertex, the interior of the polygon it is part of is on your right. In other words, vertices are ordered clockwise for the outer perimeter of polygons, and they are ordered counterclockwise for holes.
* Second vertices of line segments always connect to first vertices of other line segments.
* We assume connected line segments are "close enough" to each other so that we can tell whether two vertices should be merged into one in the polygon. More specifically, callers define a value called "maxSeparation," which is the maximum distance the start vertex of one line segment and the end vertex of another line segment can be if they are to be connected in the polygon. If endpoints are farther than this distance away, it's possible that they could be incidentally connected in some situations, but there is no guarantee.

### Example usage
```
using PolygonDraw;
using System.Collections.Generic;
using System.Collections.Immutable;

// ...

float maxSeparation = 0.01f;

List<LineSegment> lineSegments = new List<LineSegment>()
{
    new LineSegment(new Vector2(2, 0), new Vector2(0, 0)),
    new LineSegment(new Vector2(0, 0), new Vector2(1, 2)),
    new LineSegment(new Vector2(1.016f, 1), new Vector2(2, 0)),
    new LineSegment(new Vector2(1, 1.991f), new Vector2(1.009f, 1)),
};

// A single quadrilateral is created.
PolygonArrangement arrangement =
    LineSegmentConnect.ConnectLineSegments(lineSegments, maxSeparation);
ImmutableList<Polygon> polygonsCreated = arrangement.polygons;
ImmutableList<Polygon> holesCreated = arrangement.holes;
```

## How to test

Run test cases: `dotnet test`

Run sample project: `dotnet run --project=sample/PolygonDrawSample.csproj`