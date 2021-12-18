namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 18: Snailfish
/// https://adventofcode.com/2021/day/18
/// </summary>
[Description("Snailfish")]
public class Day18 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static long Solution1(string[] input) {
		List<SnailfishNumber> snailfishNumbers = input.Select(i => ParseLine(i)).ToList();

		SnailfishNumber result = snailfishNumbers.First();
		foreach (var sfn in snailfishNumbers.Skip(1)) {
			result = result.Plus(sfn);
		}

		return result.Magnitude;
	}

	private static long Solution2(string[] input) {
		List<SnailfishNumber> snailfishNumbers = input.Select(i => ParseLine(i)).ToList();

		long maxMagnitude = 0;

		foreach (var sfn in snailfishNumbers.Combinations(2)) {
			long magnitude = sfn.First().Plus(sfn.Last()).Magnitude;
			maxMagnitude = Math.Max(maxMagnitude, magnitude);
			magnitude = sfn.Last().Plus(sfn.First()).Magnitude;
			maxMagnitude = Math.Max(maxMagnitude, magnitude);
		}

		return maxMagnitude;
	}

	private static SnailfishNumber ParseLine(string input) => new(input);
	public record SnailfishNumber(string Number) {

		public long Magnitude => CalculateMagnitude(Number);

		private long CalculateMagnitude(string number) {
			string input = number;
			long result = 0;
			Match? pair = Regex.Match(input, @"\[(?<lv>\d+),(?<rv>\d+)\]");
			if (pair.Success) {
				result = 3 * int.Parse(pair.Groups["lv"].Value);
				result += 2 * int.Parse(pair.Groups["rv"].Value);
				string newNumber = $"{number[..(pair.Index)]}{result}{input[(pair.Index + pair.Length)..]}";

				long newResult = CalculateMagnitude(newNumber);
				if (newResult != 0) {
					result = newResult;
				}
			}

			return result;
		}

		public SnailfishNumber Plus(SnailfishNumber rho) => new(Reduce($"[{Number},{rho.Number}]"));


		public static string Reduce(string input) {
			string output = input;

			// Repeat
			//     If nested 4 deep EXPLODE
			//     If number is 10+ SPLIT

			bool somethingChanged = true;
			while (somethingChanged) {
				somethingChanged = false;

				string newOutput = Explode(output);
				if (newOutput != output) {
					somethingChanged = true;
				} else {
					newOutput = Split(output);
					if (newOutput != output) {
						somethingChanged = true;
					}
				}
				output = newOutput;
			}

			return output;
		}

		public static string Explode(string input) {
			int depthCount = 0;
			int? lastNumberStart = null;
			int? lastNumberEnd = null;
			string output = "";
			for (int i = 0; i < input.Length; i++) {
				char c = input[i];
				if (c == '[') {
					depthCount++;
				} else if (c == ']') {
					depthCount--;
				} else if (depthCount > 4) {
					long leftValue = long.Parse(input[i..].Split(",")[0]);
					long rightValue = long.Parse(input[i..].Split(",")[1].Split("]")[0]);
					string lhs = input[..i];
					string rhs = input[(input[i..].IndexOf(']') + i + 1)..];
					if (lastNumberStart is null || lastNumberEnd is null) {
						lhs = $"{input[..(i - 1)]}";
					} else {
						int index = Math.Min(lhs.IndexOf(']'), lhs.IndexOf(','));
						long lhsValue = int.Parse(lhs[(int)lastNumberStart..((int)lastNumberEnd + 1)]) + leftValue;
						lhs = $"{lhs[..(int)lastNumberStart]}{lhsValue}{lhs[((int)lastNumberEnd + 1)..^1]}";
					}
					Match? findNumber = Regex.Match(rhs, @"\d+");
					if (findNumber.Success) {
						long rhsValue = long.Parse(findNumber.Value) + rightValue;
						int fI = findNumber.Index;
						rhs = $"{rhs[..(fI)]}{rhsValue}{rhs[(fI + findNumber.Length)..]}";
					}
					output = $"{lhs}0{rhs}";
					return output;
				} else if (Char.IsDigit(c)) {
					lastNumberEnd = i;
					if (i == 0 || (Char.IsDigit(input[i - 1]) is false)) {
						lastNumberStart = i;
					}
				}
			}

			return input;
		}

		private static string Split(string input) {
			string output = input;
			for (int i = 0; i < input.Length; i++) {
				char c = input[i];
				if (Char.IsDigit(c)) {
					int numberStart = i;
					int numberEnd = numberStart;
					while (Char.IsDigit(input[numberEnd])) {
						numberEnd++;
					}
					long value = long.Parse(input[numberStart..(numberEnd)]);
					if (value >= 10) {
						long lhsValue = value / 2;
						long rhsValue = value - lhsValue;
						output = $"{input[..numberStart]}[{lhsValue},{rhsValue}]{input[numberEnd..]}";
						return output;
					}
				}
			}

			return output;
		}
	}
}
