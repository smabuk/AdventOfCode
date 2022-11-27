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

	record Instruction(string OperationName, string RegisterName, int Value);

	private static int Solution1(string[] input, string outputRegister) {
		List<Instruction> instructions = input.Select(i => ParseLine(i)).ToList();

		Computer computer = new();
		computer.ExecuteProgram(instructions);

		return computer.registers[outputRegister];
	}

	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		List<Instruction> instructions = input.Select(i => ParseLine(i)).ToList();
		return "** Solution not written yet **";
	}

	record class Computer {
		public Dictionary<string, int> registers = new();
		public int RegisterA => registers["a"];
		public int RegisterB => registers["b"];

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
						break;
				}
			}

			return true;
		}
	}




	private static Instruction ParseLine(string input) {
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

	[GeneratedRegex("""(?<op>\D\D\D) (?<reg>a|b)*(, )*(?<value>[\+\-]\d+)*""")]
	private static partial Regex InstructionRegex();
}
