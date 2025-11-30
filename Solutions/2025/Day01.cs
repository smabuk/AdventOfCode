namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 01: Title
/// https://adventofcode.com/2025/day/01
/// </summary>
[Description("")]
public partial class Day01 {

	private static IEnumerable<Instruction> _instructions = [];

	[Init]
	public static void LoadInstructions(string[] input) => _instructions = [.. input.As<Instruction>()];
	public static string Part1() => NO_SOLUTION_WRITTEN_MESSAGE;
	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;


	private sealed record Instruction(string Name, int Value) : IParsable<Instruction>
	{
		public static Instruction Parse(string s, IFormatProvider? provider)
		{
			//MatchCollection match = InputRegEx().Matches(input);
			Match match = InputRegEx().Match(s);
			return match.Success
				? new(match.Groups["opts"].Value, match.As<int>("number"))
				: null!;
		}

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	[GeneratedRegex("""(?<opts>opt1|opt2|opt3) (?<number>[\+\-]?\d+)""")]
	private static partial Regex InputRegEx();
}
