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
			count++;
			fewestTotalPresses += presses;
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
		/// Finds minimum button presses using Z3 SMT solver
		/// </summary>
		public int FindMinimumPressesForJoltage()
		{
			int numJoltages = Joltages.Length;
			int numButtons = Buttons.Count;

			// Create Z3 context and optimizer
			using Microsoft.Z3.Context ctx = new();
			using Microsoft.Z3.Optimize opt = ctx.MkOptimize();

			// Create integer variables for button press counts
			Microsoft.Z3.IntExpr[] buttonVars = new Microsoft.Z3.IntExpr[numButtons];
			for (int i = 0; i < numButtons; i++) {
				buttonVars[i] = ctx.MkIntConst($"button_{i}");
				// Constraint: button presses >= 0
				opt.Assert(ctx.MkGe(buttonVars[i], ctx.MkInt(0)));
			}

			// Add constraints: for each joltage, sum of affecting buttons = target
			for (int j = 0; j < numJoltages; j++) {
				List<Microsoft.Z3.ArithExpr> terms = [];

				for (int b = 0; b < numButtons; b++) {
					if (Buttons[b].Values.Contains(j)) {
						terms.Add(buttonVars[b]);
					}
				}

				if (terms.Count > 0) {
					Microsoft.Z3.ArithExpr sum = terms.Count == 1
						? terms[0]
						: ctx.MkAdd([.. terms]);
					opt.Assert(ctx.MkEq(sum, ctx.MkInt(Joltages[j])));
				} else {
					// No buttons affect this joltage
					if (Joltages[j] != 0) {
						throw new ApplicationException("No solution found for joltage configuration.");
					}
				}
			}

			// Objective: minimize sum of all button presses
			Microsoft.Z3.ArithExpr totalPresses = ctx.MkAdd(buttonVars);
			_ = opt.MkMinimize(totalPresses);

			// Solve
			Microsoft.Z3.Status status = opt.Check();

			if (status is Microsoft.Z3.Status.SATISFIABLE) {
				Microsoft.Z3.Model model = opt.Model;
				int total = 0;

				for (int i = 0; i < numButtons; i++) {
					Microsoft.Z3.Expr value = model.Evaluate(buttonVars[i], true);
					if (value is Microsoft.Z3.IntNum intNum) {
						total += intNum.Int;
					}
				}

				return total;
			}

			throw new ApplicationException("No solution found for joltage configuration.");
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
