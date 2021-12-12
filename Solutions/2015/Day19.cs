namespace AdventOfCode.Solutions.Year2015;

/// <summary>
/// Day 19: Medicine for Rudolph
/// https://adventofcode.com/2015/day/19
/// </summary>
[Description("Medicine for Rudolph")]
public class Day19 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) {
		bool testing = GetArgument(args, 1, false);
		if (testing is false) { return "** Solution not written yet **"; }
		return Solution2(input).ToString();
	}

	record Replacement(string From, string To);

	private static int Solution1(string[] input) {
		List<Replacement> replacements = input[..^2].Select(i => ParseLine(i)).ToList();
		string startingMolecule = input[^1];

		List<string> molecules = new();

		foreach (var r in replacements) {
			int i = 0;
			do {
				i = startingMolecule.IndexOf(r.From, i);
				if (i >= 0) {
					molecules.Add($"{startingMolecule[..i]}{r.To}{startingMolecule[(i + r.From.Length)..]}");
					i++;
				}
			} while (i >= 0);
		}

		return molecules.Distinct().Count();
	}

	private static int Solution2(string[] input) {
		var replacements = input[..^2].Select(i => ParseLine(i)).ToArray();
		string targetMolecule = input[^1];
		List<string> foundMolecules = new();
		string finishingMolecule = "e";

		List<string> molecules = new() { targetMolecule };
		int steps = 0;
		bool foundMolecule = false;
		do {
			List<string> newMolecules = molecules.Distinct().ToList();
			molecules = new();
			steps++;
			foreach (string startingMolecule in newMolecules) {
				foreach (var r in replacements) {
					int i = 0;
					do {
						i = startingMolecule.IndexOf(r.To, i);
						if (i >= 0) {
							string molecule = $"{startingMolecule[..i]}{r.From}{startingMolecule[(i + r.To.Length)..]}";
							if (molecule == finishingMolecule) {
								foundMolecule = true;
								break;
							}
							if (!foundMolecules.Contains(molecule)) {
								foundMolecules.Add(molecule);
								molecules.Add(molecule);
							}
							i++;
						}
					} while (i >= 0);
					if (foundMolecule == true) {
						break;
					}
				}
				if (foundMolecule == true) {
					break;
				}
			}
		} while (foundMolecule == false);

		return steps;
	}

	private static Replacement ParseLine(string input) {
		Match match = Regex.Match(input, @"(?<from>\w+) => (?<to>\w+)");
		if (match.Success) {
			return new(match.Groups["from"].Value, match.Groups["to"].Value);
		}
		return null!;
	}
}
