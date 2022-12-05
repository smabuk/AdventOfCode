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
		(Stack<char>[] stacks, IEnumerable<Instruction> instructions) = ParseInput(input);

		foreach (Instruction instruction in instructions) {
			for (int i = 0; i < instruction.Quantity; i++) {
				stacks[instruction.To - 1].Push(stacks[instruction.From - 1].Pop());
			}
		}

		return GetMessage(stacks);
	}

	private static string Solution2(string[] input) {
		(Stack<char>[] stacks, IEnumerable<Instruction> instructions) = ParseInput(input);

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


	private static string GetMessage(Stack<char>[] stacks) {
		string message = "";
		for (int s = 0; s < stacks.Length; s++) {
			message += stacks[s].Peek();
		}

		return message;
	}

	record struct Instruction(int Quantity, int From, int To);

	private static (Stack<char>[], IEnumerable<Instruction>) ParseInput(string[] input) {
		int noOfStacks = 0;
		int blankLineNo = 0;
		for (int i = 0; i < input.Length; i++) {
			if (String.IsNullOrWhiteSpace(input[i])) {
				noOfStacks = int.Parse(input[i - 1].Trim().Split(' ').Last());
				blankLineNo = i;
				break;
			}
		}

		Stack<char>[] stacks = ParseStacks(input[..(blankLineNo - 1)], noOfStacks);
		var instructions = input[(blankLineNo + 1)..].Select(i => ParseInstruction(i));

		return (stacks, instructions);
	}

	private static Stack<char>[] ParseStacks(string[] input, int noOfStacks) {
		Stack<char>[] stacks = new Stack<char>[noOfStacks];
		for (int s = 0; s < noOfStacks; s++) {
			stacks[s] = new();
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
		string[] tokens = input.Split(' ');
		return new(int.Parse(tokens[1]), int.Parse(tokens[3]), int.Parse(tokens[5]));
	}
}
