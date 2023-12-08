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

	private static long Solution2(string[] input) {
		string instructions = input[0];

		Dictionary<string, Node> nodeMap = input[2..]
			.As<Node>()
			.ToDictionary(node => node.Name, node => node);

		int steps = 0;
		Node[] currentNodes = [.. nodeMap.Values.Where(kvp => kvp.Name.EndsWith('A'))];
		long[] cycleLengths = [.. Enumerable.Repeat(int.MinValue, currentNodes.Length)];
		do {
			char instruction = instructions[steps++ % instructions.Length];
			for (int i = 0; i < currentNodes.Length; i++) {
				string nextNode = instruction == LEFT ? nodeMap[currentNodes[i].Name].LeftNode : nodeMap[currentNodes[i].Name].RightNode;
				currentNodes[i] = nodeMap[nextNode];
				if (cycleLengths[i] < 0 && currentNodes[i].Name.EndsWith('Z')) {
					cycleLengths[i] = steps;
				}
			}
		} while (cycleLengths.Any(cycle => cycle < 0));

		return cycleLengths.LowestCommonMultiple();
	}

	private record Node(string Name, string LeftNode, string RightNode) : IParsable<Node> {
		public static Node Parse(string s, IFormatProvider? provider) => new(s[0..3], s[7..10], s[12..15]);
		public static Node Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Node result)
			=> ISimpleParsable<Node>.TryParse(s, provider, out result);
	}
}

file static class Day08Helpers
{
	public static long LowestCommonMultiple(this long[] numbers) => numbers.Aggregate(LowestCommonMultipleOf2Numbers);

	/// <summary>
	/// lcm calculation uses Abs(a*b)/gcd(a,b) , refer to Reduction by the greatest common divisor.
	/// http://en.wikipedia.org/wiki/Least_common_multiple
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static long GreatestCommonDenominator(long a, long b) => b == 0 ? a : GreatestCommonDenominator(b, a % b);

	/// <summary>
	///  Uses the Euclidean algorithm
	///  https://en.wikipedia.org/wiki/Euclidean_algorithm
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static long LowestCommonMultipleOf2Numbers(long a, long b) => Math.Abs(a * b) / GreatestCommonDenominator(a, b);
}
