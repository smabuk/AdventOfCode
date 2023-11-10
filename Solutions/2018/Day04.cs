namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 04: Repose Record
/// https://adventofcode.com/2018/day/04
/// </summary>
[Description("Repose Record")]
public sealed partial class Day04
{

	[Init]
	public static void   Init(string[] input, params object[]? _)  => LoadShifts(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static readonly List<Asleep> _sleeps = [];

	private static void LoadShifts(string[] input)
	{
		List<string> sortedInput = [.. input];
		sortedInput.Sort();

		int currentGuardId = 0;
		TimeOnly start = default;
		TimeOnly end;
		foreach (string line in sortedInput) {
			DateTime dateTime = DateTime.Parse(line[1..17]);
			if (line.Contains("shift")) {
				currentGuardId = GetGuardId(line);
			} else if (line.Contains("asleep")) {
				start = TimeOnly.FromDateTime(dateTime);
			} else if (line.Contains("wakes")) {
				end = TimeOnly.FromDateTime(dateTime);
				Asleep asleep = new(currentGuardId, start, end);
				_sleeps.Add(asleep);
			}
		}

		static int GetGuardId(string input) => int.Parse(input[26..].Split(' ')[0]);
	}

	private static int Solution1(string[] _)
	{
		int guardId = _sleeps
			.GroupBy(x => x.GuardId)
			.Select(x => new
			{
				GuardId = x.Key,
				TotalMinutes = x.Select(sleep => sleep.Duration).Sum()
			})
			.OrderByDescending(x => x.TotalMinutes)
			.Select(x => x.GuardId)
			.First();

		List<Asleep> sleeps = _sleeps.Where(s => s.GuardId == guardId).ToList();

		int bestMinute = 0;
		int bestCount = 0;
		for (int minute = 0; minute < 60; minute++) {
			int count = sleeps.Count(x => x.IsAsleep(minute));
			if (count > bestCount) {
				bestCount = count;
				bestMinute = minute;
			}
		}

		return guardId * bestMinute;
	}

	private static int Solution2(string[] input)
	{
		int bestGuardId = 0;
		int bestMinute = 0;
		int bestCount = 0;
		for (int minute = 0; minute < 60; minute++) {
			var sleepCount = _sleeps
				.GroupBy(x => x.GuardId)
				.Select(x => new
				{
					GuardId = x.Key,
					TimesAsleep = x.Count(sleep => sleep.IsAsleep(minute))
				})
				.OrderByDescending(x => x.TimesAsleep)
				.First();
			if (sleepCount.TimesAsleep > bestCount) {
				bestCount = sleepCount.TimesAsleep;
				bestGuardId = sleepCount.GuardId;
				bestMinute = minute;
			}
		}

		return bestGuardId * bestMinute;
	}

	private record Asleep(int GuardId, TimeOnly Start, TimeOnly End)
	{
		public int Duration => (End - Start).Minutes;
		public bool IsAsleep(int minute) => (Start.Minute <= minute && minute < End.Minute);
	}
}
