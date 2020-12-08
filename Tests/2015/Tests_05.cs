using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2015 {
	public class Tests_05_Doesnt_He_Have_Intern_Elves_For_This {
		[Theory]
		[InlineData(new string[] { "ugknbfddgicrmopn" }, true)]
		[InlineData(new string[] { "aaa" },              true)]
		[InlineData(new string[] { "jchzalrnumimnmhp" }, false)]
		[InlineData(new string[] { "haegwjzuvuyypxyu" }, false)]
		[InlineData(new string[] { "dvszwmarrgswjxmb" }, false)]
		public void Part1(string[] input, bool expected) {
			bool actual = Solutions.Year2015.Day05.Nice_Part1(input[0]);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] { "qjhvhtzxzqqjkmpb" }, true)]
		[InlineData(new string[] { "xxyxx" },            true)]
		[InlineData(new string[] { "uurcxstgmygtbstg" }, false)]
		[InlineData(new string[] { "ieodomkazucvgmuy" }, false)]
		public void Part2(string[] input, bool expected) {
			bool actual = Solutions.Year2015.Day05.Nice_Part2(input[0]);
			Assert.Equal(expected, actual);
		}
	}
}
