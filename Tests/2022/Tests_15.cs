namespace AdventOfCode.Tests._2022;

public class Tests_15_Beacon_Exclusion_Zone {
	[Theory]
	[InlineData("""
		Sensor at x=2, y=18: closest beacon is at x=-2, y=15
		Sensor at x=9, y=16: closest beacon is at x=10, y=16
		Sensor at x=13, y=2: closest beacon is at x=15, y=3
		Sensor at x=12, y=14: closest beacon is at x=10, y=16
		Sensor at x=10, y=20: closest beacon is at x=10, y=16
		Sensor at x=14, y=17: closest beacon is at x=10, y=16
		Sensor at x=8, y=7: closest beacon is at x=2, y=10
		Sensor at x=2, y=0: closest beacon is at x=2, y=10
		Sensor at x=0, y=11: closest beacon is at x=2, y=10
		Sensor at x=20, y=14: closest beacon is at x=25, y=17
		Sensor at x=17, y=20: closest beacon is at x=21, y=22
		Sensor at x=16, y=7: closest beacon is at x=15, y=3
		Sensor at x=14, y=3: closest beacon is at x=15, y=3
		Sensor at x=20, y=1: closest beacon is at x=15, y=3
		"""
		, 10, 26)]
	public void Part1(string input, int rowToSearch, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 15, 1, input, rowToSearch), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		a
		"""
		, 1, 9999)]
	public void Part2(string input, int noOfRounds, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2022, 15, 2, input, noOfRounds), out long actual);
		Assert.Equal(expected, actual);
	}
}
