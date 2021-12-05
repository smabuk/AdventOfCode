namespace AdventOfCode.Solutions.Year2020;

/// <summary>
/// Day 18: Operation Order
/// https://adventofcode.com/2020/day/18
/// </summary>
[Description("Operation Order")]
public class Day18 {

	public const char OPEN_PARENTHESIS = '(';
	public const char CLOSE_PARENTHESIS = ')';
	public const char PLUS = '+';
	public const char MULTIPLY = '*';

	record Expression;
	record Add(Expression LHS, Expression RHS) : Expression;
	record Multiply(Expression LHS, Expression RHS) : Expression;
	record Number(long Value) : Expression;

	private static long Solution1(string[] input) {
		List<string> inputs = input.ToList();
		long sum = 0;

		foreach (string line in inputs) {
			Expression expression = ParseLineIntoExpression(line);
			sum += CalculateExpression(expression);
		}

		return sum;
	}

	private static long Solution2(string[] input) {
		List<string> inputs = input.ToList();
		long sum = 0;

		foreach (string line in inputs) {
			Expression expression = ParseLineIntoExpression2(line);
			sum += CalculateExpression(expression);
		}

		return sum;
	}

	static Expression ParseLineIntoExpression(string line) {
		Expression LookForNumber() {
			if (line.StartsWith(OPEN_PARENTHESIS)) {
				line = line[1..];
				var res = LookForOperand();
				if (line.StartsWith(CLOSE_PARENTHESIS)) {
					line = line[1..];
				}
				return res;
			} else {
				string numberStr = Regex.Match(line, @"^\d+").Value;
				line = line[numberStr.Length..];
				return new Number(long.Parse(numberStr));
			}
		}

		Expression LookForOperand() {
			Expression result = LookForNumber();
			bool isOperand = true;
			while (isOperand) {
				if (line.StartsWith(MULTIPLY) || line.StartsWith(PLUS)) {
					char op = line[0];
					line = line[1..];
					result = op switch {
						PLUS => new Add(result, LookForNumber()),
						MULTIPLY => new Multiply(result, LookForNumber()),
						_ => throw new ArgumentException(),
					};
				} else {
					isOperand = false;
				}
			}
			return result;
		}

		line = line.Replace(" ", "");
		return LookForOperand();
	}

	static Expression ParseLineIntoExpression2(string line) {
		Expression LookForNumber() {
			if (line.StartsWith(OPEN_PARENTHESIS)) {
				line = line[1..];
				var res = LookForMultiplication();
				if (line.StartsWith(CLOSE_PARENTHESIS)) {
					line = line[1..];
				}
				return res;
			} else {
				string numberStr = Regex.Match(line, @"^\d+").Value;
				line = line[numberStr.Length..];
				return new Number(long.Parse(numberStr));
			}
		}

		Expression LookForAddition() {
			Expression result = LookForNumber();
			bool isOperand = true;
			while (isOperand) {
				if (line.StartsWith(PLUS)) {
					line = line[1..];
					result = new Add(result, LookForNumber());
				} else {
					isOperand = false;
				}
			}
			return result;
		}

		Expression LookForMultiplication() {
			Expression result = LookForAddition();
			bool isOperand = true;
			while (isOperand) {
				if (line.StartsWith(MULTIPLY)) {
					line = line[1..];
					result = new Multiply(result, LookForAddition());
				} else {
					isOperand = false;
				}
			}
			return result;
		}

		line = line.Replace(" ", "");
		return LookForMultiplication();
	}

	private static long CalculateExpression(Expression expression) {
		long value;

		value = expression switch {
			Add x => CalculateExpression(x.LHS) + CalculateExpression(x.RHS),
			Multiply x => CalculateExpression(x.LHS) * CalculateExpression(x.RHS),
			Number x => x.Value,
			_ => throw new NotImplementedException()
		};
		return value;

	}


	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
