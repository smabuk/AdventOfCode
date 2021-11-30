namespace AdventOfCode.Solutions.Year2019;

/// <summary>
/// Day 02: 1202 Program Alarm
/// https://adventofcode.com/2019/day/XX
/// </summary>
public class Day02 {

	private static string Solution1(string[] input, bool testing = false) {
		string inputLine = input[0];
		int[] inputs = inputLine.Split(",").Select(i => int.Parse(i)).ToArray();

		if (testing == false) {
			inputs[1] = 12;
			inputs[2] = 2;
		}

		return ExecuteIntcodeProgram(inputs);
	}

	private static string ExecuteIntcodeProgram(int[] inputs) {
		for (int i = 0; i < inputs.Length && inputs[i] != 99; i++) {
			switch (inputs[i]) {
				case 99:
					break;
				case 1:
					inputs[inputs[i + 3]] = inputs[inputs[i + 1]] + inputs[inputs[i + 2]];
					i += 3;
					break;
				case 2:
					inputs[inputs[i + 3]] = inputs[inputs[i + 1]] * inputs[inputs[i + 2]];
					i += 3;
					break;
				default:
					break;
			}
		}
		return String.Join(",", inputs);
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
				int result = int.Parse(ExecuteIntcodeProgram(newInputs).Split(",").First());
				if (result == ExpectedResult) {
					return 100 * noun + verb;
				};
			}
		}

		throw new Exception();
	}



	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		bool testing = GetArgument(args, 1, false);
		if (testing is false)
			{ return Solution1(input).Split(",").First().ToString(); }
		return Solution1(input, testing).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
