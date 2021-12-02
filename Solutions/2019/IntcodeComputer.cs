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
		Halt = 99
	}

	record Instruction(OpCode OpCode, ParameterMode[] ParameterModes) {

		public static Instruction Parse(int instruction) {
			const int noOfDigits = 10;

			// GetLastDigit
			int opCode = instruction % 100;
			ParameterMode[] parameterModes = new ParameterMode[noOfDigits];

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
			switch (instruction.OpCode) {
				case OpCode.Halt:
					break;
				case OpCode.Add:
					program[program[i + 3]] = program[program[i + 1]] + program[program[i + 2]];
					i += 3;
					break;
				case OpCode.Multiply:
					program[program[i + 3]] = program[program[i + 1]] * program[program[i + 2]];
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
					// outputs the value of its only parameter
					outputList.Add(program[program[i + 1]]);
					i += 1;
					break;
				default:
					break;
			}
		}

		outputs = outputList.ToArray();
		return program;
	}

}
