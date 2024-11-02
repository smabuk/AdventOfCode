using static AdventOfCode.Solutions._2016.DayXXConstants;
using static AdventOfCode.Solutions._2016.DayXXTypes;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day XX: Title
/// https://adventofcode.com/2016/day/XX
/// </summary>
[Description("")]
public sealed partial class DayXX {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static IEnumerable<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input) {
		_instructions = input.As<Instruction>();
	}

	private static string Solution1(string[] input) {
		List<Instruction> instructions = [.. input.As<Instruction>()];
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}

	private static string Solution2(string[] input) {
		List<Instruction> instructions = [.. input.As<Instruction>()];
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class DayXXExtensions
{
}

internal sealed partial class DayXXTypes
{

	public sealed record Instruction(string Name, int Value) : IParsable<Instruction>
	{
		public static Instruction Parse(string s, IFormatProvider? provider)
		{
			//MatchCollection match = InputRegEx().Matches(input);
			Match match = InputRegEx().Match(s);
			if (match.Success) {
				return new(match.Groups["opts"].Value, match.As<int>("number"));
			}
			return null!;
		}

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	[GeneratedRegex("""(?<opts>opt1|opt2|opt3) (?<number>[\+\-]?\d+)""")]
	public static partial Regex InputRegEx();
}

file static class DayXXConstants
{
}
