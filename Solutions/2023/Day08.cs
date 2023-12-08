namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 08: Haunted Wasteland
/// https://adventofcode.com/2023/day/08
/// </summary>
[Description("Haunted Wasteland")]
public sealed partial class Day08 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static readonly char LEFT  = 'L';
	//private static readonly char RIGHT = 'R';

	private static int Solution1(string[] input) {
		string instructions = input[0];

		Dictionary<string, Node> nodeMap = input[2..]
			.As<Node>()
			.ToDictionary(node => node.Name, node => node);

		int steps = 0;
		Node currentNode = nodeMap["AAA"];
		do {
			char instruction = instructions[steps++ % instructions.Length];
			string nextNode = instruction == LEFT ? nodeMap[currentNode.Name].LeftNode : nodeMap[currentNode.Name].RightNode;
			currentNode = nodeMap[nextNode];
		} while (currentNode.Name != "ZZZ");

		return steps;
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

	private record Node(string Name, string LeftNode, string RightNode) : IParsable<Node> {
		public static Node Parse(string s, IFormatProvider? provider) => new(s[0..3], s[7..10], s[12..15]);
		public static Node Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Node result)
			=> ISimpleParsable<Node>.TryParse(s, provider, out result);
	}
}
