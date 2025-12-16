namespace AdventOfCode.Solutions._1111;

/// <summary>
/// Day XX: Title
/// https://adventofcode.com/
/// Samples and examples and tests
/// </summary>
[Description("")]
[GenerateVisualiser]
public partial class DayXX {

	[Init]
	public static void LoadInstructions(string[] input) => _instructions = [.. input.As<Instruction>()];
	private static IEnumerable<Instruction> _instructions = [];

	public static string Part1() => NO_SOLUTION_WRITTEN_MESSAGE;
	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;

	[GenerateIParsable]
	private sealed partial record Instruction(string Name, int Value)
	{
		public static Instruction Parse(string s)
		{
			//MatchCollection match = InputRegEx().Matches(input);
			Match match = InputRegEx().Match(s);
			return match.Success
				? new(match.Groups["opts"].Value, match.As<int>("number"))
				: null!;
		}
	}

	[GeneratedRegex("""(?<opts>opt1|opt2|opt3) (?<number>[\+\-]?\d+)""")]
	private static partial Regex InputRegEx();
}
