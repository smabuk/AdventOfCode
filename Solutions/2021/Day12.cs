namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 12: Passage Pathing
/// https://adventofcode.com/2021/day/12
/// </summary>
[Description("Passage Pathing")]
public class Day12 {

	record Connection(string From, string To);
	
	record Cave(string Name) {
		public List<Cave> ConnectsTo = new();

		public bool IsSmall => char.IsLower(Name[0]);
		public bool IsBig => char.IsUpper(Name[0]);
	};

	const string START = "start";
	const string END = "end";

	private static int Solution1(string[] input) {
		Dictionary<string, Cave> caves = ParseInput(input);

		return FindAllPaths(caves[START], caves[END], new() { START }, caves, true);
	}

	private static int Solution2(string[] input) {
		Dictionary<string, Cave> caves = ParseInput(input);

		return FindAllPaths(caves[START], caves[END], new () { START }, caves, false);
	}

	private static int FindAllPaths(Cave current, Cave end, List<string> isVisited, Dictionary<string, Cave> caves, bool lookedTwice) {
		if (current.Name == end.Name) {
			return 1;
		}

		int pathCount = 0;

		foreach (Cave cave in caves[current.Name].ConnectsTo.Where(ct => ct.Name != START)) {
			if (cave.IsSmall is false || isVisited.Contains(cave.Name) is false) {
				pathCount += FindAllPaths(cave, end, isVisited.Append(cave.Name).ToList(), caves, lookedTwice);
			} else if (cave.IsSmall && lookedTwice is false) {
				pathCount += FindAllPaths(cave, end, isVisited, caves, true);
			}
		}

		return pathCount;
	}

	private static Dictionary<string, Cave> ParseInput(string[] input) {
		List<Connection> connections = input
			.Select(i => i.Split('-'))
			.Select(i => new Connection(i[0], i[1]))
			.ToList();

		Dictionary<string, Cave> caves = connections
			.Select(c => new Cave(c.From)).Union(connections.Select(c => new Cave(c.To)))
			.DistinctBy(x => x.Name)
			.ToDictionary(k => k.Name, v => v);

		foreach (Connection connection in connections) {
			caves[connection.From].ConnectsTo.Add(new(connection.To));
			caves[connection.To].ConnectsTo.Add(new(connection.From));
		}

		return caves;
	}




	/******************************************************************
	 *          P R O B L E M    I N I T I A L I S A T I O N          *
	 ******************************************************************/
	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		// int arg1 = GetArgument(args, 1, 25);
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		// int arg1 = GetArgument(args, 1, 25);
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
