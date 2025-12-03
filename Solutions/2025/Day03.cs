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

	public static int Part1() => _batteryBanks.Sum(bank => bank.Joltage);

	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;


	private sealed record Bank(List<Battery> Batteries) : IParsable<Bank>
	{
		public int Joltage {
			get {
				int highJoltage1 = Batteries.Take(Batteries.Count - 1).Max(b => b.Joltage);
				int maxVoltage = int.MinValue;
				for (int index = 0; index < Batteries.Count - 1; index++) {
					if (Batteries[index].Joltage == highJoltage1) {
						int highJoltage2 = Batteries.Skip(index + 1).Max(b => b.Joltage);
						int voltage = (highJoltage1 * 10) + highJoltage2;
						if (voltage > maxVoltage) {
							maxVoltage = voltage;
						}
					}
				}

				return maxVoltage;
			}
		}

		public static Bank Parse(string s, IFormatProvider? provider)
			=> new([.. s.AsDigits<int>().Select(dig => new Battery(dig))]);

		public static Bank Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Bank result)
			=> ISimpleParsable<Bank>.TryParse(s, provider, out result);
	}

	private record Battery(int Joltage);
}
