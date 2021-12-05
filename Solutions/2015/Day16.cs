namespace AdventOfCode.Solutions.Year2015;

/// <summary>
/// Day 16: Aunt Sue
/// https://adventofcode.com/2015/day/16
/// </summary>
[Description("Aunt Sue")]
public class Day16 {

	record Sue(int No, List<Fact> Facts);
	record Fact(string Name, int Value);

	private static int Solution1(string[] input) {
		List<Sue> sues = input.Select(i => ParseLine(i)).ToList();
		List<Fact> facts = new List<Fact> {
					new Fact("children", 3),
					new Fact("cats", 7),
					new Fact("samoyeds", 2),
					new Fact("pomeranians", 3),
					new Fact("akitas", 0),
					new Fact("vizslas", 0),
					new Fact("goldfish", 5),
					new Fact("trees", 3),
					new Fact("cars", 2),
					new Fact("perfumes", 1)
				};

		int SueNo = sues
			.Where(
			s => s.Facts.All(f => facts.Contains(f)))
			.Single().No;

		return SueNo;
	}

	private static int Solution2(string[] input) {
		List<Sue> sues = input.Select(i => ParseLine(i)).ToList();
		Dictionary<string, Fact> facts = new();
		facts.Add("children", new Fact("children", 3));
		facts.Add("cats", new Fact("cats", 7));
		facts.Add("samoyeds", new Fact("samoyeds", 2));
		facts.Add("pomeranians", new Fact("pomeranians", 3));
		facts.Add("akitas", new Fact("akitas", 0));
		facts.Add("vizslas", new Fact("vizslas", 0));
		facts.Add("goldfish", new Fact("goldfish", 5));
		facts.Add("trees", new Fact("trees", 3));
		facts.Add("cars", new Fact("cars", 2));
		facts.Add("perfumes", new Fact("perfumes", 1));


		int SueNo = 0;
		bool foundSue = false;
		foreach (Sue sue in sues) {
			int foundTrueFacts = 0;
			foundSue = false;
			foreach (Fact fact in sue.Facts) {
				bool isTrue = fact.Name switch {
					"children" => fact.Value == facts[fact.Name].Value,
					"cats" => fact.Value > facts[fact.Name].Value,
					"samoyeds" => fact.Value == facts[fact.Name].Value,
					"pomeranians" => fact.Value < facts[fact.Name].Value,
					"akitas" => fact.Value == facts[fact.Name].Value,
					"vizslas" => fact.Value == facts[fact.Name].Value,
					"goldfish" => fact.Value < facts[fact.Name].Value,
					"trees" => fact.Value > facts[fact.Name].Value,
					"cars" => fact.Value == facts[fact.Name].Value,
					"perfumes" => fact.Value == facts[fact.Name].Value,
					_ => false
				};
				if (isTrue == false) {
					foundSue = false;
					break;
				} else {
					foundTrueFacts++;
					if (foundTrueFacts == 3) {
						foundSue = true;
						SueNo = sue.No;
					}
				}
			}
			if (foundSue == true) {
				break;
			}
		}


		return SueNo;
	}

	private static Sue ParseLine(string input) {
		Match match = Regex.Match(input, @"Sue (?<No>\d+): (?<Fact1>\w+): (?<Value1>\d+), (?<Fact2>\w+): (?<Value2>\d+), (?<Fact3>\w+): (?<Value3>\d+)");
		if (match.Success) {
			List<Fact> facts = new List<Fact> {
					new Fact(match.Groups["Fact1"].Value, int.Parse(match.Groups["Value1"].Value)),
					new Fact(match.Groups["Fact2"].Value, int.Parse(match.Groups["Value2"].Value)),
					new Fact(match.Groups["Fact3"].Value, int.Parse(match.Groups["Value3"].Value))
				};
			return new(int.Parse(match.Groups["No"].Value), facts);
		}
		return null!;
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
