using Xunit.Sdk;

namespace AdventOfCode.Tests.Year2023;

public class Tests_14_Parabolic_Reflector_Dish(ITestOutputHelper testOutputHelper)
{
	const int DAY = 14;

	private const string TEST_DATA = """
		O....#....
		O.OO#....#
		.....##...
		OO.#O....O
		.O.....O#.
		O.#..O.#.#
		..O..#O..O
		.......O..
		#....###..
		#OO..#....
		
		""";

	[Theory]
	[InlineData(TEST_DATA, 136)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
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
