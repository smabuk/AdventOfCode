namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 21: Step Counter
/// https://adventofcode.com/2023/day/21
/// </summary>
[Description("Step Counter")]
public sealed partial class Day21 {

	public static string Part1(string[] input, params object[]? args)
	{
		int noOfSteps = GetArgument(args, argumentNumber: 1, defaultResult: 64);
		return Solution1(input, noOfSteps).ToString();
	}
	public static string Part2(string[] input, params object[]? args)
	{
		int noOfSteps = GetArgument(args, argumentNumber: 1, defaultResult: 26_501_365);
		if (noOfSteps > 5000) {
			return "** No solution written **";
		}
		return Solution2(input, noOfSteps).ToString();
	}

	private static readonly char START = 'S';
	private static readonly char ROCK  = '#';
	//private static readonly char PLOT  = '.';


	private static int Solution1(string[] input, int noOfSteps) {
		char[,] garden = input.To2dArray();
		Point start = garden.Walk2dArrayWithValues().Where(g => g.Value == START).Single();
		HashSet<Point> plots = [start]; 
		for (int i = 0; i < noOfSteps; i++) {
			HashSet<Point> nextPlots = []; 
			foreach (var plot in plots) {
				nextPlots = [.. nextPlots, .. garden.GetAdjacentCells(plot).Where(p => p.Value != ROCK).Select(p => p.Index)];
			}
			plots = [.. nextPlots];
		}
		return plots.Count;
	}

	private static int Solution2(string[] input, int noOfSteps) {
		char[,] garden = input.To2dArray();
		Point start = garden.Walk2dArrayWithValues().Where(g => g.Value == START).Single();
		//garden = ArrayHelpers.Create2dArray(1001, 1001, '.');
		//start = new(500, 500);
		HashSet<Point> plots = [start];
		for (int i = 0; i < noOfSteps; i++) {
			HashSet<Point> nextPlots = [];
			foreach (var plot in plots) {
				nextPlots = [.. nextPlots, .. garden.GetAdjacentCells(plot).Where(p => p.Value != ROCK).Select(p => p.Index)];
			}
			plots = [.. nextPlots];
		}
		int count = plots.Count;
		return count;
	}
}
