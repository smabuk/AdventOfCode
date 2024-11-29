using static AdventOfCode.Solutions._2017.Day21Constants;
using static AdventOfCode.Solutions._2017.Day21Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 21: Fractal Art
/// https://adventofcode.com/2017/day/21
/// </summary>
[Description("Fractal Art")]
public sealed partial class Day21 {

	[Init]
	public static   void  Init(string[] input) => LoadRules(input);
	public static string Part1(string[] _, params object[]? args)
	{
		int iterations = GetArgument<int>(args, argumentNumber: 1, defaultResult: 5);
		return Solution(iterations).ToString();
	}

	public static string Part2(string[] _) => NO_SOLUTION_WRITTEN_MESSAGE;
	//public static string Part2(string[] _) => Solution(18).ToString();

	private static List<EnhancementRule> _rules = [];

	private static void LoadRules(string[] input) => 
		_rules = [.. input.As<EnhancementRule>()];

	private static int Solution(int iterations) {
		char[,] grid = (char[,])START_GRID.Clone();

		for (int i = 0; i < iterations; i++) {
			grid = grid.Enhance(_rules);
		}

		return grid.WalkWithValues().Count(g => g.Value == ON);
	}
}

file static class Day21Extensions
{
	public static int Size(this char[,] grid) => grid.ColsCount();

	public static char[,] Enhance(this char[,] grid, List<EnhancementRule> enhancementRules)
	{
		char[,] oldGrid = (char[,])grid.Clone();
		int oldSize = oldGrid.Size();

		(int subGridSize, int size) = oldSize.IsEven()
			? (2, oldSize * 3 / 2)
			: (3, oldSize * 4 / 3);

		char[,] newGrid = new char[size, size];

		IEnumerable<EnhancementRule> rules =
			subGridSize == 2
			? enhancementRules.Where(r => r is EnhancementRule2)
			: enhancementRules.Where(r => r is EnhancementRule3);

		int offsetRow = 0;
		for (int row = 0; row < oldSize; row+= subGridSize) {
			int offsetCol = 0;
			for (int col = 0; col < oldSize; col+= subGridSize) {
				EnhancementRule? foundRule = null;
				Point topLeft = new(row, col);
				char[,] subGrid = grid.SubArray(topLeft, subGridSize, subGridSize);
				foreach (EnhancementRule rule in rules) {
					if (subGrid.IsMatch(rule)) {
						foundRule = rule;
						break;
					}
				}

				Debug.Assert(foundRule is not null);
				int c = col + offsetCol;
				int r = row + offsetRow;
				foreach (char newPixel in foundRule.Output) {
					if (newPixel is LINE) {
						c = col + offsetCol;
						r++;
					} else {
						newGrid[c, r] = newPixel;
						c++;
					}
				}

				offsetCol++;
			}

			offsetRow++;
		}

		return newGrid;
	}

	public static string AsRuleFormat(this char[,] grid) => string.Join(LINE, grid.AsStrings());

	public static bool IsMatch(this char[,] grid, EnhancementRule rule)
	{
		for (int i = 0; i < 360; i+=90) {
			char[,] possibleGrid = grid.Rotate(i);
			if (possibleGrid.AsRuleFormat() == rule.Input) {
				return true;
			}

			char[,] possibleGrid2 = possibleGrid.FlipHorizontally();
			if (possibleGrid2.AsRuleFormat() == rule.Input) {
				return true;
			}

			possibleGrid2 = possibleGrid.FlipVertically();
			if (possibleGrid2.AsRuleFormat() == rule.Input) {
				return true;
			}
		}

		return false;
	}
}

internal sealed partial class Day21Types
{
	public abstract record EnhancementRule(string Input, string Output) : IParsable<EnhancementRule>
	{
		public static EnhancementRule Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit("=>");
			return tokens[0].IndexOf(LINE) == 2
				? new EnhancementRule2(tokens[0], tokens[1])
				: new EnhancementRule3(tokens[0], tokens[1]);
		}

		public static EnhancementRule Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out EnhancementRule result)
			=> ISimpleParsable<EnhancementRule>.TryParse(s, provider, out result);
	}

	public record EnhancementRule2(string Input, string Output) : EnhancementRule(Input, Output);
	public record EnhancementRule3(string Input, string Output) : EnhancementRule(Input, Output);

}

file static class Day21Constants
{
	public const char ON = '#';
	public const char OFF = '.';
	public const char LINE = '/';

	public static readonly char[,] START_GRID = """
		.#.
		..#
		###
		""".Split(Environment.NewLine).To2dArray();
}
