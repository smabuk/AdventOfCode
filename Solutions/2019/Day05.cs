using System.ComponentModel;

namespace AdventOfCode.Solutions.Year2019;

/// <summary>
/// Day 05: Sunny with a Chance of Asteroids
/// https://adventofcode.com/2019/day/5
/// </summary>
[Description("Sunny with a Chance of Asteroids")]
public class Day05 {

	record RecordType(string Name, int Value);

	private static string Solution1(string[] input, int[] programInput) {
		string inputLine = input[0];
		int[] program = inputLine.Split(",").Select(i => int.Parse(i)).ToArray();

		int[] _ = IntcodeComputer.ExecuteIntcodeProgram(program, programInput, out int[] output);

		return output[^1].ToString();
	}

	private static string Solution2(string[] input, int[] programInput) {
		string inputLine = input[0];
		int[] program = inputLine.Split(",").Select(i => int.Parse(i)).ToArray();

		int[] _ = IntcodeComputer.ExecuteIntcodeProgram(program, programInput, out int[] output);

		return output[^1].ToString();
	}

	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		int[] programInput = GetArgument(args, 1, new int[] { 1 });
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input, programInput).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		int[] programInput = GetArgument(args, 1, new int[] { 5 });
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input, programInput).ToString();
	}
	#endregion

}
