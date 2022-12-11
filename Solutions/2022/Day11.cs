namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 11: Monkey in the Middle
/// https://adventofcode.com/2022/day/11
/// </summary>
[Description("Monkey in the Middle")]
public sealed partial class Day11 {

	public static string Part1(string[] input, params object[]? _) => Solution(input, 20).ToString();
	public static string Part2(string[] input, params object[]? args) {
		int noOfRounds = GetArgument<int>(args, argumentNumber: 1, 10_000);
		return Solution(input, noOfRounds).ToString();
	}
	private static long _modulo;		// by using this extra modulo we can keep the number smaller

	private static long Solution(string[] input,int noOfRounds) {
		Dictionary<int, Monkey> monkeys = Monkey.Parse(input).ToDictionary(m => m.Name);
		_modulo = monkeys.Select(m => m.Value.DivisibleBy).Aggregate((total, next) => total * next);
		bool part1 = noOfRounds == 20;
		for (int i = 1; i <= noOfRounds; i++) {
			foreach (Monkey monkey in monkeys.Values) {
				foreach ((Item item, int monkeyToThrowTo) in monkey.Inspect(part1)) {
					monkeys[monkeyToThrowTo].ReceiveItem(item);
				}
			}
		}

		return
			monkeys.Values
			.Select(m => m.InspectionCount)
			.OrderByDescending(i => i)
			.Take(2)
			.Aggregate((total, next) => total * next);
	}

	private record Monkey(int Name, List<Item> Items, Operation Operation, int DivisibleBy, int TrueMonkeyName, int FalseMonkeyName) {
		public long InspectionCount { get; set; } = 0;

		public IEnumerable<(Item, int)> Inspect(bool Part1 = true) {
			foreach (Item item in Items) {
				InspectionCount++;
				long worryLevel = Operation.Op switch {
					Operation.Operand.Add => item.WorryLevel + Operation.Value,
					Operation.Operand.Multiply when Operation.Value is Operation.SELF => item.WorryLevel * item.WorryLevel,
					Operation.Operand.Multiply => item.WorryLevel * Operation.Value,
					_ => throw new NotImplementedException(),
				};
				worryLevel = (Part1 ? worryLevel / 3 : worryLevel) % _modulo;
				int monkeyToThrowTo = (worryLevel % DivisibleBy == 0) ? TrueMonkeyName : FalseMonkeyName;
				yield return (item with { WorryLevel = worryLevel }, monkeyToThrowTo);
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
				List<Item> items = new (input[startLine + 1][17..]
					.Split(", ")
					.Select(x => new Item(int.Parse(x))));
				Operation operation =
					new(input[startLine + 2][23] == '+' ? Operation.Operand.Add : Operation.Operand.Multiply,
						input[startLine + 2][25..].AsInt());
				int test = input[startLine + 3][21..].AsInt();
				int trueName = input[startLine + 4][29..].AsInt();
				int falseName = input[startLine + 5][29..].AsInt();
				yield return new(name, items, operation, test, trueName, falseName);
			}
		}
	};
	private record struct Item(long WorryLevel);
	private record struct Operation(Operation.Operand Op, int Value) {
		public const int SELF = 0;
		public enum Operand { Add, Multiply }
	};
}
