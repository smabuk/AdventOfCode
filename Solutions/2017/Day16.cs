using static AdventOfCode.Solutions._2017.Day16Constants;
using static AdventOfCode.Solutions._2017.Day16Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 16: Permutation Promenade
/// https://adventofcode.com/2016/day/16
/// </summary>
[Description("Permutation Promenade")]
public sealed partial class Day16 {

	[Init]
	public static   void  Init(string[] input) => LoadInstructions(input);
	public static string Part1(string[] _, params object[]? args)
	{
		string programs = GetArgument<string>(args, argumentNumber: 1, defaultResult: "abcdefghijklmnop");
		return Solution1(programs).ToString();
	}

	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static List<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input) =>
		_instructions = [.. input[0].TrimmedSplit(COMMA).As<Instruction>()];

	private static string Solution1(string programs) => _instructions.Dance(programs);

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day16Extensions
{
	public static string Dance(this IEnumerable<Instruction> instructions, string programs)
	{
		string current = programs;
		foreach (var instruction in instructions) {
			current = current.Dance(instruction);
		}

		return current;
	}


	public static string Dance(this string programs, Instruction instruction)
	{
		string current = programs;
		return instruction switch
		{
			SpinInstruction spin => new([.. current[^spin.Value..], .. current[..^spin.Value]]),
			ExchangeInstruction exchange => current.Exchange(exchange.PositionA, exchange.PositionB),
			PartnerInstruction partner => current.Partner(partner.A, partner.B),
			_ => throw new NotImplementedException(),
		};
	}

	public static string Exchange(this string input, int a, int b)
	{
		Span<char> output = input.ToArray();
		(output[a], output[b]) = (output[b], output[a]);
		return output.ToString();
	}

	public static string Partner(this string input, char a, char b)
	{
		Span<char> output = input.ToArray();
		int ia = input.IndexOf(a);
		int ib = input.IndexOf(b);
		(output[ia], output[ib]) = (output[ib], output[ia]);
		return output.ToString();
	}
}

internal sealed partial class Day16Types
{

	public abstract record Instruction() : IParsable<Instruction>
	{
		public static Instruction Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s[1..].TrimmedSplit(SLASH);

			return s[0] switch
			{
				SPIN     => new SpinInstruction(tokens[0].As<int>()),
				EXCHANGE => new ExchangeInstruction(tokens[0].As<int>(), tokens[1].As<int>()),
				PARTNER  => new PartnerInstruction(tokens[0][0], tokens[1][0]),
				_ => throw new NotImplementedException(),
			};
		}

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	public record SpinInstruction(int Value) : Instruction();
	public record ExchangeInstruction(int PositionA, int PositionB) : Instruction();
	public record PartnerInstruction(char A, char B) : Instruction();
}

file static class Day16Constants
{
	public const char COMMA  = ',';
	public const char SLASH  = '/';

	public const char PARTNER  = 'p';
	public const char SPIN     = 's';
	public const char EXCHANGE = 'x';
}
