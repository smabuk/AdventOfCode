
namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 10: Factory
/// https://adventofcode.com/2025/day/10
/// </summary>
[Description("Factory")]
//[GenerateVisualiser]
public partial class Day10
{

	[Init]
	public static void LoadMachines(string[] input) => _machines = [.. input.Select(Machine.Parse)];
	private static List<Machine> _machines = [];

	public static int Part1()
	{
		int fewestTotalPresses = 0;
		foreach (Machine machine in _machines) {
			int presses = 0;
			Machine newMachine = machine;

			while (newMachine.IsNotStarted) {
				presses++;

				IEnumerable<Button[]> buttons = machine.Buttons.Permute(presses);
				foreach (Button[] buttonCombo in buttons) {
					newMachine = machine;
					foreach (Button button in buttonCombo) {
						newMachine = newMachine.PressButton(button);
					}

					if (newMachine.IsStarted) {
						fewestTotalPresses += presses;
						break;
					}
				}
			}
		}

		return fewestTotalPresses;
	}

	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;

	private sealed partial record Machine(string DesiredLightState, List<Button> Buttons, int[] Joltages)
	{
		public string CurrentLightState { get; init; } = new('.', DesiredLightState.Length);
		public bool IsStarted => CurrentLightState == DesiredLightState;
		public bool IsNotStarted => CurrentLightState != DesiredLightState;

		public Machine PressButton(Button button)
		{
			char[] newLightState = CurrentLightState.ToCharArray();
			foreach (int lightIndex in button.Lights) {
				newLightState[lightIndex] = newLightState[lightIndex] == '.' ? '#' : '.';
			}

			return this with { CurrentLightState = new string(newLightState) };
		}

		public static Machine Parse(string s)
		{
			string[] parts = s.TrimmedSplit(['[', ']', '{', '}']);

			return new Machine(
				parts[0],
				[.. parts[1].TrimmedSplit(' ').Select(Button.Parse)],
				[.. parts[2].TrimmedSplit(',').Select(int.Parse)]
			);
		}

		public override string ToString() => $"[{CurrentLightState}] ({string.Join(") (", Buttons)}) {{{string.Join(',', Joltages)}}}";
	}

	private sealed partial record Button(int[] Lights)
	{
		public static Button Parse(string s) => new([.. s.AsNumbers<int>()]);
		public override string ToString() => $"({string.Join(',', Lights)})";
	}
}
