namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 04: Camp Cleanup
/// https://adventofcode.com/2022/day/4
/// </summary>
[Description("Camp Cleanup")]
public sealed partial class Day04 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
	//       CONTAINS
	//
	//    a--------------b
	//         c-----d
	//         
	//         a-----b
	//    c--------------d
	//
	//    a >= c && b <= d
	//    c >= a && d <= b
	//
		int count = 0;
		for (int i = 0; i < input.Length; i++) {
			(int a, int b, int c, int d) = ParseLine(input[i]);
			if (   (a >= c && b <= d)
				|| (c >= a && d <= b)) {
				count++;
			}
		}
		return count;
	}

	private static int Solution2(string[] input) {
		//       OVERLAPS
		//
		//    a----------b
		//         c---------d
		//
		//         a---------b
		//    c--------d
		//
		//
		//    a--------------b
		//         c-----d
		//         
		//         a-----b
		//    c--------------d
		//
		//    a <= d && c <= b
		//
		int count = 0;
		for (int i = 0; i < input.Length; i++) {
			(int a, int b, int c, int d) = ParseLine(input[i]);
			if (a <= d && c <= b) {
				count++;
			}
		}
		return count;
	}

	private static (int a, int b, int c, int d) ParseLine(string input) =>
		input.Split(new char[] { '-', ',' }) switch {
			[var n1, var n2, var n3, var n4] => (n1.AsInt(), n2.AsInt(), n3.AsInt(), n4.AsInt()),
			_ => throw new NotImplementedException()
		};
}
