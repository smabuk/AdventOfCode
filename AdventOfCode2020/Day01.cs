using System;
using System.Collections.Generic;
using System.IO;

using AdventOfCode;

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
