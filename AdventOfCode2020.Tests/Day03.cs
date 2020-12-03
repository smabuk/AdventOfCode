using System.Collections.Generic;
using System.Linq;

using AdventOfCode2020.Shared;

using Xunit;

namespace AdventOfCode2020.Tests
{
	/// <summary>
	/// https://adventofcode.com/2020/day/2
	/// </summary>
	public class Day03Tests
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
		public void Valid_Toboggan_Trajectory_Part1()
		{
			int right = 3;
			int down = 1;
			long actual = Day03.CalculateNoOfTrees(_input, right, down);
			Assert.Equal(7, actual);
		}

		//[Fact]
		//public void Valid_CountOfValidPasswords_Part2()
		//{
		//	long actual = Day02.CountValidPasswords_Part2(_input);
		//	Assert.Equal(1, actual);
		//}

	}
}
