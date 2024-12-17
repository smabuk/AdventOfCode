namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 17: Chronospatial Computer
/// https://adventofcode.com/2024/day/17
/// </summary>
[Description("Chronospatial Computer")]
public static partial class Day17 {

	private static List<Instruction> _instructions = [];
	private static List<int> _program = [];
	private static int[] _registers = [];
	private static Action<string[], bool>? _visualise = null;

	[Init]
	public static void LoadInstructions(string[] input, Action<string[], bool>? visualise = null)
	{
		_registers = [.. input[..REG_END_LINE].Select(i => i[12..].As<int>())];

		_program = [.. input[PROGRAM_LINE][9..].AsInts()];
		_instructions = [.. input[PROGRAM_LINE][9..].AsInts().Chunk(2).Select(i => $"{i[0]} {i[1]}".As<Instruction>())];
		//_operands = [.. input[PROGRAM_LINE][9..].AsInts().Chunk(2).Select(i => i[1])];

		_visualise = visualise;
	}

	public static string Part1(string[] _)
	{
		int[] registers = [.. _registers];

		string result = _instructions.ExecuteCodePart1(registers);

		VisualiseResult(registers, result);

		return result;



		static void VisualiseResult(int[] registers, string result)
		{
			Visualise([], ""); Visualise([], "");
			registers.Visualise($"Result: {result}");
			Visualise([], "");
		}
	}

	public static string Part2(string[] input, params object[]? args) => NO_SOLUTION_WRITTEN_MESSAGE;



	public static string ExecuteCodePart1(this List<Instruction> instructions, int[] registers)
	{
		List<int> outputValues = [];

		for (int instPtr = 0; instPtr < instructions.Count; instPtr++) {
			Instruction instruction = instructions[instPtr];
			ComboOperand operand = instruction.Operand;

			registers = instruction.Evaluate(registers);

			switch (instruction) {
				case AdvInstruction:
					break;
				case BdvInstruction:
					break;
				case BxlInstruction:
					break;
				case BstInstruction:
					break;
				case CdvInstruction:
					break;
				case OutInstruction:
					int value = operand.Value(registers) % 8;
					outputValues.Add(value);

					Visualise([], "");
					registers.Visualise($"Output: {(char)(operand.Number < 4 ? operand.Number + '0' : (operand.Number - 4 + 'A'))} => {value}");
					Visualise([], "");
					break;
				case JnzInstruction:
					int regAValue = REG_A.GetReg(registers);
					registers.Visualise($"Jump  {operand.Number}: {regAValue} => {(regAValue != 0 ? instructions[operand.Number] : instPtr)}");

					instPtr = regAValue != 0
						? operand.Number - 1
						: instPtr;
					break;
				default:
					break;
			}
		}

		return string.Join(",", outputValues);
	}

	public static int GetReg(this string valueOrReg, int[] registers)
	{
		return Char.IsLetter(valueOrReg[0])
			? registers[valueOrReg.RegIndex()]
			: valueOrReg.As<int>();
	}

	public static int[] SetReg(this int[] registers, string reg, int value)
	{
		registers[reg.RegIndex()] = value;
		return registers;
	}

	public static int RegIndex(this string registerName) => registerName[0] - REG_OFFSET;


	private static void Visualise(this int[] registers, string value)
	{
		if (_visualise is null) {
			return;
		}

		string registersString = registers is [] ? "" : $" -- Registers: A= {registers[0],8} B={registers[1],8} C={registers[2],8}";
		string[] output = ["", $"{value,-34} {registersString}"];
		_visualise?.Invoke(output, false);
	}




	public record ComboOperand(int Number)
	{
		public int Value(int[] registers) => Number switch
		{
			0 => Number,
			1 => Number,
			2 => Number,
			3 => Number,
			4 => REG_A.GetReg(registers),
			5 => REG_B.GetReg(registers),
			6 => REG_C.GetReg(registers),
			_ => throw new NotImplementedException(),
		};
	}

	public abstract record Instruction(ComboOperand Operand) : IParsable<Instruction>
	{
		public abstract int[] Evaluate(int[] registers);

