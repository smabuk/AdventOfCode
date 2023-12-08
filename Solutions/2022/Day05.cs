namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 05: Supply Stacks
/// https://adventofcode.com/2022/day/5
/// </summary>
[Description("Supply Stacks")]
public sealed partial class Day05 {

	[Init]
	public static    void Init(string input, params object[]? _) => LoadInstructionsAndStacks(input);
	public static string Part1(string input, params object[]? _) => Solution1().ToString();
	public static string Part2(string input, params object[]? _) => Solution2().ToString();

	private static IEnumerable<Instruction> _instructions = Array.Empty<Instruction>();
	private static Stack<char>[] _initialStacks = Array.Empty<Stack<char>>();

	private static void LoadInstructionsAndStacks(string input) {
		string[] inputBlocks = input.Split(Environment.NewLine + Environment.NewLine);

		_initialStacks = ParseStacks(inputBlocks[0].Split(Environment.NewLine));
		_instructions = inputBlocks[1].Split(Environment.NewLine).Select(i => Instruction.Parse(i));
	}

	private static string Solution1() {
		Stack<char>[] stacks = _initialStacks.Select(s => new Stack<char>(s.Reverse())).ToArray();

		foreach (Instruction instruction in _instructions) {
			for (int i = 0; i < instruction.Quantity; i++) {
				stacks[instruction.To - 1].Push(stacks[instruction.From - 1].Pop());
			}
		}

		return GetMessage(stacks);
	}

	private static string Solution2() {
		Stack<char>[] stacks = _initialStacks.Select(s => new Stack<char>(s.Reverse())).ToArray();

		foreach (Instruction instruction in _instructions) {
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

	private record struct Instruction(int Quantity, int From, int To) {
		/// <summary>
		/// 
		/// Input Pattern:
		/// move 1 from 2 to 1
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static Instruction Parse(string input) =>
			input.Split(new char[] { ' ' }) switch {
				[_, var n1, _, var n3, _, var n5, ..] => new(n1.As<int>(), n3.As<int>(), n5.As<int>()),
				_ => throw new NotImplementedException()
			};
	}
	/// <summary>
	/// 
	/// Input Pattern:
	///      [D]    
	///  [N] [C]
	///  [Z] [M] [P]
	///   1   2   3 
	/// 
	/// </summary>
	/// <param name="input"></param>
	/// <param name="noOfStacks"></param>
	/// <returns></returns>
	private static Stack<char>[] ParseStacks(string[] input) {
		int noOfStacks = int.Parse(input[^1].Trim().Split(' ')[^1]);
		Stack<char>[] stacks = new Stack<char>[noOfStacks];
		for (int s = 0; s < noOfStacks; s++) {
			stacks[s] = new();
		}

		for (int i = input.Length - 2; i >= 0; i--) {
			for (int s = 0; s < noOfStacks; s++) {
				int offset = (s * 4) + 1;
				char crate = input[i][offset];
				if (crate != ' ') {
					stacks[s].Push(crate);
				}
			}
		}

		return stacks;
	}

}
