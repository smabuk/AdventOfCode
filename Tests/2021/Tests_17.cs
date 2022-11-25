namespace AdventOfCode.Tests.Year2021;

public class Tests_17_Trick_Shot {
	[Theory]
	[InlineData(new string[] { "target area: x=20..30, y=-10..-5" }, 45)]
	[InlineData(new string[] { "target area: x=-20..-30, y=-10..-5" }, 45)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 17, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { "target area: x=20..30, y=-10..-5" }, 112)]
	[InlineData(new string[] { "target area: x=-20..-30, y=-10..-5" }, 112)]
	public void Part2(string[] input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2021, 17, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("target area: x=20..30, y=-10..-5", 7, 2 , true)]
	[InlineData("target area: x=20..30, y=-10..-5", 6, 3 , true)]
	[InlineData("target area: x=20..30, y=-10..-5", 9, 0 , true)]
	[InlineData("target area: x=20..30, y=-10..-5", 17, -4 , false)]
	[InlineData("target area: x=-20..-30, y=-10..-5", -7, 2 , true)]
	public void TargetArea_SuccessfulProbeVelocity(string input, int xV, int yV, bool expected) {
		(bool actual, _) = new Solutions._2021.Day17.TargetArea(input).WillProbeHit(new Point(xV, yV));
		Assert.Equal(expected, actual);
	}






}
