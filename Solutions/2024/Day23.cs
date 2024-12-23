namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 23: LAN Party
/// https://adventofcode.com/2024/day/23
/// </summary>
[Description("LAN Party")]
public static partial class Day23 {

	private static ILookup<Computer, Computer> _Lan = default!;

	[Init]
	public static void LoadComputers(string[] input)
	{
		List<(Computer, Computer)> pairs = [..input.Select(i => (new Computer(i[..2]), new Computer(i[3..])))];

		_Lan = pairs.Concat(pairs.Select(p => (p.Item2, p.Item1))).ToLookup(p => p.Item1, p => p.Item2);
	}

	public static int Part1(string[] _, params object[]? args)
	{
		return _Lan
			.Combinations(3)
			.Count(combination => combination.Any(c => c.Key.Name.StartsWith("t")) && IsTriumvarate(combination));
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




	public sealed record Computer(string Name) : IParsable<Computer>
	{
		public static Computer Parse(string s, IFormatProvider? provider) => new(s);

		public static Computer Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Computer result)
			=> ISimpleParsable<Computer>.TryParse(s, provider, out result);
	}
}
