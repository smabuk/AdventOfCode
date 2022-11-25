namespace AdventOfCode.Solutions._2015;

/// <summary>
/// Day 09: All in a Single Night
/// https://adventofcode.com/2015/day/9
/// </summary>
[Description("All in a Single Night")]
public class Day09 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

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
}
