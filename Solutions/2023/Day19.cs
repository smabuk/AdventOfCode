namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 19: Aplenty
/// https://adventofcode.com/2023/day/19
/// </summary>
[Description("Aplenty")]
public sealed partial class Day19 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static List<Workflow> _workflows = [];
	private static List<Part>     _parts     = [];

	private static int Solution1(string[] input) {
		Dictionary<string, Workflow> workflows = input
			.TakeWhile(IsNotABlankLine)
			.As<Workflow>()
			.ToDictionary(w => w.Name);

		return input[(workflows.Count + 1)..]
			.As<Part>()
			.Select(p => Workflow.Process("in", p, workflows))
			.Where(part => part.Accepted)
			.Sum(part => part.Rating);
	}

	private static long Solution2(string[] input) {
		 List<Workflow> workflows = [..input.TakeWhile(IsNotABlankLine).As<Workflow>()];

		return 0L;
	}

	private static bool IsNotABlankLine(string s) => !string.IsNullOrWhiteSpace(s);

	private record struct Part(int X, int M, int A, int S) : IParsable<Part>
	{
		public bool Accepted { get; set; }
		public bool Rejected { get; set; }
		public int  Rating   { get; set; } = X + M + A + S;

		public static Part Parse(string s, IFormatProvider? provider)
		{
			char[] splitBy = [',', '=','{','}'];
			string[] tokens = s.TrimmedSplit(splitBy);
			return new(tokens[1].As<int>(), tokens[3].As<int>(), tokens[5].As<int>(), tokens[7].As<int>());
		}

		public static Part Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Part result)
			=> ISimpleParsable<Part>.TryParse(s, provider, out result);
	}

	private sealed record Workflow(string Name, List<Rule> Rules) : IParsable<Workflow> {


		public static Part Process(string workflowName, Part part, Dictionary<string, Workflow> workflows)
		{
			bool accepted = false;
			bool rejected = false;
			
			Workflow current = workflows[workflowName];

			while (accepted is false && rejected is false) {
				foreach (Rule rule in current.Rules) {
					if (rule.Operator is Operator.GO && rule.Next is "A" or "R") {
						return part with { Accepted = rule.Next == "A", Rejected = rule.Next == "R"};
					}

					if (rule.Operator is Operator.GO) {
						return Workflow.Process(rule.Next, part, workflows);
					}

					int value = rule.Category switch
					{
						Category.X => part.X,
						Category.M => part.M,
						Category.A => part.A,
						Category.S => part.S,
						Category.None => throw new NotImplementedException(),
						_ => throw new NotImplementedException(),
					};

					bool outcome = rule.Operator switch
					{
						Operator.GT => value > rule.Value,
						Operator.LT => value < rule.Value,
						Operator.GO => true,
						Operator.None => throw new NotImplementedException(),
						_ => throw new NotImplementedException(),
					};

					if (outcome && rule.Next is "A" or "R") {
						return part with { Accepted = rule.Next == "A", Rejected = rule.Next == "R" };
					} else if (outcome) {
						return Workflow.Process(rule.Next, part, workflows);
					}
				}
			}

			return part with { Accepted = accepted, Rejected = rejected};
		}

		public static Workflow Parse(string s, IFormatProvider? provider)
		{
			char[] splitBy = [',', '{', '}'];
			string[] tokens = s.TrimmedSplit(splitBy);
			return new(tokens[0], [..tokens[1..].As<Rule>()]);
		}

		public static Workflow Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Workflow result)
			=> ISimpleParsable<Workflow>.TryParse(s, provider, out result);
	}



	private sealed record Rule(string Raw, Category Category, Operator Operator, int Value, string Next) : IParsable<Rule> {

		public static Rule Parse(string s, IFormatProvider? provider)
		{
			if (!s.Contains(':')) {
				return new(s, Category.None, Operator.GO, 0, s);
			}

			char[] splitBy = ['<', '>', ':'];
			string[] tokens = s.TrimmedSplit(splitBy);

			Operator op = s.Contains('<') ? Operator.LT : Operator.GT;
			Category category = Enum.Parse<Category>(tokens[0].ToUpperInvariant());

			return new(s, category, op, tokens[1].As<int>(), tokens[2]);
		}

		public static Rule Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Rule result)
			=> ISimpleParsable<Rule>.TryParse(s, provider, out result);
	}

	public enum Category { None, X, M, A, S }
	public enum Operator { None, GT, LT, GO }


	[GeneratedRegex("""(?<opts>opt1|opt2|opt3) (?<number>[\+\-]?\d+)""")]
	private static partial Regex InputRegEx();
}
