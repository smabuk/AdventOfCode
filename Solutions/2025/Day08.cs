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
	private static Dictionary<double, JunctionBoxPair> _distances = [];
	private static int _junctionBoxCount = 0;
	private static List<double> _sortedDistances = [];

	[Init]
	public static void LoadJunctionBoxes(string[] input)
	{
		_junctionBoxCount = input.Length;

		_distances
			= input
			.Select(JunctionBox.Parse)
			.Combinations(2)
			.Select(jbs => new JunctionBoxPair(jbs[0], jbs[1]))
			.ToDictionary(jbp => jbp.EuclideanDistance(), jbp => jbp);

		_sortedDistances = [.. _distances.Keys.OrderBy(d => d)];
	}

	public static long Part1(string[] _, object[]? args)
	{
		return ConnectCircuitsNTimes(GetArgument(args, 1, 1000))
			.Select(circuit => circuit.Count)
			.ProductOfTop3();
	}

	public static long Part2() => ConnectCircuitsUntilFullyConnected().XProductOfLastPair();



	private static Circuits ConnectCircuitsNTimes(int n) => ConnectCircuits<Circuits>((currentCircuits, pairsProcessed) => pairsProcessed >= n);
	private static JunctionBoxPair ConnectCircuitsUntilFullyConnected() => ConnectCircuits<JunctionBoxPair>((currentCircuits, _) => currentCircuits.Count == 1 && currentCircuits[0].Count == _junctionBoxCount);

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

	internal record struct JunctionBox(int X, int Y, int Z);
	internal sealed record JunctionBoxPair(JunctionBox First, JunctionBox Second);
}

file static partial class Day08Extensions
{
	extension(IEnumerable<int> numbers)
	{
		public long ProductOfTop3()
			=> numbers
			.OrderDescending()
			.ToArray() is [int n1, int n2, int n3, ..]
				? n1 * n2 * n3
				: 0;
	}

	extension(JunctionBoxPair pair)
	{
		public long XProductOfLastPair() => pair.First.X * pair.Second.X;
	}

	extension(JunctionBoxPair junctionBoxPair)
	{
		public double EuclideanDistance()
		{
			double x1 = junctionBoxPair.First.X, y1 = junctionBoxPair.First.Y, z1 = junctionBoxPair.First.Z;
			double x2 = junctionBoxPair.Second.X, y2 = junctionBoxPair.Second.Y, z2 = junctionBoxPair.Second.Z;
			return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
		}
	}

	extension(JunctionBox)
	{
		public static JunctionBox Parse(string s)
		{
			int[] numbers = [.. s.AsNumbers<int>()];
			return new(numbers[0], numbers[1], numbers[2]);
		}
	}
}
