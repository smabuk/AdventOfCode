namespace AdventOfCode.Solutions._2019;

/// <summary>
/// Day XX: Universal Orbit Map
/// https://adventofcode.com/2019/day/6
/// </summary>
[Description("Universal Orbit Map")]
public class Day06 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record OrbitRecord(string Name, string Orbits);

	private static string Solution1(string[] input) {
		(List<OrbitRecord> orbitMap, Dictionary<string, OrbitRecord> orbits) = ParseInputs(input);

		IEnumerable<string> objects = orbitMap.Select(o => o.Name)
			.Union(orbitMap.Select(o => o.Orbits).Where(o => o != "COM"))
			.Distinct();

		int count = 0;
		foreach (string objectName in objects) {
			string currObject = objectName;
			while (currObject != "COM") {
				currObject = orbits[currObject].Orbits;
				count++;
			}
		}

		return count.ToString();
	}

	private static string Solution2(string[] input) {
		(_, Dictionary<string, OrbitRecord> orbits) = ParseInputs(input);
		
		List<string> startList = [];
		List<string> finishList = [];

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


	private static (List<OrbitRecord> orbitMap, Dictionary<string, OrbitRecord> orbits) ParseInputs(string[] input) {
		List<OrbitRecord> orbitMap = input.Select(i => ParseLine(i)).ToList();
		Dictionary<string, OrbitRecord> orbits = orbitMap.ToDictionary(o => o.Name);
		return (orbitMap, orbits);
	}

	private static OrbitRecord ParseLine(string input) {
		Match match = Regex.Match(input, @"([A-Z\d]+)\)([A-Z\d]+)");
		if (match.Success) {
			return new(match.Groups[2].Value, match.Groups[1].Value);
		}
		return null!;
	}
}
