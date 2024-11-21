using static AdventOfCode.Solutions._2017.Day18Constants;
using static AdventOfCode.Solutions._2017.Day18Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 18: Duet
/// https://adventofcode.com/2016/day/18
/// </summary>
[Description("Duet")]
public sealed partial class Day18 {

	[Init]
	public static   void  Init(string[] input) => LoadInstructions(input);
	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static List<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input) =>
		_instructions = [.. input.As<Instruction>()];

	private static long Solution1() {
		long[] registers = new long[26];
		return _instructions.ExecuteCodePart1(registers);
	}

	private static int Solution2() {
		Program[] programs = [
			new(0, new long[26], _instructions, [], []),
			new(1, new long[26], _instructions, [], []),
		];

		programs[0].Registers["p".RegIndex()] = 0;
		programs[1].Registers["p".RegIndex()] = 1;

		programs[0] = programs[0] with { Input = programs[1].Output };
		programs[1] = programs[1] with { Input = programs[0].Output };
		
		//programs[1].Output.Enqueue(111);
		//programs[0].Output.Enqueue(222);



		return int.MinValue;
	}
}

file static class Day18Extensions
{
	public static long ExecuteCodePart1(this List<Instruction> instructions, long[] registers)
	{
		long lastSound = int.MinValue;

		for (int ptr = 0; ptr < instructions.Count; ptr++) {
			Instruction instruction = instructions[ptr];
			switch (instruction) {
				case SndInstruction sndInstruction:
					lastSound = sndInstruction.X.GetValue(registers);
					break;
				case SetInstruction setInstruction:
					registers[setInstruction.X.RegIndex()] = setInstruction.Y.GetValue(registers);
					break;
				case AddInstruction addInstruction:
					registers[addInstruction.X.RegIndex()] += addInstruction.Y.GetValue(registers);
					break;
				case MulInstruction mulInstruction:
					registers[mulInstruction.X.RegIndex()] *= mulInstruction.Y.GetValue(registers);
					break;
				case ModInstruction modInstruction:
					registers[modInstruction.X.RegIndex()] %= modInstruction.Y.GetValue(registers);
					break;
				case RcvInstruction rcvInstruction:
					if (rcvInstruction.X.GetValue(registers) != 0) {
						return lastSound;
					};
					break;
				case JgzInstruction jgzInstruction:
					ptr += jgzInstruction.X.GetValue(registers) > 0
						? (int)jgzInstruction.Y.GetValue(registers) - 1
						: 0;
					break;
				default:
					break;
			}
		}

		return lastSound;
	}

	public static long ExecuteCodePart2_Duet(this Program[] programs)
	{
		List<Instruction> instructions = programs[0].Instructions;
		long[] registers = programs[0].Registers;
		Queue<long> input = programs[0].Input;
		Queue<long> output = programs[0].Output;

		for (int ptr = 0; ptr < instructions.Count; ptr++) {
			Instruction instruction = instructions[ptr];
			switch (instruction) {
				case SndInstruction sndInstruction:
					output.Enqueue(sndInstruction.X.GetValue(registers));
					break;
				case SetInstruction setInstruction:
					registers[setInstruction.X.RegIndex()] = setInstruction.Y.GetValue(registers);
					break;
				case AddInstruction addInstruction:
					registers[addInstruction.X.RegIndex()] += addInstruction.Y.GetValue(registers);
					break;
				case MulInstruction mulInstruction:
					registers[mulInstruction.X.RegIndex()] *= mulInstruction.Y.GetValue(registers);
					break;
				case ModInstruction modInstruction:
					registers[modInstruction.X.RegIndex()] %= modInstruction.Y.GetValue(registers);
					break;
				case RcvInstruction rcvInstruction:
					if (input.Count != 0) {
						registers[rcvInstruction.X.RegIndex()] = input.Dequeue();
					} else {
						ptr--;
					}

					break;
				case JgzInstruction jgzInstruction:
					ptr += jgzInstruction.X.GetValue(registers) > 0
						? (int)jgzInstruction.Y.GetValue(registers) - 1
						: 0;
					break;
				default:
					break;
			}
		}

		return -999;
	}

	public static long GetValue(this string valueOrReg, long[] registers)
	{
		return Char.IsLetter(valueOrReg[0])
			? registers[valueOrReg.RegIndex()]
			: valueOrReg.As<long>();
	}

	public static int RegIndex(this string registerName) => registerName[0] - REG_OFFSET;
}

internal sealed partial class Day18Types
{

	public record Program(int Id, long[] Registers, List<Instruction> Instructions, Queue<long> Input, Queue<long> Output);

	public abstract record Instruction() : IParsable<Instruction>
	{
		public static Instruction Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit(SPACE);

			return tokens[0] switch
			{
				"snd" => new SndInstruction(tokens[1]),
				"set" => new SetInstruction(tokens[1], tokens[2]),
				"add" => new AddInstruction(tokens[1], tokens[2]),
				"mul" => new MulInstruction(tokens[1], tokens[2]),
				"mod" => new ModInstruction(tokens[1], tokens[2]),
				"rcv" => new RcvInstruction(tokens[1]),
				"jgz" => new JgzInstruction(tokens[1], tokens[2]),
				_ => throw new NotImplementedException(),
			};
		}

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	public record SndInstruction(string X) : Instruction();
	public record SetInstruction(string X, string Y) : Instruction();
	public record AddInstruction(string X, string Y) : Instruction();
	public record MulInstruction(string X, string Y) : Instruction();
	public record ModInstruction(string X, string Y) : Instruction();
	public record RcvInstruction(string X) : Instruction();
	public record JgzInstruction(string X, string Y) : Instruction();
}

file static class Day18Constants
{
	public const int REG_OFFSET = 'a';
	public const char SPACE = ' ';
}
