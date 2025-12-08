using static AdventOfCode.Solutions._2025.Day08;

using Circuits = System.Collections.Generic.List<System.Collections.Generic.HashSet<AdventOfCode.Solutions._2025.Day08.JunctionBox>>;

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
			.ToDictionary(jbs => jbs[0].EuclideanDistance(jbs[1]), jbs => new JunctionBoxPair(jbs[0], jbs[1]));

		_sortedDistances = [.. _distances.Keys.OrderBy(d => d)];
	}

	private static List<JunctionBox> _junctionBoxes = [];
	private static Dictionary<double, JunctionBoxPair> _distances = [];
	private static List<double> _sortedDistances = [];

	public static long Part1(string[] _, object[]? args)
	{
		int noOfPairs = GetArgument(args, 1, 1000);

		Circuits circuits
			= ConnectCircuits<Circuits>((currentCircuits, pairsProcessed) => pairsProcessed >= noOfPairs);

		List<int> orderCircuitCounts = [
			.. circuits.Select(circuit => circuit.Count).OrderDescending().Take(3)
			];

		return orderCircuitCounts[0] * orderCircuitCounts[1] * orderCircuitCounts[2];
	}

	public static long Part2()
	{
		JunctionBoxPair lastPair = ConnectCircuits<JunctionBoxPair>(
			(currentCircuits, _) => currentCircuits.Count == 1 && currentCircuits[0].Count == _junctionBoxes.Count
		);

		return lastPair.First.X * lastPair.Second.X;
	}

	internal record JunctionBoxPair(JunctionBox First, JunctionBox Second);

	// Violates all laws of C# generics by returning different types based on use case,
	// but it works and it makes the calling side cleaner.
	private static T ConnectCircuits<T>(Func<Circuits, int, bool> shouldStop)
	{
		const int NOT_FOUND = -1;

		Circuits circuits = [];
		JunctionBox first = default;
		JunctionBox second = default;
		int pairsProcessed = 0;

		while (!shouldStop(circuits, pairsProcessed)) {
			(first, second) = _distances[_sortedDistances[pairsProcessed++]];

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
		}

		object result;
		if (typeof(T) == typeof(Circuits)) {
			result = circuits;
		} else if (typeof(T) == typeof(JunctionBoxPair)) {
			result = new JunctionBoxPair(first, second);
		} else {
			throw new InvalidOperationException($"Unsupported return type {typeof(T).FullName}");
		}

		return (T)result;
	}

	[GenerateIParsable]
	internal partial record struct JunctionBox(int X, int Y, int Z)
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
