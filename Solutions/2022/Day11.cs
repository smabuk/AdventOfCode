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

	private static long _modulo;		// by using this extra modulo we can keep the number smaller (also see Lowest Common Multiplier)

	private static long Solution(string[] input,int noOfRounds) {
		bool isPart1 = noOfRounds == 20;

		List<Monkey> monkeys = Monkey
			.Parse(input)
			.ToList();

		_modulo = monkeys
			.Select(m => m.TestDivisor)
			.Aggregate(1, (total, next) => total * next);

		for (int i = 1; i <= noOfRounds; i++) {
			foreach (Monkey monkey in monkeys) {
				foreach (var throwing in monkey.Inspect(isPart1)) {
					monkeys[throwing.MonkeyToThrowTo].Catch(throwing.Item);
				}
			}
		}

		return
			monkeys
			.Select(m => m.InspectionCount)
			.OrderByDescending(i => i)
			.Take(2)
			.Aggregate(1L, (total, next) => total * next);
	}

	private record Monkey(int Name, Operation Operation, int TestDivisor, int TrueMonkey, int FalseMonkey) {
		
		public long InspectionCount { get; set; } = 0;

		private List<Item> _items = [];
		public required IEnumerable<Item> Items { get => _items; init => _items = value.ToList(); }

		public IEnumerable<(Item Item, int MonkeyToThrowTo)> Inspect(bool isPart1 = true) {
			foreach (Item item in Items) {
				InspectionCount++;
				long worryLevel = UpdateWorryLevel(item, isPart1);
				int monkeyToThrowTo = (worryLevel % TestDivisor == 0) ? TrueMonkey : FalseMonkey;
				yield return (item with { WorryLevel = worryLevel }, monkeyToThrowTo);
			}
			_items.Clear();

			long UpdateWorryLevel(Item item, bool isPart1) {
				long worryLevel = Operation.Calculate(item.WorryLevel, Operation);
				worryLevel = isPart1 ? worryLevel / 3 : worryLevel % _modulo;
				return worryLevel;
			}
		}

		public void Catch(Item item) => _items.Add(item);

		static public IEnumerable<Monkey> Parse(string[] input) {
			const int LinesPerMonkey = 7;

			int noOfMonkeys = (input.Length + 1) / LinesPerMonkey;

			for (int i = 0; i < noOfMonkeys; i++) {
				int startLine = i * LinesPerMonkey;

				int name        = input[startLine][7..^1].As<int>();
				int divisibleBy = input[startLine + 3][21..].As<int>();
				int trueName    = input[startLine + 4][29..].As<int>();
				int falseName   = input[startLine + 5][29..].As<int>();

				List<Item> items = new(input[startLine + 1][17..]
					.Split(", ")
					.Select(x => new Item(x.As<int>())));
				Operation operation = new(
					Op:    input[startLine + 2][23] == '+' ? Operation.OpType.Add : Operation.OpType.Multiply,
					Value: input[startLine + 2][25..].As<int>());

				yield return new(name, operation, divisibleBy, trueName, falseName) { Items = items };
			}
		}
	}
	private record struct Item(long WorryLevel);
	
	private record struct Operation(Operation.OpType Op, int Value) {
		private const int SELF = 0;

		public static long Calculate(long old, Operation operation) {
			return operation.Op switch {
				Operation.OpType.Multiply when operation.Value is Operation.SELF 
				                          => old * old,
				Operation.OpType.Multiply => old * operation.Value,
				Operation.OpType.Add when operation.Value is Operation.SELF
										  => old + old,
				Operation.OpType.Add      => old + operation.Value,
				_ => throw new NotImplementedException(),
			};
		}

		public enum OpType { Add, Multiply }
	};
}
