using System.Collections.Generic;
using System.Linq;

using AdventOfCode2020.Shared;

using Xunit;

namespace AdventOfCode2020.Tests
{
	/// <summary>
	/// https://adventofcode.com/2020/day/1
	/// </summary>
	public class Day01Tests
	{
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
		public void Valid_FindSumsEqualTo(int sum, int noOfEntries)
		{
			bool successful = Day01.FindSumsEqualTo(sum, _input.ToList(), noOfEntries, out List<int> actual);
			Assert.True(successful);
			Assert.Equal(noOfEntries, actual.Count);
			Assert.Equal(sum, actual.Sum());
		}

		[Fact]
		public void Day01_01_Test()
		{
			List<int> actual = Day01.Find2SumsEqualTo2020(_input);
			Assert.Equal(2, actual.Count);
			Assert.Equal(2020, actual[0] + actual[1]);
		}

		[Fact]
		public void Day01_02_Test()
		{
			List<int> actual = Day01.Find3SumsEqualTo2020(_input);
			Assert.Equal(3, actual.Count);
			Assert.Equal(2020, actual[0] + actual[1] + actual[2]);
		}
	}
}
