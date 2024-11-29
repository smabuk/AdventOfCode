﻿using static AdventOfCode.Solutions._2017.Day23Constants;
using static AdventOfCode.Solutions._2017.Day23Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 23: Coprocessor Conflagration
/// https://adventofcode.com/2017/day/23
/// </summary>
[Description("Coprocessor Conflagration")]
public sealed partial class Day23 {

	[Init]
	public static   void  Init(string[] input) => LoadInstructions(input);
	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static List<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input) {
		_instructions = [.. input.As<Instruction>()];
	}

	private static long Solution1() {
		return _instructions.ExecuteCodePart1(new long[8]);
	}

	private static string Solution2() {
		return NO_SOLUTION_WRITTEN_MESSAGE;
		//long[] registers = new long[8];
		//registers[0] = 1;
		//_ = _instructions.ExecuteCodePart1(registers);
		//return "h".GetValue(registers).ToString();
	}
}

file static class Day23Extensions
{
	public static long ExecuteCodePart1(this List<Instruction> instructions, long[] registers)
	{
		long noOfMuls = 0;

		for (int ptr = 0; ptr < instructions.Count; ptr++) {
			Instruction instruction = instructions[ptr];
			switch (instruction) {
				case SetInstruction setInstruction:
					_ = registers.SetValue(setInstruction.X, setInstruction.Y);
					break;
				case SubInstruction subInstruction:
					registers[subInstruction.X.RegIndex()] -= subInstruction.Y.GetValue(registers);
					break;
				case MulInstruction mulInstruction:
					registers[mulInstruction.X.RegIndex()] *= mulInstruction.Y.GetValue(registers);
					noOfMuls++;
					break;
				case JnzInstruction jnzInstruction:
					ptr += jnzInstruction.X.GetValue(registers) != 0
						? (int)jnzInstruction.Y.GetValue(registers) - 1
						: 0;
					break;
				default:
					break;
			}
		}

		return noOfMuls;
	}

	public static long GetValue(this string valueOrReg, long[] registers)
	{
		return Char.IsLetter(valueOrReg[0])
			? registers[valueOrReg.RegIndex()]
			: valueOrReg.As<long>();
	}

	public static long[] SetValue(this long[] registers, string reg, string valueOrReg )
	{
		registers[reg.RegIndex()] = valueOrReg.GetValue(registers);
		return registers;
	}

	public static int RegIndex(this string registerName) => registerName[0] - REG_OFFSET;
}

internal sealed partial class Day23Types
{

	public abstract record Instruction() : IParsable<Instruction>
	{
		public static Instruction Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit(SPACE);

			return tokens[0] switch
			{
				"set" => new SetInstruction(tokens[1], tokens[2]),
				"sub" => new SubInstruction(tokens[1], tokens[2]),
				"mul" => new MulInstruction(tokens[1], tokens[2]),
				"jnz" => new JnzInstruction(tokens[1], tokens[2]),
				_ => throw new NotImplementedException(),
			};
		}

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	public record SetInstruction(string X, string Y) : Instruction();
	public record SubInstruction(string X, string Y) : Instruction();
	public record MulInstruction(string X, string Y) : Instruction();
	public record JnzInstruction(string X, string Y) : Instruction();
}

file static class Day23Constants
{
	public const int REG_OFFSET = 'a';
	public const char SPACE = ' ';
}
