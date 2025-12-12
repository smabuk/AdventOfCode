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
		VisualiseString("");
		return _regions
			.Select(region =>
			{
				bool canFit = CanFitPresentsWithCPSAT(region);
				VisualiseString(canFit ? $"YES: {region}" : $" NO: {region}");
				return canFit;
			})
			.Count(canFit => canFit);
	}

	public static string Part2() => "⭐ CONGRATULATIONS ⭐";

	/// <summary>
	/// Solve using Google OR-Tools CP-SAT solver with optimized model
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

		// For very large problems, use a simpler heuristic check
		if (presentIndices.Count > 100 || region.Length * region.Width > 500) {
			// Use greedy placement as heuristic
			return TryGreedyPlacement(region, presentIndices);
		}

		// Create CP-SAT model
		CpModel model = new();

		// For each present, enumerate all valid placements (but limit the number)
		List<List<(int row, int col, int transformIdx, List<(int r, int c)> occupiedCells)>> presentPlacements = [];
		int totalPlacements = 0;

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
						totalPlacements++;

						// If we're generating too many placements, fall back to greedy
						if (totalPlacements > 50000) {
							return TryGreedyPlacement(region, presentIndices);
						}
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
				BoolVar isChosen = model.NewBoolVar($"p{presentIdx}_{placementIdx}");
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

		// Solve with tight timeout and parallel workers
		CpSolver solver = new()
		{
			StringParameters = "max_time_in_seconds:2.0, num_search_workers:8, log_search_progress:false"
		};
		CpSolverStatus status = solver.Solve(model);

		return status is CpSolverStatus.Optimal or CpSolverStatus.Feasible;
	}

	/// <summary>
	/// Fast greedy placement heuristic for large problems
	/// </summary>
	private static bool TryGreedyPlacement(Region region, List<int> presentIndices)
	{
		bool[,] occupied = new bool[region.Length, region.Width];

		// Sort presents by size (largest first) for better packing
		List<int> sortedIndices = [.. presentIndices.OrderByDescending(idx => CountShapeCells(_shapes[idx].Shape))];

		foreach (int shapeIndex in sortedIndices) {
			List<Grid<char>> transformations = _transformationsCache[shapeIndex];
			bool placed = false;

			// Try each transformation
			foreach (Grid<char> shape in transformations) {
				if (placed) {
					break;
				}

				// Try each position
				for (int row = 0; row <= region.Length - shape.Height; row++) {
					if (placed) {
						break;
					}
					for (int col = 0; col <= region.Width - shape.Width; col++) {
						// Check if this placement is valid
						bool canPlace = true;
						for (int dr = 0; dr < shape.Height && canPlace; dr++) {
							for (int dc = 0; dc < shape.Width && canPlace; dc++) {
								if (shape[dr, dc] != EMPTY && occupied[row + dr, col + dc]) {
									canPlace = false;
								}
							}
						}

						if (canPlace) {
							// Place the present
							for (int dr = 0; dr < shape.Height; dr++) {
								for (int dc = 0; dc < shape.Width; dc++) {
									if (shape[dr, dc] != EMPTY) {
										occupied[row + dr, col + dc] = true;
									}
								}
							}
							placed = true;
							break;
						}
					}
				}
			}

			if (!placed) {
				return false; // Couldn't place this present
			}
		}

		return true; // All presents placed successfully
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

			Grid<char> flipped = current.FlipHorizontally();
			key = GridToString(flipped);
			if (seen.Add(key)) {
				unique.Add(flipped);
			}

			current = current.Rotate(90);
		}

		return unique;
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
	private static int CountShapeCells(Grid<char> shape) => shape.Values().Count(val => val is not EMPTY);

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
	}
}
