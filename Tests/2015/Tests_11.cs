using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2015 {
	public class Tests_11_Corporate_Policy {
		[Theory]
		[InlineData(new string[] { "abcdefgh" }, "abcdffaa")]
		[InlineData(new string[] { "ghijklmn" }, "ghjaabcc")]
		public void Part1(string[] input, string expected) {
			string actual = SolutionRouter.SolveProblem(2015, 11, 1, input);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("ab", 1, "ac")]
		[InlineData("az", 1, "ba")]
		[InlineData("uk", 26, "vk")]
		[InlineData("azk", 26, "bak")]
		[InlineData("zzy", 1, "zzz")]
		[InlineData("abcdezzy", 1, "abcdezzz")]
		[InlineData("abcdezzz", 1, "abcdfaaa")]
		public void Password_Should_IncrementBy(string input, long increment, string expected) {
			string actual = Solutions.Year2015.Day11.IncrementPasswordBy(input, increment);
			Assert.Equal(expected, actual);
		}


	}
}
