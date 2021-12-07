namespace AdventOfCode.Tests.HelperMethodTests;

public class SequenceHelperTests {
	[Theory]
	[InlineData(4, false, new int[] { 1, 3, 6, 10 })]
	[InlineData(5, false, new int[] { 1, 3, 6, 10, 15 })]
	[InlineData(4, true, new int[] { 0, 1, 3, 6 })]
	[InlineData(5, true, new int[] { 0, 1, 3, 6, 10 })]
	public void TriangularNumbers_Sequences(int count, bool startAtZero, int[] expected) {
		int[] actual = SequenceHelpers.TriangularNumbers(count, startAtZero).ToArray();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(0, 0)]
	[InlineData(1, 1)]
	[InlineData(2, 3)]
	[InlineData(3, 6)]
	[InlineData(4, 10)]
	[InlineData(11, 66)]
	public void TriangularNumbers(int n, int expected) {
		int actual = SequenceHelpers.TriangularNumber(n);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(6, new long[] { 0, 1, 1, 2, 3, 5 })]
	[InlineData(7, new long[] { 0, 1, 1, 2, 3, 5, 8 })]
	[InlineData(8, new long[] { 0, 1, 1, 2, 3, 5, 8, 13 })]
	public void FibonacciNumbers_Sequences(int count, long[] expected) {
		long[] actual = SequenceHelpers.FibonacciNumbers(count).ToArray();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(1, 0)]
	[InlineData(2, 1)]
	[InlineData(3, 1)]
	[InlineData(4, 2)]
	[InlineData(5, 3)]
	[InlineData(11, 55)]
	public void FibonacciNumbers(int n, long expected) {
		long actual = SequenceHelpers.Fibonacci(n);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(4, new long[] { 1, 2, 6, 24 })]
	[InlineData(5, new long[] { 1, 2, 6, 24, 120 })]
	public void Factorial_Sequences(int count, long[] expected) {
		long[] actual = SequenceHelpers.FactorialNumbers(count).ToArray();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(0, 1)]
	[InlineData(1, 1)]
	[InlineData(2, 2)]
	[InlineData(3, 6)]
	[InlineData(4, 24)]
	[InlineData(11, 39916800)]
	public void Factorials(int n, long expected) {
		long actual = SequenceHelpers.Factorial(n);
		Assert.Equal(expected, actual);
	}
}
