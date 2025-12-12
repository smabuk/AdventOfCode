namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 12: Christmas Tree Farm
/// https://adventofcode.com/2025/day/12
/// </summary>
[Description("Christmas Tree Farm")]
//[GenerateVisualiser]
public partial class Day12 {

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
	}

	private static List<PresentShape> _shapes = [];
	private static List<Region> _regions = [];

	public static int Part1()
	{
		int count = _regions.Count;



		return count;
	}

	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;

	[GenerateIParsable]
	private sealed partial record Region(int Width, int Length, int[] QuantitiesOfShapes)
	{
		public static Region Parse(string s)
		{
			string[] parts = s.TrimmedSplit([' ', 'x', ':']);
			return new Region(int.Parse(parts[0]), int.Parse(parts[1]), [.. parts[2..].As<int>()]);
		}
	}

	[GenerateIParsable] private sealed partial record PresentShape(int Index, Grid<char> Shape)
	{
		public static PresentShape Parse(string s)
		{
			string[] parts = s.TrimmedSplit(Environment.NewLine);
			int index = int.Parse(parts[0][..^1]);
			return new PresentShape(index, parts[1..].To2dGrid());
		}
	}
}
