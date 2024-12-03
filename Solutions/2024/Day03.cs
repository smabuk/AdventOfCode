using Smab.Helpers;

using static AdventOfCode.Solutions._2024.Day03Constants;
using static AdventOfCode.Solutions._2024.Day03Types;
namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 03: Mull It Over
/// https://adventofcode.com/2024/day/03
/// </summary>
[Description("Mull It Over")]
public sealed partial class Day03 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static IEnumerable<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input)
		=> _instructions = [.. Instruction.ParseAll(input)];

	private static int Solution1(string[] input) => _instructions.Sum(i => i.Value1 * i.Value2);

	private static string Solution2(string[] input) => NO_SOLUTION_WRITTEN_MESSAGE;
}

file static class Day03Extensions
{
}

internal sealed partial class Day03Types
{

	public sealed record Instruction(int Value1, int Value2) : IParsable<Instruction>
	{
		public static IEnumerable<Instruction> ParseAll(string[] s)
		{
			foreach (var line in s) {
				MatchCollection matches = InputRegEx().Matches(line);
				foreach (Match match in matches) {
					yield return new(match.As<int>("number1"), match.As<int>("number2"));
				}
			}
		}

		public static Instruction Parse(string s, IFormatProvider? provider) => null!;

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	[GeneratedRegex("""mul\((?<number1>\d+),(?<number2>\d+)\)""")]
	public static partial Regex InputRegEx();
}

file static class Day03Constants
{
}
