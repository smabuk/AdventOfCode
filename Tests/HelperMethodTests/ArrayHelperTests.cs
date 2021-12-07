namespace AdventOfCode.Tests.HelperMethodTests;

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

	[Fact]
	public void Should_FindHighest_Of_Multiple_Int_Parameters() {
		Assert.Equal(6, ArrayHelpers.HighestValue(3, 6, 2, 4, -3, 4));
	}
	[Fact]
	public void Should_FindLowestt_Of_Multiple_Int_Parameters() {
		Assert.Equal(-3, ArrayHelpers.LowestValue(3, 6, 2, 4, -3, 4));
	}
}
