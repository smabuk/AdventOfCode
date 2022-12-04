namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 04: Camp Cleanup
/// https://adventofcode.com/2022/day/4
/// </summary>
[Description("Camp Cleanup")]
public sealed partial class Day04 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record ElfPair(int Elf1Start, int Elf1End, int Elf2Start, int Elf2End);

	private static int Solution1(string[] input) {
		List<ElfPair> elfPairs = input.Select(i => ParseLine(i)).ToList();
		int count = 0;
		foreach (ElfPair elfPair in elfPairs) {
			if (elfPair.Elf1Start >= elfPair.Elf2Start && elfPair.Elf1End <= elfPair.Elf2End
				|| elfPair.Elf2Start >= elfPair.Elf1Start && elfPair.Elf2End <= elfPair.Elf1End) {
				count++;
			}
		}
		return count;
	}

	private static int Solution2(string[] input) {
		List<ElfPair> elfPairs = input.Select(i => ParseLine(i)).ToList();
		int count = 0;
		foreach (ElfPair elfPair in elfPairs) {
			if (elfPair.Elf1Start >= elfPair.Elf2Start && elfPair.Elf1Start <= elfPair.Elf2End
				|| elfPair.Elf1End >= elfPair.Elf2Start && elfPair.Elf1End <= elfPair.Elf2End
				|| elfPair.Elf2Start >= elfPair.Elf1Start && elfPair.Elf2Start <= elfPair.Elf1End
				|| elfPair.Elf2End >= elfPair.Elf1Start && elfPair.Elf2End <= elfPair.Elf1End) {
				count++;
			}
		}
		return count;
	}

	private static ElfPair ParseLine(string input) {
		Match match = InputRegEx().Match(input);
		if (match.Success) {
			return new(int.Parse(match.Groups["Elf1Start"].Value), int.Parse(match.Groups["Elf1End"].Value)
					 , int.Parse(match.Groups["Elf2Start"].Value), int.Parse(match.Groups["Elf2End"].Value));
		}
		return null!;
	}

	[GeneratedRegex("""(?<Elf1Start>\d+)-(?<Elf1End>\d+),(?<Elf2Start>\d+)-(?<Elf2End>\d+)""")]
	private static partial Regex InputRegEx();
}
