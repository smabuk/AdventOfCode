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

	private static Action<string[], bool>? _visualise = null;
	private static IEnumerable<Instruction> _instructions = [];

	[Init]
	public static void LoadInstructions(string[] input, Action<string[], bool>? visualise = null)
	{
		_visualise = visualise;
		_instructions = [.. input.As<Instruction>()];
	}

	public static int Part1()
	{
		Dial dial = new(START_POSITION, NUMBERS_ON_DIAL);
		int count = 0;

		_visualise?.Invoke([$"The dial starts by pointing at {dial.CurrentPosition}."], false);

		foreach (Instruction instruction in _instructions) {
			dial = dial.Rotate(instruction);
			if (dial.CurrentPosition is 0) {
				count++;
			}

			_visualise?.Invoke([$"The dial is rotated {instruction,-4} to point at {dial.CurrentPosition,2}."], false);
		}

		return count;
	}

	public static int Part2()
	{
		Dial dial = new(START_POSITION, NUMBERS_ON_DIAL);
		int count = 0;

		_visualise?.Invoke([$"The dial starts by pointing at {dial.CurrentPosition}."], false);

		foreach (Instruction instruction in _instructions) {
			int rotations = dial.CountZeros(instruction);
			count += rotations;

			dial = dial.Rotate(instruction);

			if (rotations is 0) {
				_visualise?.Invoke([$"The dial is rotated {instruction,-4} to point at {dial.CurrentPosition,2}."], false);
			} else {
				_visualise?.Invoke([$"The dial is rotated {instruction,-4} to point at {dial.CurrentPosition,2}; during this rotation, it points at 0  {rotations,3} time(s)."], false);
			}
		}

		return count;
	}

	[GenerateIParsable]
	private sealed partial record Instruction(RotationDirection Direction, int Distance)
	{
		public int Delta => Direction is RotationDirection.Left ? -Distance : Distance;

		public override string ToString() => $"{Direction}{Distance}";

		public static Instruction Parse(string s) => new(s[0].AsEnum<RotationDirection>(), s[1..].As<int>());
	}

	private sealed record Dial(int CurrentPosition, int NumbersOnDial)
	{
		public Dial Rotate(Instruction instruction)
			=> this with { CurrentPosition = (((CurrentPosition + instruction.Delta) % NumbersOnDial) + NumbersOnDial) % NumbersOnDial };

		public int CountZeros(Instruction instruction)
		{
			// Full rotations
			int rotations = instruction.Distance / NUMBERS_ON_DIAL;

			// Partial rotation
			int partialDistance = instruction.Delta % NUMBERS_ON_DIAL;
			rotations += partialDistance switch
			{
				0 => 0,
				_ when CurrentPosition + partialDistance >= NUMBERS_ON_DIAL => 1,
				_ when CurrentPosition is not 0 && CurrentPosition + partialDistance <= 0 => 1,
				_ => 0,
			};

			return rotations;
		}
	}

	private enum RotationDirection
	{
		L,
		R,

		Left = L,
		Right = R,
	}
}
