namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 03: Lobby
/// https://adventofcode.com/2025/day/03
/// </summary>
[Description("Lobby")]
public partial class Day03 {

	private static IEnumerable<Bank> _batteryBanks = [];

	[Init]
	public static void LoadBatteryBanks(string[] input) => _batteryBanks = [.. input.As<Bank>()];

	public static long Part1() => _batteryBanks.Sum(bank => bank.Joltage(2));
	public static long Part2() => _batteryBanks.Sum(bank => bank.Joltage(12));

	private sealed record Battery(int Joltage);

	[GenerateIParsable]
	private sealed partial record Bank(List<Battery> Batteries)
	{
		public long Joltage(int noOfBatteries)
		{
			long joltage = 0;
			int batteriesNeeded = noOfBatteries;
			int startIndex = 0;

			while (batteriesNeeded > 0) {
				int lookAheadWindowSize = Batteries.Count - startIndex - batteriesNeeded + 1;

				int maxJoltage = int.MinValue;
				int maxIndex = startIndex;

				for (int i = startIndex; i < startIndex + lookAheadWindowSize; i++) {
					if (Batteries[i].Joltage > maxJoltage) {
						maxJoltage = Batteries[i].Joltage;
						maxIndex = i;
					}
				}

				startIndex = maxIndex + 1;
				batteriesNeeded--;

				joltage += maxJoltage * batteriesNeeded.Pow10;
			}

			return joltage;
		}

		public static Bank Parse(string s) => new([.. s.AsDigits<int>().Select(digit => new Battery(digit))]);
	}
}
