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


	private sealed record Bank(List<Battery> Batteries) : IParsable<Bank>
	{
		public long Joltage(int noOfBatteries)
		{
			List<int> selectedIndices = [];
			int batteriesNeeded = noOfBatteries;
			int startIndex = 0;

			while (batteriesNeeded > 0) {
				int lookAheadWindow = Batteries.Count - startIndex - batteriesNeeded + 1;

				int maxJoltage = int.MinValue;
				int maxIndex = startIndex;

				for (int i = startIndex; i < startIndex + lookAheadWindow; i++) {
					if (Batteries[i].Joltage > maxJoltage) {
						maxJoltage = Batteries[i].Joltage;
						maxIndex = i;
					}
				}

				selectedIndices.Add(maxIndex);
				startIndex = maxIndex + 1;
				batteriesNeeded--;
			}

			long joltage = 0;
			foreach (int index in selectedIndices) {
				joltage = (joltage * 10) + Batteries[index].Joltage;
			}

			return joltage;
		}

		public static Bank Parse(string s, IFormatProvider? provider)
			=> new([.. s.AsDigits<int>().Select(dig => new Battery(dig))]);

		public static Bank Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Bank result)
			=> ISimpleParsable<Bank>.TryParse(s, provider, out result);
	}

	private record Battery(int Joltage);
}
