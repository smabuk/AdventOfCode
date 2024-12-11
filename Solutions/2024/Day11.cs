namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 11: Plutonian Pebbles
/// https://adventofcode.com/2024/day/11
/// </summary>
[Description("Plutonian Pebbles")]
public static partial class Day11 {

	private static List<long> _initialStones = [];

	
	[Init]
	public static void InitialStones(string[] input)
		=> _initialStones = [ .. input[0].AsNumbers<long>(separator: ' ')];


	public static long Part1(string[] _, params object[]? args)
	{
		return args.Method() switch
		{
			"blink" => Enumerable
					.Repeat(0, args.NoOfBlinksPart1())
					.Aggregate(new LinkedList<long>(_initialStones), (stones, _) => stones.Blink())
					.Count,
			"count" => _initialStones.Sum(stone => stone.CountStones([], args.NoOfBlinksPart1())),
			_ => throw new NotImplementedException(),
		};
	}

	public static long Part2(string[] _, params object[]? args)
		=> _initialStones
			.Sum(stone => stone.CountStones([], args.NoOfBlinksPart2()));


	private static LinkedList<long> Blink(this LinkedList<long> stones)
	{
		LinkedList<long> newStones = new(stones);
		LinkedListNode<long>? current = newStones.First;

		while (current is not null) {
			if (current.Value == 0) {
				current.Value = 1;
			} else if (current.Value.TrySplit(out (long Left, long Right) values)) {
				_ = newStones.AddBefore(current, values.Left);
				current.Value = values.Right;
			} else {
				current.Value *= 2024;
			}
			current = current.Next;
		}

		return newStones;
	}

	private static bool TrySplit(this long value, out (long Left, long Right) leftAndRight)
	{
		int length = value.Length();
		if (length.IsOdd()) {
			leftAndRight = (0, 0);
			return false;
		}

		long divisor = (int)Math.Pow(10, length / 2);
		leftAndRight = (value / divisor, value % divisor);

		return true;
	}

	private static long CountStones(this long stone, Dictionary<(long, long), long> cache, int blinksRemaining)
	{
		if (blinksRemaining == 0) {
			return 1;
		}

		if (cache.TryGetValue((stone, blinksRemaining), out long cacheValue) ) {
			return cacheValue;
		}

		long count = 0;

		if (stone == 0) {
			count = CountStones(1, cache, blinksRemaining - 1);
		} else if (stone.TrySplit(out (long Left, long Right) values)) {
			count += CountStones(values.Left, cache,  blinksRemaining - 1);
			count += CountStones(values.Right, cache, blinksRemaining - 1);
		} else {
			count += CountStones(stone * 2024, cache, blinksRemaining - 1);
		}

		cache.Add((stone, blinksRemaining), count);

		return count;
	}



	private static string Method(this object[]? args) => GetArgument(args, 1, "count");

	private static int NoOfBlinksPart1(this object[]? args)
	{
		return args?.Length switch
		{
			>= 1 when args[0] is string => GetArgument(args, 2, 25),
			_ => GetArgument(args, 1, 25)
		};
	}

	private static int NoOfBlinksPart2(this object[]? args) => GetArgument(args, 1, 75);
}
