namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 22: Reactor Reboot
/// https://adventofcode.com/2021/day/22
/// </summary>
[Description("Reactor Reboot")]
public class Day22 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record RebootStep(bool TurnOn, int xMin, int xMax, int yMin, int yMax, int zMin, int zMax);

	private static int Solution1(string[] input) {
		List<RebootStep> rebootSteps = input.Select(i => ParseLine(i)).ToList();

		HashSet<Point3d> cubes = new();

		foreach (var rebootStep in rebootSteps) {
			if (OutOfBounds(rebootStep.xMin, rebootStep.yMin, rebootStep.zMin)
				|| OutOfBounds(rebootStep.xMax, rebootStep.yMax, rebootStep.zMax)) {
				continue;
			}
			for (int z = rebootStep.zMin; z <= rebootStep.zMax; z++) {
				for (int y = rebootStep.yMin; y <= rebootStep.yMax; y++) {
					for (int x = rebootStep.xMin; x <= rebootStep.xMax; x++) {
						if (rebootStep.TurnOn) {
							cubes.Add(new Point3d(x, y, z));
						} else {
							cubes.Remove(new Point3d(x, y, z));
						}
					}
				}
			}
		}

		return cubes.Count;

		static bool OutOfBounds(int x, int y, int z)
			=> x < -50 || x > 50 || y < -50 || y > 50 || z < -50 || z > 50;
	}

	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		List<RebootStep> instructions = input.Select(i => ParseLine(i)).ToList();
		return "** Solution not written yet **";
	}

	private static RebootStep ParseLine(string input) {
		MatchCollection matches = Regex.Matches(input, @"([\+\-]*\d+)");
		if (matches.Count == 6) {
			return new(input.StartsWith("on")
				, int.Parse(matches[0].Groups[0].ValueSpan)
				, int.Parse(matches[1].Groups[0].ValueSpan)
				, int.Parse(matches[2].Groups[0].ValueSpan)
				, int.Parse(matches[3].Groups[0].ValueSpan)
				, int.Parse(matches[4].Groups[0].ValueSpan)
				, int.Parse(matches[5].Groups[0].ValueSpan)
				);
		}
		return null!;
	}
}
