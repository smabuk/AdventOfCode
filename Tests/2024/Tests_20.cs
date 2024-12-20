namespace AdventOfCode.Tests.Year2024;

public class Tests_20_Race_Condition(ITestOutputHelper testOutputHelper)
{
	const int DAY = 20;

	const string TEST_INPUT = """
		###############
		#...#...#.....#
		#.#.#.#.#.###.#
		#S#...#.#.#...#
		#######.#.#.###
		#######.#.#...#
		#######.#.###.#
		###..E#...#...#
		###.#######.###
		#...###...#...#
		#.#####.#.###.#
		#.#...#.#.#...#
		#.#.#.#.#.#.###
		#...#...#...###
		###############
		""";

	[Theory]
	[InlineData("""
		###############
		#...#...12....#
		#.#.#.#.#.###.#
		#S#...#.#.#...#
		#######.#.#.###
		#######.#.#...#
		#######.#.###.#
		###..E#...#...#
		###.#######.###
		#...###...#...#
		#.#####.#.###.#
		#.#...#.#.#...#
		#.#.#.#.#.#.###
		#...#...#...###
		###############
		""", 12, 1)]
	[InlineData("", 2, 14)]
	[InlineData("", 4, 28)]
	public void Part1(string extraInput, int picosecondsToSave, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, TEST_INPUT, new Action<string[], bool>(Callback), picosecondsToSave, extraInput), out int actual);
		actual.ShouldBe(expected);
	}


	private void Callback(string[] lines, bool _)
	{
		if (lines is null or []) {
			return;
		}

		testOutputHelper.WriteLine(string.Join(Environment.NewLine, lines));
	}

}
