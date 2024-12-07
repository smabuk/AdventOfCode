using static AdventOfCode.Solutions._2024.Day07;

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

	public static long Part1(string[] _, params object[]? args)
	{
		string method = args.Method();

		return method switch
		{
			"recursive" => _equations
				.Where(eq => eq.CouldPossiblyBeTrueRecursive(2))
				.Sum(e => e.Result),
			"force" => _equations
				.Where(eq => eq.CouldPossiblyBeTrueSlow(2))
				.Sum(e => e.Result),
			_ => throw new NotImplementedException(),
		};
	}

	public static long Part2(string[] _, params object[]? args)
	{
		string method = args.Method();

		return method switch
		{
			"recursive" => _equations
				.Where(eq => eq.CouldPossiblyBeTrueRecursive(3))
				.Sum(e => e.Result),
			"force" => _equations
				.Where(eq => eq.CouldPossiblyBeTrueSlow(3))
				.Sum(e => e.Result),
			_ => throw new NotImplementedException(),
		};
	}

	private static bool CouldPossiblyBeTrueRecursive(this Equation equation, int operatorCount)
	{
		for (int operators = 0; operators < operatorCount; operators++) {
			long result = operators switch
			{
				ADD => equation.Values[0] + equation.Values[1],
				MUL => equation.Values[0] * equation.Values[1],
				DOT => $"{equation.Values[0]}{equation.Values[1]}".As<long>(),
				_ => throw new NotImplementedException(),
			};

			if (equation.Values.Count == 2) {
				if (result == equation.Result) {
					return true;
				}
			} else {
				bool isValid = (equation with { Values = [result, .. equation.Values[2..]] }).CouldPossiblyBeTrueRecursive(operatorCount);
				if (isValid) {
					return true;
				}
			}
		}

		return false;
	}

	private static string Method(this object[]? args) => GetArgument(args, 1, "recursive").ToLower();

	internal const int ADD = 0;
	internal const int MUL = 1;
	internal const int DOT = 2;

	public sealed record Equation(long Result, List<long> Values) : IParsable<Equation>
	{
		public static Equation Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit(':');
			return new(tokens[0].As<long>(), [.. tokens[1].As<long>(' ')]);
		}

		public static Equation Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Equation result)
			=> ISimpleParsable<Equation>.TryParse(s, provider, out result);
	}
}

file static class Day07SlowExtensions
{
	public static bool CouldPossiblyBeTrueSlow(this Equation equation, int operatorCount)
	{
		const char ADD = '0';
		const char MUL = '1';
		const char DOT = '2';

		int iterations = (int)Math.Pow(operatorCount, equation.Values.Count - 1);
		int noOfOperators = operatorCount;

		for (int i = 0; i < iterations; i++) {
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

	public static string ToBaseString(this int number, int baseNumber)
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
}
