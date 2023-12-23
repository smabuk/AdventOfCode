namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 19: Aplenty
/// https://adventofcode.com/2023/day/19
/// </summary>
[Description("Aplenty")]
public sealed partial class Day19 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static readonly List<Workflow> _workflows = [];
	private static readonly List<Part>     _parts     = [];

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
		Dictionary<string, Workflow> workflows = input
			.TakeWhile(IsNotABlankLine)
			.As<Workflow>()
			.ToDictionary(w => w.Name);
		List<int> OneTo4000 = Enumerable.Range(1, 4000).ToList();
		Dictionary<string, Possibles> categoryRatings = [];
		categoryRatings["x"] = new (1, 4000);
		categoryRatings["m"] = new (1, 4000);
		categoryRatings["a"] = new (1, 4000);
		categoryRatings["s"] = new (1, 4000);

		


		categoryRatings  =  Workflow.CalculateCombinations("in", categoryRatings, workflows);
		return (categoryRatings["x"].End + 1 - categoryRatings["x"].Start)
			 * (categoryRatings["m"].End + 1 - categoryRatings["m"].Start)
			 * (categoryRatings["a"].End + 1 - categoryRatings["a"].Start)
			 * (categoryRatings["s"].End + 1 - categoryRatings["s"].Start);

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
		public static Dictionary<string, Possibles> CalculateCombinations(string workflowName, Dictionary<string, Possibles> categoryRatings, Dictionary<string, Workflow> workflows)
		{
			Dictionary<string, Possibles> ratings = categoryRatings.ToDictionary();
			Dictionary<string, Possibles> trueRatings = categoryRatings.ToDictionary();
			Dictionary<string, Possibles> falseRatings = categoryRatings.ToDictionary();
			Workflow current = workflows[workflowName];

			//long combinations = 0;
#pragma warning disable CS0219 // Variable is assigned but its value is never used
			long trueCombinations = 0;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
							  //long falseCombinations = 0;

			foreach (Rule rule in current.Rules) {


				if (rule.Operator is Operator.GO && rule.Next is "A" or "R") {
					if (rule.Next == "R") {
						return [];
					}
					return categoryRatings;
					//return (ratings["x"].End + 1 - ratings["x"].Start)
					//	 * (ratings["m"].End + 1 - ratings["m"].Start)
					//	 * (ratings["a"].End + 1 - ratings["a"].Start)
					//	 * (ratings["s"].End + 1 - ratings["s"].Start);
				}

				if (rule.Operator is Operator.GO) {
					return Workflow.CalculateCombinations(rule.Next, categoryRatings, workflows);
				}

				string category = rule.Category switch
				{
					Category.X => "x",
					Category.M => "m",
					Category.A => "a",
					Category.S => "s",
					_ => throw new NotImplementedException(),
				};

				Possibles trueSplit  = ratings[category];
				Possibles falseSplit = ratings[category];

				if (rule.Operator is Operator.GT) {
					int start = 0;
					int end   = 0;
					if (rule.Value < trueSplit.Start) {
						start = trueSplit.Start;
						end = trueSplit.End;
					} else if (rule.Value >= trueSplit.End) {
					} else {
						start = rule.Value + 1;
						end = trueSplit.End;
					}
					trueSplit = new(start, end);

					if (rule.Value < falseSplit.Start) {
					} else if (rule.Value >= falseSplit.End) {
						start = falseSplit.Start;
						end   = falseSplit.End;
					} else {
						start = falseSplit.Start;
						end = rule.Value;
					}
					falseSplit = new(start, end);
				}

				if (rule.Operator is Operator.LT) {
					int start = 0;
					int end = 0;
					if (rule.Value > trueSplit.End) {
						start = trueSplit.Start;
						end = trueSplit.End;
					} else if (rule.Value <= trueSplit.Start) {
					} else {
						start = trueSplit.Start;
						end = rule.Value - 1;
					}
					trueSplit = new(start, end);

					if (rule.Value > falseSplit.End) {
					} else if (rule.Value <= falseSplit.Start) {
						start = falseSplit.Start;
						end = falseSplit.End;
					} else {
						start = rule.Value;
						end = falseSplit.End;
					}
					falseSplit = new(start, end);
				}

				trueRatings[category] = trueSplit;
				falseRatings[category] = falseSplit;

				if (rule.Next is "R") {
					trueCombinations = 0;
				} else if (rule.Next is "A") {
					return categoryRatings;

					//trueCombinations += (trueRatings["x"].End + 1 - trueRatings["x"].Start)
					//				  * (trueRatings["m"].End + 1 - trueRatings["m"].Start)
					//				  * (trueRatings["a"].End + 1 - trueRatings["a"].Start)
					//				  * (trueRatings["s"].End + 1 - trueRatings["s"].Start);
				} else {
					if (trueSplit.Start != 0) {
						trueRatings = Workflow.CalculateCombinations(rule.Next, trueRatings, workflows);
					}
					if (falseSplit.Start != 0) {
						falseRatings = Workflow.CalculateCombinations(rule.Next, falseRatings, workflows);
					}
				}
			}


			throw new ApplicationException("Whoops");
		}

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

			throw new ApplicationException("Whoops");
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
			if (s.DoesNotContain(':')) {
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

	private record Possibles(int Start, int End);

	public enum Category { None, X, M, A, S }
	public enum Operator { None, GT, LT, GO }
}
