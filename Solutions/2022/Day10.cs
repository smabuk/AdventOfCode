namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 10: Cathode-Ray Tube
/// https://adventofcode.com/2022/day/10
/// </summary>
[Description("Cathode-Ray Tube")]
public sealed partial class Day10 {

	[Init] public static void Init(string[] input, params object[]? _) => LoadInstructions(input);

	public static string Part1(string[] input, params object[]? args) {
		int cycleCheckStart = GetArgument<int>(args, argumentNumber: 1, 20);
		int cycleCheckInterval = GetArgument<int>(args, argumentNumber: 2, 40);
		return Solution1(cycleCheckStart, cycleCheckInterval).ToString();
	}
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();


	private static IEnumerable<Instruction> _instructions = Array.Empty<Instruction>();

	private static void LoadInstructions(string[] input) {
		_instructions = input.Select(i => Instruction.Parse(i));
	}

	private static int Solution1(int cycleCheckStart, int cycleCheckInterval) {
		List<int> signalStrengths = new Computer()
			.ExecuteProgram(_instructions.ToList(), cycleCheckStart, cycleCheckInterval)
			.ToList();

		return signalStrengths.Sum();
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}


	private record Instruction(string Command, int Value) {
		public static Instruction Parse(string input) {
			string[] tokens = input.Split(' ');
			int value = tokens.Length > 1 ? tokens[1].AsInt() : 0;
			return new(tokens[0],value);
		}
	};


	private record class Computer {
		public Dictionary<string, int> registers = new();
		public int CycleCount { get; set; } = 0;

		public Computer() {
			registers["x"] = 1;
			CycleCount = 1;
		}

		public IEnumerable<int> ExecuteProgram(List<Instruction> Instructions, int cycleCheckStart, int cycleCheckInterval) {
			int currentInstructionNo = 0;
			Instruction currentInstruction = Instructions[currentInstructionNo];

			int countdown = int.MinValue;
			while (currentInstructionNo <= Instructions.Count) {

				if ((CycleCount + cycleCheckStart) % cycleCheckInterval == 0 && CycleCount > 0) {
					yield return registers["x"] * CycleCount;
				}

				if (currentInstructionNo < Instructions.Count) {
					currentInstruction = Instructions[currentInstructionNo];
				}

				switch (currentInstruction.Command) {
					case "noop":
						currentInstructionNo++;
						break;
					case "addx": // Increment
						if (countdown < 0) {
							countdown = 1;
						} else if (countdown == 0) {
							registers["x"] += currentInstruction.Value;
							currentInstructionNo++;
						}
						countdown--;
						break;
					default:
						break;
				}

				CycleCount++;
			}

		}
	}
}
