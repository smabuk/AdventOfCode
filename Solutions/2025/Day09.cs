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

	/// <summary>
	/// Finds the largest axis-aligned rectangle whose opposite corners are both polygon vertices
	/// and which fits entirely within the polygon.
	/// Uses coordinate compression with a 2D prefix sum for O(1) subgrid validation,
	/// giving O(n²) overall vs the original O(n³–n⁴).
	/// </summary>
	/// <param name="input">The puzzle input lines, each parsed as a <see cref="Tile"/>.</param>
	/// <returns>The area of the largest valid rectangle.</returns>
	public static long Part2(string[] input)
	{
		Polygon polygon = Polygon.Build([.. input.Select(Tile.Parse)]);
		Dictionary<Point, bool> pointCache = [];
		(bool[,] grid, int[] xs, int[] ys) = BuildInteriorGrid(polygon, pointCache);
		int[,] prefixSum = BuildPrefixSum(grid);
		return polygon
			.Vertices
			.Combinations(2)
			.Where(pair => IsRectangleInsideGrid(pair[0], pair[1], xs, ys, prefixSum))
			.Max(pair => pair.Area());
	}



	/// <summary>
	/// Builds a compressed interior grid from the polygon's vertices.
	/// </summary>
	/// <remarks>
	/// Extracts all unique X and Y coordinates from the polygon vertices and uses them as cell boundaries.
	/// For each compressed cell, a sample point one unit inside the top-left corner is tested against the polygon
	/// using <see cref="IsPointInsideOrOnPolygon"/>. The returned <paramref name="xs"/> and <paramref name="ys"/> arrays
	/// are the sorted unique vertex coordinates; cell <c>(c, r)</c> spans <c>[xs[c], xs[c+1]] × [ys[r], ys[r+1]]</c>.
	/// </remarks>
	/// <param name="polygon">The polygon whose interior is to be sampled.</param>
	/// <param name="pointCache">Cache for point-in-polygon checks.</param>
	/// <returns>
	/// A tuple of the interior grid indexed [col, row], the sorted unique X coordinates, and the sorted unique Y coordinates.
	/// </returns>
	private static (bool[,] Grid, int[] Xs, int[] Ys) BuildInteriorGrid(Polygon polygon, Dictionary<Point, bool> pointCache)
	{
		int[] xs = [.. polygon.Vertices.Select(t => t.X).Distinct().Order()];
		int[] ys = [.. polygon.Vertices.Select(t => t.Y).Distinct().Order()];

		int cols = xs.Length - 1;
		int rows = ys.Length - 1;
		bool[,] grid = new bool[cols, rows];

		for (int c = 0; c < cols; c++) {
			for (int r = 0; r < rows; r++) {
				// Sample one unit inside the top-left corner of the cell
				Point sample = new(xs[c] + 1, ys[r] + 1);
				grid[c, r] = IsPointInsideOrOnPolygon(sample, polygon, pointCache);
			}
		}

		return (grid, xs, ys);
	}

	/// <summary>
	/// Builds a 2D prefix sum table from a boolean grid for O(1) rectangular region sum queries.
	/// </summary>
	/// <param name="grid">The source boolean grid, indexed [col, row].</param>
	/// <returns>A <c>(cols+1) × (rows+1)</c> prefix sum table, also indexed [col, row].</returns>
	private static int[,] BuildPrefixSum(bool[,] grid)
	{
		int cols = grid.GetLength(0);
		int rows = grid.GetLength(1);
		int[,] ps = new int[cols + 1, rows + 1];

		for (int c = 0; c < cols; c++) {
			for (int r = 0; r < rows; r++) {
				ps[c + 1, r + 1] = (grid[c, r] ? 1 : 0)
					+ ps[c, r + 1] + ps[c + 1, r] - ps[c, r];
			}
		}

		return ps;
	}

	/// <summary>
	/// Determines in O(1) whether every compressed cell inside the axis-aligned rectangle
	/// defined by two vertex tiles is marked as interior.
	/// </summary>
	/// <param name="v1">The first corner vertex.</param>
	/// <param name="v2">The opposite corner vertex.</param>
	/// <param name="xs">Sorted unique X coordinates from the compressed grid.</param>
	/// <param name="ys">Sorted unique Y coordinates from the compressed grid.</param>
	/// <param name="prefixSum">Prefix sum table built from the interior grid.</param>
	/// <returns><see langword="true"/> if all cells in the rectangle are interior; otherwise <see langword="false"/>.</returns>
	private static bool IsRectangleInsideGrid(Tile v1, Tile v2, int[] xs, int[] ys, int[,] prefixSum)
	{
		int c1 = Array.IndexOf(xs, Math.Min(v1.X, v2.X));
		int c2 = Array.IndexOf(xs, Math.Max(v1.X, v2.X));
		int r1 = Array.IndexOf(ys, Math.Min(v1.Y, v2.Y));
		int r2 = Array.IndexOf(ys, Math.Max(v1.Y, v2.Y));

		if (c1 < 0 || c2 < 0 || r1 < 0 || r2 < 0) { return false; }
		if (c1 == c2 || r1 == r2) { return false; } // degenerate rectangle

		// Rectangle spans compressed cols [c1, c2-1] and rows [r1, r2-1]
		int total = prefixSum[c2, r2] - prefixSum[c1, r2] - prefixSum[c2, r1] + prefixSum[c1, r1];
		return total == (c2 - c1) * (r2 - r1);
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

	[GenerateIParsable] internal sealed partial record Tile(Point Position);
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
