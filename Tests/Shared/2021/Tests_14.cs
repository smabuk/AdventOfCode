namespace AdventOfCode.Tests.Year2021;

public class Tests_14_Extended_Polymerization {
	[Theory]
	[InlineData(new string[] {
		"NNCB",
		"",
		"CH -> B",
		"HH -> N",
		"CB -> H",
		"NH -> C",
		"HB -> C",
		"HC -> B",
		"HN -> C",
		"NN -> C",
		"BH -> H",
		"NC -> B",
		"NB -> B",
		"BN -> B",
		"BB -> N",
		"BC -> B",
		"CC -> N",
		"CN -> C",  
	}, 10, 1588)]
	public void Part1(string[] input, int steps, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 14, 1, input, steps), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
		"NNCB",
		"",
		"CH -> B",
		"HH -> N",
		"CB -> H",
		"NH -> C",
		"HB -> C",
		"HC -> B",
		"HN -> C",
		"NN -> C",
		"BH -> H",
		"NC -> B",
		"NB -> B",
		"BN -> B",
		"BB -> N",
		"BC -> B",
		"CC -> N",
		"CN -> C",
	}, 40, 2188189693529)]
	public void Part2(string[] input,int steps, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2021, 14, 2, input, steps), out long actual);
		Assert.Equal(expected, actual);
	}
}
