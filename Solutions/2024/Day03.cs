using static AdventOfCode.Solutions._2024.Day03Constants;
using static AdventOfCode.Solutions._2024.Day03Types;
namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 03: Mull It Over
/// https://adventofcode.com/2024/day/03
/// </summary>
[Description("Mull It Over")]
public sealed partial class Day03 {

	[Init]
	public static   void  Init(string[] input) => LoadInstructions(input);
	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static IEnumerable<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input)
		=> _instructions = [.. Instruction.ParseAll(input)];

	private static int Solution1()
		=> _instructions
			.OfType<MulInstruction>()
			.Sum(mul => mul.Value1 * mul.Value2);

	private static int Solution2()
	{
		int sum = 0;
		bool doit = true;

		foreach (Instruction? instruction in _instructions.OrderBy( i => i.Index)) {
			if (instruction is DoInstruction doInstruction) {
				doit = doInstruction.DoIt;
			} else if (doit) {
				MulInstruction mul = (MulInstruction)instruction;
				sum += mul.Value1 * mul.Value2;
			}
		}

		return sum;
	}
}

file static class Day03Extensions
{
}

internal sealed partial class Day03Types
{
	public abstract record Instruction(int Index) : IParsable<Instruction>
	{
		public static IEnumerable<Instruction> ParseAll(string[] s)
		{
			const int INDEX_MULTIPLIER = 10_000;

			for (int i = 0; i < s.Length; i++) {
				string line = s[i];
				MatchCollection matches = MulRegEx().Matches(line);
				foreach (Match match in matches) {
					yield return new MulInstruction((i * INDEX_MULTIPLIER) + match.Index, match.As<int>("number1"), match.As<int>("number2"));
				}
			}

			for (int i = 0; i < s.Length; i++) {
				string line = s[i];
				MatchCollection matches = DoRegEx().Matches(line);
				foreach (Match match in matches) {
					yield return new DoInstruction((i * INDEX_MULTIPLIER) + match.Index, match.Value == "do()");
				}
			}
		}

		public static Instruction Parse(string s, IFormatProvider? provider) => null!;

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	public sealed record MulInstruction(int Index, int Value1, int Value2) : Instruction(Index);
	public sealed record DoInstruction(int Index, bool DoIt) : Instruction(Index);


	[GeneratedRegex("""mul\((?<number1>\d+),(?<number2>\d+)\)""")]
	public static partial Regex MulRegEx();

	[GeneratedRegex("""(?<do>do\(\)|don't\(\))""")]
	public static partial Regex DoRegEx();
}

file static class Day03Constants
{
}
