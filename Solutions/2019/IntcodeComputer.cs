namespace AdventOfCode.Solutions.Year2019;

public class IntcodeComputer {

	public enum ParameterMode {
		Position = 0,
		Immediate = 1
	}

	public enum OpCode {
		Add = 1,
		Multiply = 2,
		Input = 3,
		Output = 4,
		JumpIfTrue = 5,
		JumpIfFalse = 6,
		LessThan = 7,
		Equals = 8,
		Halt = 99
	}

	record Instruction(OpCode OpCode, ParameterMode[] ParameterModes) {

		public static Instruction Parse(int instruction) {
			const int noOfDigits = 10;

			int opCode = instruction % 100;
			string instructionString = instruction.ToString();
			ParameterMode[] parameterModes = new ParameterMode[noOfDigits];
			for (int i = 0; i < instructionString.Length - 2; i++) {
				if (instructionString[i] == '1') {
					parameterModes[instructionString.Length - 3 - i] = ParameterMode.Immediate;
				}
			}

			return new Instruction((OpCode)opCode, parameterModes);
		}
	}
	public static int[] ExecuteIntcodeProgram(int[] program) {
		return ExecuteIntcodeProgram(program, null, out int[] _);
	}

	public static int[] ExecuteIntcodeProgram(int[] program, int[]? inputs, out int[] outputs) {
		int inputIndex = 0;
		List<int> outputList = new();

		for (int i = 0; i < program.Length && program[i] != 99; i++) {
			Instruction instruction = Instruction.Parse(program[i]);
			int value1;
			int value2;
			switch (instruction.OpCode) {
				case OpCode.Halt:
					break;
				case OpCode.Add:
					value1 = instruction.ParameterModes[0] == ParameterMode.Position ? program[program[i + 1]] : program[i + 1];
					value2 = instruction.ParameterModes[1] == ParameterMode.Position ? program[program[i + 2]] : program[i + 2];
					program[program[i + 3]] = value1 + value2;
					i += 3;
					break;
				case OpCode.Multiply:
					value1 = instruction.ParameterModes[0] == ParameterMode.Position ? program[program[i + 1]] : program[i + 1];
					value2 = instruction.ParameterModes[1] == ParameterMode.Position ? program[program[i + 2]] : program[i + 2];
					program[program[i + 3]] = value1 * value2;
					i += 3;
					break;
				case OpCode.Input:
					// takes a single integer as input and saves it to the position given by its only parameter
					ArgumentNullException.ThrowIfNull(inputs);
					if (inputIndex >= inputs.Length) {
						throw new ArgumentOutOfRangeException(nameof(inputs));
					}
					program[program[i + 1]] = inputs[inputIndex++];
					i += 1;
					break;
				case OpCode.Output:
					value1 = instruction.ParameterModes[0] == ParameterMode.Position ? program[program[i + 1]] : program[i + 1];
					outputList.Add(value1);
					i += 1;
					break;
				case OpCode.JumpIfTrue:
					value1 = instruction.ParameterModes[0] == ParameterMode.Position ? program[program[i + 1]] : program[i + 1];
					value2 = instruction.ParameterModes[1] == ParameterMode.Position ? program[program[i + 2]] : program[i + 2];
					if (value1 != 0) {
						i = value2 - 1;
					} else {
						i += 2;
					}
					break;
				case OpCode.JumpIfFalse:
					value1 = instruction.ParameterModes[0] == ParameterMode.Position ? program[program[i + 1]] : program[i + 1];
					value2 = instruction.ParameterModes[1] == ParameterMode.Position ? program[program[i + 2]] : program[i + 2];
					if (value1 == 0) {
						i = value2 - 1;
					} else {
						i += 2;
					}
					break;
				case OpCode.LessThan:
					value1 = instruction.ParameterModes[0] == ParameterMode.Position ? program[program[i + 1]] : program[i + 1];
					value2 = instruction.ParameterModes[1] == ParameterMode.Position ? program[program[i + 2]] : program[i + 2];
					program[program[i + 3]] = value1 < value2 ? 1 : 0;
					i += 3;
					break;
				case OpCode.Equals:
					value1 = instruction.ParameterModes[0] == ParameterMode.Position ? program[program[i + 1]] : program[i + 1];
					value2 = instruction.ParameterModes[1] == ParameterMode.Position ? program[program[i + 2]] : program[i + 2];
					program[program[i + 3]] = value1 == value2 ? 1 : 0;
					i += 3;
					break;
				default:
					throw new Exception();
			}
		}

		outputs = outputList.ToArray();
		return program;
	}

}
