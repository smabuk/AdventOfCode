using static AdventOfCode.Solutions._2022.Day25;

namespace AdventOfCode.Tests._2022;

public class Tests_25_Full_of_Hot_Air {
	[Theory]
	[InlineData("""
		1=-0-2
		12111 
		 2=0= 
		   21 
		 2=01 
		  111 
		20012 
		  112 
		1=-1= 
		 1-12 
		   12 
		   1= 
		  122 
		"""
		, "2=-1=0")]
	public void Part1(string input, string expected) {
		string actual = SolutionRouter.SolveProblem(2022, 25, 1, input);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("            1", 1)]
	[InlineData("            2", 2)]
	[InlineData("           1=", 3)]
	[InlineData("           1-", 4)]
	[InlineData("           10", 5)]
	[InlineData("           11", 6)]
	[InlineData("           12", 7)]
	[InlineData("           2=", 8)]
	[InlineData("           2-", 9)]
	[InlineData("           20", 10)]
	[InlineData("          1=0", 15)]
	[InlineData("          1-0", 20)]
	[InlineData("       1=11-2", 2022)]
	[InlineData("      1-0---0", 12345)]
	[InlineData("1121-1110-1=0", 314159265)]
	public void ConvertFromSnafu(string input, long expected) {
		SnafuNumber actual = input;
		Assert.Equal(expected, (long)actual);
	}

	[Theory]
	[InlineData(1,                     "1")]
	[InlineData(2,                     "2")]
	[InlineData(3,                    "1=")]
	[InlineData(4,                    "1-")]
	[InlineData(5,                    "10")]
	[InlineData(6,                    "11")]
	[InlineData(7,                    "12")]
	[InlineData(8,                    "2=")]
	[InlineData(9,                    "2-")]
	[InlineData(10,                   "20")]
	[InlineData(15,                  "1=0")]
	[InlineData(20,                  "1-0")]
	[InlineData(2022,             "1=11-2")]
	[InlineData(12345,           "1-0---0")]
	[InlineData(314159265, "1121-1110-1=0")]
	public void ConvertToSnafu(long input, string expected) {
		SnafuNumber actual = input;
		Assert.Equal(expected.Trim(), actual);
	}
}
