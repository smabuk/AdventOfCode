using static AdventOfCode.Solutions._2024.Day07Constants;
using static AdventOfCode.Solutions._2024.Day07Types;
namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 07: Bridge Repair
/// https://adventofcode.com/2024/day/07
/// </summary>
[Description("Bridge Repair")]
public static partial class Day07 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static List<Equation> _equations = [];

	[Init]
	public static void LoadEquations(string[] input) => _equations = [.. input.As<Equation>()];

	private static long Solution1(string[] _)
	{
		var result = _equations
			.Where(CouldPossibleBeTrue)
			.Sum(e => e.Result);
		return result;
	}

	private static string Solution2(string[] _) => NO_SOLUTION_WRITTEN_MESSAGE;

	private static bool CouldPossibleBeTrue(this Equation equation)
	{
		int iterations = (int)Math.Pow(2, equation.Values.Count - 1);
		for (int i = 0; i < iterations ; i++) {
			string operators = Convert.ToString(i, 2).PadLeft(equation.Values.Count - 1, ZERO).Replace(ZERO, ADD).Replace(ONE, MUL);
			long result = equation.Values[0];

			for (int ix = 0; ix < operators.Length; ix++) {
				result = operators[ix] switch
				{
					ADD => result + equation.Values[ix + 1],
					MUL => result * equation.Values[ix + 1],
					_ => throw new NotImplementedException(),
				};
			}

			if (result == equation.Result) {
				return true;
			}
		}

		return false;
	}

	private const char ADD = '+';
	private const char MUL = '*';
	
	private const char ZERO = '0';
	private const char ONE = '1';



}

internal sealed partial class Day07Types
{

	public sealed record Equation(long Result, List<int> Values) : IParsable<Equation>
	{
		public static Equation Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit(':');
			return new(tokens[0].As<long>(), [.. tokens[1].As<int>(' ')]);
		}

		public static Equation Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Equation result)
			=> ISimpleParsable<Equation>.TryParse(s, provider, out result);
	}

	[GeneratedRegex("""(?<opts>opt1|opt2|opt3) (?<number>[\+\-]?\d+)""")]
	public static partial Regex InputRegEx();
}

file static class Day07Constants
{
}
