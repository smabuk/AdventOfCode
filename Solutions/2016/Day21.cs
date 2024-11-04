using System;

using static AdventOfCode.Solutions._2016.Day21Constants;
using static AdventOfCode.Solutions._2016.Day21Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 21: Scrambled Letters and Hash
/// https://adventofcode.com/2016/day/21
/// </summary>
[Description("Scrambled Letters and Hash")]
public sealed partial class Day21 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? args)
	{
		string password = GetArgument<string>(args, argumentNumber: 1, defaultResult: "abcdefgh");
		return Solution1(input, password).ToString();
	}

	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static List<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input) {
		_instructions = [..input.As<Instruction>()];
	}

	private static string Solution1(string[] input, string password) {
		string scrambled = password;
		foreach (Instruction instruction in _instructions) {
			scrambled = instruction.Scramble(scrambled);
		}

		return scrambled;
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day21Extensions
{
}

internal sealed partial class Day21Types
{

	public abstract record Instruction() : IParsable<Instruction>
	{
		public abstract string Scramble(ReadOnlySpan<char> input);
		public static Instruction Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit([' ']);
			return (tokens[0], tokens[1]) switch
			{
				("swap", "position") => new SwapPosition(tokens[2].As<int>(), tokens[5].As<int>()),
				("swap", "letter") => new SwapLetter(tokens[2][0], tokens[5][0]),
				("rotate", "left") => new RotateLeft(tokens[2].As<int>()),
				("rotate", "right") => new RotateRight(tokens[2].As<int>()),
				("rotate", "based") => new RotateRightBasedOnPosition(tokens[6][0]),
				("reverse", "positions") => new ReversePositions(tokens[2].As<int>(), tokens[4].As<int>()),
				("move", "position") => new MovePosition(tokens[2].As<int>(), tokens[5].As<int>()),
				_ => throw new NotImplementedException(),
			};
		}

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	public record SwapPosition(int X, int Y) : Instruction
	{
		public override string Scramble(ReadOnlySpan<char> input)
		{
			Span<char> output = input.ToArray();
			(output[X], output[Y]) = (output[Y], output[X]); 
			return output.ToString();
		}
	}

	public record SwapLetter(char X, char Y) : Instruction
	{
		public override string Scramble(ReadOnlySpan<char> input)
		{
			Span<char> output = input.ToArray();
			output[input.IndexOf(X)] = Y;
			output[input.IndexOf(Y)] = X;
			return output.ToString();
		}
	}

	public record RotateLeft(int X) : Instruction
	{
		public override string Scramble(ReadOnlySpan<char> input)
		{
			int amount = X % input.Length;
			return new string([..input[amount..], ..input[0..amount]]);
		}
	}

	public record RotateRight(int X) : Instruction
	{
		public override string Scramble(ReadOnlySpan<char> input)
		{
			int amount = input.Length - (X % input.Length);
			return new string([.. input[amount..], .. input[0..amount]]);
		}
	}

	public record RotateRightBasedOnPosition(char X) : Instruction
	{
		public override string Scramble(ReadOnlySpan<char> input)
		{
			int indexX = input.IndexOf(X);
			RotateRight rotateRight = new(1 + indexX + ((indexX >= 4) ? 1 : 0));
			return rotateRight.Scramble(input);
		}
	}

	public record ReversePositions(int X, int Y) : Instruction
	{
		public override string Scramble(ReadOnlySpan<char> input)
		{
			Span<char> output = input.ToArray();
			for (int i = 0; i <= Y - X; i++) {
				output[X + i] = input[Y - i];
			}

			return output.ToString();
		}
	}

	public record MovePosition(int X, int Y) : Instruction
	{
		public override string Scramble(ReadOnlySpan<char> input)
		{
			List<char> output = [..input];
			output.RemoveAt(X);
			output.Insert(Y, input[X]);
			return string.Join("", output);
		}
	}
}

file static class Day21Constants
{
}
