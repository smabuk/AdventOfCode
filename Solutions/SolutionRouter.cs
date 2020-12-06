using System.IO;

namespace AdventOfCode.Shared {
	static public class SolutionRouter {
		private const string NO_SOLUTION = "** Solution not written yet **";
		private const string NO_INPUT = "** NO INPUT DATA **";

		public static string SolveProblem(int year, int day, int problemNo, string[] input) {

			if (input is null) {
				return NO_INPUT;
			}

			return year switch {
				2020 => SolveProblem2020(day, problemNo, input),
				//2019 => SolveProblem2019(day, problemNo, input),
				//2018 => SolveProblem2018(day, problemNo, input),
				//2017 => SolveProblem2017(day, problemNo, input),
				//2016 => SolveProblem2016(day, problemNo, input),
				2015 => SolveProblem2015(day, problemNo, input),
				_ => NO_SOLUTION
			};
		}

		public static string SolveProblem2020(int day, int problemNo, string[]? input = null) {
			return (day, problemNo) switch {
				(1, 1) => Solution_2020_01.Part1(input).ToString(),
				(1, 2) => Solution_2020_01.Part2(input).ToString(),

				(2, 1) => Solution_2020_02.Part1(input).ToString(),
				(2, 2) => Solution_2020_02.Part2(input).ToString(),

				(3, 1) => Solution_2020_03.Part1(input).ToString(),
				(3, 2) => Solution_2020_03.Part2(input).ToString(),

				(4, 1) => Solution_2020_04.Part1(input).ToString(),
				(4, 2) => Solution_2020_04.Part2(input).ToString(),

				(5, 1) => Solution_2020_05.Part1(input).ToString(),
				(5, 2) => Solution_2020_05.Part2(input).ToString(),

				(6, 1) => Solution_2020_06.Part1(input).ToString(),
				(6, 2) => Solution_2020_06.Part2(input).ToString(),

				/*
				(7, 1) => Solution_2020_07.Part1(input).ToString(),
				(7, 2) => Solution_2020_07.Part2(input).ToString(),

				(8, 1) => Solution_2020_08.Part1(input).ToString(),
				(8, 2) => Solution_2020_08.Part2(input).ToString(),

				(9, 1) => Solution_2020_09.Part1(input).ToString(),
				(9, 2) => Solution_2020_09.Part2(input).ToString(),

				(10, 1) => Solution_2020_10.Part1(input).ToString(),
				(10, 2) => Solution_2020_10.Part2(input).ToString(),

				(11, 1) => Solution_2020_11.Part1(input).ToString(),
				(11, 2) => Solution_2020_11.Part2(input).ToString(),

				(12, 1) => Solution_2020_12.Part1(input).ToString(),
				(12, 2) => Solution_2020_12.Part2(input).ToString(),

				(13, 1) => Solution_2020_13.Part1(input).ToString(),
				(13, 2) => Solution_2020_13.Part2(input).ToString(),

				(14, 1) => Solution_2020_14.Part1(input).ToString(),
				(14, 2) => Solution_2020_14.Part2(input).ToString(),

				(15, 1) => Solution_2020_15.Part1(input).ToString(),
				(15, 2) => Solution_2020_15.Part2(input).ToString(),

				(16, 1) => Solution_2020_16.Part1(input).ToString(),
				(16, 2) => Solution_2020_16.Part2(input).ToString(),

				(17, 1) => Solution_2020_17.Part1(input).ToString(),
				(17, 2) => Solution_2020_17.Part2(input).ToString(),

				(18, 1) => Solution_2020_18.Part1(input).ToString(),
				(18, 2) => Solution_2020_18.Part2(input).ToString(),

				(19, 1) => Solution_2020_19.Part1(input).ToString(),
				(19, 2) => Solution_2020_19.Part2(input).ToString(),

				(20, 1) => Solution_2020_20.Part1(input).ToString(),
				(20, 2) => Solution_2020_20.Part2(input).ToString(),

				(21, 1) => Solution_2020_21.Part1(input).ToString(),
				(21, 2) => Solution_2020_21.Part2(input).ToString(),

				(22, 1) => Solution_2020_22.Part1(input).ToString(),
				(22, 2) => Solution_2020_22.Part2(input).ToString(),

				(23, 1) => Solution_2020_23.Part1(input).ToString(),
				(23, 2) => Solution_2020_23.Part2(input).ToString(),

				(24, 1) => Solution_2020_24.Part1(input).ToString(),
				(24, 2) => Solution_2020_24.Part2(input).ToString(),

				(25, 1) => Solution_2020_25.Part1(input).ToString(),
				(25, 2) => Solution_2020_25.Part2(input).ToString(),
				*/
				_ => NO_SOLUTION
			};
		}


