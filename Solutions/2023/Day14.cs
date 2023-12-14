namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 14: Parabolic Reflector Dish
/// https://adventofcode.com/2023/day/14
/// </summary>
[Description("Parabolic Reflector Dish")]
public sealed partial class Day14 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input, Action<string[], bool>? visualise = null, params object[]? args)
	{
		return Solution2(input, visualise).ToString();
	}

	public const char ROUNDED_ROCK = 'O';
	public const char FLAT_ROCK    = '#';

	private static int Solution1(string[] input) {
		return input
			.AsCells([ROUNDED_ROCK, FLAT_ROCK])
			.Tilt(0, 0, 0)
			.Sum(r => input.Length - r.Y);
	}

	private static int Solution2(string[] input, Action<string[], bool>? visualise = null) {
		long noOfCycles = 1_000_000_000;
		IEnumerable<Cell<char>> rocks = input.AsCells([ROUNDED_ROCK, FLAT_ROCK]);
		List<Cell<char>> flatRocks = [..rocks.Where(r => r.Value == FLAT_ROCK)];
		Dictionary<string, long> state = [];

		int maxX = input[0].Length;
		int maxY = input.Length;
		state[rocks.ToState(maxY)] = 0;

		long firstSeen = 0;
		long interval = 0;
		for (long i = 0; i < (noOfCycles * 4); i++) {
			rocks = [..flatRocks, ..rocks.Tilt((int)(i % 4), maxY, maxY)];
			string stateString = rocks.ToState(maxY);
			if (interval == 0 && state.TryGetValue(stateString, out firstSeen)) {
				interval = i - firstSeen;
				i += (((noOfCycles * 4 / interval) - (i / interval) - 1) * interval);
			}
			if (interval == 0) {
				state[stateString] = i;
			}
		}

		long index = ((noOfCycles - firstSeen) % interval);
		VisualiseGrid(rocks, $"Final: ", maxX, maxY, visualise);
		return rocks.Where(r => r.Value == ROUNDED_ROCK).Sum(r => input.Length - r.Y);
	}

	private static void VisualiseGrid(IEnumerable<Cell<char>> rocks, string title, int maxX, int maxY, Action<string[], bool>? visualise)
	{
		char[,] grid = ArrayHelpers.Create2dArray(maxY, maxX, '.');
		foreach (var rock in rocks) {
			grid[rock.X, rock.Y] = rock.Value;
		}
		if (visualise is not null) {
			string[] output = ["", title, .. grid.PrintAsStringArray(0)];
			_ = Task.Run(() => visualise?.Invoke(output, false));
		}
	}
}

public static class Day14Helpers
{
	public static string ToState(this IEnumerable<Cell<char>> rocks, int maxY)
	{
		int load = rocks.Where(r => r.Value == Day14.ROUNDED_ROCK).Sum(r => maxY - r.Y);
		return $"{load}|{string.Join(":", rocks.Where(r => r.Value == Day14.ROUNDED_ROCK).Select(r => r.Index.ToString()))}";
	}

	public static IEnumerable<Cell<char>> Tilt(this IEnumerable<Cell<char>> rocks, int cycle, int maxX, int maxY)
	{
		Direction direction = (Direction)(cycle % 4);
		List<Cell<char>> theRocks = direction switch
		{
			Direction.North => [.. rocks.OrderBy(r => r.Index.Y)],
			Direction.West  => [.. rocks.OrderBy(r => r.Index.X)],
			Direction.South => [.. rocks.OrderByDescending(r => r.Index.Y)],
			Direction.East  => [.. rocks.OrderByDescending(r => r.Index.X)],
			_ => throw new NotImplementedException(),
		};

		for (int i = 0; i < theRocks.Count; i++) {
			Cell<char> rock = theRocks[i];
			if (rock.Value is Day14.ROUNDED_ROCK) {
				int newX = rock.X;
				int newY = rock.Y;

				(newX, newY) = direction switch
				{
					Direction.North => (newX, theRocks.Where(r => r.Index.X == rock.X && r.Index.Y < rock.Y).Select(r => r.Y).DefaultIfEmpty(-1).Max() + 1)  ,
					Direction.West  => (      theRocks.Where(r => r.Index.Y == rock.Y && r.Index.X < rock.X).Select(r => r.X).DefaultIfEmpty(-1).Max() + 1   , newY),
					Direction.South => (newX, theRocks.Where(r => r.Index.X == rock.X && r.Index.Y > rock.Y).Select(r => r.Y).DefaultIfEmpty(maxY).Min() - 1),
					Direction.East  => (      theRocks.Where(r => r.Index.Y == rock.Y && r.Index.X > rock.X).Select(r => r.X).DefaultIfEmpty(maxX).Min() - 1 , newY),
					_ => throw new NotImplementedException(),
				};
				Debug.Assert(newX >= 0 && newY >= 0);
				theRocks[i] = rock with { Index = new(newX, newY) };
				yield return theRocks[i];
			}
		}
	}

	private enum Direction
	{
		North,
		West,
		South,
		East
	}

}
