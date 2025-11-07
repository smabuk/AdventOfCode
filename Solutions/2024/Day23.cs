
namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 23: LAN Party
/// https://adventofcode.com/2024/day/23
/// </summary>
[Description("LAN Party")]
public static partial class Day23 {

	private static List<Computer> _computers = [];
	private static ILookup<Computer, Computer> _lan = default!;
	private static HashSet<Pair> _connections = [];

	[Init]
	public static void LoadComputers(string[] input)
	{
		_computers = [..input.Select(i => new Computer(i[..2])).Concat(input.Select(i => new Computer(i[3..]))).Distinct()];
		List<Pair> pairs = [..input.Select(i => new Pair(new Computer(i[..2]), new Computer(i[3..])))];

		_lan = pairs.Concat(pairs.Select(p => new Pair(p.Computer2, p.Computer1))).ToLookup(p => p.Computer1, p => p.Computer2);
		_connections = [ ..pairs.Concat(pairs.Select(p => new Pair(p.Computer2, p.Computer1)))];
	}

	public static int Part1(string[] _, params object[]? args)
	{
		return _computers
			.Combinations(3)
			.Count(combination => combination.Any(c => c.Name.StartsWith("t")) && IsTriumvarate(combination, _connections));
		//return _lan
		//	.Combinations(3)
		//	.Count(combination => combination.Any(c => c.Key.Name.StartsWith("t")) && IsTriumvarate(combination));
	}

	private static bool IsTriumvarate(IEnumerable<Computer> combination, HashSet<Pair> _connections)
	{
		List<Computer> computers = [.. combination];
		return
			   _connections.Contains(new Pair(computers[0], computers[1]))
			&& _connections.Contains(new Pair(computers[0], computers[2]))
			&& _connections.Contains(new Pair(computers[1], computers[2]))
			;
	}

	private static bool IsTriumvarate(IEnumerable<IGrouping<Computer, Computer>> combination)
	{
		List<IGrouping<Computer, Computer>> computers = [.. combination];

		return
			   computers[0].Contains(computers[1].Key) && computers[0].Contains(computers[2].Key)
			&& computers[1].Contains(computers[0].Key) && computers[1].Contains(computers[2].Key)
			&& computers[2].Contains(computers[0].Key) && computers[2].Contains(computers[1].Key)
			;
	}

	public static string Part2(string[] input, params object[]? args) => NO_SOLUTION_WRITTEN_MESSAGE;


	public sealed record Pair(Computer Computer1, Computer Computer2);

	public sealed record Computer(string Name) : IParsable<Computer>
	{
		public static Computer Parse(string s, IFormatProvider? provider) => new(s);

		public static Computer Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Computer result)
			=> ISimpleParsable<Computer>.TryParse(s, provider, out result);
	}
}
