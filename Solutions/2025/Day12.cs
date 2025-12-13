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
	private static readonly List<int> ROTATIONS = [0, 90, 180, 270];

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
		_transformationsCache = _shapes
			.ToDictionary(shape => shape.Index, shape => GetAllTransformations(shape.Shape));

	}

	private static List<PresentShape> _shapes = [];
	private static List<Region> _regions = [];
	private static Dictionary<int, List<Grid<char>>> _transformationsCache = [];

	public static int Part1()
	{
		int count = 0;

		foreach (Region region in _regions) {
			bool canFit;
			Grid<int>? placement;
			if (_regions.Count > 3) { // Not test data
				canFit = TryGreedyPlacement(region, out placement);
			} else {
				canFit = CanFitPresentsWithCPSAT(region, out placement);
			}
			if (canFit && placement is not null) {
				count++;
				VisualisePlacement(region, placement);
			} else {
				//VisualiseString($"{region} NO FIT:");
			}
		}

		return count;
	}

	public static string Part2() => "⭐ CONGRATULATIONS ⭐";




	/// <summary>
	/// Generate all unique transformations (rotations and flips) of a shape
	/// </summary>
	private static List<Grid<char>> GetAllTransformations(Grid<char> shape)
		=> [.. ROTATIONS
			.Select(rotation => shape.Rotate(rotation))
			.SelectMany(rotatedShape => new[] { rotatedShape, rotatedShape.FlipHorizontally() })
			.DistinctBy(g => g.AsStringWithNewLines())];

	/// <summary>
	/// Fast greedy placement heuristic for large problems
	/// </summary>
	private static bool TryGreedyPlacement(Region region, [NotNullWhen(true)] out Grid<int>? placement)
	{
		placement = null;
		if (!IsThereEnoughSpace(region, out List<int> presentIndices)) {
			return false;
		}

		bool[,] occupied = new bool[region.Width, region.Length];
		Grid<int> placementGrid = new(region.Width, region.Length);
		placementGrid = placementGrid.Fill(-1);

		// Sort presents by size (largest first) for better packing
		List<int> sortedIndices = [.. presentIndices.OrderByDescending(idx => CountShapeCells(_shapes[idx].Shape))];

		for (int presentIdx = 0; presentIdx < sortedIndices.Count; presentIdx++) {
			int shapeIndex = sortedIndices[presentIdx];
			List<Grid<char>> transformations = _transformationsCache[shapeIndex];
			bool placed = false;

			// Try each transformation
			foreach (Grid<char> shape in transformations) {
				if (placed) { break; }

				// Try each position
				for (int col = 0; col <= region.Width - shape.Width; col++) {
					if (placed) { break; }

					for (int row = 0; row <= region.Length - shape.Height; row++) {
						// Check if this placement is valid
						bool canPlace = true;
						for (int dc = 0; dc < shape.Width && canPlace; dc++) {
							for (int dr = 0; dr < shape.Height && canPlace; dr++) {
								if (shape[dc, dr] != EMPTY && occupied[col + dc, row + dr]) {
									canPlace = false;
								}
							}
						}

						if (canPlace) {
							// Place the present
							for (int dc = 0; dc < shape.Width; dc++) {
								for (int dr = 0; dr < shape.Height; dr++) {
									if (shape[dc, dr] != EMPTY) {
										occupied[col + dc, row + dr] = true;
										placementGrid[col + dc, row + dr] = presentIdx;
									}
								}
							}
							placed = true;
							break;
						}
					}
				}
			}

			if (placed is false) {
				return false; // Couldn't place this present
			}
		}

		placement = placementGrid.Copy();
		return true; // All presents placed successfully
	}

	private static bool IsThereEnoughSpace(Region region, out List<int> presentIndices)
	{
		// Build list of present indices and total required cells
		presentIndices = [.. Enumerable
			.Range(0, region.QuantitiesOfShapes.Length)
			.SelectMany(shapeIndex => Enumerable.Repeat(shapeIndex, region.QuantitiesOfShapes[shapeIndex]))];

		int totalRequiredCells = presentIndices.Sum(idx => CountShapeCells(_shapes[idx].Shape));
		int totalAvailableCells = region.Length * region.Width;

		return totalRequiredCells <= totalAvailableCells;
	}

	/// <summary>
	/// Solve using Google OR-Tools CP-SAT solver with optimized model
	/// </summary>
	private static bool CanFitPresentsWithCPSAT(Region region, [NotNullWhen(true)] out Grid<int>? placement)
	{
		placement = null;
		if (!IsThereEnoughSpace(region, out List<int> presentIndices)) {
			return false;
		}

		// For very large problems, use a simpler heuristic check
		if (presentIndices.Count > 100 || region.Length * region.Width > 500) {
			// Use greedy placement as heuristic
			return TryGreedyPlacement(region, out placement);
		}

		// Create CP-SAT model
		CpModel model = new();

		// For each present, enumerate all valid placements (but limit the number)
		List<List<(int col, int row, int transformIdx, List<(int c, int r)> occupiedCells)>> presentPlacements = [];
		int totalPlacements = 0;

		for (int presentIdx = 0; presentIdx < presentIndices.Count; presentIdx++) {
			int shapeIndex = presentIndices[presentIdx];
			List<Grid<char>> transformations = _transformationsCache[shapeIndex];
			List<(int, int, int, List<(int, int)>)> placements = [];

			// For each transformation
			for (int transformIdx = 0; transformIdx < transformations.Count; transformIdx++) {
				Grid<char> shape = transformations[transformIdx];

				// For each possible position
				for (int startCol = 0; startCol <= region.Width - shape.Width; startCol++) {
					for (int startRow = 0; startRow <= region.Length - shape.Height; startRow++) {
						// Collect occupied cells for this placement
						List<(int c, int r)> occupied = [];
						for (int dc = 0; dc < shape.Width; dc++) {
							for (int dr = 0; dr < shape.Height; dr++) {
								if (shape[dc, dr] != EMPTY) {
									occupied.Add((startCol + dc, startRow + dr));
								}
							}
						}
						placements.Add((startCol, startRow, transformIdx, occupied));
						totalPlacements++;

						// If we're generating too many placements, fall back to greedy
						if (totalPlacements > 50000) {
							return TryGreedyPlacement(region, out placement);
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
		Dictionary<(int col, int row), List<BoolVar>> cellOccupation = [];

		for (int presentIdx = 0; presentIdx < presentIndices.Count; presentIdx++) {
			for (int placementIdx = 0; placementIdx < presentPlacements[presentIdx].Count; placementIdx++) {
				List<(int c, int r)> occupiedCells = presentPlacements[presentIdx][placementIdx].occupiedCells;
				BoolVar placementVar = presentPlacementVars[presentIdx][placementIdx];

				foreach ((int c, int r) cell in occupiedCells) {
					if (!cellOccupation.TryGetValue(cell, out List<BoolVar>? value)) {
						value = [];
						cellOccupation[cell] = value;
					}

					value.Add(placementVar);
				}
			}
		}

		foreach (KeyValuePair<(int col, int row), List<BoolVar>> kvp in cellOccupation) {
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

		if (status is CpSolverStatus.Optimal or CpSolverStatus.Feasible) {
			// Extract placement for visualization
			Grid<int> placementGrid = new(region.Width, region.Length);
			for (int c = 0; c < region.Width; c++) {
				for (int r = 0; r < region.Length; r++) {
					placementGrid[c, r] = -1; // Empty
				}
			}

			for (int presentIdx = 0; presentIdx < presentIndices.Count; presentIdx++) {
				for (int placementIdx = 0; placementIdx < presentPlacements[presentIdx].Count; placementIdx++) {
					if (solver.Value(presentPlacementVars[presentIdx][placementIdx]) == 1) {
						List<(int c, int r)> occupiedCells = presentPlacements[presentIdx][placementIdx].occupiedCells;
						foreach ((int c, int r) in occupiedCells) {
							placementGrid[c, r] = presentIdx;
						}
						break;
					}
				}
			}

			placement = placementGrid.Copy();
			return true;
		}

		return false;
	}

	private static void VisualisePlacement(Region region, Grid<int> placement)
	{
		const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		const char BLOCK = Utf16Chars.BlockElements.FULL_BLOCK;
		string[] COLOURS = [
			"[#FF0000]", "[#00FF00]", "[#0000FF]", "[#FFFF00]", "[#FF00FF]", "[#00FFFF]",
			"[#FFA500]", "[#800080]", "[#008000]", "[#000080]", "[#FF6347]", "[#4682B4]",
			"[#32CD32]", "[#FF1493]", "[#00CED1]", "[#FFD700]", "[#DC143C]", "[#00FA9A]",
			"[#9370DB]", "[#FF4500]", "[#2E8B57]", "[#D2691E]", "[#5F9EA0]", "[#FF69B4]",
			"[#CD5C5C]", "[#4169E1]", "[#8B008B]", "[#556B2F]", "[#FF8C00]", "[#9932CC]",
			"[#8B0000]", "[#E9967A]", "[#8FBC8F]", "[#483D8B]", "[#2F4F4F]", "[#00CED1]",
			"[#9400D3]", "[#FF1493]", "[#00BFFF]", "[#696969]", "[#1E90FF]", "[#B22222]",
			"[#FFFACD]", "[#ADD8E6]", "[#F08080]", "[#E0FFFF]", "[#90EE90]", "[#FFB6C1]",
			"[#FFA07A]", "[#20B2AA]", "[#87CEFA]", "[#778899]", "[#B0C4DE]", "[#FFFFE0]",
			"[#00FF00]", "[#32CD32]", "[#FAF0E6]", "[#FF00FF]", "[#800000]", "[#66CDAA]",
			"[#0000CD]", "[#BA55D3]", "[#9370DB]", "[#3CB371]", "[#7B68EE]", "[#00FA9A]"
		];

		Grid<string> displayGrid = new(placement.Width, placement.Height);
		HashSet<int> presentsSeen = [];

		for (int r = 0; r < region.Length; r++) {
			for (int c = 0; c < region.Width; c++) {
				int presentIdx = placement[c, r];
				if (presentIdx != -1) {
					_ = presentsSeen.Add(presentIdx);
					string cell = _visualise.IsCapableOfMarkup()
						? $"{COLOURS[presentIdx % COLOURS.Length]}{BLOCK}[/]"
						: $"{CHARS[presentIdx % CHARS.Length]}";
					displayGrid[c, r] = cell;
				} else {
					displayGrid[c, r] = $"{EMPTY}";
				}
			}
		}

		if (_visualise is not null) {

			string[] start = _visualise.IsCapableOfMarkup()
				? ["markup"]
				: [""];

			string[] output = [.. start, $"{region}", .. displayGrid.ToDisplayStrings()];

			_visualise?.Invoke(output, true);
		}
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

		public override string ToString() => $"Region {Width,2}x{Length,2}: ({string.Join(", ", QuantitiesOfShapes)})";
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
