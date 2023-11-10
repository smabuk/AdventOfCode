namespace AdventOfCode.Tests.Year2018;

public class Tests_04_Repose_Record
{
	const int DAY = 4;

	[Theory]
	[InlineData("""
		[1518-11-01 00:00] Guard #10 begins shift
		[1518-11-01 00:05] falls asleep
		[1518-11-01 00:25] wakes up
		[1518-11-01 00:30] falls asleep
		[1518-11-01 00:55] wakes up
		[1518-11-01 23:58] Guard #99 begins shift
		[1518-11-02 00:40] falls asleep
		[1518-11-02 00:50] wakes up
		[1518-11-03 00:05] Guard #10 begins shift
		[1518-11-03 00:24] falls asleep
		[1518-11-03 00:29] wakes up
		[1518-11-04 00:02] Guard #99 begins shift
		[1518-11-04 00:36] falls asleep
		[1518-11-04 00:46] wakes up
		[1518-11-05 00:03] Guard #99 begins shift
		[1518-11-05 00:45] falls asleep
		[1518-11-05 00:55] wakes up
		"""
		, 240)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		[1518-11-01 00:00] Guard #10 begins shift
		[1518-11-01 00:05] falls asleep
		[1518-11-01 00:25] wakes up
		[1518-11-01 00:30] falls asleep
		[1518-11-01 00:55] wakes up
		[1518-11-01 23:58] Guard #99 begins shift
		[1518-11-02 00:40] falls asleep
		[1518-11-02 00:50] wakes up
		[1518-11-03 00:05] Guard #10 begins shift
		[1518-11-03 00:24] falls asleep
		[1518-11-03 00:29] wakes up
		[1518-11-04 00:02] Guard #99 begins shift
		[1518-11-04 00:36] falls asleep
		[1518-11-04 00:46] wakes up
		[1518-11-05 00:03] Guard #99 begins shift
		[1518-11-05 00:45] falls asleep
		[1518-11-05 00:55] wakes up
		"""
		, 4455)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
