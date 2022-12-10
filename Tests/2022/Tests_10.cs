namespace AdventOfCode.Tests._2022;

public class Tests_10_Cathode_Ray_Tube {
	[Theory]
	[InlineData("""
		addx 15
		addx -11
		addx 6
		addx -3
		addx 5
		addx -1
		addx -8
		addx 13
		addx 4
		noop
		addx -1
		addx 5
		addx -1
		addx 5
		addx -1
		addx 5
		addx -1
		addx 5
		addx -1
		addx -35
		addx 1
		addx 24
		addx -19
		addx 1
		addx 16
		addx -11
		noop
		noop
		addx 21
		addx -15
		noop
		noop
		addx -3
		addx 9
		addx 1
		addx -3
		addx 8
		addx 1
		addx 5
		noop
		noop
		noop
		noop
		noop
		addx -36
		noop
		addx 1
		addx 7
		noop
		noop
		noop
		addx 2
		addx 6
		noop
		noop
		noop
		noop
		noop
		addx 1
		noop
		noop
		addx 7
		addx 1
		noop
		addx -13
		addx 13
		addx 7
		noop
		addx 1
		addx -33
		noop
		noop
		noop
		addx 2
		noop
		noop
		noop
		addx 8
		noop
		addx -1
		addx 2
		addx 1
		noop
		addx 17
		addx -9
		addx 1
		addx 1
		addx -3
		addx 11
		noop
		noop
		addx 1
		noop
		addx 1
		noop
		noop
		addx -13
		addx -19
		addx 1
		addx 3
		addx 26
		addx -30
		addx 12
		addx -1
		addx 3
		addx 1
		noop
		noop
		noop
		addx -9
		addx 18
		addx 1
		addx 2
		noop
		noop
		addx 9
		noop
		noop
		noop
		addx -1
		addx 2
		addx -37
		addx 1
		addx 3
		noop
		addx 15
		addx -21
		addx 22
		addx -6
		addx 1
		noop
		addx 2
		addx 1
		noop
		addx -10
		noop
		noop
		addx 20
		addx 1
		addx 2
		addx 2
		addx -6
		addx -11
		noop
		noop
		noop
		"""
		, 20, 40, 13140)]
	public void Part1(string input, int cycleCheckStart, int cycleCheckInterval, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 10, 1, input, cycleCheckStart, cycleCheckInterval), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		addx 15
		addx -11
		addx 6
		addx -3
		addx 5
		addx -1
		addx -8
		addx 13
		addx 4
		noop
		addx -1
		addx 5
		addx -1
		addx 5
		addx -1
		addx 5
		addx -1
		addx 5
		addx -1
		addx -35
		addx 1
		addx 24
		addx -19
		addx 1
		addx 16
		addx -11
		noop
		noop
		addx 21
		addx -15
		noop
		noop
		addx -3
		addx 9
		addx 1
		addx -3
		addx 8
		addx 1
		addx 5
		noop
		noop
		noop
		noop
		noop
		addx -36
		noop
		addx 1
		addx 7
		noop
		noop
		noop
		addx 2
		addx 6
		noop
		noop
		noop
		noop
		noop
		addx 1
		noop
		noop
		addx 7
		addx 1
		noop
		addx -13
		addx 13
		addx 7
		noop
		addx 1
		addx -33
		noop
		noop
		noop
		addx 2
		noop
		noop
		noop
		addx 8
		noop
		addx -1
		addx 2
		addx 1
		noop
		addx 17
		addx -9
		addx 1
		addx 1
		addx -3
		addx 11
		noop
		noop
		addx 1
		noop
		addx 1
		noop
		noop
		addx -13
		addx -19
		addx 1
		addx 3
		addx 26
		addx -30
		addx 12
		addx -1
		addx 3
		addx 1
		noop
		noop
		noop
		addx -9
		addx 18
		addx 1
		addx 2
		noop
		noop
		addx 9
		noop
		noop
		noop
		addx -1
		addx 2
		addx -37
		addx 1
		addx 3
		noop
		addx 15
		addx -21
		addx 22
		addx -6
		addx 1
		noop
		addx 2
		addx 1
		noop
		addx -10
		noop
		noop
		addx 20
		addx 1
		addx 2
		addx 2
		addx -6
		addx -11
		noop
		noop
		noop
		"""
		,
		"""
		██  ██  ██  ██  ██  ██  ██  ██  ██  ██  
		███   ███   ███   ███   ███   ███   ███ 
		████    ████    ████    ████    ████    
		█████     █████     █████     █████     
		██████      ██████      ██████      ████
		███████       ███████       ███████     
		""")]
	public void Part2(string input, string expected) {
		string actual = SolutionRouter.SolveProblem(2022, 10, 2, input, false);
		Assert.Equal(expected, actual);
	}
}
