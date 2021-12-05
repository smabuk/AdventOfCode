namespace AdventOfCode.Solutions.Year2015;

/// <summary>
/// Day 09: All in a Single Night
/// https://adventofcode.com/2015/day/9
/// </summary>
[Description("All in a Single Night")]
public class Day09 {

	record Route(string Location1, string Location2, int Distance);

	private static int Solution1(string[] input) {
		List<Route> routes = input.Select(r => ParseLine(r)).ToList();

		List<string> locations = routes
			.Select(r => r.Location1).Union(routes.Select(r => r.Location2))
			.Distinct()
			.ToList();

		Dictionary<string, Route> allRoutes = routes
			.Union(routes
					.Select(route => route with { Location1 = route.Location2, Location2 = route.Location1 }))
			.ToDictionary(r => $"{r.Location1}->{r.Location2}");

		int shortestRoute = int.MaxValue;
		foreach (string[] legs in routes
			.Select(r => r.Location1).Union(routes.Select(r => r.Location2))
			.Distinct()
			.Permute()) {
			int routeDistance = 0;
			for (int i = 0; i < legs.Length - 1; i++) {
				routeDistance += allRoutes[$"{legs[i]}->{legs[i + 1]}"].Distance;
			}
			if (shortestRoute > routeDistance) {
				shortestRoute = routeDistance;
			}
		}

		return shortestRoute;
	}

	private static int Solution2(string[] input) {
		List<Route> routes = input.Select(r => ParseLine(r)).ToList();

		List<string> locations = routes
			.Select(r => r.Location1).Union(routes.Select(r => r.Location2))
			.Distinct()
			.ToList();

		Dictionary<string, Route> allRoutes = routes
			.Union(routes
					.Select(route => route with { Location1 = route.Location2, Location2 = route.Location1 }))
			.ToDictionary(r => $"{r.Location1}->{r.Location2}");

		int longestRoute = 0;
		foreach (string[] legs in routes
			.Select(r => r.Location1).Union(routes.Select(r => r.Location2))
			.Distinct()
			.Permute()) {
			int routeDistance = 0;
			for (int i = 0; i < legs.Length - 1; i++) {
				routeDistance += allRoutes[$"{legs[i]}->{legs[i + 1]}"].Distance;
			}
			if (longestRoute < routeDistance) {
				longestRoute = routeDistance;
			}
		}

		return longestRoute;
	}

	private static Route ParseLine(string input) {
		//MatchCollection match = Regex.Matches(input, @"(\w) to (\w) = ([\+\-]\d+)");
		Match match = Regex.Match(input, @"(\w+) to (\w+) = (\d+)");
		if (match.Success) {
			return new(match.Groups[1].Value, match.Groups[2].Value, int.Parse(match.Groups[3].Value));
		}
		return null!;
	}





	#region Problem initialisation
	/// <summary>
	/// Sets up the inputs for Part1 of the problem and calls Solution1
	/// </summary>
	/// <param name="input"></param>
	/// Array of strings
	/// <param name="args"></param>
	/// Optional extra parameters that may be required as input to the problem
	/// <returns></returns>
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		// int arg1 = GetArgument(args, 1, 25);
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	/// <summary>
	/// Sets up the inputs for Part2 of the problem and calls Solution2
	/// </summary>
	/// <param name="input"></param>
	/// Array of strings
	/// <param name="args"></param>
	/// Optional extra parameters that may be required as input to the problem
	/// <returns></returns>
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		// int arg1 = GetArgument(args, 1, 25);
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
