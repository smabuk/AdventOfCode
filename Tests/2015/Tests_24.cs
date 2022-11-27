namespace AdventOfCode.Tests._2015;

public class Tests_24_It_Hangs_in_the_Balance {
	[Theory]
	[InlineData("""
		1
		2
		3
		4
		5
		7
		8
		9
		10
		11
		"""
		, 99)]
	[InlineData("""
		1
		3
		5
		11
		13
		17
		19
		23
		29
		31
		41
		43
		47
		53
		59
		61
		67
		71
		73
		79
		83
		89
		97
		101
		103
		107
		109
		113
		"""
		, 11266889531)]
	public void Part1(string input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2015, 24, 1, input), out long actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		1
		2
		3
		4
		5
		7
		8
		9
		10
		11
		"""
		, 44)]
	[InlineData("""
		1
		3
		5
		11
		13
		17
		19
		23
		29
		31
		41
		43
		47
		53
		59
		61
		67
		71
		73
		79
		83
		89
		97
		101
		103
		107
		109
		113
		"""
		, 77387711)]
	public void Part2(string input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2015, 24, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}
}
