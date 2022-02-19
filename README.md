# Polygon Draw

This library primarily lets you do two things:
1. Triangulation: Take any arbitrary set of polygons and completely-contained holes in polygons, and return a set of triangles that cover the entire area of polygons not covered by holes.
2. Clipping: Take two arbitrary polygons, a subject polygon and a clip polygon, and return a set of polygons that cover the entire area of the subject polygon that is not covered by the clip polygon.

You should be able to triangulate and clip any arbitrary polygons. The assumptions this library makes are the following:
* Polygon vertices are represented in clockwise order.
* No two edges of a single polygon intersect.
* In triangulation, any hole in a polygon is completely contained inside some other polygon.
* In triangulation, polygons do not intersect or overlap.

The following scenarios are supported:
* Convex and concave polygons are supported in triangulation and clipping.
* For triangulation, holes in polygons are supported.
* For clipping, vertices and edges of separate polygons can intersect or overlap.

## How to test

`dotnet test`

## Sample usage

```
using PolygonDraw;
using System.Collections.Generic;

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
Polygon overlappingSquare = new Polygon(new List<Vector2>()
{
    new Vector2(0, 0), new Vector2(0, 3), new Vector2(3, 3), new Vector2(3, 0),
});

// Divide polygons into triangles.
List<Triangle> triangles = Triangulation.Triangulate(
    polygons: new List<Polygon>() { bigSquare },
    holes: new List<Polygon>() { smallSquare });

// Clip polygons with other polygons to produce smaller pieces.
PolygonArrangement arrangement = bigSquare.ClipToPolygons(overlappingSquare);
List<Polygon> polygonsCreated = arrangement.polygons;
List<Polygon> holesCreated = arrangement.holes;
```