		public int OpCode => this switch
		{
			AdvInstruction => 0,
			BxlInstruction => 1,
			BstInstruction => 2,
			JnzInstruction => 3,
			BxcInstruction => 4,
			OutInstruction => 5,
			BdvInstruction => 6,
			CdvInstruction => 7,
			_ => throw new NotImplementedException(),
		};

		public static Instruction Create(int opCode, int operand) => Parse($"{opCode} {operand}");

		public static Instruction Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit(SPACE);
			ComboOperand operand = new(tokens[1].As<int>());

			return tokens[0] switch
			{
				"0" => new AdvInstruction(operand),
				"1" => new BxlInstruction(operand),
				"2" => new BstInstruction(operand),
				"3" => new JnzInstruction(operand),
				"4" => new BxcInstruction(operand),
				"5" => new OutInstruction(operand),
				"6" => new BdvInstruction(operand),
				"7" => new CdvInstruction(operand),
				_ => throw new NotImplementedException(),
			};
		}

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	private record AdvInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override int[] Evaluate(int[] registers) {
			int numerator = REG_A.GetReg(registers);
			int value = Operand.Value(registers);
			string debug = $"ADV  {value}: {numerator} / {(int)Math.Pow(2, value)}";

			registers = registers.SetReg(REG_A, numerator / (int)Math.Pow(2, value));

			registers.Visualise($"{debug} = {REG_A.GetReg(registers)}");
			return registers;
		}
	}

	private record BdvInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override int[] Evaluate(int[] registers)
		{
			int numerator = REG_A.GetReg(registers);
			int value = Operand.Value(registers);
			string debug = $"BDV  {value}: {numerator} / {(int)Math.Pow(2, value)}";

			registers = registers.SetReg(REG_B, numerator / (int)Math.Pow(2, value));

			registers.Visualise($"{debug} = {REG_B.GetReg(registers)}");
			return registers;
		}
	}

	private record CdvInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override int[] Evaluate(int[] registers)
		{
			int numerator = REG_A.GetReg(registers);
			int value = Operand.Value(registers);
			string debug = $"CDV  {value}: {numerator} / {(int)Math.Pow(2, value)}";

			registers = registers.SetReg(REG_C, numerator / (int)Math.Pow(2, value));

			registers.Visualise($"{debug} = {REG_C.GetReg(registers)}");
			return registers;
		}
	}

	private record BxlInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override int[] Evaluate(int[] registers)
		{
			int regBValue = REG_B.GetReg(registers);
			string debug = $"BXL  {Operand.Number}: {regBValue} XOR {Operand.Number}";
			registers = registers.SetReg(REG_B, regBValue ^ Operand.Number);

			registers.Visualise($"{debug} = {REG_B.GetReg(registers)}");
			return registers;
		}
	}

	private record BstInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override int[] Evaluate(int[] registers)
		{
			int operandValue = Operand.Value(registers);
			string debug = $"BST  {Operand.Number}: {operandValue} % 8";

			registers = registers.SetReg(REG_B, operandValue % 8);

			registers.Visualise($"{debug} = {REG_B.GetReg(registers)}");
			return registers;
		}
	}

	private record BxcInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override int[] Evaluate(int[] registers)
		{
			string debug = $"BXC   : {REG_B.GetReg(registers)} XOR {REG_C.GetReg(registers)}";

			registers = registers.SetReg(REG_B, REG_B.GetReg(registers) ^ REG_C.GetReg(registers));

			registers.Visualise($"{debug} = {REG_B.GetReg(registers)}");
			return registers;
		}
	}

	private record JnzInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override int[] Evaluate(int[] registers) => registers;
	}

	private record OutInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override int[] Evaluate(int[] registers) => registers;
	}

	private const int REG_OFFSET = 'A';
	private const string REG_A = "A";
	private const string REG_B = "B";
	private const string REG_C = "C";

	private const int REG_END_LINE = 3;
	private const int PROGRAM_LINE = 4;
	private const char SPACE = ' ';

}
