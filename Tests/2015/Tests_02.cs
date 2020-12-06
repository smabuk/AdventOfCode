using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2015
{
    public class Tests_02
    {
		[Theory]
		[InlineData( new string[] { "2x3x4" }, 58)]
		[InlineData( new string[] { "1x1x10" }, 43)]
		public void Part1_I_Was_Told_There_Would_Be_No_Math(string[] input, int expected) {
			_ = int.TryParse(Solution_2015_02.Part1(input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] { "2x3x4" }, 34)]
		[InlineData(new string[] { "1x1x10" }, 14)]
		public void Part2_I_Was_Told_There_Would_Be_No_Math(string[] input, int expected) {
			_ = int.TryParse(Solution_2015_02.Part2(input), out int actual);
			Assert.Equal(expected, actual);
		}
	}
}
