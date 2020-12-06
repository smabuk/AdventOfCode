using System.Collections.Generic;
using System.Linq;

using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2020 {
	/// <summary>
	/// https://adventofcode.com/2020/day/2
	/// </summary>
	public class Tests_02
	{
		static readonly string[] _input =
		{
			"1-3 a: abcde",
			"1-3 b: cdefg",
			"2-9 c: ccccccccc"
		};

		[Fact]
		public void Valid_CountOfValidPasswords_Part1()
		{
			long actual = Solution_2020_02.CountValidPasswords_Part1(_input);
			Assert.Equal(2, actual);
		}

		[Fact]
		public void Valid_CountOfValidPasswords_Part2()
		{
			long actual = Solution_2020_02.CountValidPasswords_Part2(_input);
			Assert.Equal(1, actual);
		}

	}
}
