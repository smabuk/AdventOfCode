using static AdventOfCode.Solutions._2017.Day08Constants;
using static AdventOfCode.Solutions._2017.Day08Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 08: I Heard You Like Registers
/// https://adventofcode.com/2017/day/08
/// </summary>
[Description("I Heard You Like Registers")]
public sealed partial class Day08 {

	[Init]
	public static   void  Init(string[] input) => LoadInstructions(input);
	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static IEnumerable<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input) => _instructions = input.As<Instruction>();

	private static int Solution1() {
		Dictionary<string, int> registers = [];

		foreach (Instruction instruction in _instructions) {
			int regValue = registers.GetValueOrDefault(instruction.TargetRegister);
			int opRegValue = registers.GetValueOrDefault(instruction.Predicate.Register);

			if (instruction.Predicate.IsTrue(opRegValue)) {
				registers[instruction.TargetRegister] = instruction is IncInstruction
					? regValue + instruction.Value
					: regValue - instruction.Value;
			}
		}

		return registers.Values.Max();
	}

	private static int Solution2() {
		Dictionary<string, int> registers = [];
		int maxValue = int.MinValue;

		foreach (Instruction instruction in _instructions) {
			int opRegValue = registers.GetValueOrDefault(instruction.Predicate.Register);

			if (instruction.Predicate.IsTrue(opRegValue)) {
				int regValue = registers.GetValueOrDefault(instruction.TargetRegister);
				registers[instruction.TargetRegister] = instruction is IncInstruction
					? regValue + instruction.Value
					: regValue - instruction.Value;
				maxValue = int.Max(maxValue, registers[instruction.TargetRegister]);
			}
		}

		return maxValue;
	}
}

file static class Day08Extensions
{
	public static bool IsTrue(this InstructionPredicate ip, int regValue = 0)
	{
		return ip.Operator switch
		{
			Operator.Equal              => regValue == ip.Value,
			Operator.NotEqual           => regValue != ip.Value,
			Operator.LessThan           => regValue < ip.Value,
			Operator.LessThanOrEqual    => regValue <= ip.Value,
			Operator.GreaterThan        => regValue > ip.Value,
			Operator.GreaterThanOrEqual => regValue >= ip.Value,
			_ => throw new NotImplementedException(),
		};
	}

	public static Operator ToOperator(this string op)
	{
		return op switch
		{
			"==" => Operator.Equal,
			"!=" => Operator.NotEqual,
			"<"  => Operator.LessThan,
			"<=" => Operator.LessThanOrEqual,
			">"  => Operator.GreaterThan,
			">=" => Operator.GreaterThanOrEqual,

			_ => throw new NotImplementedException(),
		};
	}
}

internal sealed partial class Day08Types
{
	public abstract record Instruction(string TargetRegister, int Value, InstructionPredicate Predicate) : IParsable<Instruction>
	{
		public static Instruction Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit(SPACE);

			InstructionPredicate ip = new(tokens[4], tokens[5].ToOperator(), tokens[6].As<int>());

			return tokens[1] switch
			{
				"inc" => new IncInstruction(tokens[0], tokens[2].As<int>(), ip),
				"dec" => new DecInstruction(tokens[0], tokens[2].As<int>(), ip),
				_ => throw new NotImplementedException(),
			};
		}

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	public sealed record IncInstruction(string TargetRegister, int Value, InstructionPredicate Predicate)
		: Instruction(TargetRegister, Value, Predicate);

	public sealed record DecInstruction(string TargetRegister, int Value, InstructionPredicate Predicate)
		: Instruction(TargetRegister, Value, Predicate);

	public record InstructionPredicate(string Register, Operator Operator, int Value);

	public enum Operator
	{
		Equal,
		NotEqual,
		LessThan,
		LessThanOrEqual,
		GreaterThan,
		GreaterThanOrEqual,
	}

}

file static class Day08Constants
{
	public const char SPACE = ' ';
}
