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
		_junctionBoxes = [.. input.Select(n => n.As<Point3d>())];

		_distances
			= _junctionBoxes
			.Combinations(2)
			.ToDictionary(jbs => jbs.First().EuclideanDistance(jbs.Last()), jbs => (jbs.First(), jbs.Last()));

		_sortedDistances = [.. _distances.Keys.OrderBy(d => d)];
	}

	private static List<Point3d> _junctionBoxes = [];
	private static Dictionary<double, (Point3d First, Point3d Second)> _distances = [];
	private static List<double> _sortedDistances = [];

	public static long Part1(string[] _, object[]? args)
	{
		int noOfPairs = GetArgument(args, 1, 1000);
		List<HashSet<Point3d>> circuits = [];

		for (int i = 0; i < noOfPairs;) {
			(Point3d first, Point3d second) = _distances[_sortedDistances[i++]];
			circuits = CalculateCircuits(circuits, first, second);
		}

		List<int> orderCircuitCounts = [
			.. circuits.Select(circuit => circuit.Count).OrderDescending()
			];

		return orderCircuitCounts[0] * orderCircuitCounts[1] * orderCircuitCounts[2];
	}

	public static long Part2()
	{
		List<HashSet<Point3d>> circuits = [];
		(Point3d First, Point3d Second) lastPair = default;

		int i = 0;
		while (!(circuits.Count == 1 && circuits[0].Count == _junctionBoxes.Count)) {
			lastPair = _distances[_sortedDistances[i++]];
			circuits = CalculateCircuits(circuits, lastPair.First, lastPair.Second);
		}

		return lastPair.First.X * lastPair.Second.X;
	}

	private static List<HashSet<Point3d>> CalculateCircuits(List<HashSet<Point3d>> inputCircuits, Point3d first, Point3d second)
	{
		const int NOT_FOUND = -1;

		List<HashSet<Point3d>> circuits = [.. inputCircuits];

		int firstCircuitIndex = NOT_FOUND;
		int secondCircuitIndex = NOT_FOUND;

		for (int i = 0; i < circuits.Count; i++) {
			if (circuits[i].Contains(first)) {
				firstCircuitIndex = i;
			}
			if (circuits[i].Contains(second)) {
				secondCircuitIndex = i;
			}
		}

		if (firstCircuitIndex == NOT_FOUND && secondCircuitIndex == NOT_FOUND) {        // Neither in any circuit - create new circuit
			circuits.Add([first, second]);
		} else if (firstCircuitIndex != NOT_FOUND && secondCircuitIndex == NOT_FOUND) { // Only first is in a circuit - add second to it
			_ = circuits[firstCircuitIndex].Add(second);
		} else if (firstCircuitIndex == NOT_FOUND && secondCircuitIndex != NOT_FOUND) { // Only second is in a circuit - add first to it
			_ = circuits[secondCircuitIndex].Add(first);
		} else if (firstCircuitIndex != secondCircuitIndex) {                           // Both in different circuits - merge them
			circuits[firstCircuitIndex].UnionWith(circuits[secondCircuitIndex]);
			circuits.RemoveAt(secondCircuitIndex);
		}

		return circuits;
	}
}

file static partial class Day08Extensions
{
	extension(Point3d point)
	{
		public double EuclideanDistance(Point3d other)
		{
			double x1 = point.X, y1 = point.Y, z1 = point.Z;
			double x2 = other.X, y2 = other.Y, z2 = other.Z;
			return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
		}
	}
}
