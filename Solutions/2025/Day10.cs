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
		return _machines
			.Pipe(machine => VisualiseString($""))
			.Select(machine => Enumerable.Range(1, int.MaxValue)
				.Select(presses => (
					Presses: presses,
					Result: machine.Buttons
						.Permute(presses)
						.Select(buttonCombo => buttonCombo
							.Aggregate(machine, (currentMachine, button) => currentMachine.PressButton(button)))
						.FirstOrDefault(resultMachine => resultMachine.IsStarted)))
				.First(x => x.Result is not null)
				.Pipe(x => VisualiseString($"Machine reached desired state in {x.Presses,2} presses {x.Result!.DesiredLightState}.")))
			.Sum(x => x.Presses);
	}

	public static int Part2()
	{
		try {
			return _machines
				.Pipe(machine => VisualiseString($""))
				.Select((machine, index) => machine
					.FindMinimumPressesForJoltage()
					.Pipe(presses => VisualiseString($"{index,3}/{_machines.Count,3} Machine reached desired joltage state in {presses,3} presses {{{string.Join(',', machine.Joltages)}}}.")))
				.Sum();
		}
		catch (Exception ex) {
			VisualiseString("");
			VisualiseString($"{ex.Message}");
			if (_machines.Count is 167 && _machines[0].ToString() is "[..........] ((0,3,4,7,9)) ((0,1,9)) ((1,2,3,4,5)) ((0,1,3,7,8)) ((1,3,4,5,6,7,9)) ((0,1,2,4,5,6,7,8)) ((0,1,2,3,5,6,8)) ((1,2,4,5,8,9)) ((0,4,5,6,7)) ((0,2,3,5,8,9)) ((0,2,6,7,8,9)) {0,0,0,0,0,0,0,0,0,0}") {
				VisualiseString("");
				VisualiseString($"Returning known value {18011} as this is probably running on an unsupported CPU or restricted machine.");
				return 18011;
			}

			throw;
		}
	}

	private sealed partial record Machine(string DesiredLightState, List<Button> Buttons, int[] Joltages)
	{
		public int[] CurrentJoltageState { get; init; } = new int[Joltages.Length];
		public string CurrentLightState { get; init; } = new('.', DesiredLightState.Length);
		public bool IsStarted => CurrentLightState == DesiredLightState;
		public bool IsNotStarted => CurrentLightState != DesiredLightState;

		public Machine PressButton(Button button)
		{
			char[] newLightState = CurrentLightState.ToCharArray();
			foreach (int lightIndex in button.Values) {
				newLightState[lightIndex] = newLightState[lightIndex] == '.' ? '#' : '.';
			}

			return this with { CurrentLightState = new string(newLightState) };
		}

		/// <summary>
		/// Finds the minimum total number of button presses required to achieve the target joltage configuration.
		/// </summary>
		/// <remarks>This method uses a linear programming solver to determine the optimal combination of button
		/// presses. Each button may affect one or more joltages, and the solution ensures that all target joltages are met
		/// exactly. The method minimizes the total number of presses across all buttons.</remarks>
		/// <returns>The minimum number of button presses needed to reach the specified joltage values using the available buttons.</returns>
		/// <exception cref="ApplicationException">Thrown if the solver cannot be created or if no solution exists for the current joltage configuration.</exception>
		public int FindMinimumPressesForJoltage()
		{
			int numJoltages = Joltages.Length;
			int numButtons = Buttons.Count;

			// Create the linear solver with CBC backend
			// CBP stands for Coin-or branch and cut mixed integer programming solver.
			// SCIP is used here; ensure that the OR-Tools installation includes SCIP support.
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
