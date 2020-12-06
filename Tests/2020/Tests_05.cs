using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCode.Shared;

using Xunit;

namespace AdventOfCode.Tests.Year2020 {
	public class Tests_05 {
		[Theory]
		[InlineData(new string[] { "BFFFBBFRRR" }, 567)]
		[InlineData(new string[] { "FFFBBBFRRR" }, 119)]
		[InlineData(new string[] { "BBFFBBFRLL" }, 820)]
		[InlineData(new string[] { "BBFFBBFRRL" }, 822)]
		public void Part1_Binary_Boarding(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2020, 5, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void Part2_Binary_Boarding() {
			int year = 2020;
			int day = 5;

			string filename = Path.GetFullPath(Path.Combine(".",$"{year}_{day:D2}.txt"));

			// Start: Fix for xUnit test project
			if (Path.GetFileName(Path.GetDirectoryName(filename))?.StartsWith("net") ?? false) {
				filename = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "Data", $"{year}_{day:D2}.txt"));
			}
			// End: Fix for xUnit test project
			string[]? input = null;
			if (File.Exists(filename)) {
				input = File.ReadAllText(filename).Split("\n");
			}

			_ = int.TryParse(SolutionRouter.SolveProblem(2020,5,2, input), out int actual);
			Assert.Equal(671, actual);
		}
	}
}
