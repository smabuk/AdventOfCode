namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 24: Arithmetic Logic Unit
/// https://adventofcode.com/2021/day/24
/// </summary>
[Description("Arithmetic Logic Unit")]
public class Day24 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static string Solution1(string[] input) {
		List<Instruction> instructions = input.Select(i => ParseLine(i)).ToList();
		int[] digits = new int[14];

		ALU submarineAlu = new();
		//                 01234567890123
		// Too low Guess = 92922311699991;
		// Too low Guess = 92922314999991;
		long modelNumber = 92928914999991;

		Console.WriteLine();

		Console.WriteLine($"{modelNumber}");
		digits = modelNumber.ToString().Select(d => int.Parse($"{d}")).ToArray();
		submarineAlu = new();
		submarineAlu.ExecuteInstructions(instructions, digits);
		Console.WriteLine();
		if (submarineAlu.HasException) {
			Console.WriteLine(submarineAlu.ExceptionMessage);
		} else {
			Console.WriteLine($"{modelNumber} w: {submarineAlu.W} x: {submarineAlu.X} y: {submarineAlu.Y} z: {submarineAlu.Z} ");
		}

		if (submarineAlu.Z == 0) {
			return String.Join("", digits.Select(d => $"{d}"));
		} else {
			return "No idea";
		}
	}

	private static string Solution2(string[] input) {
		List<Instruction> instructions = input.Select(i => ParseLine(i)).ToList();
		int[] digits = new int[14];

		ALU submarineAlu = new();
		//                 01234567890123
		// Too low Guess = 92922311699991;
		// Too low Guess = 92922314999991;
		long modelNumber = 92928914999991;

		Console.WriteLine();

		Console.WriteLine($"{modelNumber}");
		digits = modelNumber.ToString().Select(d => int.Parse($"{d}")).ToArray();
		submarineAlu = new();
		submarineAlu.ExecuteInstructions(instructions, digits);
		Console.WriteLine();
		if (submarineAlu.HasException) {
			Console.WriteLine(submarineAlu.ExceptionMessage);
		} else {
			Console.WriteLine($"{modelNumber} w: {submarineAlu.W} x: {submarineAlu.X} y: {submarineAlu.Y} z: {submarineAlu.Z} ");
		}

		// too low 92922311699991
		if (submarineAlu.Z == 0) {
			return String.Join("", digits.Select(d => $"{d}"));
		} else {
			return "No idea";
		}
	}

	public record class ALU {
		public int W { get; set; } = 0;
		public int X { get; set; } = 0;
		public int Y { get; set; } = 0;
		public int Z { get; set; } = 0;
		public bool HasException { get; set; } = false;
		public string ExceptionMessage { get; set; } = "";

		public Queue<int> InputQueue { get; set; } = new();



		public bool ExecuteInstructions(List<Instruction> instructions, int[] inputs) {
			foreach (int input in inputs) {
				InputQueue.Enqueue(input);
			}
			int section = -1;
			Console.WriteLine();
			foreach (var instruction in instructions) {
				switch (instruction.Operation) {
					case "inp":
						section++;
						Console.Write($"Section: {section,-2}: ");
						Console.WriteLine($"w: {W,-11} x: {X,-11} y: {Y,-11} z: {Z,-11} inst: {instruction.Operation} {instruction.VariableA} {instruction.VariableB}");
						Input(instruction.VariableA, InputQueue.Dequeue());
						break;
					case "add":
						Add(instruction.VariableA, instruction.VariableB);
						break;
					case "mul":
						Multiply(instruction.VariableA, instruction.VariableB);
						break;
					case "div":
						Divide(instruction.VariableA, instruction.VariableB);
						break;
					case "mod":
						Modulo(instruction.VariableA, instruction.VariableB);
						break;
					case "eql":
						Equal(instruction.VariableA, instruction.VariableB);
						break;
					default:
						break;
				}
				//if (section == 13) {
				Console.WriteLine($"w: {W,-11} x: {X,-11} y: {Y,-11} z: {Z,-11} inst: {instruction.Operation} {instruction.VariableA} {instruction.VariableB}");
				//}
				if (HasException) {
					ExceptionMessage = $"{String.Join("", inputs.Select(d => $"{d}"))} {ExceptionMessage}";
					break;
				}
			}
			return HasException;

		}

		private void Add(string variableA, string variableB) {
			int a = GetVariable(variableA);
			int b = GetVariable(variableB);
			SetVariable(variableA, a + b);
		}

		private void Multiply(string variableA, string variableB) {
			int a = GetVariable(variableA);
			int b = GetVariable(variableB);
			SetVariable(variableA, a * b);
		}

		private void Divide(string variableA, string variableB) {
			int a = GetVariable(variableA);
			int b = GetVariable(variableB);
			if (b == 0) {
				HasException = true;
				ExceptionMessage = $"Divide by Zero of {a}";
			}
			SetVariable(variableA, a / b);
		}

		private void Modulo(string variableA, string variableB) {
			int a = GetVariable(variableA);
			int b = GetVariable(variableB);
			if (a < 0 || b < 0) {
				HasException = true;
				ExceptionMessage = $"Negative Modulo {a} {b}";
			}
			SetVariable(variableA, a % b);
		}

		private void Equal(string variableA, string variableB) {
			int a = GetVariable(variableA);
			int b = GetVariable(variableB);
			SetVariable(variableA, a == b ? 1 : 0);
		}

		private void Input(string variableA, int value) => SetVariable(variableA, value);
		private int GetVariable(string variable) {
			return variable switch {
				"w" => W,
				"x" => X,
				"y" => Y,
				"z" => Z,
				_ => int.Parse(variable),
			};
		}

		private void SetVariable(string variable, int value) {
			switch (variable) {
				case "w":
					W = value;
					break;
				case "x":
					X = value;
					break;
				case "y":
					Y = value;
					break;
				case "z":
					Z = value;
					break;
				default:
					break;
			}
		}

		public static IEnumerable<Instruction> Parse(string[] inputs) {
			foreach (string input in inputs) {
				string[] i = input.Split(' ');
				yield return new(i[0], i[1], i.Length < 3 ? "" : i[2]);
			}
		}
	}






	public record Instruction(string Operation, string VariableA, string VariableB) {
		public int NumberB {
			get {
				if (string.IsNullOrWhiteSpace(VariableB) || char.IsLetter(VariableB[0])) {
					return 0;
				} else {
					return int.Parse(VariableB);
				}
			}
		}
	};

	private static Instruction ParseLine(string input) {
		string[] i = input.Split(' ');
		return new(i[0], i[1], i.Length < 3 ? "" : i[2]);
	}
}

