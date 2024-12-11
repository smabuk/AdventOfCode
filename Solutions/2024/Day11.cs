﻿namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 11: Plutonian Pebbles
/// https://adventofcode.com/2024/day/11
/// </summary>
[Description("Plutonian Pebbles")]
public static partial class Day11 {

	public static long Part1(string[] input, params object[]? args)
	{
		int noOfBlinks = args.NoOfBlinksPart1();
		List<long> initialStones = [ .. input[0].AsNumbers<long>(' ')];

		LinkedList<long> stones = new(initialStones);

		for (int i = 0; i < noOfBlinks; i++) {
			stones = stones.Blink();
		}

		return stones.Count;
	}

	public static long Part2(string[] input, params object[]? args)
	{
		int noOfBlinks = args.NoOfBlinksPart2();
		List<long> initialStones = [.. input[0].AsNumbers<long>(' ')];

		//LinkedList<long> stones =new(initialStones);

		//for (int i = 0; i < noOfBlinks; i++) {
		//	//(stones, otherCount) = stones.Blink(otherCount);
		//	//Console.WriteLine($"No of stones: {i, 2} => {stones.Count}");
		//}
		Dictionary<(long, long), long> cache = [];

		long count = 0;

		foreach (long stone in initialStones) {
			count += stone.CountStones(cache, noOfBlinks);
		}

		return count;
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

	private static long CountStones(this long stone, Dictionary<(long, long), long> cache, int blinks)
	{
		if (blinks == 0) {
			return 1;
		}

		if (cache.TryGetValue((stone, blinks), out long cacheValue) ) {
			return cacheValue;
		}

		long count = 0;

		if (stone == 0) {
			count = CountStones(1, cache, blinks - 1);
		} else if (stone.TrySplit(out (long Left, long Right) values)) {
			count += CountStones(values.Left, cache,  blinks - 1);
			count += CountStones(values.Right, cache, blinks - 1);
		} else {
			count += CountStones(stone * 2024, cache, blinks - 1);
		}

		cache.Add((stone, blinks), count);

		return count;
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



	private static int NoOfBlinksPart1(this object[]? args) => GetArgument(args, 1, 25);
	private static int NoOfBlinksPart2(this object[]? args) => GetArgument(args, 1, 75);
}
