using static AdventOfCode.Solutions._2017.Day25Constants;
using static AdventOfCode.Solutions._2017.Day25Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 25: The Halting Problem
/// https://adventofcode.com/2017/day/25
/// </summary>
[Description("The Halting Problem")]
public sealed partial class Day25 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] _) => "⭐CONGRATULATIONS⭐";

	private static int Solution1(string[] input) {
		string currentStateName = $"{input[0][^2]}";
		int steps = input[1].TrimmedSplit(' ')[^2].As<int>();

		Dictionary<string, StateRule> stateRules =
			input[3..]
			.Chunk(10)
			.Select(StateRule.Parse)
			.ToDictionary(sr => sr.Name);

		LinkedList<bool> tape = [];
		LinkedListNode<bool> current = tape.AddFirst(false);

		for (int step = 0; step < steps; step++) {
			Rule rule = current.Value
				? stateRules[currentStateName].Rule1
				: stateRules[currentStateName].Rule0;
			current.Value = rule.WriteValue;
			current = rule.Right
				? current.Next     ?? tape.AddLast(false)
				: current.Previous ?? tape.AddFirst(false);
			currentStateName = rule.NextStateName;
		}

		return tape.Count(v => v is true);
	}
}

file static class Day25Extensions
{
}

internal sealed partial class Day25Types
{
	public sealed record StateRule(string Name, Rule Rule0, Rule Rule1) : IParsable<StateRule>
	{
		public static StateRule Parse(string[] s)
		{
			return new StateRule(
				$"{s[0][^2]}",
				Rule.Parse(s[1..5]),
				Rule.Parse(s[5..9])
				);
		}

		public static StateRule Parse(string s, IFormatProvider? provider) => null!;

		public static StateRule Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out StateRule result)
			=> ISimpleParsable<StateRule>.TryParse(s, provider, out result);
	}

	public sealed record Rule(bool CurrentValue, bool WriteValue, bool Right, string NextStateName) : IParsable<Rule>
	{
		public static Rule Parse(string[] s)
		{
			return new Rule(
				s[0][^2] == ONE,
				s[1][^2] == ONE,
				s[2][^3] == 'h',  // rig 'h' t
				$"{s[3][^2]}"
				);
		}

		public static Rule Parse(string s, IFormatProvider? provider) => null!;

		public static Rule Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Rule result)
			=> ISimpleParsable<Rule>.TryParse(s, provider, out result);
	}
}

file static class Day25Constants
{
	public const char ZERO = '0';
	public const char ONE  = '1';
}
