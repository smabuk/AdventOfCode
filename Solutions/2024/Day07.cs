using static AdventOfCode.Solutions._2024.Day07;

using Op = System.Func<AdventOfCode.Solutions._2024.Day07.Equation, long>;

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 07: Bridge Repair
/// https://adventofcode.com/2024/day/07
/// </summary>
[Description("Bridge Repair")]
public static partial class Day07 {

	private static List<Equation> _equations = [];
	private static HashSet<Equation> _part1Equations = [];

	[Init]
	public static void LoadEquations(string[] input, params object[]? args)
	{
		string method = args.Method();
		_equations = [.. input.As<Equation>()];

		if (method == "onepass") {
			_part1Equations = [.. _equations.Where(eq => eq.CouldBeTrue([Multiply, Add]))];
		}
	}

	public static long Part1(string[] _, params object[]? args)
	{
		string method = args.Method();

		return method switch
		{
			"onepass" => _part1Equations.Sum(e => e.Result),
			"recursive" => _equations
				.Where(eq => eq.CouldBeTrue([Multiply, Add]))
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
			"onepass" =>
				((IEnumerable<Equation>)[.. _equations.Except(_part1Equations)
					.Where(eq => eq.CouldBeTrue([Multiply, Add, FastConcatenate]))
					, .. _part1Equations])
				.Sum(e => e.Result),
			"recursive" => _equations
				.Where(eq => eq.CouldBeTrue([Multiply, Add, FastConcatenate]))
				.Sum(e => e.Result),
			"force" => _equations
				.Where(eq => eq.CouldPossiblyBeTrueSlow(3))
				.Sum(e => e.Result),
			_ => throw new NotImplementedException(),
		};
	}

	private static bool CouldBeTrue(this Equation equation, List<Op> operations)
	{
		foreach (Op operation in operations) {
			long result = equation.PerformOperation(operation);

			if (equation.LastPair()) {
				if (result == equation.Result) { return true; }
			} else {
				if (equation.MergeValues(result).CouldBeTrue(operations)) {
					return true;
				}
			}
		}

		return false;
	}



	private static Equation MergeValues(this Equation equation, long result)
		=> equation with { Values = [result, .. equation.Values[2..]] };
	private static long PerformOperation(this Equation equation, Op operation) => operation(equation);
	private static bool LastPair(this Equation equation) => equation.Values.Count == 2;
	private static long Add(this Equation equation) => equation.Values[0] + equation.Values[1];
	private static long Multiply(this Equation equation) => equation.Values[0] * equation.Values[1];
	//private static long Concatenate(this Equation equation) => $"{equation.Values[0]}{equation.Values[1]}".As<long>();
	private static long FastConcatenate(this Equation equation) => equation.Values[0].ShiftLeftAndAdd(equation.Values[1]);

	public static long ShiftLeftAndAdd(this long number, long otherNumber)
	{
		return otherNumber switch
		{
			<    10 => number * 10L,
			<   100 => number * 100L,
			< 1_000 => number * 1_000L,
			_ => throw new NotImplementedException(),
		} + otherNumber;
	}

	private static string Method(this object[]? args) => GetArgument(args, 1, "onepass").ToLower();

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
