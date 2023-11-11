namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 08: Memory Maneuver
/// https://adventofcode.com/2018/day/08
/// </summary>
[Description("Memory Maneuver")]
public sealed partial class Day08 {

	[Init]
	public static    void Init(string[] input, params object[]? _) => BuildTree(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static Node _licenseTree = new();

	private static void BuildTree(string[] input) {
		List<int> inputs = input[0].Split(' ').AsInts().ToList();
		(_licenseTree, int _) = GetNode(inputs);
	}

	private static int Solution1(string[] _) {
		return _licenseTree.SumOfMetadataEntries;
	}

	private static int Solution2(string[] input) {
		return _licenseTree.Value;
	}

	private static (Node Node, int Skip) GetNode(List<int> inputs)
	{
		Node node = new();
		int noOfNodes = inputs[0];
		int noOfMetadataEntries = inputs[1];
		int skip = 2;

		for (int index = 0; index < noOfNodes; index++) {
			(Node childNode, int skipMore) = GetNode(inputs[(skip)..]);
			node.Nodes.Add(childNode);
			skip += skipMore;
		}

		for (int index = 0; index < noOfMetadataEntries; index++) {
			node.MetaData.Add(inputs[index + skip]);
		}

		skip += noOfMetadataEntries;

		return (node, skip);
	}

	private record Node
	{
		public List<Node> Nodes = [];
		public List<int> MetaData = [];

		public int SumOfMetadataEntries => Nodes.Sum(n => n.SumOfMetadataEntries) + MetaData.Sum();
		public int Value
		{
			get
			{
				if (Nodes.Count == 0) {
					return MetaData.Sum();
				}
				int value = 0;
				foreach (int metadata in MetaData) {
					if (metadata <= Nodes.Count) {
						value += Nodes[metadata - 1].Value;
					}
				}
				return value;
			}
		}
	}

}
