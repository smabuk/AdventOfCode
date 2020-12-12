namespace AdventOfCode.Solutions {
	static public class SolutionRouter {
		private const string NO_SOLUTION = "** Solution not written yet **";
		private const string NO_INPUT = "** NO INPUT DATA **";

		public static string SolveProblem(int year, int day, int problemNo, string[]? input, params object[]? args) {

			if (input is null) {
				return NO_INPUT;
			}

			return year switch {
				2020 => SolveProblem2020(day, problemNo, input, args),
				//2019 => SolveProblem2019(day, problemNo, input),
				//2018 => SolveProblem2018(day, problemNo, input),
				//2017 => SolveProblem2017(day, problemNo, input),
				//2016 => SolveProblem2016(day, problemNo, input),
				2015 => SolveProblem2015(day, problemNo, input, args),
				_ => NO_SOLUTION
			};
		}
		
		public static string SolveProblem2020(int day, int problemNo, string[]? input = null, params object[]? args) {
			return (day, problemNo) switch {
				(1, 1) => Year2020.Day01.Part1(input).ToString(),
				(1, 2) => Year2020.Day01.Part2(input).ToString(),
				(2, 1) => Year2020.Day02.Part1(input).ToString(),
				(2, 2) => Year2020.Day02.Part2(input).ToString(),
				(3, 1) => Year2020.Day03.Part1(input).ToString(),
				(3, 2) => Year2020.Day03.Part2(input).ToString(),
				(4, 1) => Year2020.Day04.Part1(input).ToString(),
				(4, 2) => Year2020.Day04.Part2(input).ToString(),
				(5, 1) => Year2020.Day05.Part1(input).ToString(),
				(5, 2) => Year2020.Day05.Part2(input).ToString(),
				(6, 1) => Year2020.Day06.Part1(input).ToString(),
				(6, 2) => Year2020.Day06.Part2(input).ToString(),
				(7, 1) => Year2020.Day07.Part1(input).ToString(),
				(7, 2) => Year2020.Day07.Part2(input).ToString(),
				(8, 1) => new Year2020.Day08().Part1(input).ToString(),
				(8, 2) => new Year2020.Day08().Part2(input).ToString(),
				(9, 1) => Year2020.Day09.Part1(input, args).ToString(),
				(9, 2) => Year2020.Day09.Part2(input, args).ToString(),
				(10, 1) => Year2020.Day10.Part1(input).ToString(),
				(10, 2) => Year2020.Day10.Part2(input).ToString(),
				(11, 1) => Year2020.Day11.Part1(input).ToString(),
				(11, 2) => Year2020.Day11.Part2(input).ToString(),
				(12, 1) => Year2020.Day12.Part1(input).ToString(),
				(12, 2) => Year2020.Day12.Part2(input).ToString(),
				/*
				(13, 1) => Year2020.Day13.Part1(input).ToString(),
				(13, 2) => Year2020.Day13.Part2(input).ToString(),
				(14, 1) => Year2020.Day14.Part1(input).ToString(),
				(14, 2) => Year2020.Day14.Part2(input).ToString(),
				(15, 1) => Year2020.Day15.Part1(input).ToString(),
				(15, 2) => Year2020.Day15.Part2(input).ToString(),
				(16, 1) => Year2020.Day16.Part1(input).ToString(),
				(16, 2) => Year2020.Day16.Part2(input).ToString(),
				(17, 1) => Year2020.Day17.Part1(input).ToString(),
				(17, 2) => Year2020.Day17.Part2(input).ToString(),
				(18, 1) => Year2020.Day18.Part1(input).ToString(),
				(18, 2) => Year2020.Day18.Part2(input).ToString(),
				(19, 1) => Year2020.Day19.Part1(input).ToString(),
				(19, 2) => Year2020.Day19.Part2(input).ToString(),
				(20, 1) => Year2020.Day20.Part1(input).ToString(),
				(20, 2) => Year2020.Day20.Part2(input).ToString(),
				(21, 1) => Year2020.Day21.Part1(input).ToString(),
				(21, 2) => Year2020.Day21.Part2(input).ToString(),
				(22, 1) => Year2020.Day22.Part1(input).ToString(),
				(22, 2) => Year2020.Day22.Part2(input).ToString(),
				(23, 1) => Year2020.Day23.Part1(input).ToString(),
				(23, 2) => Year2020.Day23.Part2(input).ToString(),
				(24, 1) => Year2020.Day24.Part1(input).ToString(),
				(24, 2) => Year2020.Day24.Part2(input).ToString(),
				(25, 1) => Year2020.Day25.Part1(input).ToString(),
				(25, 2) => Year2020.Day25.Part2(input).ToString(),
				*/
				_ => NO_SOLUTION
			};
		}


		public static string SolveProblem2015(int day, int problemNo, string[]? input = null, params object[]? args) {
			return (day, problemNo) switch {
				(1, 1) => Year2015.Day01.Part1(input).ToString(),
				(1, 2) => Year2015.Day01.Part2(input).ToString(),
				(2, 1) => Year2015.Day02.Part1(input).ToString(),
				(2, 2) => Year2015.Day02.Part2(input).ToString(),
				(3, 1) => Year2015.Day03.Part1(input).ToString(),
				(3, 2) => Year2015.Day03.Part2(input).ToString(),
				(4, 1) => Year2015.Day04.Part1(input).ToString(),
				(4, 2) => Year2015.Day04.Part2(input).ToString(),
				(5, 1) => Year2015.Day05.Part1(input).ToString(),
				(5, 2) => Year2015.Day05.Part2(input).ToString(),
				(6, 1) => Year2015.Day06.Part1(input).ToString(),
				(6, 2) => Year2015.Day06.Part2(input).ToString(),
				(7, 1) => Year2015.Day07.Part1(input, args).ToString(),
				(7, 2) => Year2015.Day07.Part2(input, args).ToString(),
				(8, 1) => Year2015.Day08.Part1(input).ToString(),
				(8, 2) => Year2015.Day08.Part2(input).ToString(),
				(9, 1)  => Year2015.Day09.Part1(input).ToString(),
				(9, 2)  => Year2015.Day09.Part2(input).ToString(),
				(10, 1) => Year2015.Day10.Part1(input).ToString(),
				(10, 2) => Year2015.Day10.Part2(input).ToString(),
				(11, 1) => Year2015.Day11.Part1(input).ToString(),
				(11, 2) => Year2015.Day11.Part2(input).ToString(),
				(12, 1) => Year2015.Day12.Part1(input).ToString(),
				(12, 2) => Year2015.Day12.Part2(input).ToString(),
				(13, 1) => Year2015.Day13.Part1(input).ToString(),
				(13, 2) => Year2015.Day13.Part2(input).ToString(),
				(14, 1) => Year2015.Day14.Part1(input, args).ToString(),
				(14, 2) => Year2015.Day14.Part2(input, args).ToString(),
				(15, 1) => Year2015.Day15.Part1(input).ToString(),
				(15, 2) => Year2015.Day15.Part2(input).ToString(),
				/*
				(16, 1) => Year2015.Day16.Part1(input).ToString(),
				(16, 2) => Year2015.Day16.Part2(input).ToString(),
				(17, 1) => Year2015.Day17.Part1(input).ToString(),
				(17, 2) => Year2015.Day17.Part2(input).ToString(),
				(18, 1) => Year2015.Day18.Part1(input).ToString(),
				(18, 2) => Year2015.Day18.Part2(input).ToString(),
				(19, 1) => Year2015.Day19.Part1(input).ToString(),
				(19, 2) => Year2015.Day19.Part2(input).ToString(),
				(20, 1) => Year2015.Day20.Part1(input).ToString(),
				(20, 2) => Year2015.Day20.Part2(input).ToString(),
				(21, 1) => Year2015.Day21.Part1(input).ToString(),
				(21, 2) => Year2015.Day21.Part2(input).ToString(),
				(22, 1) => Year2015.Day22.Part1(input).ToString(),
				(22, 2) => Year2015.Day22.Part2(input).ToString(),
				(23, 1) => Year2015.Day23.Part1(input).ToString(),
				(23, 2) => Year2015.Day23.Part2(input).ToString(),
				(24, 1) => Year2015.Day24.Part1(input).ToString(),
				(24, 2) => Year2015.Day24.Part2(input).ToString(),
				(25, 1) => Year2015.Day25.Part1(input).ToString(),
				(25, 2) => Year2015.Day25.Part2(input).ToString(),
				*/
				_ => NO_SOLUTION
			};
		}
	}
}
