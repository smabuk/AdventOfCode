﻿namespace AdventOfCode.Tests.HelperMethodTests;

public class ArrayHelperTests {
	[Theory]
	[InlineData(new int[] { 1, 2, 3, 4, 5, 6 }
		, 2, 3
		, 2, 3, 6)]
	[InlineData(new int[] { 1, 2, 3, 4, 5, 6 }
		, 3, 2
		, 3, 2, 6)]
	[InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }
		, 3, null
		, 3, 3, 9)]
	[InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }
		, 5, null
		, 5, 2, 10)]
	public void AsArray_Int_Should_Have_Shape(int[] input, int cols, int? rows,
		int expectedCols, int expectedRows, int expectedLength) {
		int[,] array = input.As2dArray<int>(cols, rows);
		Assert.Equal(expectedLength, array.Length);
		Assert.Equal(expectedCols, array.GetUpperBound(0) + 1);
		Assert.Equal(expectedRows, array.GetUpperBound(1) + 1);
		//Assert.Equal(expected, actual);
	}

	[Fact]
	public void AsArray_Tuple_Should_HaveShape() {
		(char, int)[] input = new (char, int)[8];
		for (int i = 0; i < input.GetUpperBound(0); i++) {
			input[i] = new((char)(65 + i), i + 1);
		}
		(char, int)[,] array = input.As2dArray<(char, int)>(4, 2);
		Assert.Equal(8, array.Length);
		Assert.Equal(4, array.GetUpperBound(0) + 1);
		Assert.Equal(2, array.GetUpperBound(1) + 1);
		Assert.Equal(('E', 5), array[0, 1]);

		Array.Clear(array);
		array = input.As2dArray<(char, int)>(2);
		Assert.Equal(8, array.Length);
		Assert.Equal(2, array.GetUpperBound(0) + 1);
		Assert.Equal(4, array.GetUpperBound(1) + 1);
		Assert.Equal(('C', 3), array[0, 1]);

		Array.Clear(array);
		array = input.As2dArray<(char, int)>(3);
		Assert.Equal(9, array.Length);
		Assert.Equal(3, array.GetUpperBound(0) + 1);
		Assert.Equal(3, array.GetUpperBound(1) + 1);
		Assert.Equal(('G', 7), array[0, 2]);
	}


	[Theory]
	[InlineData(new int[] { 1, 2, 3, 4, 5, 6 }
		, 3, 2, 2
		, " 1 2 3")]
	[InlineData(new int[] { 1, 2, 3, 4, 5, 6 }
		, 2, 3, 4
		, "   1   2")]
	public void PrintAsStringArray_Int_Should_Have_Shape(int[] input, int cols, int? rows, int width,
		string expected) {
		string[] actual = input.As2dArray<int>(cols, rows).PrintAsStringArray<int>(width).ToArray();
		Assert.Equal(expected, actual[0]);
	}



	[Theory]
	[InlineData(new int[] { 1, 2, 3, 4 }, 1)]
	[InlineData(new int[] { 4, 3, 2, 1 }, 1)]
	[InlineData(new int[] { 4, 3, -1, 1 }, -1)]
	public void Should_FindLowest_Int_Value(int[] input, int expected) {
		int actual = ArrayHelpers.LowestValue(input);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new int[] { 2, 5, 4, 4, 4, 4, 3, 2, 2, 2 }, 3.2)]
	[InlineData(new int[] { 5 }, 5)]
	public void Should_Find_Mean(int[] input, double expected) {
		double actual = ArrayHelpers.Mean<int>(input);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new int[] { 2, 5, 4, 4, 4, 4, 3, 2, 2, 2 }, 3.5)]
	[InlineData(new int[] { 5, 1, 3 }, 3)]
	public void Should_Find_Median(int[] input, double expected) {
		double actual = ArrayHelpers.Median<int>(input);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new int[] { 1, 5, 4, 4, 4, 4, 3, 2, 2, 2 }, new int[] { 4 })]
	[InlineData(new int[] { 2, 5, 4, 4, 4, 4, 3, 2, 2, 2 }, new int[] { 2, 4 })]
	[InlineData(new int[] { 5, 1, 3 }, new int[] { 5, 1, 3 })]
	public void Should_Find_Mode(int[] input, int[] expected) {
		int[] actual = ArrayHelpers.Modes(input).ToArray();
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
