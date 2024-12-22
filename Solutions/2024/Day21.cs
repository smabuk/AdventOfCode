﻿using static Smab.Helpers.Direction;

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 21: Keypad Conundrum
/// https://adventofcode.com/2024/day/21
/// </summary>
[Description("Keypad Conundrum")]
public static partial class Day21 {

	private const int ROBOTS_PART1 =  3;
	private const int ROBOTS_PART2 = 26;
	private const char EMPTY = '.';
	private static readonly char[,] _keypad0 = """789456123.0A""".ToCharArray().To2dArray(3);
	private static readonly char[,] _keypad1 = """.^A<v>""".ToCharArray().To2dArray(3);
	private static Dictionary<ButtonPair, ButtonPairLookup> _lookup = []; 

	[Init]
	public static void Init(string[] _)
	{
		IEnumerable<ButtonPairLookup> result0 = _keypad0
			.ForEachCell()
			.Where(cell => cell.Value is not EMPTY)
			.Permute(2)
			.Select(pair => new ButtonPairLookup(
				new(pair[0].Value, pair[1].Value),
				pair[0].Index,
				pair[1].Index,
				new ButtonPair(pair[0].Value, pair[1].Value).GetSequence(pair[0].Index, pair[1].Index))
				)
			;
		IEnumerable<ButtonPairLookup> result1 = _keypad1
			.ForEachCell()
			.Where(cell => cell.Value is not EMPTY)
			.Permute(2)
			.Select(pair => new ButtonPairLookup(
				new(pair[0].Value, pair[1].Value),
				pair[0].Index,
				pair[1].Index,
				new ButtonPair(pair[0].Value, pair[1].Value).GetSequence(pair[0].Index, pair[1].Index))
				)
			;

		_lookup = result0.Concat(result1).ToDictionary(bp => bp.Pair, bp => bp);
	}

	public static long Part1(string[] codes)
	{
		return codes.Sum(code =>
		{
			char[] sequence = code.ToCharArray();
			sequence = Enumerable.Range(0, ROBOTS_PART1).Aggregate(sequence, (seq, _) => seq.GetNewSequences());
			return sequence.Complexity(code);
		});
	}

	public static string Part2(string[] codes)
	{
		int noOfRobots = ROBOTS_PART2;
		noOfRobots = 3;

		long test = codes.Sum(code =>
		{
			char[] sequence = ['a', .. code.ToCharArray()];
			sequence = Enumerable.Range(0, noOfRobots).Aggregate(sequence, (seq, _) => seq.GetNewSequences());
			//Console.WriteLine($"{code} {noOfRobots} {sequence.Length,10} = {sequence.Complexity(code),14}"); 
			return sequence.Complexity(code);
		});

		return "* WIP " + test.ToString();
	}

	private static char[] GetNewSequences(this char[] sequence)
	{
		char[] seq = ['A', .. sequence];

		return [..
			seq
			.Zip(seq[1..])
			.Select(p => new ButtonPair(p.First, p.Second))
			.Select(bp => _lookup.GetValueOrDefault(bp)?.SingleSequence.ToCharArray() ?? ['A']).SelectMany(s => s)
			];
	}

	private static long Complexity(this char[] sequence, string code)
		=> sequence.Length * code[..^1].As<long>();


	private static string GetSequence(this ButtonPair pair, Point startPoint, Point endPoint)
	{
		int distance = startPoint.ManhattanDistance(endPoint);
		char[,] keypad = pair.IsKeypad0 ? _keypad0 : _keypad1;
		return keypad.FindAllShortestPaths(startPoint, endPoint, distance).ToList().MinScoreSequence(_keypad1);
	}

	static IEnumerable<string> FindAllShortestPaths(this char[,] keypad, Point start, Point end, int distance)
	{
		Queue<(Point, List<char>)> queue = [];
		queue.Enqueue((start, []));
		bool[,] visited = new bool[keypad.ColsCount(), keypad.RowsCount()];

		while (queue.Count > 0) {
			(Point current, List<char> path) = queue.Dequeue();
			visited[current.X, current.Y] = true;

			if (current == end) {
				yield return new string([.. path, 'A']);
				continue;
			}

			foreach (Direction dir in Directions.UDLR) {
				if (distance < path.Count) {
					continue;
				}

				Point newPoint = current.Translate(dir);

				if (keypad.IsInBounds(newPoint) && !visited[newPoint.X, newPoint.Y] && keypad[newPoint.X, newPoint.Y] != EMPTY) {
					List<char> newPath = [.. path,
						dir switch
						{
							U => '^',
							D => 'v',
							L => '<',
							R => '>',
							_ => throw new NotImplementedException(),
						}];
					queue.Enqueue((newPoint, newPath));
				}
			}
		}
	}

	private static string MinScoreSequence(this List<string> sequences, char[,] keypad)
	{
		int minIndex = int.MaxValue;
		int minScore = int.MaxValue;
		for (int i = 0; i < sequences.Count; i++) {
			string seq = sequences[i];
			int score = 0;
			char prev = seq[0];
			foreach (char end in seq[1..]) {
				if (prev != end) {
					Point prevIndex = keypad.ForEachCell().Single(cell => cell.Value == prev).Index;
					Point endIndex  = keypad.ForEachCell().Single(cell => cell.Value == end).Index;
					score += prevIndex.ManhattanDistance(endIndex);
				}
				prev = end;
			}

			if (score < minScore) {
				minScore = score;
				minIndex = i;
			}
		}

		return sequences[minIndex];
	}

	private record ButtonPair(char Start, char End)
	{
		public bool IsKeypad0 => $"{Start}{End}".Intersect("0123456789").Any();
		public bool IsKeypad1 => $"{Start}{End}".Intersect("<^v>").Any();
	}

	private record ButtonPairLookup(ButtonPair Pair, Point StartPoint, Point EndPoint, string SingleSequence);
}
