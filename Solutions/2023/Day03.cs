namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 03: Gear Ratios
/// https://adventofcode.com/2023/day/03
/// </summary>
[Description("Gear Ratios")]
public sealed partial class Day03 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) => FindPartNosAndGears(input, 1).Select(p => p.PartNo).Sum();

	private static int Solution2(string[] input) {
		List<PartNoAndAPosition> possibleGears = FindPartNosAndGears(input, 2);

		IEnumerable<Point> gearPositions = possibleGears
			.GroupBy(p => p.Position)
			.Where(p => p.Count() == 2)
			.Select(g => g.Key);

		int sumOfGearRatios = 0;
		foreach (Point position in gearPositions) {
			List<PartNoAndAPosition> gears = [.. possibleGears.Where(g => g.Position == position)];
			sumOfGearRatios += gears[0].PartNo * gears[1].PartNo;
		}

		return sumOfGearRatios;
	}

	private static List<PartNoAndAPosition> FindPartNosAndGears(string[] input, int solutionPartNo)
	{
		const char GEAR  = '*';
		const char SPACE = '.';

		char[,] engineSchematic = input.To2dArray();
		int maxX = engineSchematic.NoOfColumns() - 1;
		int maxY = engineSchematic.NoOfRows() - 1;

		List<PartNoAndAPosition> list = [];

		for (int row = 0; row < input.Length; row++) {
			foreach (Match number in NumberRegex().Matches(input[row]).Cast<Match>()) {
				SearchAdjacent(row, number);
			}
		}

		return list;

		void SearchAdjacent(int row, Match number)
		{
			Point corner1 = new(Math.Max(number.Index - 1, 0), Math.Max(row - 1, 0));
			Point corner2 = new(Math.Min(number.Index + number.Length + 1, maxX), Math.Min(row + 1, maxY));
			int skip   = Math.Max(number.Length - 2, 1); // Can skip self
			for (int y = corner1.Y; y <= corner2.Y; y++) {
				for (int x = corner1.X; x < corner2.X; x += (y == row ? skip : 1)) {
					if (solutionPartNo == 1) {
						if (IsSymbol(engineSchematic[x, y])) {
							list.Add(new(number.Value.As<int>(), new(number.Index, row)));
							return;
						}
					} else if (engineSchematic[x, y] is GEAR) { // Part2
						list.Add(new(number.Value.As<int>(), new(x, y)));
					}
				}
			}
		}

		bool IsSymbol(char cell) => (char.IsDigit(cell) || cell is SPACE) is false;
	}
	private record struct Point(int X, int Y);
	private record struct PartNoAndAPosition(int PartNo, Point Position);

	[GeneratedRegex(@"\d+")]
	private static partial Regex NumberRegex();

}

