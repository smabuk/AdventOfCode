namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 01: Secret Entrance
/// https://adventofcode.com/2025/day/01
/// </summary>
[Description("Secret Entrance")]
public partial class Day01
{

	const int NUMBERS_ON_DIAL = 100;
	private static IEnumerable<Instruction> _instructions = [];

	[Init]
	public static void LoadInstructions(string[] input) => _instructions = [.. input.As<Instruction>()];

	public static int Part1()
	{
		int current = 50;
		List<int> numbers = [];

		foreach (Instruction instruction in _instructions) {
			current += instruction.Direction == RotationDirection.Left
				? -instruction.Distance
				: instruction.Distance;
			current = ((current % NUMBERS_ON_DIAL) + NUMBERS_ON_DIAL) % NUMBERS_ON_DIAL;
			numbers.Add(current);
		}

		return numbers.Count(num => num is 0);
	}

	public static int Part2()
	{
		int currentPosition = 50;
		int count = 0;

		foreach (Instruction instruction in _instructions) {
			int delta = instruction.Direction == RotationDirection.Left
				? -instruction.Distance
				: instruction.Distance;

			int distance = int.Abs(delta);
			int newPosition = (((currentPosition + delta) % NUMBERS_ON_DIAL) + NUMBERS_ON_DIAL) % NUMBERS_ON_DIAL;

			count += distance / NUMBERS_ON_DIAL;

			// Check if we cross 0 in the partial rotation
			int partialDistance = distance % NUMBERS_ON_DIAL;
			if (partialDistance > 0) {
				if (delta > 0) {
					// Moving right: cross 0 if we wrap around OR end at 0
					if (currentPosition + partialDistance >= NUMBERS_ON_DIAL) {
						count++;
					}
				} else {
					// Moving left: cross 0 if we wrap around (but not if starting at 0)
					if (currentPosition > 0 && currentPosition - partialDistance <= 0) {
						count++;
					}
				}
			}

			currentPosition = newPosition;
		}

		return count;
	}

	private sealed record Instruction(RotationDirection Direction, int Distance) : IParsable<Instruction>
	{
		public static Instruction Parse(string s, IFormatProvider? provider)
			=> new(Enum.Parse<RotationDirection>($"{s[0]}"), int.Parse($"{s[1..]}"));

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	private enum RotationDirection
	{
		L,
		R,

		Left = L,
		Right = R,
	}
}
