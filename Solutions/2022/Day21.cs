namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 21: Monkey Math
/// https://adventofcode.com/2022/day/21
/// </summary>
[Description("Monkey Math")]
public sealed partial class Day21 {

	//[Init]
	//public static    void Init(string[] input, params object[]? _) => LoadMonkeys(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();


	private static long Solution1(string[] input) {
		List<Monkey> monkeys = input.Select(Monkey.Parse).ToList();

		Monkey rootMonkey = monkeys.Where(m => m.Name == "root").Single();
		return CalculateValue(rootMonkey, monkeys);
		;
	}
	private static string Solution2(string[] input) {
		List<Monkey> Monkeys = input.Select(Monkey.Parse).ToList();
		return "** Solution not written yet **";
	}


	private static long CalculateValue(Monkey monkey, List<Monkey> monkeys) {

		if (monkey is NUMBER_Monkey numberMonkey) {
			return numberMonkey.Value;
		}

		if (monkey is MATH_Monkey mathMonkey) {
			return mathMonkey.Operation switch {
				Operation.add => CalculateValue(Find(mathMonkey.Monkey1), monkeys) + CalculateValue(Find(mathMonkey.Monkey2), monkeys),
				Operation.subtract => CalculateValue(Find(mathMonkey.Monkey1), monkeys) - CalculateValue(Find(mathMonkey.Monkey2), monkeys),
				Operation.multiply => CalculateValue(Find(mathMonkey.Monkey1), monkeys) * CalculateValue(Find(mathMonkey.Monkey2), monkeys),
				Operation.divide => CalculateValue(Find(mathMonkey.Monkey1), monkeys) / CalculateValue(Find(mathMonkey.Monkey2), monkeys),
				_ => throw new NotImplementedException(),
			};
		}

		throw new NotImplementedException();

		Monkey Find(Monkey monkey) => monkeys.Where(m => m.Name == monkey.Name).Single();
	}


	record Monkey(string Name) {
		public static Monkey Parse(string s) {
			string name = s[0..4];
			if (Char.IsNumber(s[6])) {
				return new NUMBER_Monkey(Name: name, Value: s[6..].AsInt());
			} else {
				return new MATH_Monkey(
					Name: name,
					Monkey1: new(s[6..10]),
					Monkey2: new(s[13..17]),
					Operation: s[11] switch {
						'+' => Operation.add,
						'-' => Operation.subtract,
						'*' => Operation.multiply,
						'/' => Operation.divide,
						_ => throw new NotImplementedException(),
					}
				);
			}
		}

		public static Monkey Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Monkey result) => throw new NotImplementedException();
	};
	record NUMBER_Monkey(string Name, long Value) : Monkey(Name);
	record MATH_Monkey  (string Name, Operation Operation, Monkey Monkey1, Monkey Monkey2) : Monkey(Name);

	public enum Operation {
		add,
		subtract,
		multiply,
		divide,
	}
}
