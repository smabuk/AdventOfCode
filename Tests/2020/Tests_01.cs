namespace AdventOfCode.Tests.Year2020;

/// <summary>
/// https://adventofcode.com/2020/day/1
/// </summary>
public class Tests_01_Report_Repair {
	static readonly int[] _input =
		{
				1721,
				979,
				366,
				299,
				675,
				1456
			};

	[Theory]
	[InlineData(2020, 2)]
	[InlineData(2020, 3)]
	public void Valid_FindSumsEqualTo(int sum, int noOfEntries) {
		bool successful = Solutions.Year2020.Day01.FindSumsEqualTo(sum, _input.ToList(), noOfEntries, out List<int> actual);
		Assert.True(successful);
		Assert.Equal(noOfEntries, actual.Count);
		Assert.Equal(sum, actual.Sum());
	}

}
