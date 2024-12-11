namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 11: Plutonian Pebbles
/// https://adventofcode.com/2024/day/11
/// </summary>
[Description("Plutonian Pebbles")]
public static partial class Day11 {

	private static IEnumerable<Stone> _instructions = [];

	[Init]
	public static void InitialStones(string[] input) => _instructions = [.. input.As<Stone>()];

	public static long Part1(string[] input, params object[]? args)
	{
		int noOfBlinks = args.NoOfBlinks();
		List<long> initialStones = [ .. input[0].AsNumbers<long>(' ')];

		LinkedList<long> stones = [];

		_ = stones.AddFirst(initialStones[0]);
		for (int i = 1; i < initialStones.Count; i++) {
			_ = stones.AddLast(initialStones[i]);
		}

		for (int i = 0; i < noOfBlinks; i++) {
			stones = stones.Blink();
		}


		return stones.Count;
	}

	public static string Part2(string[] input, params object[]? args) => NO_SOLUTION_WRITTEN_MESSAGE;

	private static LinkedList<long> Blink(this LinkedList<long> stones)
	{
		LinkedList<long> newStones = new(stones);
		LinkedListNode<long>? current = newStones.First;

		while (current is not null) {
			if (current.Value == 0) {
				current.Value = 1;
			} else if (current.Value.TrySplit(out (long Left, long Right) values)) {
				//(long left, long right) = current.Value.Split();
				_ = newStones.AddBefore(current, values.Left);
				current.Value = values.Right;
			} else {
				current.Value *= 2024;
			}
			current = current.Next;
		}

		return newStones;
	}

	private static bool TrySplit(this long value, [NotNull] out (long Left, long Right) leftAndRight)
	{
		string valueString = value.ToString();
		if (valueString.Length.IsOdd()) {
			leftAndRight = (0, 0);
			return false;
		}
		int length = valueString.Length / 2;
		leftAndRight = (long.Parse(valueString[..length]), long.Parse(valueString[length..]));
		return true;
	}

	private static int NoOfBlinks(this object[]? args) => GetArgument(args, 1, 25);

	public sealed record Stone(string Name, int Value) : IParsable<Stone>
	{
		public static Stone Parse(string s, IFormatProvider? provider)
		{
			Match match = InputRegEx().Match(s);
			return match.Success
				? new(match.Groups["opts"].Value, match.As<int>("number"))
				: null!;
		}

		public static Stone Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Stone result)
			=> ISimpleParsable<Stone>.TryParse(s, provider, out result);
	}

	[GeneratedRegex("""(?<opts>opt1|opt2|opt3) (?<number>[\+\-]?\d+)""")]
	public static partial Regex InputRegEx();
}
