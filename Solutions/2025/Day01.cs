namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 01: Secret Entrance
/// https://adventofcode.com/2025/day/01
/// </summary>
[Description("Secret Entrance")]
public partial class Day01
{

	private const int NUMBERS_ON_DIAL = 100;
	private const int START_POSITION = 50;

	private static IEnumerable<Instruction> _instructions = [];

	[Init]
	public static void LoadInstructions(string[] input) => _instructions = [.. input.As<Instruction>()];

	public static int Part1()
	{
		int currentPosition = START_POSITION;
		int count = 0;

		foreach (Instruction instruction in _instructions) {
			currentPosition += instruction;
			if (currentPosition is 0) {
				count++;
			}
		}

		return count;
	}

	public static int Part2()
	{
		int currentPosition = START_POSITION;
		int count = 0;

		foreach (Instruction instruction in _instructions) {
			// Full rotations
			count += instruction.Distance / NUMBERS_ON_DIAL;

			// Partial rotation
			int partialDistance = instruction.Delta % NUMBERS_ON_DIAL;
			count += partialDistance switch
			{
				0 => 0,
				_ when currentPosition + partialDistance >= NUMBERS_ON_DIAL => 1,
				_ when currentPosition is not 0 && currentPosition + partialDistance <= 0 => 1,
				_ => 0,
			};

			currentPosition += instruction;
		}

		return count;
	}

	private sealed record Instruction(RotationDirection Direction, int Distance) : IParsable<Instruction>
	{
		public int Delta => Direction is RotationDirection.Left ? -Distance : Distance;

		public static int operator +(int currentPosition, Instruction instruction)
			=> (((currentPosition + instruction.Delta) % NUMBERS_ON_DIAL) + NUMBERS_ON_DIAL) % NUMBERS_ON_DIAL;

		public static Instruction Parse(string s, IFormatProvider? provider)
			=> new(s[0].ToString().AsEnum<RotationDirection>(), s[1..].As<int>());

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
