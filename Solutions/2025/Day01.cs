namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 01: Secret Entrance
/// https://adventofcode.com/2025/day/01
/// </summary>
[Description("Secret Entrance")]
public partial class Day01
{

	private static IEnumerable<Instruction> _instructions = [];

	[Init]
	public static void LoadInstructions(string[] input) => _instructions = [.. input.As<Instruction>()];

	public static int Part1()
	{
		const int NUMBERS_ON_DIAL = 100;
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

	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;


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
