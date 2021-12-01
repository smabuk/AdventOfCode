namespace AdventOfCode.Tests.Year2019;

public class Tests_04_Secure_Container{
	[Theory]
	[InlineData(new string[] { "122345-122345" }, 1)]
	[InlineData(new string[] { "111111-111111" }, 1)]
	[InlineData(new string[] { "223450-223450" }, 0)]
	[InlineData(new string[] { "123789-123789" }, 0)]
	[InlineData(new string[] { "172930-683082" }, 1675)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2019, 4, 1, input, true), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { "112233-112233" }, 1)]
	[InlineData(new string[] { "123444-123444" }, 0)]
	[InlineData(new string[] { "111122-111122" }, 1)]
	[InlineData(new string[] { "172930-683082" }, 1142)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2019, 4, 2, input, true), out int actual);
		Assert.Equal(expected, actual);
	}

}
