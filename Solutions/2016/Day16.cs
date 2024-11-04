using static AdventOfCode.Solutions._2016.Day16Constants;
using static AdventOfCode.Solutions._2016.Day16Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 16: Dragon Checksum
/// https://adventofcode.com/2016/day/16
/// </summary>
[Description("Dragon Checksum")]
public sealed partial class Day16 {

	public static string Part1(string[] input, params object[]? args)
	{
		int diskLength = GetArgument<int>(args, argumentNumber: 1, defaultResult: DISK_LENGTH_PART1);
		return Solve(input, diskLength).ToString();
	}

	public static string Part2(string[] input) => Solve(input, DISK_LENGTH_PART2).ToString();

	private static string Solve(string[] input, int diskLength) {
		string state = input[0];
		while (diskLength > state.Length) {
			state = state.GenerateDragonCurve();
		}

		return state[..diskLength].Checksum();
	}
}

file static class Day16Extensions
{
	public static string GenerateDragonCurve(this string a)
	{
		Span<char> b = new string([.. a.Reverse()]).ToCharArray();
		for (int i = 0; i < b.Length; i++) {
			b[i] = b[i] == ZERO ? ONE : ZERO;
		}

		return $"{a}{ZERO}{b}";
	}

	public static string Checksum(this string a)
	{
		string checksum = a;
		while (checksum.Length.IsEven()) {
			checksum = string.Join("", checksum
				.Chunk(2)
				.Select(c => c[0] == c[1] ? ONE : ZERO));
		}

		return checksum;
	}

	public static bool IsEven(this int number) => number %2 == 0;
}

internal sealed partial class Day16Types
{
}

file static class Day16Constants
{
	public const int DISK_LENGTH_PART1 = 272;
	public const int DISK_LENGTH_PART2 = 35651584;

	public const char ONE = '1'; 
	public const char ZERO = '0'; 
}
