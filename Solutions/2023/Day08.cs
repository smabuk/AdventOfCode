namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 08: Haunted Wasteland
/// https://adventofcode.com/2023/day/08
/// </summary>
[Description("Haunted Wasteland")]
public sealed partial class Day08 {

	[Init]
	public static    void Init(string[] input, params object[]? args) => LoadInstructionsAndMap(input);
	public static string Part1(string[] input, params object[]? args) => Solution("AAA", "ZZZ").ToString();
	public static string Part2(string[] input, params object[]? args) => Solution("A", "Z").ToString();

	private static string _instructions = "";
	private static Dictionary<string, Node> _nodeMaps = [];

	private static void LoadInstructionsAndMap(string[] input)
	{
		if (input is [string instructions, _, .. string[] nodeMaps]) {
			_instructions = instructions;
			_nodeMaps     = nodeMaps.As<Node>().ToDictionary(node => node.Name, node => node);
		}
	}

	static long Solution(string start, string end)
	{
		const char LEFT = 'L';

		Node[] currentNodes = [.. _nodeMaps.Values.Where(kvp => kvp.Name.EndsWith(start))];
		long[] cycleLengths = new long[currentNodes.Length];

		int steps = 0;
		do {
			char instruction = _instructions[steps++ % _instructions.Length];
			for (int i = 0; i < currentNodes.Length; i++) {
				if (cycleLengths[i] > 0) { continue; }

				string nextNode = instruction == LEFT ? _nodeMaps[currentNodes[i].Name].LeftNode : _nodeMaps[currentNodes[i].Name].RightNode;
				currentNodes[i] = _nodeMaps[nextNode];

				if (currentNodes[i].Name.EndsWith(end)) {
					cycleLengths[i] = steps;
				}
			}
		} while (cycleLengths.Any(cycle => cycle == 0));

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
	///  Uses the Euclidean algorithm
	///  https://en.wikipedia.org/wiki/Euclidean_algorithm
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static long LowestCommonMultipleOf2Numbers(long a, long b) => Math.Abs(a * b) / GreatestCommonDenominator(a, b);

	/// <summary>
	/// lcm calculation uses Abs(a*b)/gcd(a,b) , refer to Reduction by the greatest common divisor.
	/// http://en.wikipedia.org/wiki/Least_common_multiple
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static long GreatestCommonDenominator(long a, long b) => b == 0 ? a : GreatestCommonDenominator(b, a % b);
}
