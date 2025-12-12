namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 12: Christmas Tree Farm
/// https://adventofcode.com/2025/day/12
/// </summary>
[Description("Christmas Tree Farm")]
[GenerateVisualiser]
public partial class Day12
{

	private const char EMPTY = '.';

	[Init]
	public static void LoadChristmasTreeFarm(string[] input)
	{
		int sectionStart = input.Index().First(line => line.Item.Contains('x')).Index;
		_shapes = [.. input
			.Take(sectionStart)
			.SplitBy(string.IsNullOrWhiteSpace)
			.Select(shapeLines => PresentShape.Parse(string.Join(Environment.NewLine, shapeLines)))];
		_regions = [.. input.Skip(sectionStart).As<Region>()];

		_transformationsCache.Clear();
		for (int i = 0; i < _shapes.Count; i++) {
			_transformationsCache[i] = GetAllTransformations(_shapes[i].Shape);
		}
	}

	private static List<PresentShape> _shapes = [];
	private static List<Region> _regions = [];
	private static readonly Dictionary<int, List<Grid<char>>> _transformationsCache = [];

	public static int Part1() => _regions.AsParallel().Count(CanFitPresents);

	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;

	/// <summary>
	/// Determines if all presents can fit in the given region
	/// </summary>
	private static bool CanFitPresents(Region region)
	{
		VisualiseString($"Region: {region}");
		char[,] grid = new char[region.Length, region.Width];
		for (int row = 0; row < region.Length; row++) {
			for (int col = 0; col < region.Width; col++) {
				grid[row, col] = EMPTY;
			}
		}

		List<(int ShapeIndex, int ShapeSize)> presentsToPlace = [];
		int totalRequiredCells = 0;
		for (int shapeIndex = 0; shapeIndex < region.QuantitiesOfShapes.Length; shapeIndex++) {
			for (int count = 0; count < region.QuantitiesOfShapes[shapeIndex]; count++) {
				int size = CountShapeCells(_shapes[shapeIndex].Shape);
				presentsToPlace.Add((shapeIndex, size));
				totalRequiredCells += size;
			}
		}

		int totalAvailableCells = region.Length * region.Width;
		if (totalRequiredCells > totalAvailableCells) {
			return false;
		}

		presentsToPlace.Sort((a, b) => b.ShapeSize.CompareTo(a.ShapeSize));

		return Backtrack(grid, presentsToPlace, 0, totalRequiredCells);
	}

	/// <summary>
	/// Backtracking solver to place presents on the grid (optimized)
	/// NOTE: This is an NP-complete problem - may timeout on very large inputs
	/// See Day12_Note.md for details on complexity and potential solutions
	/// </summary>
	private static bool Backtrack(char[,] grid, List<(int ShapeIndex, int ShapeSize)> presentsToPlace, int presentIndex, int remainingCells)
	{
		if (presentIndex == presentsToPlace.Count) {
			return true;
		}

		(int shapeIndex, int shapeSize) = presentsToPlace[presentIndex];
		List<Grid<char>> transformations = _transformationsCache[shapeIndex];
		int gridRows = grid.GetLength(0);
		int gridCols = grid.GetLength(1);

		foreach (Grid<char> transformation in transformations) {
			int maxRow = gridRows - transformation.Height + 1;
			int maxCol = gridCols - transformation.Width + 1;

			for (int row = 0; row < maxRow; row++) {
				for (int col = 0; col < maxCol; col++) {
					if (CanPlaceShapeFast(grid, transformation, row, col, gridRows, gridCols)) {
						PlaceShapeFast(grid, transformation, row, col);
						if (Backtrack(grid, presentsToPlace, presentIndex + 1, remainingCells - shapeSize)) {
							return true;
						}
						RemoveShapeFast(grid, transformation, row, col);
					}
				}
			}
		}

		return false;
	}

	/// <summary>
	/// Generate all unique transformations (rotations and flips) of a shape
	/// </summary>
	private static List<Grid<char>> GetAllTransformations(Grid<char> shape)
	{
		HashSet<string> seen = [];
		List<Grid<char>> unique = [];

		Grid<char> current = shape;
		for (int rotation = 0; rotation < 4; rotation++) {
			string key = GridToString(current);
			if (seen.Add(key)) {
				unique.Add(current);
			}

			Grid<char> flipped = FlipHorizontal(current);
			key = GridToString(flipped);
			if (seen.Add(key)) {
				unique.Add(flipped);
			}

			current = Rotate90(current);
		}

		return unique;
	}

