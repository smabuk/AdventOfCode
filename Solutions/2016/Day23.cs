using static AdventOfCode.Solutions._2016.Day23Constants;
using static AdventOfCode.Solutions._2016.Day23Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 23: Safe Cracking
/// https://adventofcode.com/2016/day/23
/// </summary>
[Description("Safe Cracking")]
public sealed partial class Day23 {

	[Init]
	public static   void  Init(string[] input) => LoadInstructions(input);
	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static List<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input) => _instructions = [.. input.As<Instruction>()];

	private static int Solution1()
	{
		int[] registers = [7, 0, 0, 0];
		ExecuteCode(registers);
		return registers["a".RegIndex()];
	}

	private static string Solution2()
	{
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}

	private static void ExecuteCode(int[] registers)
	{
		List<Instruction> instructions = [.._instructions];

		for (int assembunnyPtr = 0; assembunnyPtr < instructions.Count; assembunnyPtr++) {
			Instruction instruction = instructions[assembunnyPtr];
			switch (instruction) {
				case CpyInstruction cpyInstruction:
					if (char.IsAsciiLetterLower(cpyInstruction.Y[0])) {
						registers[cpyInstruction.Y.RegIndex()] = cpyInstruction.X.GetValue(registers);
					}

					break;
				case IncInstruction incInstruction:
					if (char.IsAsciiLetterLower(incInstruction.X[0])) {
						registers[incInstruction.X.RegIndex()]++;
					}

					break;
				case DecInstruction decInstruction:
					if (char.IsAsciiLetterLower(decInstruction.X[0])) {
						registers[decInstruction.X.RegIndex()]--;
					}

					break;
				case JnzInstruction jnzInstruction:
					assembunnyPtr += jnzInstruction.X.GetValue(registers) == 0
						? 0
						: jnzInstruction.Y.GetValue(registers) - 1;
					break;
				case TglInstruction tglInstruction:
					if (char.IsAsciiLetterLower(tglInstruction.X[0])) {
						int targetIndex = registers[tglInstruction.X.RegIndex()] + assembunnyPtr;
						if (targetIndex >= instructions.Count) {
							break;
						}
						Instruction target = instructions[targetIndex];
						instructions[targetIndex] = target.ToggleInstruction();
					}
					break;
				default:
					break;
			}
		}
	}
}

file static class Day23Extensions
{
	public static int GetValue(this string valueOrReg, int[] registers)
	{
		return Char.IsLetter(valueOrReg[0])
			? registers[valueOrReg.RegIndex()]
			: valueOrReg.As<int>();
	}

	public static int RegIndex(this string registerName) => registerName[0] - REG_OFFSET;
	
	public static Instruction ToggleInstruction(this Instruction instruction)
	{
		Instruction toggled = instruction switch
		{
			IncInstruction inc => new DecInstruction(inc.X),
			DecInstruction dec => new IncInstruction(dec.X),
			JnzInstruction jnz => new CpyInstruction(jnz.X, jnz.Y),
			CpyInstruction cpy => new JnzInstruction(cpy.X, cpy.Y),
			TglInstruction tgl => new IncInstruction(tgl.X),
			_ => throw new NotImplementedException()
		};

		return toggled;
	}
}

internal sealed partial class Day23Types
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
				"tgl" => new TglInstruction(tokens[1]),
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
	public record TglInstruction(string X) : Instruction();
}

file static class Day23Constants
{
	public const int REG_OFFSET = 'a';
}
