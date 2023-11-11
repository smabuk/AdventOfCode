namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 07: The Sum of Its Parts
/// https://adventofcode.com/2018/day/07
/// </summary>
[Description("The Sum of Its Parts")]
public sealed partial class Day07 {

	[Init]
	public static    void Init(string[] input, params object[]? _) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static IEnumerable<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input) {
		_instructions = input.Select(Instruction.Parse);
	}

	private static string Solution1(string[] input) {
		List<Instruction> instructions = input.Select(ParseLine).ToList();
		HashSet<char> steps = [
			.. instructions.Select(instruction => instruction.BeforeCanBeginStep),
			.. instructions.Select(instruction => instruction.MustBeFinishedStep),
		];

		string correctOrder = "";
		do {
			char nextAvailable = steps
				.ExceptBy(instructions.Select(instruction => instruction.BeforeCanBeginStep), s => s)
				.Order()
				.First();
			_ = instructions.RemoveAll(i => i.MustBeFinishedStep == nextAvailable);
			_ = steps.Remove(nextAvailable);
			correctOrder += nextAvailable;
		} while (steps.Count != 0);

		return correctOrder;
	}

	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		List<Instruction> instructions = input.Select(ParseLine).ToList();
		return "** Solution not written yet **";
	}

	private record Instruction(char MustBeFinishedStep, char BeforeCanBeginStep) : IParsable<Instruction> {
		public static Instruction Parse(string s) => ParseLine(s);
		public static Instruction Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result) => throw new NotImplementedException();
	}

	private static Instruction ParseLine(string input) => new(input[5], input[36]);
}
