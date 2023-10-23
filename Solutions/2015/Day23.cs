namespace AdventOfCode.Solutions._2015;

/// <summary>
/// Day 23: Opening the Turing Lock
/// https://adventofcode.com/2015/day/23
/// </summary>
[Description("Opening the Turing Lock")]
public sealed partial class Day23 {

	public static string Part1(string[] input, params object[]? args) {
		string outputRegister = GetArgument(args, 1, "b");
		return Solution1(input, outputRegister).ToString();
	}
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input, string outputRegister) {
		Computer computer = new();
		_ = computer.ExecuteProgram(Instruction.ParseProgram(input));

		return computer.registers[outputRegister];
	}

	private static int Solution2(string[] input) {
		Computer computer = new();
		computer.registers["a"] = 1;
		_ = computer.ExecuteProgram(Instruction.ParseProgram(input));

		return computer.registers["b"];
	}



	private partial record Instruction(string OperationName, string RegisterName, int Value) {
		[GeneratedRegex("""(?<op>\D\D\D) (?<reg>a|b)*(, )*(?<value>[\+\-]\d+)*""")]
		private static partial Regex InstructionRegex();

		public static Instruction Parse(string input) {
			Match match = InstructionRegex().Match(input);
			if (match.Success) {
				int value = 0;
				if (!String.IsNullOrWhiteSpace(match.Groups["value"].Value)) {
					value = int.Parse(match.Groups["value"].Value);
				}
				return new(match.Groups["op"].Value, match.Groups["reg"].Value, value);
			}
			return null!;
		}

		public static List<Instruction> ParseProgram(string[] input) =>
			input.Select(i => Parse(i)).ToList();
	};

	private record class Computer {
		public Dictionary<string, int> registers = [];

		public Computer() {
			registers["a"] = 0;
			registers["b"] = 0;
		}

		public bool ExecuteProgram(List<Instruction> Instructions) {
			int currentInstructionNo = 0;

			while (currentInstructionNo < Instructions.Count) {
				Instruction currentInstruction = Instructions[currentInstructionNo];
				switch (currentInstruction.OperationName) {
					case "hlf": // Half
						registers[currentInstruction.RegisterName] = registers[currentInstruction.RegisterName] / 2;
						currentInstructionNo++;
						break;
					case "inc": // Increment
						registers[currentInstruction.RegisterName]++;
						currentInstructionNo++;
						break;
					case "jie": // Jump If Even
						if (registers[currentInstruction.RegisterName] % 2 == 0) {
							currentInstructionNo += currentInstruction.Value;
						} else {
							currentInstructionNo++;
						}
						break;
					case "jio": // Jump If One
						if (registers[currentInstruction.RegisterName] == 1) {
							currentInstructionNo += currentInstruction.Value;
						} else {
							currentInstructionNo++;
						}
						break;
					case "jmp": // Jump
						currentInstructionNo += currentInstruction.Value;
						break;
					case "tpl": // Triple
						registers[currentInstruction.RegisterName] = registers[currentInstruction.RegisterName] * 3;
						currentInstructionNo++;
						break;
					default:
						return false;
				}
			}

			return true;
		}
	}
}
