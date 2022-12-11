namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 11: Monkey in the Middle
/// https://adventofcode.com/2022/day/11
/// </summary>
[Description("Monkey in the Middle")]
public sealed partial class Day11 {

	// [Init] public static void Init(string[] input, params object[]? _) => Load(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private record Monkey(int Name, List<Item> Items, Operation Operation, int DivisibleBy, int TrueMonkeyName, int FalseMonkeyName) {
		public int InspectionCount { get; set; } = 0;

		public IEnumerable<(Item, int)>  Inspect() {
			foreach (Item item in Items) {
				InspectionCount++;
				long worryLevel = Operation.Op switch {
					Operation.Operand.Add => item.WorryLevel + Operation.Value,
					Operation.Operand.Multiply when Operation.Value is Operation.SELF => item.WorryLevel * item.WorryLevel,
					Operation.Operand.Multiply => item.WorryLevel * Operation.Value,
					_ => throw new NotImplementedException(),
				};
				worryLevel = worryLevel / 3;
				if (worryLevel % DivisibleBy == 0) {
					yield return (item with { WorryLevel = worryLevel}, TrueMonkeyName);
				} else {
					yield return (item with { WorryLevel = worryLevel}, FalseMonkeyName);
				}
			}
			Items.Clear();
		}

		public void ReceiveItem(Item item) => Items.Add(item);

		static public IEnumerable<Monkey> Parse(string[] input) {
			const int LinesPerMonkey = 7;
			int noOfMonkeys = (input.Length + 1) / LinesPerMonkey;
			for (int i = 0; i < noOfMonkeys; i++) {
				int startLine = i * LinesPerMonkey;
				int name = input[startLine][7..^1].AsInt();
				List<Item> items = input[startLine + 1][17..]
					.Split(", ")
					.Select(x => new Item(long.Parse(x)))
					.ToList();
				Operation operation = 
					new(input[startLine + 2][23] == '+' ? Operation.Operand.Add : Operation.Operand.Multiply,
						input[startLine + 2][25..].AsInt());
				int test = input[startLine + 3][21..].AsInt();
				int trueName = input[startLine + 4][29..].AsInt();
				int falseName = input[startLine + 5][29..].AsInt();
				yield return new(name, items, operation, test, trueName, falseName);
			}
			yield break;
		}
	};
	private record struct Item(long WorryLevel);
	private record struct Operation(Operation.Operand Op, int Value) {
		public const int SELF = 0;
		public enum Operand { Add, Multiply } 
	};

	private static long Solution1(string[] input, int noOfRounds = 20) {
		Dictionary<int, Monkey> monkeys = 
			Monkey.Parse(input)
			.ToDictionary(m => m.Name);

		for (int i = 0; i < noOfRounds; i++) {
			foreach (Monkey monkey in monkeys.Values) {
				foreach ((Item item, int toMonkey) in monkey.Inspect()) {
					monkeys[toMonkey].ReceiveItem(item);
				}
			}
		}
		long monkeyBusiness = 
			monkeys.Values
			.Select(m => m.InspectionCount)
			.OrderByDescending(m => m)
			.Take(2)
			.Aggregate((total, next) => total * next);
		return monkeyBusiness;
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}
}
