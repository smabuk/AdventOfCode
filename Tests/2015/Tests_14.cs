namespace AdventOfCode.Tests.Year2015;

public class Tests_14_ReindeerOlympics {
	[Theory]
	[InlineData(new string[] {
			"Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.",
			"Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds."
		}, 1, 16)]
	[InlineData(new string[] {
			"Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.",
			"Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds."
		}, 10, 160)]
	[InlineData(new string[] {
			"Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.",
			"Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds."
		}, 11, 176)]
	[InlineData(new string[] {
			"Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.",
			"Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds."
		}, 1000, 1120)]
	public void Part1(string[] input, int raceTime, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 14, 1, input, raceTime), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
			"Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.",
			"Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds."
		}, 1, 1)]
	[InlineData(new string[] {
			"Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.",
			"Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds."
		}, 140, 139)]
	[InlineData(new string[] {
			"Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.",
			"Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds."
		}, 1000, 689)]
	public void Part2(string[] input, int raceTime, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 14, 2, input, raceTime), out int actual);
		Assert.Equal(expected, actual);
	}


}
