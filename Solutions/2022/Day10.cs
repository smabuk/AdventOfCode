namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 10: Cathode-Ray Tube
/// https://adventofcode.com/2022/day/10
/// </summary>
[Description("Cathode-Ray Tube")]
public sealed partial class Day10 {


	[Init]
	public static    void Init(string[] input, params object[]? _) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? args) {
		int cycleCheckStart    = GetArgument<int>(args, argumentNumber: 1, 20);
		int cycleCheckInterval = GetArgument<int>(args, argumentNumber: 2, 40);
		return Solution1(cycleCheckStart, cycleCheckInterval).ToString();
	}
	public static string Part2(string[] input, params object[]? args) {
		bool includeOcr = GetArgument<bool>(args, argumentNumber: 1, true);
		return Solution2(includeOcr).ToString();
	}

	private static IEnumerable<Instruction> _instructions = Array.Empty<Instruction>();


	private static void LoadInstructions(string[] input)
		=> _instructions = input.Select(Instruction.Parse);

	private static int Solution1(int cycleCheckStart, int cycleCheckInterval) {
		return
			new CPU() {
				Mode = CPU.CpuMode.SignalStrength,
				Cycle = (cycleCheckStart, cycleCheckInterval)
			}
			.ExecuteProgram(_instructions.ToList())
			.Sum();
	}

	private static string Solution2(bool includeOcr = false) {
		const int CRT_COLS = 40;
		const int CRT_ROWS = 6;
		char[,] crt = new char[CRT_COLS, CRT_ROWS];

		foreach (Cell<int> cpuCycle
			in new CPU() { Mode = CPU.CpuMode.Crt, Cycle = (1, 1) }
				.ExecuteProgram(_instructions.ToList())
				.To2dArray(CRT_COLS, CRT_ROWS)
				.WalkWithValues()) {
			crt[cpuCycle.X, cpuCycle.Y] = PixelState(cpuCycle.X, cpuCycle.Value) ? '█' : ' ';
		}

		string outputString = String.Join(Environment.NewLine, crt.PrintAsStringArray(width: 0)); ;
		if (includeOcr) {
			string ocrString = OcrHelpers.IdentifyMessage(crt.PrintAsStringArray(width: 0), ' ', '█');
			return ocrString + Environment.NewLine + outputString;
		} else {
			return outputString;
		}

		static bool PixelState(int col, int value) => Math.Abs(value - col) <= 1;
	}


	private record class CPU {
		public CpuMode Mode { get; set; } = CpuMode.SignalStrength;
		public (int Start, int Interval) Cycle { get; set; } = (20, 40);
		public static CpuMode SignalStrength { get; private set; }

		int registerX = 1;
		int cycleCount = 1;

		public IEnumerable<int> ExecuteProgram(List<Instruction> instructions) {
			int instructionNo = 0;
			int countdown = int.MinValue;

			while (instructionNo < instructions.Count) {
				if (Mode == CpuMode.SignalStrength) {
					if ((cycleCount + Cycle.Start) % Cycle.Interval == 0 && cycleCount > 0) {
						yield return registerX * cycleCount;
					}
				} else {
					yield return registerX;
				}

				if (instructions[instructionNo].Command is Instruction.InstructionType.addx) {
					if (countdown == 0) {
						registerX += instructions[instructionNo].Value;
						instructionNo++;
					} else if (countdown < 0) {
						countdown = 1;
					}
					countdown--;
				} else {
					instructionNo++;
				}

				cycleCount++;
			}
		}
		public enum CpuMode { SignalStrength, Crt }
	}

	private record struct Instruction(Instruction.InstructionType Command, int Value) {
		public static Instruction Parse(string input) {
			string[] tokens = input.Split(' ');
			int value = tokens.Length > 1 ? tokens[1].As<int>() : 0;
			return new(Enum.Parse<InstructionType>(tokens[0]), value);
		}
		public enum InstructionType { noop, addx }
	};


}
