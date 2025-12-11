using static AdventOfCode.Solutions._2025.Day11;

namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 11: Reactor
/// https://adventofcode.com/2025/day/11
/// </summary>
[Description("Reactor")]
[GenerateVisualiser]
public partial class Day11 {

	[Init]
	public static void LoadServerRack(string[] input)
		=> _serverRack = input.Select(i => i.AsConnection()).ToDictionary();
	private static Dictionary<Device, HashSet<Device>> _serverRack = [];

	public static int Part1() => _serverRack.FindAllPaths(new("you"), new("out")).Count();

	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;

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
		public IEnumerable<List<Device>> FindAllPaths(Device start, Device target)
		{
			List<List<Device>> paths = [];
			Queue<List<Device>> queue = new();
			queue.Enqueue([start]);
			while (queue.Count > 0)
			{
				List<Device> path = queue.Dequeue();
				Device lastDevice = path.Last();
				if (lastDevice.Equals(target))
				{
					paths.Add(path);
					continue;
				}

				foreach (Device? connectedTo in rack[lastDevice].Where(connectedDevice => path.DoesNotContain(connectedDevice))) {
					List<Device> newPath = [.. path, connectedTo];
					queue.Enqueue(newPath);
				}
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
