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
	public static void LoadTiles(string[] input) => _redTiles = [.. input.As<Tile>()];
	private static List<Tile> _redTiles = [];
	private static List<List<Tile>> _polygons = [];

	public static long Part1() => _redTiles.Combinations(2).Max(tiles => tiles.Area());
	public static long Part2()
	{
		// Build polygons from connected red tiles
		BuildPolygons();

		long maximumArea = 0;

		// Process each polygon independently
		foreach (List<Tile> polygon in _polygons) {
			// Try all pairs of red tiles within this polygon as potential opposite corners
			for (int i = 0; i < polygon.Count; i++) {
				for (int j = i + 1; j < polygon.Count; j++) {
					Tile tile1 = polygon[i];
					Tile tile2 = polygon[j];

					// Skip if they can't form a proper rectangle (need different X and Y)
					if (tile1.X == tile2.X || tile1.Y == tile2.Y) {
						continue;
					}

					// Check if all points in this rectangle are red or green within this polygon
					if (IsRectangleValid(tile1, tile2, polygon)) {
						long area = tile1.Area(tile2);
						maximumArea = Math.Max(maximumArea, area);
					}
				}
			}
		}

		return maximumArea;
	}

	/// <summary>
	/// Groups red tiles into connected polygons based on connectivity
	/// </summary>
	private static void BuildPolygons()
	{
		_polygons.Clear();

		if (_redTiles.Count == 0) {
			return;
		}

		// Separate polygons based on whether consecutive tiles are connected
		List<Tile> currentPolygon = [];

		for (int i = 0; i < _redTiles.Count; i++) {
			Tile current = _redTiles[i];
			Tile next = _redTiles[(i + 1) % _redTiles.Count];

			currentPolygon.Add(current);

			// Check if current and next are connected (same row or column)
			bool connected = (current.X == next.X) || (current.Y == next.Y);

			// If disconnected and we have tiles, save this polygon
			if (!connected && currentPolygon.Count > 0) {
				_polygons.Add(currentPolygon);
				currentPolygon = [];
			}
		}

		// Add the last polygon if it exists
		if (currentPolygon.Count > 0) {
			_polygons.Add(currentPolygon);
		}

		// If no polygons were separated, treat all tiles as one polygon
		if (_polygons.Count == 0) {
			_polygons.Add([.. _redTiles]);
		}
	}

	/// <summary>
	/// Checks if all tiles in a rectangle are red or green (on/inside the polygon)
	/// </summary>
	private static bool IsRectangleValid(Tile corner1, Tile corner2, List<Tile> polygon)
	{
		int minX = Math.Min(corner1.X, corner2.X);
		int maxX = Math.Max(corner1.X, corner2.X);
		int minY = Math.Min(corner1.Y, corner2.Y);
		int maxY = Math.Max(corner1.Y, corner2.Y);

		// Check all points in the rectangle
		for (int x = minX; x <= maxX; x++) {
			for (int y = minY; y <= maxY; y++) {
				Point point = new(x, y);
				if (!IsPointGreenOrRed(point, polygon)) {
					return false;
				}
			}
		}

		return true;
	}

	/// <summary>
	/// Checks if a point is red (in the polygon) or green (on polygon edge or inside)
	/// </summary>
	private static bool IsPointGreenOrRed(Point point, List<Tile> polygon)
	{
		// Check if it's a red tile in this polygon
		if (polygon.Any(tile => tile.Position == point)) {
			return true;
		}

		// Check if it's on the polygon edge
		if (IsPointOnPolygonEdge(point, polygon)) {
			return true;
		}

		// Check if it's inside the polygon
		return IsPointInsidePolygon(point, polygon);
	}

	/// <summary>
	/// Checks if a point lies on any edge of the polygon formed by red tiles
	/// </summary>
	private static bool IsPointOnPolygonEdge(Point point, List<Tile> polygon)
	{
		for (int i = 0; i < polygon.Count; i++) {
			Tile current = polygon[i];
			Tile next = polygon[(i + 1) % polygon.Count];

			if (IsPointOnLineSegment(point, current.Position, next.Position)) {
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Checks if a point lies on a line segment between two points
	/// </summary>
	private static bool IsPointOnLineSegment(Point point, Point start, Point end)
	{
		// Check if point is collinear with start and end
		int crossProduct = ((point.Y - start.Y) * (end.X - start.X)) - ((point.X - start.X) * (end.Y - start.Y));
		if (crossProduct != 0) {
			return false;
		}

		// Check if point is within the bounding box of the segment
		int minX = Math.Min(start.X, end.X);
		int maxX = Math.Max(start.X, end.X);
		int minY = Math.Min(start.Y, end.Y);
		int maxY = Math.Max(start.Y, end.Y);

		return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
	}

	/// <summary>
	/// Uses ray casting algorithm to determine if a point is inside the polygon
	/// </summary>
	private static bool IsPointInsidePolygon(Point point, List<Tile> polygon)
	{
		int intersections = 0;

		for (int i = 0; i < polygon.Count; i++) {
			Point vertex1 = polygon[i].Position;
			Point vertex2 = polygon[(i + 1) % polygon.Count].Position;

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
