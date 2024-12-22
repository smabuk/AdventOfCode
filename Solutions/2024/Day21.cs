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
		ButtonPairLookup test = new(
				new('<', 'A'),
				new(0, 1),
				new(2, 0),
				new Point(0, 1).ManhattanDistance(new(2, 0))
				);
	}

	public static long Part1(string[] codes, params object[]? args)
	{
		int noOfRobots = args.NoOfRobots();
		long complexity = 0;

		foreach (var code in codes) {
			List<string> sequences = [code];
			List<string> alternateSequences = [code];
			for (int ri = 0; ri < noOfRobots; ri++) {
				List<string> listOfSequences = sequences.GetNewSequences();
				int min = listOfSequences.Select(s => s.Length).Min();
				sequences = [.. listOfSequences.Where(s => s.Length == min)];

				List<string> alternateListOfSequences = alternateSequences.GetAlternateNewSequences();
				int minAlt = alternateListOfSequences.Select(s => s.Length).Min();
				alternateSequences = [.. alternateListOfSequences.Where(s => s.Length == minAlt)];

				//if (min != minAlt) { min = min; };
			}


			//            3                          7          9                 A
			//        ^   A       ^^        <<       A     >>   A        vvv      A
			//    <   A > A   <   AA  v <   AA >> ^  A  v  AA ^ A  v <   AAA >  ^ A
			// v<<A>>^AvA^Av<<A>>^AAv<A<A>>^AAvAA^<A>Av<A>^AA<A>Av<A<A>>^AAAvA^<A>A

			// Could be:
			// v<<A^>>AvA^Av<A<AA^>>AA<Av>A^AAvA^Av<A^>AA<A>Av<A<A^>>AAA<Av>A^A

			//Console.WriteLine();
			//Console.WriteLine($"No of sequences for {code}: {sequences.Count}");
			complexity += sequences[0].Complexity(code);
			//complexity += alternateSequences[0].Complexity(code);
		}

		return complexity;
	}

	private static List<string> GetNewSequences(this List<string> sequences)
	{
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

		return listOfSequences;
	}

	private static List<string> GetAlternateNewSequences(this List<string> sequences)
	{
		List<string> listOfSequences = [];
		List<List<string>> newListOfSequences = [];
		for (int si = 0; si < sequences.Count; si++) {
			string seq = sequences[si];
			string sequence = $"{'A'}{seq}";

			newListOfSequences = [..
						sequence
						.Zip(sequence[1..])
						.Select(p => new ButtonPair(p.First, p.Second))
						.Select(bp => _lookup.GetValueOrDefault(bp)?.SequencesSingle ?? ["A"])
				];

			listOfSequences.AddRange(newListOfSequences.GenerateCombinations());
		}

		return listOfSequences;
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

	private record ButtonPair(char Start, char End)
	{
		public bool IsKeypad0 => $"{Start}{End}".Intersect("0123456789").Any();
		public bool IsKeypad1 => $"{Start}{End}".Intersect("<^v>").Any();
	}

	private record ButtonPairLookup(ButtonPair Pair, Point StartPoint, Point EndPoint, int Distance)
	{
		//public string Sequence => Sequences[0];
		public List<string> Sequences = [.. GetSequences(Pair, StartPoint, EndPoint, Distance)];
		public List<string> SequencesSingle = [.. GetAlternateSequences(Pair, StartPoint, EndPoint, Distance)];

		private static IEnumerable<string> GetAlternateSequences(ButtonPair pair, Point startPoint, Point endPoint, int distance)
		{
			Point delta = endPoint - startPoint;
			char[] result = pair.IsKeypad0
				? [
					..delta.X > 0 ? Enumerable.Repeat('>', delta.X) : [],
					..delta.Y < 0 ? Enumerable.Repeat('^', -delta.Y) : [],
					..delta.Y > 0 ? Enumerable.Repeat('v', delta.Y) : [],
					..delta.X < 0 ? Enumerable.Repeat('<', -delta.X) : [],
					'A']
				: [
					..delta.X > 0 ? Enumerable.Repeat('>', delta.X) : [],
					..delta.Y < 0 ? Enumerable.Repeat('^', -delta.Y) : [],
					..delta.Y > 0 ? Enumerable.Repeat('v', delta.Y) : [],
					..delta.X < 0 ? Enumerable.Repeat('<', -delta.X) : [],
					'A'];

			yield return new string(result);
		}

		private static List<string> GetSequences(ButtonPair pair, Point startPoint, Point endPoint, int distance)
		{
			List<string> sequences = pair.IsKeypad0
				? [.. _keypad0.FindAllShortestPaths(startPoint, endPoint, distance)]
				: [.. _keypad1.FindAllShortestPaths(startPoint, endPoint, distance)];
			int min = sequences.Select(s => s.Length).Min();

			return [.. sequences.Where(s => s.Length == min)];
		}
	}


	static IEnumerable<string> FindAllShortestPaths(this char[,] keypad, Point start, Point end, int distance)
	{
		Queue<(Point, List<char>)> queue =[];
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


	private static int NoOfRobots(this object[]? args) => GetArgument(args, 1, 3);

}
