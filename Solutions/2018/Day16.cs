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

	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		List<Instruction> instructions = input.Select(ParseInstruction).ToList();
		return "** Solution not written yet **";
	}

	private static IEnumerable<Instruction> GetAllPossibleInstructions(Instruction instruction)
	{
		string[] instructionTypes = [
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

		foreach (string type in instructionTypes) {
			yield return type switch
			{
				"addr" => new AddrInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"addi" => new AddiInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"mulr" => new MulrInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"muli" => new MuliInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"banr" => new BanrInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"bani" => new BaniInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"borr" => new BorrInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"bori" => new BoriInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"setr" => new SetrInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"seti" => new SetiInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"gtir" => new GtirInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"gtri" => new GtriInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"gtrr" => new GtrrInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"eqir" => new EqirInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"eqri" => new EqriInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				"eqrr" => new EqrrInstruction(instruction.OpCode, instruction.A, instruction.B, instruction.C),
				_ => throw new NotImplementedException(),
			};
		}

	}

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
