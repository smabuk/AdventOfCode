namespace AdventOfCode.Tests.Year2023;

public class Tests_12_Hot_Springs {
	const int DAY = 12;

	[Theory]
	[InlineData("???.### 1,1,3",              1)]
	[InlineData(".??..??...?##. 1,1,3",       4)]
	[InlineData("?#?#?#?#?#?#?#? 1,3,1,6",    1)]
	[InlineData("????.#...#... 4,1,1",        1)]
	[InlineData("????.######..#####. 1,6,5",  4)]
	[InlineData("?###???????? 3,2,1"       , 10)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("???.### 1,1,3",                  1)]
	[InlineData(".??..??...?##. 1,1,3",       16384)]
	[InlineData("?#?#?#?#?#?#?#? 1,3,1,6",        1)]
	[InlineData("????.#...#... 4,1,1",           16)]
	[InlineData("????.######..#####. 1,6,5",   2500)]
	[InlineData("?###???????? 3,2,1"       , 506250)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
