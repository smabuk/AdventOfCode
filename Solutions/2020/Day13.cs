namespace AdventOfCode.Solutions._2020;

/// <summary>
/// Day 13: Shuttle Search
/// https://adventofcode.com/2020/day/13
/// </summary>
[Description("Shuttle Search")]
public class Day13 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static long Solution1(string[] input) {
		int arrivalTime = int.Parse(input[0]);
		int[] buses = input[1]
			.Split(',')
			.Where(b => b != "x")
			.Select(i => int.Parse(i))
			.ToArray();

		int busiD = 0;

		int currentTime = arrivalTime;
		bool lookingForBus = true;
		do {
			currentTime++;
			busiD = buses.Where(b => (currentTime % b) == 0).SingleOrDefault();
			if (busiD != 0) {
				lookingForBus = false;
				break;
			}
		} while (lookingForBus);

		int timeToWait = currentTime - arrivalTime;
		return timeToWait * busiD;
	}

	record Bus(string BusNo, int Value, int Offset);

	private static long Solution2(string[] input) {
		Bus[] buses = input[1]
			.Split(',')
			.Select((busNo, i) => new Bus(busNo, 9999, i))
			.Where(bus => bus.BusNo != "x")
			.Select(bus => bus with { Value = int.Parse(bus.BusNo) })
			.ToArray();

		long step = buses[0].Value;
		long time = step;
		for (int i = 1; i < buses.Length;) {
			time += step;
			if (Enumerable.Range(0, i + 1)
				.All(x => (time + buses[x].Offset) % (buses[x].Value) == 0)) {
				step *= buses[i].Value;
				i++;
			}
		}

		return time;
	}
}
