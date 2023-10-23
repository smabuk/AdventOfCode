namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 21: Monkey Math
/// https://adventofcode.com/2022/day/21
/// </summary>
[Description("Monkey Math")]
public sealed partial class Day21 {

	private static readonly string ROOT  = "root";
	private static readonly string HUMAN = "humn";

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static long Solution1(string[] input) {
		Dictionary<string, Monkey> monkeys = input.Select(Monkey.Parse).ToDictionary(m => m.Name, m => m);

		Monkey rootMonkey = monkeys[ROOT];

		return CalculateValue(rootMonkey, monkeys);
	}

	private static long Solution2(string[] input) {
		Dictionary<string, Monkey> monkeys = input.Select(Monkey.Parse).ToDictionary(m => m.Name, m => m);

		Monkey me = monkeys[HUMAN];
		_ = monkeys.Remove(HUMAN);

		MATH_Monkey rootMonkey = (MATH_Monkey)monkeys[ROOT];

		Monkey monkey1 = monkeys[rootMonkey.Monkey1.Name];
		Monkey monkey2 = monkeys[rootMonkey.Monkey2.Name];

		long value;
		try {
			value = CalculateValue(monkey1, monkeys);
		} catch (Exception) {
			value = CalculateValue(monkey2, monkeys);
		}

		return UnCalculateValue(value, me, monkeys);
	}

	private static long CalculateValue(Monkey monkey, Dictionary<string, Monkey> monkeys) {
		long result = 0;
		long v1;
		long v2;

		if (monkey is NUMBER_Monkey numberMonkey) {
			result = numberMonkey.Value;
		}

		if (monkey is MATH_Monkey mathMonkey) {
			v1 = CalculateValue(monkeys[mathMonkey.Monkey1.Name], monkeys);
			v2 = CalculateValue(monkeys[mathMonkey.Monkey2.Name], monkeys);
			result = mathMonkey.Operation switch {
				Operation.add      => v1 + v2,
				Operation.subtract => v1 - v2,
				Operation.multiply => v1 * v2,
				Operation.divide   => v1 / v2,
				_                  => throw new NotImplementedException(),
			};
		}
		
		return result;

	}

	private static long UnCalculateValue(long v1, Monkey monkey, Dictionary<string, Monkey> monkeys) {
		long result = 0;
		long v2 = 0;

		MATH_Monkey parent = (MATH_Monkey)monkeys.Values
			.Single(m => m is MATH_Monkey mm && (monkey.Name == mm.Monkey1.Name || monkey.Name == mm.Monkey2.Name));

		if (parent.Name == ROOT) {
			return v1;
		}

		if (parent.Monkey1.Name == monkey.Name) {
			if (monkeys[parent.Monkey2.Name] is NUMBER_Monkey m2Number) {
				v2 = m2Number.Value;
			}

			if (monkeys[parent.Monkey2.Name] is MATH_Monkey m2Math) {
				v2 = CalculateValue(m2Math, monkeys);
			}

			result = parent.Operation switch {
				Operation.subtract => UnCalculateValue(v1, parent, monkeys) + v2,
				Operation.add      => UnCalculateValue(v1, parent, monkeys) - v2,
				Operation.divide   => UnCalculateValue(v1, parent, monkeys) * v2,
				Operation.multiply => UnCalculateValue(v1, parent, monkeys) / v2,
				_                  => throw new NotImplementedException(),
			};

		}

		if (parent.Monkey2.Name == monkey.Name) {
			if (monkeys[parent.Monkey1.Name] is NUMBER_Monkey m1Number) {
				v2 = m1Number.Value;
			}

			if (monkeys[parent.Monkey1.Name] is MATH_Monkey m1Math) {
				v2 = CalculateValue(m1Math, monkeys);
			}

			result = parent.Operation switch {
				Operation.subtract => v2 - UnCalculateValue(v1, parent, monkeys),
				Operation.add      => UnCalculateValue(v1, parent, monkeys) - v2,
				Operation.divide   => UnCalculateValue(v1, parent, monkeys) * v2,
				Operation.multiply => UnCalculateValue(v1, parent, monkeys) / v2,
				_                  => throw new NotImplementedException(),
			};
		}

		return result;
	}


	record Monkey(string Name) {
		public static Monkey Parse(string s) {
			string name = s[0..4];
			if (Char.IsNumber(s[6])) {
				return new NUMBER_Monkey(Name: name, Value: long.Parse(s[6..]));
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
						_   => throw new NotImplementedException(),
					}
				);
			}
		}

		public static Monkey Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Monkey result) => throw new NotImplementedException();
	};
	record NUMBER_Monkey(string Name, long Value) : Monkey(Name);
	record MATH_Monkey(string Name, Operation Operation, Monkey Monkey1, Monkey Monkey2) : Monkey(Name);

	public enum Operation {
		add,
		subtract,
		multiply,
		divide,
		equality,
	}
}
