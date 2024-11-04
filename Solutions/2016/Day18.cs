﻿using static AdventOfCode.Solutions._2016.Day18Constants;
using static AdventOfCode.Solutions._2016.Day18Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 18: Like a Rogue
/// https://adventofcode.com/2016/day/18
/// </summary>
[Description("Like a Rogue")]
public sealed partial class Day18 {

	public static string Part1(string[] input, Action<string[], bool>? visualise = null, params object[]? args)
	{
		int noOfRows = GetArgument<int>(args, argumentNumber: 1, defaultResult: 40);
		return Solve(input, noOfRows, visualise).ToString();
	}

	public static string Part2(string[] input) => Solve(input, 400_000).ToString();

	private static int Solve(string[] input, int noOfRows, Action<string[], bool>? visualise = null) {
		string[] room = [..input[0].RoomRows().Take(noOfRows)];
		if (noOfRows < 50) {
			room.VisualiseStrings("Final", visualise);
		}
		return room.Sum(r => r.Count(c => c == SAFE));
	}
}

file static class Day18Extensions
{
	public static IEnumerable<string> RoomRows(this string startRow)
	{
		string row = startRow;
		yield return row;
		while (true) {
			string prevRow = $"{SAFE}{row}{SAFE}";
			Span<char> newRow = new string(row).ToCharArray();
			for (int i = 0; i < newRow.Length; i++) {
				newRow[i] = prevRow[i..(i + 3)].NewTileType();
			}

			row = newRow.ToString();
			yield return row;
		}
	}

	public static char NewTileType(this string slice)
	{
		       if (slice[LEFT] is TRAP && slice[CENTRE] is TRAP && slice[RIGHT] is SAFE) {
			return TRAP;
		} else if (slice[LEFT] is SAFE && slice[CENTRE] is TRAP && slice[RIGHT] is TRAP) {
			return TRAP;
		} else if (slice[LEFT] is TRAP && slice[CENTRE] is SAFE && slice[RIGHT] is SAFE) {
			return TRAP;
		} else if (slice[LEFT] is SAFE && slice[CENTRE] is SAFE && slice[RIGHT] is TRAP) {
			return TRAP;
		}

		return SAFE;
	} 
}

internal sealed partial class Day18Types
{
}

file static class Day18Constants
{
	public const char SAFE = '.';
	public const char TRAP = '^';

	public const int LEFT = 0;
	public const int CENTRE = 1;
	public const int RIGHT = 2;
}
