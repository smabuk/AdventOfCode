namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 25: Full of Hot Air
/// https://adventofcode.com/2022/day/25
/// </summary>
[Description("Full of Hot Air")]
public sealed partial class Day25 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => "⭐CONGRATULATIONS⭐";

	private static string Solution1(string[] input) {
		SnafuNumber sum = input.Select(i => (long)(SnafuNumber)i).Sum();
		return sum;
	}

	[DebuggerDisplay("{DebuggerDisplay()}")]
	public record struct SnafuNumber(long Value) : IParsable<SnafuNumber> {

		public static SnafuNumber Parse(string s) {
			string positive = s.Trim().Replace(MINUS_2, '0').Replace(MINUS_1, '0');
			string negative = s.Trim().Replace('2', '0').Replace('1', '0').Replace(MINUS_2, '2').Replace(MINUS_1, '1');

			return new SnafuNumber(FromBase(positive) - FromBase(negative));
		}

		public override string ToString() {
			long number = Value;
			string result = "";

			while (number > 0) {
				int digit = (int)((number + 2) % BASE) - 2;
				result = digit switch {
					-1 => MINUS_1,
					-2 => MINUS_2,
					_ => digit.ToString(),
				} + result;
				number -= digit;
				number /= BASE;
			}

			return result;
		}

		private static long FromBase(string number) {
			long value = 0;
			for (int power = 0; power < number.Length; power++) {
				value += (long)Math.Pow(BASE, power) * int.Parse($"{number[number.Length - power - 1]}");
			}
			return value;
		}

		public static implicit operator SnafuNumber(  long value) => new(value);
		public static implicit operator SnafuNumber(string input) => Parse(input);
		
		public static implicit operator   long(SnafuNumber snafu) => snafu.Value;
		public static implicit operator string(SnafuNumber snafu) => snafu.ToString();

		private const char MINUS_1 = '-';
		private const char MINUS_2 = '=';
		private const int  BASE    = 5;

		public static SnafuNumber Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out SnafuNumber result) => throw new NotImplementedException();

		private string DebuggerDisplay() => $$$"""{{{{nameof(SnafuNumber)}}} { Value = {{{Value}}}, String = {{{ToString()}}} }}""";
	}

}
