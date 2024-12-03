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
		=> _instructions = [.. Instruction.Parse(string.Join("", input))];

	private static int Solution1() => _instructions.OfType<MulInstruction>().ProcessProgram();
	private static int Solution2() => _instructions.ProcessProgram();
}

file static class Day03Extensions
{
	public static int ProcessProgram(this IEnumerable<Instruction> instructions)
	{
		return instructions
			// Already in Index order so no need to actually sort it
			//.OrderBy(i => i.Index)
			.Aggregate(
				(Sum: 0, DoIt: true),
				((int Sum, bool DoIt) agg, Instruction instruction) => instruction switch
				{
					DoInstruction  doi               => (agg.Sum            , doi.DoIt),
					MulInstruction mul when agg.DoIt => (agg.Sum + mul.Value, true),
					_ => agg,
				},
				agg => agg.Sum
			);
	}
}

internal sealed partial class Day03Types
{
	public abstract record Instruction(int Index)
	{
		public static IEnumerable<Instruction> Parse(string s)
		{
			MatchCollection matches = InstructionRegEx().Matches(s);
			foreach (Match match in matches) {
				yield return match.Value[0] switch
				{
					'm' => new MulInstruction(match.Index, match.As<int>("number1"), match.As<int>("number2")),
					'd' => new DoInstruction (match.Index, match.Value == "do()"),
					_ => throw new NotImplementedException(),
				};
			}
		}
	}

	public sealed record MulInstruction(int Index, int Value1, int Value2) : Instruction(Index)
	{
		public int Value => Value1 * Value2;
	}

	public sealed record DoInstruction(int Index, bool DoIt) : Instruction(Index);


	[GeneratedRegex("""mul\((?<number1>\d+),(?<number2>\d+)\)|do\(\)|don't\(\)""")]
	public static partial Regex InstructionRegEx();
}

file static class Day03Constants
{
}
