namespace AdventOfCode.Solutions.Year2019;

/// <summary>
/// Day 05: Sunny with a Chance of Asteroids
/// https://adventofcode.com/2019/day/5
/// </summary>
public class Day05 {

	record RecordType(string Name, int Value);

	private static string Solution1(string[] input, int[] programInput) {
		string inputLine = input[0];
		int[] program = inputLine.Split(",").Select(i => int.Parse(i)).ToArray();

		int[] newProgram = IntcodeComputer.ExecuteIntcodeProgram(program, programInput, out int[] output);
		return output[^1].ToString();
	}

	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		List<RecordType> instructions = input.Select(i => ParseLine(i)).ToList();
		return "** Solution not written yet **";
	}

	private static RecordType ParseLine(string input) {
		//MatchCollection match = Regex.Matches(input, @"(opt1|opt2|opt3) ([\+\-]\d+)");
		Match match = Regex.Match(input, @"(opt1|opt2|opt3) ([\+\-]\d+)");
		if (match.Success) {
			return new(match.Groups[1].Value, int.Parse(match.Groups[2].Value));
		}
		return null!;
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
		bool testing = GetArgument(args, 1, false);
		if (testing is false) { return "** Solution not written yet **"; }
		// int arg1 = GetArgument(args, 1, 25);
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
