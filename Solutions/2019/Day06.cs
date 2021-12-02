using Microsoft.VisualBasic;

namespace AdventOfCode.Solutions.Year2019;

/// <summary>
/// Day XX: Universal Orbit Map
/// https://adventofcode.com/2019/day/6
/// </summary>
public class Day06 {

	record OrbitRecord(string Name, string Orbits) {
		public int DirectCount { get; set; } = 0;
		public int IndirectCount { get; set; } = 0;
	};

	private static string Solution1(string[] input) {
		List<OrbitRecord> orbitMap = input.Select(i => ParseLine(i)).ToList();
		Dictionary<string, OrbitRecord> orbits = orbitMap.ToDictionary(o => o.Name);

		IEnumerable<string> objects = orbitMap.Select(o => o.Name)
			.Union(orbitMap.Select(o => o.Orbits).Where(o => o != "COM"))
			.Distinct();

		int count = 0;
		foreach (string objectName  in objects) {
			string currObject = objectName;
			while (currObject != "COM") {
				currObject = orbits[currObject].Orbits;
				count++;
			}
		}

		return count.ToString();
	}

	private static string Solution2(string[] input) {
		List<OrbitRecord> orbitMap = input.Select(i => ParseLine(i)).ToList();
		Dictionary<string, OrbitRecord> orbits = orbitMap.ToDictionary(o => o.Name);
		List<string> startList = new();
		List<string> finishList = new();

		string currObject = "YOU";
		while (currObject != "COM") {
			currObject = orbits[currObject].Orbits;
			startList.Add(currObject);
		}

		currObject = "SAN";
		while (currObject != "COM") {
			currObject = orbits[currObject].Orbits;
			finishList.Add(currObject);
		}

		string sharedNode = startList.Intersect(finishList).FirstOrDefault() ?? "COM";
		int count = startList.IndexOf(sharedNode) + finishList.IndexOf(sharedNode);

		return count.ToString();
	}

	private static OrbitRecord ParseLine(string input) {
		Match match = Regex.Match(input, @"([A-Z\d]+)\)([A-Z\d]+)");
		if (match.Success) {
			return new(match.Groups[2].Value, match.Groups[1].Value);
		}
		return null!;
	}





	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
