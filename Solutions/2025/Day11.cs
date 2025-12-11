using static AdventOfCode.Solutions._2025.Day11;

namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 11: Reactor
/// https://adventofcode.com/2025/day/11
/// </summary>
[Description("Reactor")]
[GenerateVisualiser]
public partial class Day11
{

	[Init]
	public static void LoadServerRack(string[] input)
		=> _serverRack = input.Select(i => i.AsConnection()).ToDictionary();

	private static Dictionary<Device, HashSet<Device>> _serverRack = [];

	public static int Part1() => _serverRack.FindAllPaths(new("you"), new("out")).Count();

	public static long Part2()
	{
		Device svr = new("svr");
		Device dac = new("dac");
		Device fft = new("fft");
		Device end = new("out");

		VisualiseString("");
		VisualiseString("=== DAG Path Counting ===");

		// One of these is guaranteed to fail or we would have loops

		// Count paths: svr → fft → dac → out
		long total = _serverRack.CountPathsViaWaypoints(svr, end, fft, dac);
		VisualiseString($"Paths svr → fft → dac → out: {total:N0}");

		if (total == 0) {
			// Count paths: svr → dac → fft → out
			total = _serverRack.CountPathsViaWaypoints(svr, end, dac, fft);
			VisualiseString($"Paths svr → dac → fft → out: {total:N0}");
		}

		return total;
	}

	[GenerateIParsable]
	internal sealed partial record Device(string Name)
	{
		public static Device Parse(string s) => new(s);
	}
}

file static partial class Day11Extensions
{
	extension(Dictionary<Device, HashSet<Device>> rack)
	{
		/// <summary>
		/// Calculates the total number of distinct paths from the specified start device to the end device that pass through
		/// both waypoint devices in order.
		/// </summary>
		/// <remarks>Paths are counted only if they traverse the waypoints in the specified order: start → waypoint1 →
		/// waypoint2 → end. If there are no paths between any consecutive devices in this sequence, the result is
		/// 0.</remarks>
		/// <param name="start">The device from which each path begins.</param>
		/// <param name="end">The device at which each path ends.</param>
		/// <param name="waypoint1">The first waypoint device that each path must pass through after leaving the start device.</param>
		/// <param name="waypoint2">The second waypoint device that each path must pass through after passing through the first waypoint.</param>
		/// <returns>The number of distinct paths from the start device to the end device that pass through both waypoints in sequence.
		/// Returns 0 if no such path exists.</returns>
		public long CountPathsViaWaypoints(Device start, Device end, Device waypoint1, Device waypoint2)
		{
			long pathsToWp1 = rack.CountPathsDAG(start, waypoint1);
			if (pathsToWp1 == 0) { return 0; }

			long pathsWp1ToWp2 = rack.CountPathsDAG(waypoint1, waypoint2);
			if (pathsWp1ToWp2 == 0) { return 0; }

			long pathsWp2ToEnd = rack.CountPathsDAG(waypoint2, end);
			if (pathsWp2ToEnd == 0) { return 0; }

			return pathsToWp1 * pathsWp1ToWp2 * pathsWp2ToEnd;
		}

		public long CountPathsDAG(Device start, Device end) => CountPathsDAGRecursive(rack, start, end, [], []);

		/// <summary>
		/// Recursively counts the number of distinct paths from the specified starting device to the target device in a
		/// directed acyclic graph (DAG).
		/// </summary>
		/// <remarks>This method assumes the input graph is a DAG. If cycles are detected, they are ignored and do not
		/// contribute to the path count. The method uses memoization to improve performance for large graphs.</remarks>
		/// <param name="graph">A dictionary representing the DAG, where each key is a device and its value is a set of neighboring devices
		/// directly reachable from it.</param>
		/// <param name="current">The device from which to start counting paths.</param>
		/// <param name="target">The destination device for which all distinct paths from the current device are counted.</param>
		/// <param name="dp">A dictionary used for memoization, mapping devices to the number of paths from that device to the target. This
		/// optimizes repeated subproblem calculations.</param>
		/// <param name="visited">A set of devices currently in the recursion stack, used to detect cycles and prevent infinite recursion.</param>
		/// <returns>The total number of distinct paths from the current device to the target device. Returns 0 if no such path exists.</returns>
		private static long CountPathsDAGRecursive(
			Dictionary<Device, HashSet<Device>> graph,
			Device current,
			Device target,
			Dictionary<Device, long> dp,
			HashSet<Device> visited)
		{
			if (current.Equals(target)) { return 1; }
			if (dp.TryGetValue(current, out long cached)) { return cached; }
			if (visited.Contains(current)) { return 0; }

			_ = visited.Add(current);
			long pathCount = 0;

			if (graph.TryGetValue(current, out HashSet<Device>? nextDevices)) {
				foreach (Device next in nextDevices) {
					pathCount += CountPathsDAGRecursive(graph, next, target, dp, visited);
				}
			}

			_ = visited.Remove(current);
			dp[current] = pathCount;

			return pathCount;
		}

		/// <summary>
		/// Finds all possible paths between the specified start and target devices within the rack network.
		/// </summary>
		/// <remarks>The method limits the maximum path length to 200 devices and the total number of paths explored
		/// to 100,000,000 to prevent excessive resource usage. Paths will not revisit the same device within a single path.
		/// If the start device is not present in the rack, the method returns an empty collection.</remarks>
		/// <param name="start">The device from which to begin searching for paths.</param>
		/// <param name="target">The device to which all paths should lead.</param>
		/// <returns>An enumerable collection of lists, where each list represents a distinct path from the start device to the target
		/// device. If no paths are found, the collection is empty.</returns>
		/// <exception cref="ApplicationException">Thrown if the exploration limit is reached before all possible paths are found.</exception>
		public IEnumerable<List<Device>> FindAllPaths(Device start, Device target)
		{
			if (!rack.ContainsKey(start)) {
				return [];
			}

			List<List<Device>> paths = [];
			Queue<List<Device>> queue = new();
			queue.Enqueue([start]);

			int maxPathLength = 200;
			long maxPathsToExplore = 100_000_000;
			long pathsExplored = 0;

			while (queue.Count > 0 && pathsExplored < maxPathsToExplore) {
				List<Device> path = queue.Dequeue();
				pathsExplored++;

				if (path.Count > maxPathLength) {
					continue;
				}

				Device lastDevice = path.Last();
				if (lastDevice.Equals(target)) {
					paths.Add(path);
					continue;
				}

				if (!rack.ContainsKey(lastDevice)) {
					continue;
				}

				foreach (Device? connectedTo in rack[lastDevice].Where(connectedDevice => path.DoesNotContain(connectedDevice))) {
					List<Device> newPath = [.. path, connectedTo];
					queue.Enqueue(newPath);
				}
			}

			if (pathsExplored >= maxPathsToExplore) {
				throw new ApplicationException($"WARNING: Hit exploration limit for {start.Name} → {target.Name}. Found {paths.Count} paths so far.");
			}

			return paths;
		}
	}

	extension(string s)
	{
		public KeyValuePair<Device, HashSet<Device>> AsConnection()
		{
			Device[] devices = [.. s.TrimmedSplit([':', ' ']).Select(Device.Parse)];
			return new(devices[0], [.. devices[1..]]);
		}
	}
}
