using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCode.Shared.Helpers;

using Xunit;

namespace AdventOfCode.Tests.HelperMethodTests {
	public class ArrayHelperTests {
		[Theory]
		[InlineData(new int[] { 1, 2, 3, 4 }, 4)]
		[InlineData(new int[] { 4, 3, 2, 1 }, 4)]
		[InlineData(new int[] { 4, 3, -1, 1 }, 4)]
		[InlineData(new int[] { -4, -3, -2, -6 }, -2)]
		public void Should_FindHighest_Int_Value(int[] input, int expected) {
			int actual = ArrayHelpers.HighestValue(input);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new int[] { 1, 2, 3, 4 }, 1)]
		[InlineData(new int[] { 4, 3, 2, 1 }, 1)]
		[InlineData(new int[] { 4, 3, -1, 1 }, -1)]
		public void Should_FindLowest_Int_Value(int[] input, int expected) {
			int actual = ArrayHelpers.LowestValue(input);
			Assert.Equal(expected, actual);
		}
	}

}
