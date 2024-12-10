namespace AdventOfCode.Tests.Year2024;

public class Tests_10_Hoof_It
{
	const int DAY = 10;

	[Theory]
	[InlineData("""
		...0...
		...1...
		...2...
		6543456
		7.....7
		8.....8
		9.....9
		""", 2)]
	[InlineData("""
		..90..9
		...1.98
		...2..7
		6543456
		765.987
		876....
		987....
		""", 4)]
	[InlineData("""
		10..9..
		2...8..
		3...7..
		4567654
		...8..3
		...9..2
		.....01
		""", 3)]
	[InlineData("""
		89010123
		78121874
		87430965
		96549874
		45678903
		32019012
		01329801
		10456732
		""", 36)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("""
		.....0.
		..4321.
		..5..2.
		..6543.
		..7..4.
		..8765.
		..9....
		""", 3)]
	[InlineData("""
		..90..9
		...1.98
		...2..7
		6543456
		765.987
		876....
		987....
		""", 13)]
	[InlineData("""
		012345
		123456
		234567
		345678
		4.6789
		56789.
		""", 227)]
	[InlineData("""
		89010123
		78121874
		87430965
		96549874
		45678903
		32019012
		01329801
		10456732
		""", 81)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
