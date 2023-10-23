namespace AdventOfCode.Solutions._2019;

/// <summary>
/// Day 02: 1202 Program Alarm
/// https://adventofcode.com/2019/day/2
/// </summary>
[Description("1202 Program Alarm")]
public class Day02 {

	public static string Part1(string[] input, params object[]? args) {
		int[] programInput = GetArgument(args, 1, new int[] { 12, 2 });
		return Solution1(input, programInput).ToString();
	}

	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input, int[] programReplacements) {
		string inputLine = input[0];
		int[] inputs = inputLine.Split(",").Select(i => int.Parse(i)).ToArray();

		for (int i = 0; i < programReplacements.Length; i++) {
			inputs[i + 1] = programReplacements[i];
		}

		return IntcodeComputer.ExecuteIntcodeProgram(inputs).First();
	}

	private static int Solution2(string[] input) {
		string inputLine = input[0];
		int[] inputs = inputLine.Split(",").Select(i => int.Parse(i)).ToArray();
		const int ExpectedResult = 19690720;

		for (int noun = 0; noun < 100; noun++) {
			for (int verb = 0; verb < 100; verb++) {
				int[] newInputs = (int[])inputs.Clone();
				newInputs[1] = noun;
				newInputs[2] = verb;
				int result = IntcodeComputer.ExecuteIntcodeProgram(newInputs)[0];
				if (result == ExpectedResult) {
					return (100 * noun) + verb;
				};
			}
		}

		throw new Exception();
	}
}
