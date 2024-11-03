using static AdventOfCode.Solutions._2016.Day12Constants;
using static AdventOfCode.Solutions._2016.Day12Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 12: Leonardo's Monorail
/// https://adventofcode.com/2016/day/12
/// </summary>
[Description("Leonardo's Monorail")]
public sealed partial class Day12 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static List<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input) {
		_instructions = [..input.As<Instruction>()];
	}

	private static int Solution1(string[] input) {
		int[] registers = [0, 0, 0, 0];

		for (int assembunnyPtr = 0; assembunnyPtr < _instructions.Count(); assembunnyPtr++) {
			Instruction instruction = _instructions[assembunnyPtr];
			switch (instruction) {
				case CpyInstruction cpyInstruction:
					registers[cpyInstruction.Y.RegIndex()] = cpyInstruction.X.GetValue(registers);
					break;
				case IncInstruction incInstruction:
					registers[incInstruction.X.RegIndex()]++;
					break;
				case DecInstruction decInstruction:
					registers[decInstruction.X.RegIndex()]--;
					break;
				case JnzInstruction jnzInstruction:
					assembunnyPtr += jnzInstruction.X.GetValue(registers) == 0
						? 0
						: jnzInstruction.Y.GetValue(registers) - 1;
					break;
				default:
					break;
			}
		}

		return registers["a".RegIndex()];
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day12Extensions
{
	public static int GetValue(this string valueOrReg, int[] registers)
	{
		return Char.IsLetter(valueOrReg[0])
			? registers[valueOrReg.RegIndex()]
			: valueOrReg.As<int>();
	}

	public static int RegIndex(this string registerName) => registerName[0] - REG_OFFSET;
}

internal sealed partial class Day12Types
{
	public abstract record Instruction() : IParsable<Instruction>
	{
		public static Instruction Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit([' ']);
			return tokens[0] switch
			{
				"cpy" => new CpyInstruction(tokens[1], tokens[2]),
				"inc" => new IncInstruction(tokens[1]),
				"dec" => new DecInstruction(tokens[1]),
				"jnz" => new JnzInstruction(tokens[1], tokens[2]),
				_ => throw new NotImplementedException(),
			};
		}

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	public record CpyInstruction(string X, string Y) : Instruction();
	public record IncInstruction(string X) : Instruction();
	public record DecInstruction(string X) : Instruction();
	public record JnzInstruction(string X, string Y) : Instruction();
}

file static class Day12Constants
{
	public const int REG_OFFSET = 'a';
}
