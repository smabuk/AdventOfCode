using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace AdventOfCode.Tests.Year2019;

public class Tests_03_Crossed_Wires {
	[Theory]
	[InlineData(new string[] {
		"R8,U5,L5,D3",
		"U7,R6,D4,L4"
		}, 6)]
	[InlineData(new string[] { 
		"R75,D30,R83,U83,L12,D49,R71,U7,L72",
		"U62,R66,U55,R34,D71,R55,D58,R83" 
		}, 159)]
	[InlineData(new string[] { 
		"R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51",
		"U98,R91,D20,R16,D67,R40,U7,R15,U6,R7" 
		}, 135)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2019, 3, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
		"R8,U5,L5,D3",
		"U7,R6,D4,L4"
		}, 30)]
	[InlineData(new string[] {
		"R75,D30,R83,U83,L12,D49,R71,U7,L72",
		"U62,R66,U55,R34,D71,R55,D58,R83"
		}, 610)]
	[InlineData(new string[] {
		"R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51",
		"U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"
		}, 410)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2019, 3, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}

}
