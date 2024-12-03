using static AdventOfCode.Solutions._2024.Day03Constants;
using static AdventOfCode.Solutions._2024.Day03Types;
namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 03: Mull It Over
/// https://adventofcode.com/2024/day/03
/// </summary>
[Description("Mull It Over")]
public sealed partial class Day03 {

	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static IEnumerable<Instruction> _instructions = [];

	[Init]
	public static void LoadInstructions(string[] input)
		=> _instructions = [.. Instruction.ParseAll(input)];

	private static int Solution1()
		=> _instructions.OfType<MulInstruction>().Sum(mul => mul.Value);

	private static int Solution2() => _instructions.ProcessProgram();

}

file static class Day03Extensions
{
	public static int ProcessProgram(this IEnumerable<Instruction> instructions)
	{
		int sum = 0;
		bool doit = true;

		foreach (Instruction instruction in instructions.OrderBy(i => i.Index)) {
			if (instruction is DoInstruction doInstruction) {
				doit = doInstruction.DoIt;
			} else if (doit) {
				sum += ((MulInstruction)instruction).Value;
			}
		}

		return sum;
	}
}

internal sealed partial class Day03Types
{
	public abstract record Instruction(int Index)
	{
		public static IEnumerable<Instruction> ParseAll(string[] s)
		{
			const int INDEX_MULTIPLIER = 100_000;

			for (int i = 0; i < s.Length; i++) {
				MatchCollection matches = InstructionRegEx().Matches(s[i]);
				foreach (Match match in matches) {
					yield return match.Value[0] switch
					{
						'm' => new MulInstruction((i * INDEX_MULTIPLIER) + match.Index, match.As<int>("number1"), match.As<int>("number2")),
						'd' => new DoInstruction((i * INDEX_MULTIPLIER) + match.Index, match.Value == "do()"),
						_ => throw new NotImplementedException(),
					};
				}
			}
		}
	}

	public sealed record MulInstruction(int Index, int Value1, int Value2) : Instruction(Index)
	{
		public int Value => Value1 * Value2;
	}

	public sealed record DoInstruction(int Index, bool DoIt) : Instruction(Index);


	[GeneratedRegex("""mul\((?<number1>\d+),(?<number2>\d+)\)|(?<do>do\(\)|don't\(\))""")]
	public static partial Regex InstructionRegEx();
}

file static class Day03Constants
{
}
