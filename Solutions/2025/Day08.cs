using static AdventOfCode.Solutions._2025.Day08;

namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 08: Playground
/// https://adventofcode.com/2025/day/08
/// </summary>
[Description("Playground")]
public partial class Day08
{

	[Init]
	public static void LoadJunctionBoxes(string[] input)
	{
		_junctionBoxes = [.. input.Select(n => n.As<JunctionBox>())];

		_distances
			= _junctionBoxes
			.Combinations(2)
			.ToDictionary(jbs => jbs[0].EuclideanDistance(jbs[1]), jbs => (jbs[0], jbs[1]));

		_sortedDistances = [.. _distances.Keys.OrderBy(d => d)];
	}

	private static List<JunctionBox> _junctionBoxes = [];
	private static Dictionary<double, (JunctionBox First, JunctionBox Second)> _distances = [];
	private static List<double> _sortedDistances = [];

	public static long Part1(string[] _, object[]? args)
	{
		int noOfPairs = GetArgument(args, 1, 1000);
		List<HashSet<JunctionBox>> circuits = [];

		for (int i = 0; i < noOfPairs;) {
			(JunctionBox first, JunctionBox second) = _distances[_sortedDistances[i++]];
			circuits = CalculateCircuits(circuits, first, second);
		}

		List<int> orderCircuitCounts = [
			.. circuits.Select(circuit => circuit.Count).OrderDescending()
			];

		return orderCircuitCounts[0] * orderCircuitCounts[1] * orderCircuitCounts[2];
	}

	public static long Part2()
	{
		List<HashSet<JunctionBox>> circuits = [];
		JunctionBox first = default, second = default;

		int i = 0;
		while (!(circuits.Count == 1 && circuits[0].Count == _junctionBoxes.Count)) {
			(first, second) = _distances[_sortedDistances[i++]];
			circuits = CalculateCircuits(circuits, first, second);
		}

		return first.X * second.X;
	}

	private static List<HashSet<JunctionBox>> CalculateCircuits(List<HashSet<JunctionBox>> inputCircuits, JunctionBox first, JunctionBox second)
	{
		const int NOT_FOUND = -1;

		List<HashSet<JunctionBox>> circuits = [.. inputCircuits];

		int firstCircuitIndex = circuits.FindIndex(c => c.Contains(first));
		int secondCircuitIndex = circuits.FindIndex(c => c.Contains(second));

		switch (firstCircuitIndex, secondCircuitIndex) {
			case (NOT_FOUND, NOT_FOUND):
				circuits.Add([first, second]);
				break;
			case (NOT_FOUND, int secondIdx):
				_ = circuits[secondIdx].Add(first);
				break;
			case (int firstIdx, NOT_FOUND):
				_ = circuits[firstIdx].Add(second);
				break;
			case (int firstIdx, int secondIdx) when firstIdx != secondIdx:
				circuits[firstIdx].UnionWith(circuits[secondIdx]);
				circuits.RemoveAt(secondIdx);
				break;
		}

		return circuits;
	}

	[GenerateIParsable] internal partial record struct JunctionBox(int X, int Y, int Z)
	{
		public static JunctionBox Parse(string s)
		{
			int[] numbers = [.. s.AsNumbers<int>()];
			return new(numbers[0], numbers[1], numbers[2]);
		}
	}
}

file static partial class Day08Extensions
{
	extension(JunctionBox junctionBox)
	{
		public double EuclideanDistance(JunctionBox other)
		{
			double x1 = junctionBox.X, y1 = junctionBox.Y, z1 = junctionBox.Z;
			double x2 = other.X, y2 = other.Y, z2 = other.Z;
			return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
		}
	}
}
