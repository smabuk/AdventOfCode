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
						VisualiseString($"Machine reached desired state {newMachine.DesiredLightState} in {presses} presses.");
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
		VisualiseString($"Total Machines: {_machines.Count}");
		foreach (Machine machine in _machines) {
			VisualiseString($"Processing Machine {count + 1}/{_machines.Count}...{DateTime.Now}");
			int presses = machine.FindMinimumPressesForJoltage();
			fewestTotalPresses += presses;
			count++;
			VisualiseString($"{count}/{_machines.Count} Machine reached desired joltage state {{{string.Join(',', machine.Joltages)}}} in {presses} presses.");
		}

		return fewestTotalPresses;
	}

	private sealed partial record Machine(string DesiredLightState, List<Button> Buttons, int[] Joltages)
	{
		public string CurrentLightState { get; init; } = new('.', DesiredLightState.Length);
		public bool IsStarted => CurrentLightState == DesiredLightState;
		public bool IsNotStarted => CurrentLightState != DesiredLightState;

		public int[] CurrentJoltageState { get; init; } = new int[Joltages.Length];
		public bool IsCorrectJoltage => CurrentJoltageState.SequenceEqual(Joltages);
		public bool IsNotCorrectJoltage => !CurrentJoltageState.SequenceEqual(Joltages);

		public Machine PressButton(Button button)
		{
			char[] newLightState = CurrentLightState.ToCharArray();
			foreach (int lightIndex in button.Values) {
				newLightState[lightIndex] = newLightState[lightIndex] == '.' ? '#' : '.';
			}

			return this with { CurrentLightState = new string(newLightState) };
		}

		public Machine PressJoltageButton(Button button)
		{
			int[] newJoltageState = [.. CurrentJoltageState];
			foreach (int joltageIndex in button.Values) {
				newJoltageState[joltageIndex]++;
			}

			return this with { CurrentJoltageState = newJoltageState };
		}

		/// <summary>
		/// Finds minimum button presses using Dijkstra on button press count state space
		/// Guaranteed to find optimal solution with aggressive pruning
		/// </summary>
		public int FindMinimumPressesForJoltage()
		{
			int numJoltages = Joltages.Length;
			int numButtons = Buttons.Count;

			// Dijkstra with state = button press counts (not joltage values!)
			PriorityQueue<int[], int> queue = new();
			HashSet<string> visited = [];

			int[] startCounts = new int[numButtons];
			queue.Enqueue(startCounts, 0);

			int maxTarget = Joltages.Max();

			while (queue.Count > 0) {
				int[] currentCounts = queue.Dequeue();
				int totalPresses = currentCounts.Sum();

				// Compute resulting joltages
				int[] resultingJoltages = new int[numJoltages];
				for (int b = 0; b < numButtons; b++) {
					foreach (int idx in Buttons[b].Values) {
						resultingJoltages[idx] += currentCounts[b];
					}
				}

				// Check if we reached the goal
				if (resultingJoltages.SequenceEqual(Joltages)) {
					return totalPresses;
				}

				// Create state key for visited check
				string stateKey = string.Join(",", currentCounts);
				if (visited.Contains(stateKey)) {
					continue;
				}
				_ = visited.Add(stateKey);

				// Prune: if any joltage is already over target, skip
				bool overshot = false;
				for (int i = 0; i < numJoltages; i++) {
					if (resultingJoltages[i] > Joltages[i]) {
						overshot = true;
						break;
					}
				}
				if (overshot) {
					continue;
				}

				// Prune: if total presses exceeds reasonable bound, skip
				if (totalPresses > maxTarget * 2) {
					continue;
				}

				// Try pressing each button one more time
				for (int b = 0; b < numButtons; b++) {
					int[] nextCounts = [.. currentCounts];
					nextCounts[b]++;

					// Check if this would overshoot any target
					bool wouldOvershoot = false;
					foreach (int idx in Buttons[b].Values) {
						if (resultingJoltages[idx] >= Joltages[idx]) {
							wouldOvershoot = true;
							break;
						}
					}

					if (!wouldOvershoot) {
						int nextTotal = totalPresses + 1;

						// Heuristic: max remaining deficit
						int maxDeficit = 0;
						foreach (int idx in Buttons[b].Values) {
							int deficit = Joltages[idx] - resultingJoltages[idx] - 1;
							if (deficit > maxDeficit) {
								maxDeficit = deficit;
							}
						}
						for (int i = 0; i < numJoltages; i++) {
							if (!Buttons[b].Values.Contains(i)) {
								int deficit = Joltages[i] - resultingJoltages[i];
								if (deficit > maxDeficit) {
									maxDeficit = deficit;
								}
							}
						}

						queue.Enqueue(nextCounts, nextTotal + maxDeficit);
					}
				}
			}

			return -1;
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
