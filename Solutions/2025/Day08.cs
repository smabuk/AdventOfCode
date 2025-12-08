namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 08: Playground
/// https://adventofcode.com/2025/day/08
/// </summary>
[Description("Playground")]
[GenerateVisualiser]
public partial class Day08
{

	[Init]
	public static void LoadJunctionBoxes(string[] input) => _junctionBoxes = [.. input.Select(n => n.As<Point3d>())];
	private static List<Point3d> _junctionBoxes = [];

	public static long Part1(string[] input, object[]? args)
	{
		const int NOT_FOUND = -1;

		int noOfPairs = GetArgument(args, 1, 1000);

		Dictionary<double, (Point3d First, Point3d Second)> distances
			= _junctionBoxes
			.Combinations(2)
			.Where(jbs => jbs.First() != jbs.Last())
			.ToDictionary(jbs => jbs.First().EuclideanDistance(jbs.Last()), jbs => (jbs.First(), jbs.Last()));

		List<double> sortedDistances = [.. distances.Keys.OrderBy(d => d)];
		List<List<Point3d>> circuits = [];

		int i = 0;
		while (i < noOfPairs) {
			double distance = sortedDistances[i];
			(Point3d first, Point3d second) = distances[sortedDistances[i]];

			int firstCircuitIndex = NOT_FOUND;
			int secondCircuitIndex = NOT_FOUND;

			for (int j = 0; j < circuits.Count; j++) {
				if (circuits[j].Contains(first)) {
					firstCircuitIndex = j;
				}
				if (circuits[j].Contains(second)) {
					secondCircuitIndex = j;
				}
			}

			if (firstCircuitIndex == NOT_FOUND && secondCircuitIndex == NOT_FOUND) {        // Neither in any circuit - create new circuit
				circuits.Add([first, second]);
			} else if (firstCircuitIndex != NOT_FOUND && secondCircuitIndex == NOT_FOUND) { // Only first is in a circuit - add second to it
				circuits[firstCircuitIndex].Add(second);
			} else if (firstCircuitIndex == NOT_FOUND && secondCircuitIndex != NOT_FOUND) { // Only second is in a circuit - add first to it
				circuits[secondCircuitIndex].Add(first);
			} else if (firstCircuitIndex != secondCircuitIndex) {                           // Both in different circuits - merge them
				circuits[firstCircuitIndex].AddRange(circuits[secondCircuitIndex]);
				circuits.RemoveAt(secondCircuitIndex);
			}

			i++;
		}

		List<int> orderCircuitCounts = [.. circuits
			.Select(circuit => circuit.Count)
			.OrderByDescending(count => count)];

		return orderCircuitCounts[0] * orderCircuitCounts[1] * orderCircuitCounts[2];
	}

	public static long Part2()
	{
		const int NOT_FOUND = -1;

		Dictionary<double, (Point3d First, Point3d Second)> distances
			= _junctionBoxes
			.Combinations(2)
			.Where(jbs => jbs.First() != jbs.Last())
			.ToDictionary(jbs => jbs.First().EuclideanDistance(jbs.Last()), jbs => (jbs.First(), jbs.Last()));

		List<double> sortedDistances = [.. distances.Keys.OrderBy(d => d)];
		List<List<Point3d>> circuits = [];
		(Point3d First, Point3d Second) lastPair = default;

		int i = 0;
		while (!(circuits.Count == 1 && circuits[0].Count == _junctionBoxes.Count)) {
			double distance = sortedDistances[i];
			(Point3d first, Point3d second) = distances[sortedDistances[i]];

			int firstCircuitIndex = NOT_FOUND;
			int secondCircuitIndex = NOT_FOUND;

			for (int j = 0; j < circuits.Count; j++) {
				if (circuits[j].Contains(first)) {
					firstCircuitIndex = j;
				}
				if (circuits[j].Contains(second)) {
					secondCircuitIndex = j;
				}
			}

			if (firstCircuitIndex == NOT_FOUND && secondCircuitIndex == NOT_FOUND) {        // Neither in any circuit - create new circuit
				circuits.Add([first, second]);
			} else if (firstCircuitIndex != NOT_FOUND && secondCircuitIndex == NOT_FOUND) { // Only first is in a circuit - add second to it
				circuits[firstCircuitIndex].Add(second);
			} else if (firstCircuitIndex == NOT_FOUND && secondCircuitIndex != NOT_FOUND) { // Only second is in a circuit - add first to it
				circuits[secondCircuitIndex].Add(first);
			} else if (firstCircuitIndex != secondCircuitIndex) {                           // Both in different circuits - merge them
				circuits[firstCircuitIndex].AddRange(circuits[secondCircuitIndex]);
				circuits.RemoveAt(secondCircuitIndex);
			}

			lastPair = (first, second);
			i++;
		}

		return lastPair.First.X * lastPair.Second.X;
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
			double distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
			return distance;
		}
	}
	extension(Point3d)
	{
		public static double operator -(Point3d point, Point3d other) => point.EuclideanDistance(other);
	}
}
