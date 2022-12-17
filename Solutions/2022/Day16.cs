namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 16: Proboscidea Volcanium
/// https://adventofcode.com/2022/day/16
/// </summary>
[Description("Proboscidea Volcanium")]
public sealed partial class Day16 {

	[Init]
	public static    void Init(string[] input, params object[]? _) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static IEnumerable<Instruction> _instructions = Array.Empty<Instruction>();

	private static void LoadInstructions(string[] input) {
		_instructions = input.Select(Instruction.Parse);
	}

	private static string Solution1(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		List<Instruction> instructions = input.Select(ParseLine).ToList();
		return "** Solution not written yet **";
	}

	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		List<Instruction> instructions = input.Select(ParseLine).ToList();
		return "** Solution not written yet **";
	}

	private record Instruction(string Name, int Value) : IParsable<Instruction> {
		public static Instruction Parse(string s) => throw new NotImplementedException();
		public static Instruction Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result) => throw new NotImplementedException();
	}

	private static Instruction ParseLine(string input) {
		//MatchCollection match = InputRegEx().Matches(input);
		Match match = InputRegEx().Match(input);
		if (match.Success) {
			return new(match.Groups["opts"].Value, int.Parse(match.Groups["number"].Value));
		}
		return null!;
	}

	[GeneratedRegex("""(?<opts>opt1|opt2|opt3) (?<number>[\+\-]\d+)""")]
	private static partial Regex InputRegEx();
}
