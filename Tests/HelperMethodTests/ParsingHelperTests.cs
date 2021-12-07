namespace AdventOfCode.Tests.HelperMethodTests;

public class ParsingHelperTests {
	[Theory]
	[InlineData(new string[] { "1", "2", "3"}, new int[] { 1, 2, 3, })]
	public void AsInts_ShouldBe(string[] input, int[] expected) {
		int[] actual = ParsingHelpers.AsInts(input).ToArray();
		Assert.Equal(expected, actual);
	}
	[Theory]
	[InlineData(new string[] { "1", "2", "3"}, new int[] { 1, 2, 3, })]
	[InlineData(new string[] { "3", "2", "1"}, new int[] { 3, 2, 1, })]
	public void AsInts_ShouldBe_AsExtensionMethod(string[] input, int[] expected) {
		int[] actual = input.AsInts().ToArray();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { "1", "2", "3"}, new long[] { 1, 2, 3, })]
	public void AsLongs_ShouldBe(string[] input, long[] expected) {
		long[] actual = ParsingHelpers.AsLongs(input).ToArray();
		Assert.Equal(expected, actual);
	}
	[Theory]
	[InlineData(new string[] { "1", "2", "3"}, new long[] { 1, 2, 3, })]
	[InlineData(new string[] { "3", "2", "1"}, new long[] { 3, 2, 1, })]
	public void AsLongs_ShouldBe_AsExtensionMethod(string[] input, long[] expected) {
		long[] actual = input.AsLongs().ToArray();
		Assert.Equal(expected, actual);
	}
}
