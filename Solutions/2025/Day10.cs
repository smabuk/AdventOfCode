using Google.OrTools.LinearSolver;

namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 10: Factory
/// https://adventofcode.com/2025/day/10
/// </summary>
[Description("Factory")]
[GenerateVisualiser]
public partial class Day10
{

	[Init]
	public static void LoadMachines(string[] input) => _machines = [.. input.Select(Machine.Parse)];
	private static List<Machine> _machines = [];

	public static int Part1()
	{
		int fewestTotalPresses = 0;
		VisualiseString("");
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
						VisualiseString($"Machine reached desired state in {presses,2} presses {newMachine.DesiredLightState}.");
						break;
					}
				}
			}
		}

		return fewestTotalPresses;
	}

	public static int Part2()
	{
		int fewestTotalPresses = 0;
		int count = 0;
		VisualiseString("");
		VisualiseString($"Total Machines: {_machines.Count}");
		foreach (Machine machine in _machines) {
			int presses = machine.FindMinimumPressesForJoltage();
			fewestTotalPresses += presses;
			count++;
			VisualiseString($"{count,3}/{_machines.Count,3} Machine reached desired joltage state in {presses,3} presses {{{string.Join(',', machine.Joltages)}}}.");
		}

		return fewestTotalPresses;
	}

	private sealed partial record Machine(string DesiredLightState, List<Button> Buttons, int[] Joltages)
	{
		public string CurrentLightState { get; init; } = new('.', DesiredLightState.Length);
		public bool IsStarted => CurrentLightState == DesiredLightState;
		public bool IsNotStarted => CurrentLightState != DesiredLightState;

		public int[] CurrentJoltageState { get; init; } = new int[Joltages.Length];

		public Machine PressButton(Button button)
		{
			char[] newLightState = CurrentLightState.ToCharArray();
			foreach (int lightIndex in button.Values) {
				newLightState[lightIndex] = newLightState[lightIndex] == '.' ? '#' : '.';
			}

			return this with { CurrentLightState = new string(newLightState) };
		}

		/// <summary>
		/// Finds minimum button presses using Google OR-Tools solver
		/// </summary>
		public int FindMinimumPressesForJoltage()
		{
			int numJoltages = Joltages.Length;
			int numButtons = Buttons.Count;

			// Create the linear solver with CBC backend
			using Solver solver = Solver.CreateSolver("SCIP") ?? throw new ApplicationException("Could not create solver.");

			// Create integer variables for button press counts
			Variable[] buttonVars = new Variable[numButtons];
			for (int i = 0; i < numButtons; i++) {
				// Variable with lower bound 0, upper bound infinity
				buttonVars[i] = solver.MakeIntVar(0.0, double.PositiveInfinity, $"button_{i}");
			}

			// Add constraints: for each joltage, sum of affecting buttons = target
			for (int j = 0; j < numJoltages; j++) {
				Constraint constraint = solver.MakeConstraint(Joltages[j], Joltages[j], $"joltage_{j}");

				for (int b = 0; b < numButtons; b++) {
					if (Buttons[b].Values.Contains(j)) {
						constraint.SetCoefficient(buttonVars[b], 1);
					}
				}
			}

			// Objective: minimize sum of all button presses
			Objective objective = solver.Objective();
			for (int i = 0; i < numButtons; i++) {
				objective.SetCoefficient(buttonVars[i], 1);
			}
			objective.SetMinimization();

			// Solve
			Solver.ResultStatus resultStatus = solver.Solve();

			if (resultStatus == Solver.ResultStatus.OPTIMAL) {
				int total = 0;

				for (int i = 0; i < numButtons; i++) {
					total += (int)Math.Round(buttonVars[i].SolutionValue());
				}

				return total;
			}

			throw new ApplicationException($"No solution found for joltage configuration. Status: {resultStatus}");
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

		public override string ToString() => $"[{CurrentLightState}] ({string.Join(") (", Buttons)}) {{{string.Join(',', CurrentJoltageState)}}}";
	}

	private sealed partial record Button(int[] Values)
	{
		public static Button Parse(string s) => new([.. s.AsNumbers<int>()]);
		public override string ToString() => $"({string.Join(',', Values)})";
	}
}

file static class Day10_Extensions
{
}
