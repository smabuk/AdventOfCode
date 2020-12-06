using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2015
{
    public class Tests_03_Perfectly_Spherical_Houses_in_a_Vacuum {
		[Theory]
		[InlineData( new string[] { ">" }, 2)]
		[InlineData( new string[] { "^>v<" }, 4)]
		[InlineData( new string[] { "^v^v^v^v^v" }, 2)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 03, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] { "^v" }, 3)]
		[InlineData(new string[] { "^>v<" }, 3)]
		[InlineData(new string[] { "^v^v^v^v^v" }, 11)]
		public void Part2(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 03, 2, input), out int actual);
			Assert.Equal(expected, actual);
		}
	}
}
