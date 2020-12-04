namespace AdventOfCode.Shared {
	static public class SolutionRouter {
		public static string SolveProblem(int year, int day, int problemNo) {
			return year switch {
				2020 => SolveProblem2020(day, problemNo),
				_ => ""
			};
		}

		public static string SolveProblem2020(int day, int problemNo) {
			return (day, problemNo) switch {
				(1, 1) => Solution_2020_01.Part1().ToString(),
				(1, 2) => Solution_2020_01.Part2().ToString(),
				(2, 1) => Solution_2020_02.Part1().ToString(),
				(2, 2) => Solution_2020_02.Part2().ToString(),
				(3, 1) => Solution_2020_03.Part1().ToString(),
				(3, 2) => Solution_2020_03.Part2().ToString(),
				_ => ""
			};
		}
	}
}
