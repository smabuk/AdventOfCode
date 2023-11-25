namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 16: Chronal Classification
/// https://adventofcode.com/2018/day/16
/// </summary>
[Description("Chronal Classification")]
public sealed partial class Day16 {

	[Init]
	public static    void Init(string[] input, params object[]? _) => LoadSamplesAndInstructions(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static IEnumerable<Instruction> _instructions = [];
	private static IEnumerable<Sample> _samples = [];

	private static void LoadSamplesAndInstructions(string[] input) {
		List<Sample> samples = [];
		int i = 0;
		for (i = 0; i < input.Length; i+=4) {
			if (input[i] == "" && input[i+1] == "") {
				break;
			}
			samples.Add(Sample.Parse(input[i..(i+4)]));
		}
		_samples = samples;
		_instructions = input[(i+2)..].Select(Instruction.Parse);
	}

	private static int Solution1(string[] input) {
		int noOfSamplesWith3OrMoreSolutions = 0;

		foreach (Sample sample in _samples) {
			int count = 0;
			foreach (Instruction instruction in GetAllPossibleInstructions(sample.Instruction)) {
				int[] after = instruction.Execute(sample.Before);
				if (sample.After[0] == after[0]
					&& sample.After[1] == after[1]
					&& sample.After[2] == after[2]
					&& sample.After[3] == after[3]
					) {
					count++;
				}
			}
			if (count >= 3) {
				noOfSamplesWith3OrMoreSolutions++;
			}

		}

		return noOfSamplesWith3OrMoreSolutions;
	}

	private static int Solution2(string[] input)
	{
		Dictionary<int, string[]> possibleOpCodes = UseSamplesToNarrowDownPossibilities();
		Dictionary<string, int[]> possibleOpTypes = [];

		Dictionary<int, string> opCodes = [];
		for (int i = 0; i < 16; i++) {
			opCodes[i] = possibleOpCodes[i].Length switch
			{
				1 => possibleOpCodes[i][0],
				_ => "",
			};

			possibleOpTypes[instructionTypes[i]] = [.. possibleOpCodes
				.Where(kv => kv.Value.Contains(instructionTypes[i]))
				.Select(kv => kv.Key)
				];
		}

		opCodes = FindTheOpcodes(opCodes, possibleOpCodes);

		//long permutations = possibleOpCodes.Aggregate(1, (total, next) => total * next.Value.Length);



		int[] registers = [0, 0, 0, 0];
		foreach (Instruction instruction in _instructions) {
			try {
				Instruction realInstruction = instruction.Convert(opCodes[instruction.OpCode]);
				registers = instruction.Execute(registers);
			}
			catch (Exception) {
				return 1234567890;
			}
		}

		return registers[0];
	}

	private static Dictionary<int, string> FindTheOpcodes(Dictionary<int, string> opCodes, Dictionary<int, string[]> possibleOpCodes)
	{
		if (opCodes.Count(op => op.Value == "") == 0) {
			return opCodes;
		}

		foreach (var item in possibleOpCodes) {

		}
		return [];
	}

	private static Dictionary<int, string[]> UseSamplesToNarrowDownPossibilities()
	{
		int noOfOpCodes = 16;
		Dictionary<int, string[]> opCodes = [];
		for (int i = 0; i < noOfOpCodes; i++) {
			opCodes.Add(i, [.. instructionTypes]);
		}

		bool somethingHasChanged = false;
		do {
			somethingHasChanged = false;
			for (int opCode = 0; opCode < noOfOpCodes; opCode++) {
				if (opCodes[opCode].Length == 1) {
					continue;
				}
				foreach (Sample sample in _samples.Where(ins => ins.Instruction.OpCode == opCode)) {
					int count = 0;
					string foundType = "";
					foreach (string type in opCodes[opCode]) {
						Instruction instruction = sample.Instruction.Convert(type);
						int[] after = instruction.Execute(sample.Before);
						if (sample.After[0] == after[0]
							&& sample.After[1] == after[1]
							&& sample.After[2] == after[2]
							&& sample.After[3] == after[3]
							) {
							count++;
							foundType = type;
						} else {
							List<string> codes = [.. opCodes[opCode]];
							if (codes.Remove(type)) {
								opCodes[opCode] = [.. codes];
								somethingHasChanged = false;
							}
						}
					}
					if (count == 1) {
						for (int op = 0; op < noOfOpCodes; op++) {
							if (op == opCode) {
								opCodes[opCode] = [foundType];
							} else {
								List<string> codes = [.. opCodes[op]];
								if (codes.Remove(foundType)) {
									opCodes[op] = [.. codes];
								}
							}
						}
						somethingHasChanged = false;
						break;
					}

				}

			}

		} while (somethingHasChanged && opCodes.Values.Count(v => v.Length > 1) != 0);

		return opCodes;
	}

	private static IEnumerable<Instruction> GetAllPossibleInstructions(Instruction instruction)
	{
		foreach (string type in instructionTypes) {
			yield return instruction.Convert(type);
		}

	}

	private static string[] instructionTypes = [
			"addr",
			"addi",
			"mulr",
			"muli",
			"banr",
			"bani",
			"borr",
			"bori",
			"setr",
			"seti",
			"gtir",
			"gtri",
			"gtrr",
			"eqir",
			"eqri",
			"eqrr",
			];

	private record AddrInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = startingState[A] + startingState[B];
			return endingState;
		}
	}

	private record AddiInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = startingState[A] + B;
			return endingState;
		}
	}

	private record MulrInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = startingState[A] * startingState[B];
			return endingState;
		}
	}

	private record MuliInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = startingState[A] * B;
			return endingState;
		}
	}

	private record BanrInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = startingState[A] & startingState[B];
			return endingState;
		}
	}

	private record BaniInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = startingState[A] & B;
			return endingState;
		}
	}

	private record BorrInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = startingState[A] | startingState[B];
			return endingState;
		}
	}

	private record BoriInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = startingState[A] | B;
			return endingState;
		}
	}

	private record SetrInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = startingState[A];
			return endingState;
		}
	}

	private record SetiInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = A;
			return endingState;
		}
	}

	private record GtirInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = A > startingState[B] ? 1 : 0;
			return endingState;
		}
	}

	private record GtriInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = startingState[A] > B ? 1 : 0;
			return endingState;
		}
	}

	private record GtrrInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = startingState[A] > startingState[B] ? 1 : 0;
			return endingState;
		}
	}

	private record EqirInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = A == startingState[B] ? 1 : 0;
			return endingState;
		}
	}

	private record EqriInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = startingState[A] == B ? 1 : 0;
			return endingState;
		}
	}

	private record EqrrInstruction(int OpCode, int A, int B, int C) : Instruction(OpCode, A, B, C)
	{
		public override int[] Execute(int[] startingState)
		{
			int[] endingState = [.. startingState];
			endingState[C] = startingState[A] == startingState[B] ? 1 : 0;
			return endingState;
		}
	}




	private record Instruction(int OpCode, int A, int B, int C) : IParsable<Instruction> {

		public virtual int[] Execute(int[] startingState) => throw new NotImplementedException();

		public Instruction Convert(string type)
		{
			return type switch
			{
				"addr" => new AddrInstruction(OpCode, A, B, C),
				"addi" => new AddiInstruction(OpCode, A, B, C),
				"mulr" => new MulrInstruction(OpCode, A, B, C),
				"muli" => new MuliInstruction(OpCode, A, B, C),
				"banr" => new BanrInstruction(OpCode, A, B, C),
				"bani" => new BaniInstruction(OpCode, A, B, C),
				"borr" => new BorrInstruction(OpCode, A, B, C),
				"bori" => new BoriInstruction(OpCode, A, B, C),
				"setr" => new SetrInstruction(OpCode, A, B, C),
				"seti" => new SetiInstruction(OpCode, A, B, C),
				"gtir" => new GtirInstruction(OpCode, A, B, C),
				"gtri" => new GtriInstruction(OpCode, A, B, C),
				"gtrr" => new GtrrInstruction(OpCode, A, B, C),
				"eqir" => new EqirInstruction(OpCode, A, B, C),
				"eqri" => new EqriInstruction(OpCode, A, B, C),
				"eqrr" => new EqrrInstruction(OpCode, A, B, C),
				_ => throw new ArgumentOutOfRangeException(nameof(type), $"""Cannot convert from type "{type}"."""),
			};
		}
		public static Instruction Parse(string s) => ParseInstruction(s);
		public static Instruction Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result) => throw new NotImplementedException();
	}

	private static Instruction ParseInstruction(string input) {
		Match match = InputRegEx().Match(input);
		if (match.Success) {
			return new( int.Parse(match.Groups["opcode"].Value),
						int.Parse(match.Groups["A"].Value),
						int.Parse(match.Groups["B"].Value),
						int.Parse(match.Groups["C"].Value));
		}
		return null!;
	}

	[GeneratedRegex("""(?<opcode>[\+\-]*\d+) (?<A>[\+\-]*\d+) (?<B>[\+\-]*\d+) (?<C>[\+\-]*\d+)""")]
	private static partial Regex InputRegEx();

	private record Sample(int[] Before, int[] After, Instruction Instruction)
	{
		public static Sample Parse(string[] input) 
			=> new([.. input[0][9..^1].Split(", ").AsInts()], [.. input[2][9..^1].Split(", ").AsInts()], Instruction.Parse(input[1]));
	}
}
