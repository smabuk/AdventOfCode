using static AdventOfCode.Solutions._YYYY.DayXXConstants;
using static AdventOfCode.Solutions._YYYY.DayXXTypes;
namespace AdventOfCode.Solutions._YYYY;

/// <summary>
/// Day XX: Title
/// https://adventofcode.com/YYYY/day/XX
/// </summary>
[Description("Title")]
public sealed partial class DayXX {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static IEnumerable<Instruction> _instructions = [];

	[Init]
	private static void LoadInstructions(string[] input) => _instructions = [.. input.As<Instruction>()];

	private static string Solution1(string[] input) => NO_SOLUTION_WRITTEN_MESSAGE;

	private static string Solution2(string[] input) => NO_SOLUTION_WRITTEN_MESSAGE;
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
	public static partial Regex InputRegEx();
}

file static class DayXXConstants
{
}
