namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 17: Chronospatial Computer
/// https://adventofcode.com/2024/day/17
/// </summary>
[Description("Chronospatial Computer")]
public static partial class Day17 {

	private static List<Instruction> _instructions = [];
	private static List<int> _program = [];
	private static long[]  _registers = [];
	private static Action<string[], bool>? _visualise = null;

	[Init]
	public static void LoadInstructions(string[] input, Action<string[], bool>? visualise = null)
	{
		_registers = [.. input[..REG_END_LINE].Select(i => i[12..].As<int>())];

		_program = [.. input[PROGRAM_LINE][9..].AsInts()];
		_instructions = [.. input[PROGRAM_LINE][9..].AsInts().Chunk(2).Select(i => $"{i[0]}{PARSE_SEP}{i[1]}".As<Instruction>())];

		_visualise = visualise;
	}

	public static string Part1(string[] _)
	{
		long[] registers = [.. _registers];

		string result = _instructions.ExecuteCodePart1(registers);

		VisualiseResult(registers, result);

		return result;
	}

	public static string ExecuteCodePart1(this List<Instruction> instructions, long[] registers)
	{
		List<long> outputValues = [];

		for (int instPtr = 0; instPtr < instructions.Count; instPtr++) {
			Instruction instruction = instructions[instPtr];
			ComboOperand operand = instruction.Operand;

			registers = instruction.Evaluate(registers);

			switch (instruction) {
				case OutInstruction:
					long value = operand.Value(registers) % 8;
					outputValues.Add(value);

					Visualise([], "");
					registers.Visualise($"Output: {(char)(operand.Number < 4 ? operand.Number + '0' : (operand.Number - 4 + 'A'))} => {value}");
					Visualise([], "");
					break;
				case JnzInstruction:
					long regAValue = REG_A.GetReg(registers);
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


	public static long Part2(string[] _)
	{

		long[] registers = [.. _registers];
		long[] results = [.. _instructions.ExecuteCodePart2(_program, registers)];

		return results.Min();
	}

	public static IEnumerable<long> ExecuteCodePart2(this List<Instruction> instructions, List<int> values, long[] registers)
	{
		if (values.Count == 0) {
			yield return 0;
			yield break;
		}

		List<long> outputValues = [.. instructions.ExecuteCodePart2(values[1..], registers)];
		foreach (long outputValue in outputValues) {
			for (int aValue = 0; aValue < 8; aValue++) {
				long[] localRegisters = [(outputValue * 8) + aValue, 0, 0];
				string result = instructions.ExecuteCodePart1(localRegisters);
				
				if (result == string.Join(",", values)) {
					yield return (outputValue * 8) + aValue;
				}
			}
		}
	}

	public static long GetReg(this string valueOrReg, long[] registers)
	{
		return Char.IsLetter(valueOrReg[0])
			? registers[valueOrReg.RegIndex()]
			: valueOrReg.As<long>();
	}

	public static long[] SetReg(this long[] registers, string reg, long value)
	{
		registers[reg.RegIndex()] = value;
		return registers;
	}

	public static int RegIndex(this string registerName) => registerName[0] - REG_OFFSET;


	private static void Visualise(this long[] registers, string value)
	{
		if (_visualise is null) {
			return;
		}

		string registersString = registers is [] ? "" : $" -- Registers: A= {registers[0],8} B={registers[1],8} C={registers[2],8}";
		string[] output = ["", $"{value,-34} {registersString}"];
		_visualise?.Invoke(output, false);
	}

	private static void VisualiseResult(long[] registers, string result)
	{
		Visualise([], ""); Visualise([], "");
		registers.Visualise($"Result: {result}");
		Visualise([], "");
	}



	public record ComboOperand(int Number)
	{
		public long Value(long[] registers) => Number switch
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
		public abstract long[] Evaluate(long[] registers);

		public static Instruction Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit(PARSE_SEP);
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
		public override long[] Evaluate(long[] registers) {
			long numerator = REG_A.GetReg(registers);
			long value = Operand.Value(registers);
			string debug = $"ADV  {value}: {numerator} / {(long)Math.Pow(2, value)}";

			registers = registers.SetReg(REG_A, numerator / (long)Math.Pow(2, value));

			registers.Visualise($"{debug} = {REG_A.GetReg(registers)}");
			return registers;
		}
	}

	private record BdvInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override long[] Evaluate(long[] registers)
		{
			long numerator = REG_A.GetReg(registers);
			long value = Operand.Value(registers);
			string debug = $"BDV  {value}: {numerator} / {(int)Math.Pow(2, value)}";

			registers = registers.SetReg(REG_B, numerator / (int)Math.Pow(2, value));

			registers.Visualise($"{debug} = {REG_B.GetReg(registers)}");
			return registers;
		}
	}

	private record CdvInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override long[] Evaluate(long[] registers)
		{
			long numerator = REG_A.GetReg(registers);
			long value = Operand.Value(registers);
			string debug = $"CDV  {value}: {numerator} / {(int)Math.Pow(2, value)}";

			registers = registers.SetReg(REG_C, numerator / (int)Math.Pow(2, value));

			registers.Visualise($"{debug} = {REG_C.GetReg(registers)}");
			return registers;
		}
	}

	private record BxlInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override long[] Evaluate(long[] registers)
		{
			long regBValue = REG_B.GetReg(registers);
			string debug = $"BXL  {Operand.Number}: {regBValue} XOR {Operand.Number}";
			registers = registers.SetReg(REG_B, regBValue ^ Operand.Number);

			registers.Visualise($"{debug} = {REG_B.GetReg(registers)}");
			return registers;
		}
	}

	private record BstInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override long[] Evaluate(long[] registers)
		{
			long operandValue = Operand.Value(registers);
			string debug = $"BST  {Operand.Number}: {operandValue} % 8";

			registers = registers.SetReg(REG_B, operandValue % 8);

			registers.Visualise($"{debug} = {REG_B.GetReg(registers)}");
			return registers;
		}
	}

	private record BxcInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override long[] Evaluate(long[] registers)
		{
			string debug = $"BXC   : {REG_B.GetReg(registers)} XOR {REG_C.GetReg(registers)}";

			registers = registers.SetReg(REG_B, REG_B.GetReg(registers) ^ REG_C.GetReg(registers));

			registers.Visualise($"{debug} = {REG_B.GetReg(registers)}");
			return registers;
		}
	}

	private record JnzInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override long[] Evaluate(long[] registers) => registers;
	}

	private record OutInstruction(ComboOperand Operand) : Instruction(Operand)
	{
		public override long[] Evaluate(long[] registers) => registers;
	}

	private const char PARSE_SEP = ' ';
	private const int REG_OFFSET = 'A';
	private const string   REG_A = "A";
	private const string   REG_B = "B";
	private const string   REG_C = "C";

	private const int REG_END_LINE = 3;
	private const int PROGRAM_LINE = 4;
}
