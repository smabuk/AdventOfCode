namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 07: Bridge Repair
/// https://adventofcode.com/2024/day/07
/// </summary>
[Description("Bridge Repair")]
public static partial class Day07 {

	private static List<Equation> _equations = [];

	[Init]
	public static void LoadEquations(string[] input) => _equations = [.. input.As<Equation>()];

	public static long Part1(string[] _)
		=> _equations
			.Where(eq => eq.CouldPossiblyBeTrue(2))
			.Sum(e => e.Result);

	public static long Part2(string[] _)
		=> _equations
			.Where(eq => eq.CouldPossiblyBeTrue(3))
			.Sum(e => e.Result);

	private static bool CouldPossiblyBeTrue(this Equation equation, int operatorCount)
	{
		int iterations = (int)Math.Pow(operatorCount, equation.Values.Count - 1);
		int noOfOperators = operatorCount;

		for (int i = 0; i < iterations ; i++) {
			string operators =
				i.ToBaseString(noOfOperators).PadLeft(equation.Values.Count - 1, '0');

			long result = equation.Values[0];

			for (int ix = 0; ix < equation.Values.Count - 1; ix++) {
				result = operators[ix] switch
				{
					ADD => result + equation.Values[ix + 1],
					MUL => result * equation.Values[ix + 1],
					DOT => $"{result}{equation.Values[ix + 1]}".As<long>(),
					_ => throw new NotImplementedException(),
				};

				if (result > equation.Result) {
					break;
				}
			}

			if (result == equation.Result) {
				return true;
			}

		}

		return false;
	}

	private static string ToBaseString(this int number, int baseNumber)
	{
		if (number == 0) { return "0"; }

		if (baseNumber == 2) {
			return Convert.ToString(number, 2);
		}

		string result = "";
		while (number > 0) {
			int remainder = number % baseNumber;
			result = remainder + result;
			number /= baseNumber;
		}

		return result;
	}

	private const char ADD = '0';
	private const char MUL = '1';
	private const char DOT = '2';

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
}
