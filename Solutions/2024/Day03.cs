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
		=> _instructions = [.. input.GetInstructions()];

	private static int Solution1() => _instructions.OfType<MulInstruction>().ProcessProgram();
	private static int Solution2() => _instructions.ProcessProgram();
}

file static partial class Day03Extensions
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
					DoInstruction                    => (agg.Sum            , true),
					DontInstruction                  => (agg.Sum            , false),
					MulInstruction mul when agg.DoIt => (agg.Sum + mul.Value, true),
					_ => agg,
				},
				agg => agg.Sum
			);
	}

	public static IEnumerable<Instruction> GetInstructions(this string[] input)
	{
		foreach (Match match in InstructionRegEx().Matches(string.Join("", input))) {
			yield return match.Value switch
			{
				DO   => new DoInstruction(match.Index),
				DONT => new DontInstruction(match.Index),
				_    => new MulInstruction(match.Index, match.As<int>("number1"), match.As<int>("number2")),
			};
		}
	}
}

internal sealed partial class Day03Types
{
	public abstract record Instruction(int Index);
	public sealed record DoInstruction(int Index) : Instruction(Index);
	public sealed record DontInstruction(int Index) : Instruction(Index);
	public sealed record MulInstruction(int Index, int Value1, int Value2) : Instruction(Index)
	{
		public int Value => Value1 * Value2;
	}

	[GeneratedRegex("""mul\((?<number1>\d{1,3}),(?<number2>\d{1,3})\)|do\(\)|don't\(\)""")]
	public static partial Regex InstructionRegEx();
}

file static class Day03Constants
{
	public const string DO   = "do()";
	public const string DONT = "don't()";
}
