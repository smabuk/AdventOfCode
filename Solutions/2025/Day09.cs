using static AdventOfCode.Solutions._2025.Day09;

namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 09: Movie Theater
/// https://adventofcode.com/2025/day/09
/// </summary>
[Description("Movie Theater")]
public partial class Day09
{
	public static long Part1(string[] input)
		=> input
		.Select(Tile.Parse)
		.Combinations(2)
		.Max(tiles => tiles.Area());

	public static long Part2(string[] input)
	{
		Polygon polygon = Polygon.Build([.. input.Select(Tile.Parse)]);

		return polygon
			.Vertices
			.Combinations(2)
			.Where(tiles => IsRectangleValid(tiles[0], tiles[1], polygon, []))
			.Max(tiles => tiles.Area());
	}



	/// <summary>
	/// Determines whether a rectangle defined by two corner tiles is valid according to polygon edge constraints.
	/// </summary>
	/// <remarks>A rectangle is considered valid if all four of its edges either lie on the polygon edge or are
	/// entirely contained within the polygon. Both corner tiles must be specified; their order does not affect the
	/// result.</remarks>
	/// <param name="corner1">The first corner tile of the rectangle. Specifies one vertex of the rectangle to validate.</param>
	/// <param name="corner2">The second corner tile of the rectangle. Specifies the opposite vertex of the rectangle to validate.</param>
	/// <param name="polygon">The polygon to validate against.</param>
	/// <param name="pointCache">Cache for point-in-polygon checks.</param>
	/// <returns>true if the rectangle formed by the specified corners is valid; otherwise, false.</returns>
	private static bool IsRectangleValid(Tile corner1, Tile corner2, Polygon polygon, Dictionary<Point, bool> pointCache)
	{
		int minX = Math.Min(corner1.X, corner2.X);
		int maxX = Math.Max(corner1.X, corner2.X);
		int minY = Math.Min(corner1.Y, corner2.Y);
		int maxY = Math.Max(corner1.Y, corner2.Y);

		LineSegment topEdge = new(new Point(minX, minY), new Point(maxX, minY));
		LineSegment bottomEdge = new(new Point(minX, maxY), new Point(maxX, maxY));
		LineSegment leftEdge = new(new Point(minX, minY), new Point(minX, maxY));
		LineSegment rightEdge = new(new Point(maxX, minY), new Point(maxX, maxY));

		return IsEdgeValid(topEdge, polygon, pointCache)
			&& IsEdgeValid(bottomEdge, polygon, pointCache)
			&& IsEdgeValid(leftEdge, polygon, pointCache)
			&& IsEdgeValid(rightEdge, polygon, pointCache);
	}

