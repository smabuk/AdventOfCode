namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 25: Full of Hot Air
/// https://adventofcode.com/2022/day/25
/// </summary>
[Description("Full of Hot Air")]
public sealed partial class Day25 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static string Solution1(string[] input) {
		long sum = input.Select(i => new SnafuNumber(i.Trim()).Value()).Sum();
		return SnafuNumber.ToSnafu(sum);
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

	private record struct SnafuNumber(string Number) {
		public long Value() {
			string positive = Number.Trim().Replace("=", "0").Replace("-", "0");
			string negative = Number.Trim().Replace("2", "0").Replace("1", "0").Replace("=", "2").Replace("-", "1");
			
			return FromBase5(positive) - FromBase5(negative);
		}

		public static long FromBase5(string number) {
			long value = 0;
			for (int i = 0; i < number.Length; i++) {
				value += (long)Math.Pow(5, i) * number[number.Length - i - 1].ToString().AsInt(); 
			}
			return value;
		} 

		public static string ToBase5(long value) {
			string result = "";
			long index;
			while (value != 0) {
				index = value % 5;
				value /= 5;
				result = $"{index}{result}";
			}
			return result;
		} 

		public static string ToBase3(long value) {
			string result = "";
			long index;
			while (value != 0) {
				index = value % 3;
				value /= 3;
				result = $"{index}{result}";
			}
			return result;
		}
		public static string ToSnafu(long number) {

			string result = "";
			while (number > 0) {
				int digit = (int)((number + 2) % 5) - 2;
				result = digit switch { 
					-1 => '-',
					-2 => "=",
					_ => digit.ToString(),
				} + result;
				number -= digit;
				number /= 5;
			}

			return result;
		}

	}

}
