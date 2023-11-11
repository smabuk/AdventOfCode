namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 07: The Sum of Its Parts
/// https://adventofcode.com/2018/day/07
/// </summary>
[Description("The Sum of Its Parts")]
public sealed partial class Day07 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args)
	{
		int noOfWorkers     = GetArgument<int>(args, argumentNumber: 1,  5);
		int durationDefault = GetArgument<int>(args, argumentNumber: 2, 60);
		return Solution2(input, noOfWorkers, durationDefault).ToString();
	}

	private static string Solution1(string[] input) {
		List<Instruction> instructions = input.Select(ParseLine).ToList();
		HashSet<char> steps = [
			.. instructions.Select(instruction => instruction.BeforeCanBeginStep),
			.. instructions.Select(instruction => instruction.MustBeFinishedStep),
		];

		string correctOrder = "";
		do {
			char nextAvailable = steps
				.ExceptBy(instructions.Select(instruction => instruction.BeforeCanBeginStep), s => s)
				.Order()
				.First();
			_ = instructions.RemoveAll(i => i.MustBeFinishedStep == nextAvailable);
			_ = steps.Remove(nextAvailable);
			correctOrder += nextAvailable;
		} while (steps.Count != 0);

		return correctOrder;
	}

	private static int Solution2(string[] input, int noOfWorkers, int durationDefault) {
		List<Instruction> instructions = input.Select(ParseLine).ToList();

		HashSet<char> steps = [
			.. instructions.Select(instruction => instruction.BeforeCanBeginStep),
			.. instructions.Select(instruction => instruction.MustBeFinishedStep),
		];

		Dictionary<int, AssignedStep?> workers = [];
		for (int i = 1; i <= noOfWorkers; i++) {
			workers.Add(i, null);
		}

		//List<string> progress = [];
		//string doneOrder = "";
		int elapsedSeconds = 0;
		do {
			foreach (var item in workers) {
				if (item.Value?.IsComplete(elapsedSeconds) ?? false) {
					_ = instructions.RemoveAll(instructions => instructions.MustBeFinishedStep == item.Value.Name);
					//doneOrder += item.Value.Name;
					_ = workers[item.Key] = null;
				}
			}
			List<char> nextAvailableSteps = steps
				.ExceptBy(instructions.Select(instruction => instruction.BeforeCanBeginStep), s => s)
				.Order()
				.ToList();
			List<int> nextAvailableWorkers = workers
				.Where(w => w.Value is null)
				.Select(w => w.Key)
				.Order()
				.ToList();

			for (int i = 0; i < Math.Min(nextAvailableSteps.Count, nextAvailableWorkers.Count); i++) {
				workers[nextAvailableWorkers[i]] = new(nextAvailableSteps[i], nextAvailableWorkers[i], durationDefault + nextAvailableSteps[i] - 'A' + 1, elapsedSeconds);
				_ = steps.Remove(nextAvailableSteps[i]);
			}
			//progress.Add($"{elapsedSeconds,3}: {workers[1]?.Name ?? '.'} {workers[2]?.Name ?? '.'}  {doneOrder}");
			elapsedSeconds++;
		} while (steps.Count != 0 || workers.Any(w => w.Value is not null));

		return elapsedSeconds - 1;
	}

	private record AssignedStep(char Name, int Worker, int Duration, int Start)
	{
		public bool IsComplete(int ElapsedSeconds) => ElapsedSeconds >= (Start + Duration);
	}


	private record Instruction(char MustBeFinishedStep, char BeforeCanBeginStep) : IParsable<Instruction> {
		public static Instruction Parse(string s) => ParseLine(s);
		public static Instruction Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result) => throw new NotImplementedException();
	}

	private static Instruction ParseLine(string input) => new(input[5], input[36]);
}
