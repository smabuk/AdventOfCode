namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 05: Supply Stacks
/// https://adventofcode.com/2022/day/5
/// </summary>
[Description("Supply Stacks")]
public sealed partial class Day05 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static string Solution1(string[] input) {
		(List<Stack<char>> stacks, List<Instruction> instructions) = ParseInput(input);

		foreach (Instruction instruction in instructions) {
			for (int i = 0; i < instruction.Quantity; i++) {
				stacks[instruction.To - 1].Push(stacks[instruction.From - 1].Pop());
			}
		}

		return GetMessage(stacks);
	}

	private static string Solution2(string[] input) {
		(List<Stack<char>> stacks, List<Instruction> instructions) = ParseInput(input);

		foreach (Instruction instruction in instructions) {
			char[] crates = new char[instruction.Quantity];
			for (int i = 0; i < instruction.Quantity; i++) {
				crates[i] = stacks[instruction.From - 1].Pop();
			}
			for (int i = instruction.Quantity - 1; i >= 0; i--) {
				stacks[instruction.To - 1].Push(crates[i]);
			}
		}

		return GetMessage(stacks);
	}



	private static string GetMessage(List<Stack<char>> stacks) {
		string message = "";
		for (int s = 0; s < stacks.Count; s++) {
			message += stacks[s].Peek();
		}

		return message;
	}

	record struct Instruction(int Quantity, int From, int To);

	private static (List<Stack<char>>, List<Instruction>) ParseInput(string[] input) {
		int noOfStacks = 0;
		int blankLineNo = 0;
		for (int i = 0; i < input.Length; i++) {
			if (String.IsNullOrWhiteSpace(input[i])) {
				noOfStacks = int.Parse(input[i - 1].Trim().Split(' ').Last());
				blankLineNo = i;
				break;
			}
		}

		List<Stack<char>> stacks = ParseStacks(input[..(blankLineNo - 1)], noOfStacks);
		List<Instruction> instructions = input[(blankLineNo + 1)..].Select(i => ParseInstruction(i)).ToList();

		return (stacks, instructions);
	}

	private static List<Stack<char>> ParseStacks(string[] input, int noOfStacks) {
		List<Stack<char>> stacks = new();
		for (int s = 0; s < noOfStacks; s++) {
			stacks.Add(new());
		}

		for (int i = input.Length - 1; i >= 0; i--) {
			for (int s = 0; s < noOfStacks; s++) {
				int offset = (s) * 4 + 1;
				char crate = input[i][offset];
				if (crate != ' ') {
					stacks[s].Push(crate);
				}
			}
		}

		return stacks;
	}

	private static Instruction ParseInstruction(string input) {
		Match match = InputRegEx().Match(input);
		if (match.Success) {
			return new(int.Parse(match.Groups["qty"].Value), int.Parse(match.Groups["from"].Value), int.Parse(match.Groups["to"].Value));
		}
		throw new ArgumentException($"Value: {input}", nameof(input));
	}

	[GeneratedRegex("""move (?<qty>\d+) from (?<from>\d+) to (?<to>\d+)""")]
	private static partial Regex InputRegEx();
}
