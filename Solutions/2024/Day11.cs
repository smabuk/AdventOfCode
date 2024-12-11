namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 11: Plutonian Pebbles
/// https://adventofcode.com/2024/day/11
/// </summary>
[Description("Plutonian Pebbles")]
public static partial class Day11 {

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

	public static int Part2(string[] input, params object[]? args)
	{
		int noOfBlinks = 80;
		List<long> initialStones = [.. input[0].AsNumbers<long>(' ')];
		long otherCount = 0;
		initialStones = [0];
		LinkedList<long> stones = [];

		_ = stones.AddFirst(initialStones[0]);
		for (int i = 1; i < initialStones.Count; i++) {
			if (initialStones[i] == 0) {
				otherCount = initialStones[i];
			}
			_ = stones.AddLast(initialStones[i]);
		}

		for (int i = 0; i < noOfBlinks; i++) {
			//(stones, otherCount) = stones.Blink(otherCount);
			Console.WriteLine($"No of stones: {i, 2} => {stones.Count}");
		}


		return stones.Count;
	}

	private static LinkedList<long> Blink(this LinkedList<long> stones)
	{
		LinkedList<long> newStones = new(stones);
		LinkedListNode<long>? current = newStones.First;

		while (current is not null) {
			if (current.Value == 0) {
				current.Value = 1;
				newStones.Remove(current);

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

	private static (LinkedList<long>, long Count) CountStones(this LinkedList<long> stones, int blinks)
	{
		Dictionary<long, long> cache = [];
		long otherCount = blinks;
		LinkedList<long> newStones = new(stones);
		LinkedListNode<long>? current = newStones.First;

		while (current is not null) {
			if (current.Value == 0) {
				current.Value = 1;
				newStones.Remove(current);

			} else if (current.Value.TrySplit(out (long Left, long Right) values)) {
				//(long left, long right) = current.Value.Split();
				_ = newStones.AddBefore(current, values.Left);
				current.Value = values.Right;
			} else {
				current.Value *= 2024;
			}
			current = current.Next;
		}

		return (newStones, otherCount);
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

	private static int[] ZeroEffect = [1, 2, 4, 3, 4, 8, 7, 12, 14, 13, 16, 16, 25, 35, 51, 66, 112, 111, 110, 116, 163, 168, 172, 174, 191, 190, 190, 198, 197, 224, 224, 233, 251, 274, 292, 362, 361, 360, 366, 413, 418, 422, 424, 441, 440, 440, 448, 447, 474, 474, 483, 501, 524, 542, 612, 611, 610, 616, 663, 668, 672, 674, 691, 690, 690, 698, 697, 724, 724, 733, 751, 774, 792, 862, 861, 860, 866, 913, 918];

}
