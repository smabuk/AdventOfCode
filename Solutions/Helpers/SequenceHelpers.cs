namespace AdventOfCode.Solutions.Helpers;

public static class SequenceHelpers {
	/// <summary>
	///		Generates a sequence of Triangular Numbers
	///		1, 3, 6, 10, 15 ...
	/// </summary>
	/// <param name="count">The number of sequential integers to generate</param>
	/// <param name="startAtZero">Start the sequence with the value 0</param>
	/// <returns>An IEnumerable<Int32> in C# or IEnumerable(Of Int32) in Visual Basic that contains a range of sequential triangular numbers.</returns>
	public static IEnumerable<int> TriangularNumbers(int count, bool startAtZero = false) {
		if (startAtZero) {
			yield return 0;
			count--;
		}
		for (int n = 1; n <= count; n++) {
			yield return TriangularNumber(n);
		};
	}
	public static int TriangularNumber(int n) => (1 + n) * n / 2;

	//public static int TriangularNumber(int n) =>
	//	n switch {
	//		< 0 => throw new ArgumentException($"n must be a positive integer: n={n}"),
	//		0 => 0,
	//		1 => 1,
	//		_ => n + TriangularNumber(n - 1)
	//	};




	public static IEnumerable<long> FactorialNumbers(int count) =>
		Enumerable.Range(1, count).Select(n => Factorial(n));

	public static long Factorial(int n) =>
		Enumerable.Range(1, n).Aggregate(1, (p, item) => p * item);




	public static IEnumerable<long> FibonacciNumbers(int count) {
		long prev = -1;
		long next = 1;
		for (int n = 1; n <= count; n++) {
			long sum = prev + next;
			prev = next;
			next = sum;
			yield return sum;
		};
	}

	public static long Fibonacci(int n) {
		long prev = -1;
		long next = 1;
		long sum = 0;
		for (int i = 1; i <= n; i++) {
			sum = prev + next;
			prev = next;
			next = sum;
		};
		return sum;
	}
}
