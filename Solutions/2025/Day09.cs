using static AdventOfCode.Solutions._2025.Day09;

namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 09: Movie Theater
/// https://adventofcode.com/2025/day/09
/// </summary>
[Description("Movie Theater")]
[GenerateVisualiser]
public partial class Day09
{

	[Init]
	public static void LoadTiles(string[] input)
	{
		_redTiles = [.. input.As<Tile>()];
		_redTileSet = [.. _redTiles.Select(t => t.Position)];
	}

	private static List<Tile> _redTiles = [];
	private static HashSet<Point> _redTileSet = [];
	private static readonly List<LineSegment> _polygonEdges = [];
	private static readonly Dictionary<Point, bool> _pointCache = [];

	public static long Part1() => _redTiles.Combinations(2).Max(tiles => tiles.Area());

	public static long Part2()
	{
		// Build polygon edges as line segments
		for (int i = 0; i < _redTiles.Count; i++) {
			Point start = _redTiles[i].Position;
			Point end = _redTiles[(i + 1) % _redTiles.Count].Position;
			_polygonEdges.Add(new LineSegment(start, end));
		}

		long maximumArea = 0;

		// Try all pairs of red tiles as potential opposite corners
		for (int i = 0; i < _redTiles.Count; i++) {
			for (int j = i + 1; j < _redTiles.Count; j++) {
				Tile tile1 = _redTiles[i];
				Tile tile2 = _redTiles[j];

				// Skip if they can't form a proper rectangle (need different X and Y)
				if (tile1.X == tile2.X || tile1.Y == tile2.Y) {
					continue;
				}

				// Check if rectangle is valid by checking its four edges
				if (IsRectangleValid(tile1, tile2)) {
					long area = tile1.Area(tile2);
					if (area > maximumArea) {
						maximumArea = area;
					}
				}
			}
		}

		return maximumArea;
	}

	/// <summary>
	/// Checks if a rectangle is valid by verifying all four edges are inside or on the polygon
	/// </summary>
	private static bool IsRectangleValid(Tile corner1, Tile corner2)
	{
		int minX = Math.Min(corner1.X, corner2.X);
		int maxX = Math.Max(corner1.X, corner2.X);
		int minY = Math.Min(corner1.Y, corner2.Y);
		int maxY = Math.Max(corner1.Y, corner2.Y);

		// Create the four edges of the rectangle
		LineSegment topEdge = new(new Point(minX, minY), new Point(maxX, minY));
		LineSegment bottomEdge = new(new Point(minX, maxY), new Point(maxX, maxY));
		LineSegment leftEdge = new(new Point(minX, minY), new Point(minX, maxY));
		LineSegment rightEdge = new(new Point(maxX, minY), new Point(maxX, maxY));

		// Check if all four edges are valid (lie on polygon edge or entirely inside polygon)
		return IsEdgeValid(topEdge) && IsEdgeValid(bottomEdge) && IsEdgeValid(leftEdge) && IsEdgeValid(rightEdge);
	}

	/// <summary>
	/// Checks if a rectangle edge is valid
	/// An edge is valid if its endpoints are inside/on the polygon and it doesn't cross outside
	/// </summary>
	private static bool IsEdgeValid(LineSegment edge)
	{
		// Check if both endpoints are inside or on the polygon
		if (!IsPointInsideOrOnPolygon(edge.Start)) {
			return false;
		}

		if (!IsPointInsideOrOnPolygon(edge.End)) {
			return false;
		}

		// Check if the edge crosses any polygon edge improperly
		foreach (LineSegment polygonEdge in _polygonEdges) {
			// If segments are collinear, they might overlap which is fine (edge on polygon boundary)
			if (AreSegmentsCollinear(edge, polygonEdge)) {
				continue;
			}

			// Check for non-collinear intersection (crossing)
			if (DoSegmentsIntersect(edge, polygonEdge)) {
				// Non-collinear intersection means edge crosses the polygon boundary
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Checks if two line segments intersect (using orientation method)
	/// </summary>
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
	/// Checks if two segments are collinear (lie on the same infinite line)
	/// </summary>
	private static bool AreSegmentsCollinear(LineSegment seg1, LineSegment seg2)
	{
		// Check if all four points are collinear
		long cross1 = CrossProduct(seg1.Start, seg1.End, seg2.Start);
		long cross2 = CrossProduct(seg1.Start, seg1.End, seg2.End);

		return cross1 == 0 && cross2 == 0;
	}

	/// <summary>
	/// Calculates the cross product of vectors (p2-p1) and (p3-p1)
	/// </summary>
	private static long CrossProduct(Point p1, Point p2, Point p3)
	{
		return ((long)(p2.X - p1.X) * (long)(p3.Y - p1.Y)) - ((long)(p2.Y - p1.Y) * (long)(p3.X - p1.X));
	}

	/// <summary>
	/// Checks if a point is inside or on the polygon (with caching)
	/// </summary>
	private static bool IsPointInsideOrOnPolygon(Point point)
	{
		// Check cache first
		if (_pointCache.TryGetValue(point, out bool cached)) {
			return cached;
		}

		// Check if it's a red tile
		if (_redTileSet.Contains(point)) {
			_pointCache[point] = true;
			return true;
		}

		// Check if it's on any polygon edge
		foreach (LineSegment edge in _polygonEdges) {
			if (IsPointOnSegment(point, edge)) {
				_pointCache[point] = true;
				return true;
			}
		}

		// Use ray casting to check if inside
		bool result = IsPointInsidePolygon(point);
		_pointCache[point] = result;
		return result;
	}

	/// <summary>
	/// Checks if a point lies on a line segment
	/// </summary>
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
	/// Uses ray casting algorithm to determine if a point is inside the polygon
	/// </summary>
	private static bool IsPointInsidePolygon(Point point)
	{
		int intersections = 0;

		for (int i = 0; i < _redTiles.Count; i++) {
			Point vertex1 = _redTiles[i].Position;
			Point vertex2 = _redTiles[(i + 1) % _redTiles.Count].Position;

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

	/// <summary>
	/// Represents a line segment between two points
	/// </summary>
	private readonly record struct LineSegment(Point Start, Point End);

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
}