		public static string SolveProblem2015(int day, int problemNo, string[]? input = null) {
			return (day, problemNo) switch {
				(1, 1) => Solution_2015_01.Part1(input).ToString(),
				(1, 2) => Solution_2015_01.Part2(input).ToString(),

				(2, 1) => Solution_2015_02.Part1(input).ToString(),
				(2, 2) => Solution_2015_02.Part2(input).ToString(),

				/*
				(3, 1) => Solution_2015_03.Part1(input).ToString(),
				(3, 2) => Solution_2015_03.Part2(input).ToString(),

				(4, 1) => Solution_2015_04.Part1(input).ToString(),
				(4, 2) => Solution_2015_04.Part2(input).ToString(),

				(5, 1) => Solution_2015_05.Part1(input).ToString(),
				(5, 2) => Solution_2015_05.Part2(input).ToString(),

				(6, 1) => Solution_2015_06.Part1(input).ToString(),
				(6, 2) => Solution_2015_06.Part2(input).ToString(),

				(7, 1) => Solution_2015_07.Part1(input).ToString(),
				(7, 2) => Solution_2015_07.Part2(input).ToString(),

				(8, 1) => Solution_2015_08.Part1(input).ToString(),
				(8, 2) => Solution_2015_08.Part2(input).ToString(),

				(9, 1) => Solution_2015_09.Part1(input).ToString(),
				(9, 2) => Solution_2015_09.Part2(input).ToString(),

				(10, 1) => Solution_2015_10.Part1(input).ToString(),
				(10, 2) => Solution_2015_10.Part2(input).ToString(),

				(11, 1) => Solution_2015_11.Part1(input).ToString(),
				(11, 2) => Solution_2015_11.Part2(input).ToString(),

				(12, 1) => Solution_2015_12.Part1(input).ToString(),
				(12, 2) => Solution_2015_12.Part2(input).ToString(),

				(13, 1) => Solution_2015_13.Part1(input).ToString(),
				(13, 2) => Solution_2015_13.Part2(input).ToString(),

				(14, 1) => Solution_2015_14.Part1(input).ToString(),
				(14, 2) => Solution_2015_14.Part2(input).ToString(),

				(15, 1) => Solution_2015_15.Part1(input).ToString(),
				(15, 2) => Solution_2015_15.Part2(input).ToString(),

				(16, 1) => Solution_2015_16.Part1(input).ToString(),
				(16, 2) => Solution_2015_16.Part2(input).ToString(),

				(17, 1) => Solution_2015_17.Part1(input).ToString(),
				(17, 2) => Solution_2015_17.Part2(input).ToString(),

				(18, 1) => Solution_2015_18.Part1(input).ToString(),
				(18, 2) => Solution_2015_18.Part2(input).ToString(),

				(19, 1) => Solution_2015_19.Part1(input).ToString(),
				(19, 2) => Solution_2015_19.Part2(input).ToString(),

				(20, 1) => Solution_2015_20.Part1(input).ToString(),
				(20, 2) => Solution_2015_20.Part2(input).ToString(),

				(21, 1) => Solution_2015_21.Part1(input).ToString(),
				(21, 2) => Solution_2015_21.Part2(input).ToString(),

				(22, 1) => Solution_2015_22.Part1(input).ToString(),
				(22, 2) => Solution_2015_22.Part2(input).ToString(),

				(23, 1) => Solution_2015_23.Part1(input).ToString(),
				(23, 2) => Solution_2015_23.Part2(input).ToString(),

				(24, 1) => Solution_2015_24.Part1(input).ToString(),
				(24, 2) => Solution_2015_24.Part2(input).ToString(),

				(25, 1) => Solution_2015_25.Part1(input).ToString(),
				(25, 2) => Solution_2015_25.Part2(input).ToString(),
				*/
				_ => NO_SOLUTION
			};
		}
	}
}
