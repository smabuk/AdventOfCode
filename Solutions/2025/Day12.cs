using Google.OrTools.Sat;

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

		// Pre-compute all transformations for each shape
		_transformationsCache.Clear();
		for (int i = 0; i < _shapes.Count; i++) {
			_transformationsCache[i] = GetAllTransformations(_shapes[i].Shape);
		}
	}

	private static List<PresentShape> _shapes = [];
	private static List<Region> _regions = [];
	private static readonly Dictionary<int, List<Grid<char>>> _transformationsCache = [];

	public static int Part1()
	{
		int count = 0;

		VisualiseString("");
		foreach (Region region in _regions) {
			if (CanFitPresentsWithCPSAT(region)) {
				count++;
				VisualiseString($"YES: {region}");
			} else {
				VisualiseString($" NO: {region}");
			}
		}

		return count;
		return _regions.AsParallel().Count(CanFitPresentsWithCPSAT);
	}

	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;

	/// <summary>
	/// Solve using Google OR-Tools CP-SAT solver
	/// </summary>
	private static bool CanFitPresentsWithCPSAT(Region region)
	{
		// Quick check: do we have enough space?
		int totalRequiredCells = 0;
		List<int> presentIndices = [];

		for (int shapeIndex = 0; shapeIndex < region.QuantitiesOfShapes.Length; shapeIndex++) {
			for (int count = 0; count < region.QuantitiesOfShapes[shapeIndex]; count++) {
				int size = CountShapeCells(_shapes[shapeIndex].Shape);
				totalRequiredCells += size;
				presentIndices.Add(shapeIndex);
			}
		}

		int totalAvailableCells = region.Length * region.Width;
		if (totalRequiredCells > totalAvailableCells) {
			return false;
		}

		// Create CP-SAT model
		CpModel model = new();

		// For each present, enumerate all valid placements
		List<List<(int row, int col, int transformIdx, List<(int r, int c)> occupiedCells)>> presentPlacements = [];

		for (int presentIdx = 0; presentIdx < presentIndices.Count; presentIdx++) {
			int shapeIndex = presentIndices[presentIdx];
			List<Grid<char>> transformations = _transformationsCache[shapeIndex];
			List<(int, int, int, List<(int, int)>)> placements = [];

			// For each transformation
			for (int transformIdx = 0; transformIdx < transformations.Count; transformIdx++) {
				Grid<char> shape = transformations[transformIdx];

				// For each possible position
				for (int startRow = 0; startRow <= region.Length - shape.Height; startRow++) {
					for (int startCol = 0; startCol <= region.Width - shape.Width; startCol++) {
						// Collect occupied cells for this placement
						List<(int r, int c)> occupied = [];
						for (int dr = 0; dr < shape.Height; dr++) {
							for (int dc = 0; dc < shape.Width; dc++) {
								if (shape[dr, dc] != EMPTY) {
									occupied.Add((startRow + dr, startCol + dc));
								}
							}
						}
						placements.Add((startRow, startCol, transformIdx, occupied));
					}
				}
			}
			presentPlacements.Add(placements);
		}

		// Create boolean variables: one for each possible placement of each present
		List<List<BoolVar>> presentPlacementVars = [];
		for (int presentIdx = 0; presentIdx < presentIndices.Count; presentIdx++) {
			List<BoolVar> placementVars = [];
			for (int placementIdx = 0; placementIdx < presentPlacements[presentIdx].Count; placementIdx++) {
				BoolVar isChosen = model.NewBoolVar($"present_{presentIdx}_placement_{placementIdx}");
				placementVars.Add(isChosen);
			}
			presentPlacementVars.Add(placementVars);
		}

		// Constraint: each present must choose exactly one placement
		for (int presentIdx = 0; presentIdx < presentIndices.Count; presentIdx++) {
			_ = model.Add(LinearExpr.Sum(presentPlacementVars[presentIdx]) == 1);
		}

		// Constraint: each cell can be occupied by at most one present
		Dictionary<(int row, int col), List<BoolVar>> cellOccupation = [];

		for (int presentIdx = 0; presentIdx < presentIndices.Count; presentIdx++) {
			for (int placementIdx = 0; placementIdx < presentPlacements[presentIdx].Count; placementIdx++) {
				List<(int r, int c)> occupiedCells = presentPlacements[presentIdx][placementIdx].occupiedCells;
				BoolVar placementVar = presentPlacementVars[presentIdx][placementIdx];

				foreach ((int r, int c) cell in occupiedCells) {
					if (!cellOccupation.TryGetValue(cell, out List<BoolVar>? value)) {
						value = [];
						cellOccupation[cell] = value;
					}

					value.Add(placementVar);
				}
			}
		}

		foreach (KeyValuePair<(int row, int col), List<BoolVar>> kvp in cellOccupation) {
			if (kvp.Value.Count > 0) {
				_ = model.Add(LinearExpr.Sum(kvp.Value) <= 1);
			}
		}

		// Solve
		CpSolver solver = new()
		{
			StringParameters = "max_time_in_seconds:10.0" // 10 second timeout per region
		};
		CpSolverStatus status = solver.Solve(model);

		return status is CpSolverStatus.Optimal or CpSolverStatus.Feasible;
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
