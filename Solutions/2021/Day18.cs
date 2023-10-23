namespace AdventOfCode.Solutions._2021;

/// <summary>
/// Day 18: Snailfish
/// https://adventofcode.com/2021/day/18
/// </summary>
[Description("Snailfish")]
public class Day18 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static long Solution1(string[] input) {
		return input
			.Select(i => new SnailfishNumber(i))
			.Aggregate((a, b) => a + b)
			.Magnitude;
	}

	private static int Solution2(string[] input) {
		List<SnailfishNumber> snailfishNumbers = input.Select(i => new SnailfishNumber(i)).ToList();

		return snailfishNumbers
			.SelectMany(sfn => snailfishNumbers, (a, b) => (a, b))
			.Where(x => x.a != x.b)
			.Select(x => (x.a + x.b).Magnitude)
			.Max();
	}

	public record struct SnailfishNumber(string Number) {

		private static readonly Regex _pairRegex = new(@"\[(?<lv>\d+),(?<rv>\d+)\]", RegexOptions.Compiled);
		private static readonly Regex _numberRegex = new(@"(?<d>\d+)", RegexOptions.Compiled);
		private static readonly Regex _ddRegex = new(@"(?<dd>\d\d+)", RegexOptions.Compiled);

		public static SnailfishNumber operator +(SnailfishNumber lho, SnailfishNumber rho)
			=> new(Reduce($"[{lho.Number},{rho.Number}]"));

		public int Magnitude => CalculateMagnitude(Number);

		private int CalculateMagnitude(string number) {
			string input = number;
			int result = 0;
			Match pair = _pairRegex.Match(input);
			if (pair.Success) {
				result = 3 * int.Parse(pair.Groups["lv"].ValueSpan);
				result += 2 * int.Parse(pair.Groups["rv"].ValueSpan);
				string newNumber = $"{number[..pair.Index]}{result}{input[(pair.Index + pair.Length)..]}";

				int newResult = CalculateMagnitude(newNumber);
				if (newResult != 0) {
					result = newResult;
				}
			}

			return result;
		}

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
			for (int i = 0; i < input.Length; i++) {
				char c = input[i];
				if (c == '[') {
					depthCount++;
				} else if (c == ']') {
					depthCount--;
				} else if (depthCount > 4) {
					int commaIndex = input[i..].IndexOf(',');
					int bracketIndex = input[(i + commaIndex + 1)..].IndexOf(']');
					int leftValue = int.Parse(input[i..(i + commaIndex)]);
					int rightValue = int.Parse(input[(i + commaIndex + 1)..(i + commaIndex + 1 + bracketIndex)]);
					string lhs;
					string rhs = input[(input[i..].IndexOf(']') + i + 1)..];
					MatchCollection findLastNumber = _numberRegex.Matches(input[..(i - 1)]);
					if (findLastNumber.Count == 0) {
						lhs = input[..(i - 1)];
					} else {
						lhs = input[..i];
						int lhsValue = int.Parse(findLastNumber.Last().ValueSpan) + leftValue;
						lhs = $"{lhs[..findLastNumber.Last().Index]}{lhsValue}{lhs[(findLastNumber.Last().Index + findLastNumber.Last().Length)..^1]}";
					}
					var findNextNumber = _numberRegex.Match(rhs);
					if (findNextNumber.Success) {
						int rhsValue = int.Parse(findNextNumber.ValueSpan) + rightValue;
						int fI = findNextNumber.Index;
						rhs = $"{rhs[..fI]}{rhsValue}{rhs[(fI + findNextNumber.Length)..]}";
					}
					return $"{lhs}0{rhs}";
				}
			}

			return input;
		}

		private static string Split(string input) {
			for (int i = 0; i < input.Length; i++) {
				char c = input[i];
				if (char.IsDigit(c)) {
					int numberStart = i;
					int numberEnd = numberStart;
					while (char.IsDigit(input[numberEnd])) {
						numberEnd++;
					}
					if (numberEnd - numberStart >= 2) {
						int value = int.Parse(input.AsSpan(numberStart, numberEnd - numberStart));
						int lhsValue = value / 2;
						int rhsValue = value - lhsValue;
						return $"{input[..numberStart]}[{lhsValue},{rhsValue}]{input[numberEnd..]}";
					}
				}
			}

			return input;
		}


	}
}