	/// <summary>
	/// Rotate grid 90 degrees clockwise
	/// </summary>
	private static Grid<char> Rotate90(Grid<char> grid)
	{
		int rows = grid.Height;
		int cols = grid.Width;
		char[,] rotated = new char[cols, rows];

		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {
				rotated[col, rows - 1 - row] = grid[row, col];
			}
		}

		return rotated.To2dGrid();
	}

	/// <summary>
	/// Flip grid horizontally
	/// </summary>
	private static Grid<char> FlipHorizontal(Grid<char> grid)
	{
		int rows = grid.Height;
		int cols = grid.Width;
		char[,] flipped = new char[rows, cols];

		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {
				flipped[row, cols - 1 - col] = grid[row, col];
			}
		}

		return flipped.To2dGrid();
	}

	/// <summary>
	/// Convert grid to string for deduplication
	/// </summary>
	private static string GridToString(Grid<char> grid)
	{
		StringBuilder sb = new();
		for (int row = 0; row < grid.Height; row++) {
			for (int col = 0; col < grid.Width; col++) {
				_ = sb.Append(grid[row, col]);
			}
			_ = sb.Append(Environment.NewLine);
		}
		return sb.ToString();
	}

	/// <summary>
	/// Check if a shape can be placed at the given position (optimized)
	/// </summary>
	private static bool CanPlaceShapeFast(char[,] grid, Grid<char> shape, int startRow, int startCol, int gridRows, int gridCols)
	{
		for (int row = 0; row < shape.Height; row++) {
			for (int col = 0; col < shape.Width; col++) {
				if (shape[row, col] != EMPTY && grid[startRow + row, startCol + col] != EMPTY) {
					return false;
				}
			}
		}
		return true;
	}

	/// <summary>
	/// Place a shape on the grid at the given position (optimized)
	/// </summary>
	private static void PlaceShapeFast(char[,] grid, Grid<char> shape, int startRow, int startCol)
	{
		for (int row = 0; row < shape.Height; row++) {
			for (int col = 0; col < shape.Width; col++) {
				if (shape[row, col] != EMPTY) {
					grid[startRow + row, startCol + col] = '#';
				}
			}
		}
	}

	/// <summary>
	/// Remove a shape from the grid at the given position (optimized)
	/// </summary>
	private static void RemoveShapeFast(char[,] grid, Grid<char> shape, int startRow, int startCol)
	{
		for (int row = 0; row < shape.Height; row++) {
			for (int col = 0; col < shape.Width; col++) {
				if (shape[row, col] != EMPTY) {
					grid[startRow + row, startCol + col] = EMPTY;
				}
			}
		}
	}

	/// <summary>
	/// Find the first empty cell in the grid (for smarter placement ordering)
	/// </summary>
	private static (int Row, int Col) FindFirstEmpty(char[,] grid, int gridRows, int gridCols)
	{
		for (int row = 0; row < gridRows; row++) {
			for (int col = 0; col < gridCols; col++) {
				if (grid[row, col] == EMPTY) {
					return (row, col);
				}
			}
		}
		return (-1, -1);
	}

	/// <summary>
	/// Count number of filled cells in a shape
	/// </summary>
	private static int CountShapeCells(Grid<char> shape)
	{
		int count = 0;
		for (int row = 0; row < shape.Height; row++) {
			for (int col = 0; col < shape.Width; col++) {
				if (shape[row, col] != EMPTY) {
					count++;
				}
			}
		}
		return count;
	}

	[GenerateIParsable]
	private sealed partial record Region(int Width, int Length, int[] QuantitiesOfShapes)
	{
		public static Region Parse(string s)
		{
			string[] parts = s.TrimmedSplit([' ', 'x', ':']);
			return new Region(int.Parse(parts[0]), int.Parse(parts[1]), [.. parts[2..].As<int>()]);
		}

		public override string ToString() => $"Region {Width,2}x{Length,2}: [{string.Join(", ", QuantitiesOfShapes)}]";
	}

	[GenerateIParsable]
	private sealed partial record PresentShape(int Index, Grid<char> Shape)
	{
		public static PresentShape Parse(string s)
		{
			string[] parts = s.TrimmedSplit(Environment.NewLine);
			int index = int.Parse(parts[0][..^1]);
			return new PresentShape(index, parts[1..].To2dGrid());
		}

		//public override string ToString() => $"Shape {Index}: {Shape}";
	}
}
