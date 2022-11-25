namespace AdventOfCode.Tests._2015;

public class Tests_12_JSAbacusFramework_io {
	[Theory]
	[InlineData(new string[] { @"[1,2,3]" }, 6)]
	[InlineData(new string[] { @"{""a"":2,""b"":4}" }, 6)]
	[InlineData(new string[] { @"[[[3]]]" }, 3)]
	[InlineData(new string[] { @"{""a"":{""b"":4},""c"":-1}" }, 3)]
	[InlineData(new string[] { @"{""a"":[-1,1]}" }, 0)]
	[InlineData(new string[] { @"[-1,{""a"":1}]" }, 0)]
	[InlineData(new string[] { @"[]" }, 0)]
	[InlineData(new string[] { @"{}" }, 0)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 12, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { @"[1,2,3]" }, 6)]
	[InlineData(new string[] { @"[1,{""c"":""red"",""b"":2},3]" }, 4)]
	[InlineData(new string[] { @"{""d"":""red"",""e"":[1,2,3,4],""f"":5}" }, 0)]
	[InlineData(new string[] { @"[1,""red"",5]" }, 6)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 12, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}


}
