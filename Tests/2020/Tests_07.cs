using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2020 {
	public class Tests_07_Handy_Haversacks {
		[Theory]
		[InlineData(new string[] {
			"light red bags contain 1 bright white bag, 2 muted yellow bags.",
			"dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
			"bright white bags contain 1 shiny gold bag.",
			"muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
			"shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
			"dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
			"vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
			"faded blue bags contain no other bags.",
			"dotted black bags contain no other bags."
		}, 4)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2020, 7, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
			"light red bags contain 1 bright white bag, 2 muted yellow bags.",
			"dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
			"bright white bags contain 1 shiny gold bag.",
			"muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
			"shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
			"dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
			"vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
			"faded blue bags contain no other bags.",
			"dotted black bags contain no other bags."
		}, 32)]
		[InlineData(new string[] {
			"shiny gold bags contain 2 dark red bags.",
			"dark red bags contain 2 dark orange bags.",
			"dark orange bags contain 2 dark yellow bags.",
			"dark yellow bags contain 2 dark green bags.",
			"dark green bags contain 2 dark blue bags.",
			"dark blue bags contain 2 dark violet bags.",
			"dark violet bags contain no other bags."
		}, 126)]
		public void Part2(string[] input, long expected) {
			_ = long.TryParse(SolutionRouter.SolveProblem(2020, 7, 2, input), out long actual);
			Assert.Equal(expected, actual);
		}

	}
}
