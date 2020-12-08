using System.Collections.Generic;
using System.Linq;

using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2020 {
	/// <summary>
	/// https://adventofcode.com/2020/day/3
	/// </summary>
	public class Tests_03
	{
		static readonly string[] _input =
		{
			"..##.......",
			"#...#...#..",
			".#....#..#.",
			"..#.#...#.#",
			".#...##..#.",
			"..#.##.....",
			".#.#.#....#",
			".#........#",
			"#.##...#...",
			"#...##....#",
			".#..#...#.#"
		};

		[Fact]
		public void Toboggan_Trajectory_Part1()
		{
			int right = 3;
			int down = 1;
			long actual = Day03.CalculateNoOfTrees(_input, right, down);
			Assert.Equal(7, actual);
		}

		[Theory]
		[InlineData(1, 1, 2)]
		[InlineData(3, 1, 7)]
		[InlineData(5, 1, 3)]
		[InlineData(7, 1, 4)]
		[InlineData(1, 2, 2)]
		public void Toboggan_Trajectory_Part2(int right, int down, int expected)
		{
			long actual = Day03.CalculateNoOfTrees(_input, right, down);
			Assert.Equal(expected, actual);
		}

	}
}
