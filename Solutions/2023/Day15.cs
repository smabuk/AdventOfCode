namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 15: Lens Library
/// https://adventofcode.com/2023/day/15
/// </summary>
[Description("Lens Library")]
public sealed partial class Day15 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		return input[0]
			.TrimmedSplit(",")
			.As<Step>()
			.Sum(step => step.HashNumber);
	}

	private static string Solution2(string[] input) {
		List<Step> initializationSequence = [.. input.As<Step>()];
		return "** Solution not written yet **";
	}

	private sealed record Step(string Name) : IParsable<Step> {

		public int HashNumber = GetHashNumber(Name);

		private static int GetHashNumber(string s)
		{
			int result = 0;
			foreach (char c in s) {
				result += c;
				result *= 17;
				result %= 256;
			}
			return result;
		}

		public static Step Parse(string s, IFormatProvider? provider) => new(s);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Step result)
			=> ISimpleParsable<Step>.TryParse(s, provider, out result);
	}
}
