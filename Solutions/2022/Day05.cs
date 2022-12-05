namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 05: Supply Stacks
/// https://adventofcode.com/2022/day/5
/// </summary>
[Description("Supply Stacks")]
public sealed partial class Day05 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record Instruction(int Quantity, int From, int To);

	private static string Solution1(string[] input) {
		int noOfStacks = 0;
		int blankLineNo = 0;
		for (int i = 0; i < input.Length; i++) {
			if (String.IsNullOrWhiteSpace(input[i])) {
				noOfStacks = int.Parse(input[i - 1].Trim().Split(' ').Last());
				blankLineNo = i;
				break;
			}
		}

		List<Stack<char>> stacks = new();
		for (int i = 0; i < noOfStacks; i++) {
			stacks.Add(new());
		}

		for (int i = blankLineNo - 2; i >= 0; i--) {
			for (int s = 0; s < noOfStacks; s++) {
				int offset = (s) * 4 + 1;
				char container = input[i][offset];
				if (container != ' ') {
					stacks[s].Push(container);
				}
			}
		}

		List<Instruction> instructions = input[(blankLineNo+1)..].Select(i => ParseLine(i)).ToList();
		foreach (var instruction in instructions) {
			for (int i = 0; i < instruction.Quantity; i++) {
				char container = stacks[instruction.From - 1].Pop();
				stacks[instruction.To - 1].Push(container);
			}
		}

		string message = string.Empty;
		for (int s = 0; s < noOfStacks; s++) {
			message += stacks[s].Peek();
		}

		return message;
	}

	private static string Solution2(string[] input) {
		int noOfStacks = 0;
		int blankLineNo = 0;
		for (int i = 0; i < input.Length; i++) {
			if (String.IsNullOrWhiteSpace(input[i])) {
				noOfStacks = int.Parse(input[i - 1].Trim().Split(' ').Last());
				blankLineNo = i;
				break;
			}
		}

		List<Stack<char>> stacks = new();
		for (int i = 0; i < noOfStacks; i++) {
			stacks.Add(new());
		}

		for (int i = blankLineNo - 2; i >= 0; i--) {
			for (int s = 0; s < noOfStacks; s++) {
				int offset = (s) * 4 + 1;
				char container = input[i][offset];
				if (container != ' ') {
					stacks[s].Push(container);
				}
			}
		}

		List<Instruction> instructions = input[(blankLineNo + 1)..].Select(i => ParseLine(i)).ToList();
		foreach (var instruction in instructions) {
			string containers = "";
			for (int i = 0; i < instruction.Quantity; i++) {
				containers = stacks[instruction.From - 1].Pop() + containers;
			}
			for (int i = 0; i < instruction.Quantity; i++) {
				stacks[instruction.To - 1].Push(containers[i]);
			}
		}

		string message = string.Empty;
		for (int s = 0; s < noOfStacks; s++) {
			message += stacks[s].Peek();
		}

		return message;
	}

	private static Instruction ParseLine(string input) {
		Match match = InputRegEx().Match(input);
		if (match.Success) {
			return new(int.Parse(match.Groups["qty"].Value), int.Parse(match.Groups["from"].Value), int.Parse(match.Groups["to"].Value));
		}
		return null!;
	}

	[GeneratedRegex("""move (?<qty>\d+) from (?<from>\d+) to (?<to>\d+)""")]
	private static partial Regex InputRegEx();
}
