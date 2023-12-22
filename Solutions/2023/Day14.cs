using static AdventOfCode.Solutions._2023.Day14;
using Dish = char[,];

namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 14: Parabolic Reflector Dish
/// https://adventofcode.com/2023/day/14
/// </summary>
[Description("Parabolic Reflector Dish")]
public sealed partial class Day14 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	public const char EMPTY            = '.';
	public const char ROUNDED_ROCK     = 'O';
	public const char CUBE_SHAPED_ROCK = '#';

	private static int Solution1(string[] input) {
		return input
			.To2dArray()
			.Tilt(Direction.North)
			.CalculateLoad();
	}

	private static int Solution2(string[] input)
	{
		return input
			.To2dArray()
			.Spin(1_000_000_000)
			.CalculateLoad();
	}

	public enum Direction
	{
		North,
		West,
		South,
		East
	}
}

public static class Day14Helpers
{
	public static Dish Spin(this Dish dish, int noOfCycles)
	{
		Dictionary<string, long> state = [];

		Dish spunDish = (Dish)dish.Clone();
		
		long firstSeen = 0;
		long interval = 0;

		for (long i = 0; i < noOfCycles; i++) {
			spunDish = spunDish
				.Tilt(Direction.North)
				.Tilt(Direction.West)
				.Tilt(Direction.South)
				.Tilt(Direction.East);
			if (interval == 0) {
				string currentState = spunDish.ToState();
				if (state.TryGetValue(currentState, out firstSeen)) {
					interval = i - firstSeen;
					i += ((noOfCycles / interval) - (i / interval) - 1) * interval;
				}
				state[currentState] = i;
			}
		}
		return spunDish;
	}

	public static Dish Tilt(this Dish dish, Direction direction)
	{
		Dish tiltedDish = (Dish)dish.Clone();

		(int dX, int dY) = direction switch {
			Direction.North => ( 0, -1),
			Direction.West  => (-1,  0),
			Direction.South => ( 0,  1),
			Direction.East  => ( 1,  0),
			_ => throw new ArgumentOutOfRangeException(nameof(direction)),
		};

		(int xStart, int yStart) = direction switch {
			Direction.North => (0, 0),
			Direction.West  => (0, 0),
			Direction.South => (0, dish.RowsMax()),
			Direction.East  => (dish.ColsMax(),                   0),
			_ => throw new ArgumentOutOfRangeException(nameof(direction)),
		};

		int xStep = xStart == 0 ? 1 : -1;
		int yStep = yStart == 0 ? 1 : -1;
		for (int y = yStart; dish.IsInBounds(0, y); y += yStep) {
		for (int x = xStart; dish.IsInBounds(x, y); x += xStep) {
			(int lookAtX, int lookatY) = (x + dX, y + dY);
			char current = tiltedDish[x, y];
			if (current == ROUNDED_ROCK && tiltedDish.IsInBounds(lookAtX, lookatY) && tiltedDish[lookAtX, lookatY] == EMPTY) {
				while (tiltedDish.IsInBounds(lookAtX, lookatY) && tiltedDish[lookAtX, lookatY] == EMPTY) {
					(lookAtX, lookatY) = (lookAtX + dX, lookatY + dY);
				}
				tiltedDish[lookAtX - dX, lookatY -dY] = current;
				tiltedDish[x, y] = EMPTY;
			}
		}}

		return tiltedDish;
	}

	public static string ToState(this Dish dish)
		=> $"{string.Join("", dish.WalkWithValues().Where(x => x.Value != CUBE_SHAPED_ROCK).Select(x => x.Value.ToString()))}";

	public static int CalculateLoad(this Dish dish)
	{
		int load = 0;
		for (int y = 0; dish.IsInBounds(0, y); y++) {
		for (int x = 0; dish.IsInBounds(x, y); x++) {
			if (dish[x, y] == ROUNDED_ROCK) {
				load += dish.RowsCount() - y;
			}
		}}
		
		return load;
	}
}
