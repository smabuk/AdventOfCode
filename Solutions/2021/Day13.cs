namespace AdventOfCode.Solutions._2021;

/// <summary>
/// Day 13: Transparent Origami
/// https://adventofcode.com/2021/day/13
/// </summary>
[Description("Transparent Origami")]
public class Day13 {

	public static string Part1(string[] input, params object[]? args) {
		int folds = GetArgument(args, argumentNumber: 1, defaultResult: 1);
		return Solution1(input, folds).ToString();
	}
	public static string Part2(string[] input, params object[]? args) {
		bool includeOcr = GetArgument<bool>(args, argumentNumber: 1, true);
		return Solution2(input, includeOcr).ToString();
	}

	record FoldInstruction(string Axis, int Value);

	private static int Solution1(string[] input, int folds) {
		(HashSet<Point> dots, List<FoldInstruction> instructions) = ParseInput(input);

		return Fold(folds, dots, instructions).ToHashSet().Count;
	}

	private static string Solution2(string[] input, bool includeOcr = true) {
		(HashSet<Point> dots, List<FoldInstruction> instructions) = ParseInput(input);
		string output = String.Join(Environment.NewLine,
			Fold(instructions.Count, dots, instructions)
			.To2dArray(initial: ' ', value: '█')
			.PrintAsStringArray(width: 0));
		if (includeOcr) {
			output = OcrHelpers.IdentifyMessage(output, ' ', '█') + Environment.NewLine + output;
		}
		return output;
	}

	private static IEnumerable<Point> Fold(int folds, HashSet<Point> dots, List<FoldInstruction> instructions) {
		foreach (FoldInstruction fold in instructions.Take(folds)) {
			HashSet<Point> newDots = new();
			switch (fold.Axis) {
				case "x":
					newDots = dots
						.Where(dot => dot.X > fold.Value)
						.Select(dot => dot with { X = dot.X - (2 * (dot.X - fold.Value)) })
						.ToHashSet();
					dots.RemoveWhere(dot => dot.X > fold.Value);
					break;
				case "y":
					newDots = dots
						.Where(dot => dot.Y > fold.Value)
						.Select(dot => dot with { Y = dot.Y - (2 * (dot.Y - fold.Value)) })
						.ToHashSet();
					dots.RemoveWhere(dot => dot.Y > fold.Value);
					break;
				default:
					break;
			}
			dots.UnionWith(newDots);
		}
		return dots;
	}

	private static (HashSet<Point> dots, List<FoldInstruction> instructions) ParseInput(string[] input) {
		HashSet<Point> dots = input
			.Where(i => i.Length > 0 && Char.IsDigit(i[0]))
			.AsPoints()
			.ToHashSet();
		List<FoldInstruction> instructions = input
			.Where(i => i.Length > 0 && Char.IsLetter(i[0]))
			.Select(i => ParseLine(i))
			.ToList();
		return (dots, instructions);
	}

	private static FoldInstruction ParseLine(string input) {
		Match match = Regex.Match(input, @"fold along (x|y)=(\d+)");
		return new(match.Groups[1].Value, int.Parse(match.Groups[2].Value));
	}
}