	/// <summary>
	/// Determines whether the specified edge is valid with respect to the polygon's boundaries and interior.
	/// </summary>
	/// <remarks>An edge is considered valid if both endpoints are inside or on the polygon and the edge does not
	/// cross any polygon boundary except where it is collinear with a polygon edge. Collinear edges that overlap the
	/// polygon boundary are permitted.</remarks>
	/// <param name="edge">The line segment to validate. Both endpoints must be inside or on the polygon, and the edge must not improperly
	/// cross any polygon edge.</param>
	/// <param name="polygon">The polygon to validate against.</param>
	/// <param name="pointCache">Cache for point-in-polygon checks.</param>
	/// <returns>true if the edge is entirely within or on the polygon and does not cross any polygon edge; otherwise, false.</returns>
	private static bool IsEdgeValid(LineSegment edge, Polygon polygon, Dictionary<Point, bool> pointCache)
	{
		// Check if both endpoints are inside or on the polygon
		if (!IsPointInsideOrOnPolygon(edge.Start, polygon, pointCache)) {
			return false;
		}

		if (!IsPointInsideOrOnPolygon(edge.End, polygon, pointCache)) {
			return false;
		}

		// Check if the edge crosses any polygon edge improperly
		foreach (LineSegment polygonEdge in polygon.Edges) {
			if (AreSegmentsCollinear(edge, polygonEdge)) {
				continue;
			}

			// Check for non-collinear intersection (crossing)
			if (DoSegmentsIntersect(edge, polygonEdge)) {
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Determines whether two line segments intersect at a single point, excluding cases where the segments are collinear.
	/// </summary>
	/// <remarks>This method does not consider collinear segments as intersecting. To check for collinear overlap,
	/// use a separate method such as AreSegmentsCollinear.</remarks>
	/// <param name="seg1">The first line segment to test for intersection.</param>
	/// <param name="seg2">The second line segment to test for intersection.</param>
	/// <returns>true if the segments intersect at a single point; otherwise, false.</returns>
	private static bool DoSegmentsIntersect(LineSegment seg1, LineSegment seg2)
	{
		Point p1 = seg1.Start, p2 = seg1.End;
		Point p3 = seg2.Start, p4 = seg2.End;

		long o1 = CrossProduct(p1, p2, p3);
		long o2 = CrossProduct(p1, p2, p4);
		long o3 = CrossProduct(p3, p4, p1);
		long o4 = CrossProduct(p3, p4, p2);

		// General case: segments intersect if orientations are different
		if (((o1 > 0 && o2 < 0) || (o1 < 0 && o2 > 0)) &&
			((o3 > 0 && o4 < 0) || (o3 < 0 && o4 > 0))) {
			return true;
		}

		// Special cases: collinear points (handled separately by AreSegmentsCollinear check)
		return false;
	}

	/// <summary>
	/// Determines whether two line segments are collinear, meaning all endpoints lie on the same straight line.
	/// </summary>
	/// <remarks>This method checks whether all four endpoints of the provided segments are collinear. It does not
	/// verify whether the segments overlap or intersect; it only tests for collinearity.</remarks>
	/// <param name="seg1">The first line segment to evaluate for collinearity.</param>
	/// <param name="seg2">The second line segment to evaluate for collinearity.</param>
	/// <returns>true if both endpoints of seg2 are collinear with seg1; otherwise, false.</returns>
	private static bool AreSegmentsCollinear(LineSegment seg1, LineSegment seg2)
	{
		// Check if all four points are collinear
		long cross1 = CrossProduct(seg1.Start, seg1.End, seg2.Start);
		long cross2 = CrossProduct(seg1.Start, seg1.End, seg2.End);

		return cross1 == 0 && cross2 == 0;
	}

	/// <summary>
	/// Calculates the signed area of the parallelogram formed by three points in a two-dimensional plane.
	/// </summary>
	/// <remarks>This method is commonly used to determine the relative orientation of three points, such as in
	/// computational geometry algorithms for convex hulls or polygon winding. The magnitude of the result corresponds to
	/// twice the area of the triangle formed by the points.</remarks>
	/// <param name="p1">The first point, representing the origin of the vectors.</param>
	/// <param name="p2">The second point, representing the end of the first vector.</param>
	/// <param name="p3">The third point, representing the end of the second vector.</param>
	/// <returns>A signed 64-bit integer representing the cross product of the vectors defined by the points. A positive value
	/// indicates a counterclockwise turn, a negative value indicates a clockwise turn, and zero indicates collinearity.</returns>
	private static long CrossProduct(Point p1, Point p2, Point p3)
		=> ((p2.X - p1.X) * (long)(p3.Y - p1.Y)) - ((p2.Y - p1.Y) * (long)(p3.X - p1.X));

	/// <summary>
	/// Determines whether the specified point lies inside or on the boundary of the polygon.
	/// </summary>
	/// <remarks>This method considers a point to be inside the polygon if it is strictly within the area or exactly
	/// on any edge. Points that coincide with designated red tiles are also treated as inside. The result is cached for
	/// improved performance on repeated queries.</remarks>
	/// <param name="point">The point to test for inclusion within or on the polygon.</param>
	/// <param name="polygon">The polygon to validate against.</param>
	/// <param name="pointCache">Cache for point-in-polygon checks.</param>
	/// <returns>true if the point is inside the polygon or on its boundary; otherwise, false.</returns>
	private static bool IsPointInsideOrOnPolygon(Point point, Polygon polygon, Dictionary<Point, bool> pointCache)
	{
		// Check cache first
		if (pointCache.TryGetValue(point, out bool cached)) {
			return cached;
		}

		// Check if it's on any polygon edge
		foreach (LineSegment edge in polygon.Edges) {
			if (IsPointOnSegment(point, edge)) {
				pointCache[point] = true;
				return true;
			}
		}

		// Use ray casting to check if inside
		bool result = IsPointInsidePolygon(point, polygon);
		pointCache[point] = result;
		return result;
	}

	/// <summary>
	/// Determines whether a specified point lies exactly on a given line segment.
	/// </summary>
	/// <remarks>This method considers the point to be on the segment only if it is collinear with the segment and
	/// within the segment's bounding box. The comparison is exact; floating-point inaccuracies may affect results if used
	/// with non-integer coordinates.</remarks>
	/// <param name="point">The point to test for inclusion on the line segment.</param>
	/// <param name="segment">The line segment against which to test the point.</param>
	/// <returns>true if the point lies on the segment, including its endpoints; otherwise, false.</returns>
	private static bool IsPointOnSegment(Point point, LineSegment segment)
	{
		// Check collinearity using cross product
		long cross = CrossProduct(segment.Start, segment.End, point);
		if (cross != 0) {
			return false;
		}

		// Check if point is within bounding box
		int minX = Math.Min(segment.Start.X, segment.End.X);
		int maxX = Math.Max(segment.Start.X, segment.End.X);
		int minY = Math.Min(segment.Start.Y, segment.End.Y);
		int maxY = Math.Max(segment.Start.Y, segment.End.Y);

		return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
	}

	/// <summary>
	/// Determines whether the specified point lies within the polygon defined by the positions of the red tiles.
	/// </summary>
	/// <remarks>The polygon is formed by connecting the positions of all red tiles in order. The method uses the
	/// ray casting algorithm and assumes the polygon is simple (non-self-intersecting).</remarks>
	/// <param name="point">The point to test for inclusion within the polygon.</param>
	/// <returns>true if the point is inside the polygon; otherwise, false.</returns>
	private static bool IsPointInsidePolygon(Point point, Polygon polygon)
	{
		int intersections = 0;

		for (int i = 0; i < polygon.Vertices.Count; i++) {
			Point vertex1 = polygon.Vertices[i].Position;
			Point vertex2 = polygon.Vertices[(i + 1) % polygon.Vertices.Count].Position;

			// Check if ray from point going right intersects this edge
			if ((vertex1.Y > point.Y) != (vertex2.Y > point.Y)) {
				// Calculate X coordinate of intersection
				int intersectX = vertex1.X + ((point.Y - vertex1.Y) * (vertex2.X - vertex1.X) / (vertex2.Y - vertex1.Y));

				if (point.X < intersectX) {
					intersections++;
				}
			}
		}

		// Point is inside if odd number of intersections
		return (intersections % 2) == 1;
	}

	internal sealed record Polygon(List<Tile> Vertices, List<LineSegment> Edges);

	internal readonly record struct LineSegment(Point Start, Point End);

	[GenerateIParsable]
	internal sealed partial record Tile(Point Position)
	{
		public static Tile Parse(string s) => new(Point.Parse(s));
	}
}

file static partial class Day09TileExtensions
{
	extension(Tile tile)
	{
		public int X => tile.Position.X;
		public int Y => tile.Position.Y;

		public long Area(Tile tile2)
			=> ((long)Math.Abs(tile.X - tile2.X) + 1) * ((long)Math.Abs(tile.Y - tile2.Y) + 1);
	}

	extension(Tile[] tiles)
	{
		public long Area() => tiles[0].Area(tiles[1]);
	}

	extension(Polygon polygon)
	{
		public static Polygon Build(List<Tile> redTiles) => new([.. redTiles], [.. redTiles.Select((tile, i) => new LineSegment(tile.Position, redTiles[(i + 1) % redTiles.Count].Position))]);
	}
}
