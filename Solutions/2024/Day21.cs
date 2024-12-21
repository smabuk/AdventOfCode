using static Smab.Helpers.Direction;

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 21: Keypad Conundrum
/// https://adventofcode.com/2024/day/21
/// </summary>
[Description("Keypad Conundrum")]
public static partial class Day21 {

	private const char EMPTY = '.';
	private static readonly char[,] _keypad0 = """789456123.0A""".ToCharArray().To2dArray(3);
	private static readonly char[,] _keypad1 = """.^A<v>""".ToCharArray().To2dArray(3);
	private static readonly List<Direction> _directionOrder = [R, D, U, L];
	private static readonly List<string> _codes = [];
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
				pair[0].Index.ManhattanDistance(pair[1])
				))
			;
		IEnumerable<ButtonPairLookup> result1 = _keypad1
			.ForEachCell()
			.Where(cell => cell.Value is not EMPTY)
			.Permute(2)
			.Select(pair => new ButtonPairLookup(
				new(pair[0].Value, pair[1].Value),
				pair[0].Index,
				pair[1].Index,
				pair[0].Index.ManhattanDistance(pair[1])
				))
			;

		_lookup = result0.Concat(result1).ToDictionary(bp => bp.Pair, bp => bp);
	}

	public static long Part1(string[] codes, params object[]? args)
	{
		int noOfRobots = args.NoOfRobots();
		long complexity = 0;

		foreach (var code in codes) {
			List<string> sequences = [code];
			for (int ri = 0; ri < noOfRobots; ri++) {
				List<string> listOfSequences = [];
				List<List<string>> newListOfSequences = [];
				for (int si = 0; si < sequences.Count; si++) {
					string seq = sequences[si];
					string sequence = $"{'A'}{seq}";

					newListOfSequences = [..
						sequence
						.Zip(sequence[1..])
						.Select(p => new ButtonPair(p.First, p.Second))
						.Select(bp => _lookup.GetValueOrDefault(bp)?.Sequences ?? ["A"])
						];

					listOfSequences.AddRange(newListOfSequences.GenerateCombinations());
				}
				int min = listOfSequences.Select(s => s.Length).Min();
				sequences = [.. listOfSequences.Where(s => s.Length == min)];
			}

			Console.WriteLine();
			Console.WriteLine($"No of sequences for {code}: {sequences.Count}");
			complexity += sequences[0].Complexity(code);
		}

		return complexity;
	}

	private static int Complexity(this string sequence, string code)
		=> sequence.Length * code[..^1].As<int>();

	static List<string> GenerateCombinations(this List<List<string>> sequence)
	{
		List<string> result = [];
		GenerateCombinationsRecursive(sequence, 0, [], result);
		return result;
	}

	static void GenerateCombinationsRecursive(List<List<string>> sequence, int depth, List<string> current, List<string> result)
	{
		if (depth == sequence.Count) {
			result.Add(string.Join("", current));
			return;
		}

		foreach (string item in sequence[depth]) {
			current.Add(item);
			GenerateCombinationsRecursive(sequence, depth + 1, current, result);
			current.RemoveAt(current.Count - 1);
		}
	}

	public static string Part2(string[] input, params object[]? args) => NO_SOLUTION_WRITTEN_MESSAGE;

	private record ButtonPair(char Start, char End);
	private record ButtonPairLookup(ButtonPair Pair, Point StartPoint, Point EndPoint, int Distance)
	{
		public List<string> Sequences = [.. GetSequences(Pair, StartPoint, EndPoint)];

		private static List<string> GetSequences(ButtonPair pair, Point startPoint, Point endPoint)
		{
			List<string> sequences = $"{pair.Start}{pair.End}".Union("0123456789").Any()
				? [.. _keypad0.FindAllShortestPaths(startPoint, endPoint)]
				: [.. _keypad1.FindAllShortestPaths(startPoint, endPoint)];
			int min = sequences.Select(s => s.Length).Min();

			return [.. sequences.Where(s => s.Length == min)];
		}
	}


	static IEnumerable<string> FindAllShortestPaths(this char[,] keypad, Point start, Point end)
	{
		Queue<(Point, List<char>)> queue =[];
		queue.Enqueue((start, []));
		bool[,] visited = new bool[keypad.ColsCount(), keypad.RowsCount()];

		while (queue.Count > 0) {
			(Point current, List<char> path) = queue.Dequeue();
			visited[current.X, current.Y] = true;

			if (current == end) {
				yield return $"{string.Join("", path)}A";
				continue;
			}

			foreach (var dir in Directions.UDLR) {
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


	private static int NoOfRobots(this object[]? args) => GetArgument(args, 1, 3);

}
