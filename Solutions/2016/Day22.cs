using static AdventOfCode.Solutions._2016.Day22Constants;
using static AdventOfCode.Solutions._2016.Day22Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 22: Grid Computing
/// https://adventofcode.com/2016/day/22
/// </summary>
[Description("Grid Computing")]
public sealed partial class Day22 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();
	private static int Solution1(string[] input) {
		return input
			.Skip(2)
			.As<Node>()
			.Permute(2)
			.Count(pair => pair.IsViablePair());
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day22Extensions
{
	public static bool IsViablePair(this IEnumerable<Node> pair)
	{
		Node[] nodes = [.. pair];
		Node nodeA = nodes[0];
		Node nodeB = nodes[1];

		if (nodeA.Used == 0) {
			return false;
		}

		if (nodeA.Name == nodeB.Name) {
			return false;
		}

		if (nodeA.Used > nodeB.Available) {
			return false;
		}

		return true;
	}
}

internal sealed partial class Day22Types
{

	public sealed record Node(string Name, Point Position, int Size, int Used, int Available, int UsePercentage) : IParsable<Node>
	{
		public static Node Parse(string s, IFormatProvider? provider)
		{
			if (s[0] == '/') {
				string[] tokens = s.TrimmedSplit([' ','T','%', '/']);
				string name = tokens[2];
				string[] nameTokens = name.TrimmedSplit(['-','x','y']);
				Point position = new(nameTokens[1].As<int>(), nameTokens[2].As<int>());
				return new(name,
					position,
					tokens[3].As<int>(),
					tokens[4].As<int>(),
					tokens[5].As<int>(),
					tokens[6].As<int>());
			}
			return null!;
		}

		public static Node Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Node result)
			=> ISimpleParsable<Node>.TryParse(s, provider, out result);
	}
}

file static class Day22Constants
{
}
