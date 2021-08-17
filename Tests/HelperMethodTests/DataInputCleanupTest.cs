namespace AdventOfCode.Tests.HelperMethodTests;

public class DataInputCleanupTest {
	[Theory]
	[InlineData(new string[] { "Line 1", "" }, new string[] { "Line 1" })]
	[InlineData(new string[] { "Line 1", "Line 2" }, new string[] { "Line 1", "Line 2" })]
	[InlineData(null, new string[0])]
	public void Should_RemoveBlankLineFromEnd_IfLineIsEmpty(string[] input, string[] expected) {
		string[] actual = DataInputCleanup.StripTrailingBlankLineOrDefault(input);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { "Line 1", "" }, new string[] { "Line 1" })]
	[InlineData(new string[] { "Line 1", "Line 2" }, new string[] { "Line 1", "Line 2" })]
	[InlineData(null, new string[0])]
	public void Should_StripTrailingBlankLineOrDefault_DotSyntax(string[] input, string[] expected) {
		string[] actual = input.StripTrailingBlankLineOrDefault();
		Assert.Equal(expected, actual);
	}
}
