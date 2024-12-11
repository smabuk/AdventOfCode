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
			"count" => _initialStones.Sum(stone => stone.CountStones(args.NoOfBlinksPart1(), [], [])),
			"blink" => Enumerable
					.Repeat(0, args.NoOfBlinksPart1())
					.Aggregate(new LinkedList<long>(_initialStones), (stones, _) => stones.Blink())
					.Count,
			_ => throw new NotImplementedException(),
		};
	}

	public static long Part2(string[] _, params object[]? args)
		=> _initialStones
			.Sum(stone => stone.CountStones(args.NoOfBlinksPart2(), [], []));


	private static long CountStones(this long stone, int blinksRemaining, Dictionary<CacheState, long> cache, Dictionary<long, int> evenOrOddNumber)
	{
		if (blinksRemaining == 0) {
			return 1;
		}

		CacheState cacheState = new(stone, blinksRemaining);
		if (cache.TryGetValue(cacheState, out long cacheValue) ) {
			return cacheValue;
		}

		long count = 0;
		if (!evenOrOddNumber.TryGetValue(stone, out int length)) {
			length = stone.Length();
			evenOrOddNumber[stone] = length;
		}

		if (stone == 0) {
			count = CountStones(1, blinksRemaining - 1, cache, evenOrOddNumber);
		} else if (length.IsEven()) {
			(long left, long right) = stone.Split(length);
			count += CountStones(left, blinksRemaining - 1, cache, evenOrOddNumber);
			count += CountStones(right, blinksRemaining - 1, cache, evenOrOddNumber);
		} else {
			count += CountStones(stone * 2024, blinksRemaining - 1, cache, evenOrOddNumber);
		}

		cache.Add(cacheState, count);

		return count;
	}

	private static (long Left, long Right) Split(this long value, int length)
	{
		// Maximum 19 digits in a long
		long divisor = length switch
		{
			2 => 10L,
			4 => 100L,
			6 => 1000L,
			8 => 10000L,
			10 => 100000L,
			12 => 1000000L,
			14 => 10000000L,
			16 => 100000000L,
			18 => 1000000000L,
			_ => throw new NotImplementedException(),
		};

		return (value / divisor, value % divisor);
	}


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

		//long divisor2 = (int)Math.Pow(10, length / 2);

		// Maximum 19 digits in a long
		long divisor = length switch
		{
			2 => 10L,
			4 => 100L,
			6 => 1000L,
			8 => 10000L,
			10 => 100000L,
			12 => 1000000L,
			14 => 10000000L,
			16 => 100000000L,
			18 => 1000000000L,
			_ => throw new NotImplementedException(),
		};

		leftAndRight = (value / divisor, value % divisor);

		return true;
	}

	private record struct CacheState(long Stone, int BlinksRemaining);

	private static string Method(this object[]? args) => GetArgument(args, 1, "count");

	private static int NoOfBlinksPart1(this object[]? args)
	{
		// if method is specified it will be in the first argument so retrieve blinks from the 2nd
		return args?.Length switch
		{
			>= 1 when args[0] is string => GetArgument(args, 2, 25),
			_ => GetArgument(args, 1, 25)
		};
	}

	private static int NoOfBlinksPart2(this object[]? args) => GetArgument(args, 1, 75);
}
