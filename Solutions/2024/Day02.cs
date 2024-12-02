using System;

using static AdventOfCode.Solutions._2024.Day02Constants;
using static AdventOfCode.Solutions._2024.Day02Types;
namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 02: Red-Nosed Reports
/// https://adventofcode.com/2024/day/02
/// </summary>
[Description("Red-Nosed Reports")]
public sealed partial class Day02 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static IEnumerable<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input) => _instructions = [.. input.As<Instruction>()];

	private static int Solution1(string[] input)
	{
		var count = input
			.Select(i => i.TrimmedSplit().As<int>())
			.Where(report => report.IsAscendingOrDescending() && report.HasMinMaxGap(1, 3))
			.Count();
		return count;
	}

	private static string Solution2(string[] input) => NO_SOLUTION_WRITTEN_MESSAGE;
}

file static class Day02Extensions
{
	public static bool IsAscendingOrDescending(this IEnumerable<int> report)
	{
		if (report.Order().SequenceEqual(report)) {
			return true;
		} else if (report.OrderDescending().SequenceEqual(report)) {
			return true;
		} else { return false; }
	}

	public static bool HasMinMaxGap(this IEnumerable<int> report, int min, int max)
	{
		return !report
			.Zip(report.Skip(1))
			.Select(p => int.Abs(p.First - p.Second))
			.Any(diff => diff < min || diff > max);
	}
}

internal sealed partial class Day02Types
{

	public sealed record Instruction(string Name, int Value) : IParsable<Instruction>
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
	public static partial Regex InputRegEx();
}

file static class Day02Constants
{
}
