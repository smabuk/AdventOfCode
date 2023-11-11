using static AdventOfCode.Solutions._2018.Day11;

namespace AdventOfCode.Tests.Year2018;

public class Tests_11_Chronal_Charge
{
	const int DAY = 11;

	[Theory]
	[InlineData("18", 33, 45)]
	[InlineData("42", 21, 61)]
	public void Part1(string input, int expectedX, int expectedY)
	{
		string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART1, input);
		string expected = $"{expectedX},{expectedY}";
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(8, 3, 5, 4)]
	[InlineData(57, 122, 79, -5)]
	[InlineData(39, 217, 196, 0)]
	[InlineData(71, 101, 153, 4)]
	public void FuelCellP0werLevel(int gridSerialNo, int x, int y, int expected)
	{
		int actual = CalculatePowerValue(gridSerialNo, x, y);
		Assert.Equal(expected, actual);
	}

	//[Theory]
	//[InlineData(21, 61)]
	//public void Part2(string input, int expected)
	//{
	//	_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, 32), out int actual);
	//	Assert.Equal(expected, actual);
	//}
}
