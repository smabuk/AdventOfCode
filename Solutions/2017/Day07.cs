using static AdventOfCode.Solutions._2017.Day07Constants;
using static AdventOfCode.Solutions._2017.Day07Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 07: Recursive Circus
/// https://adventofcode.com/2016/day/07
/// </summary>
[Description("Recursive Circus")]
public sealed partial class Day07 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static string Solution1(string[] input) {
		List<Program> programs = [.. input.As<Program>()];
		HashSet<string> subprograms =
			[..programs.SelectMany(program => program.ProgramNames)];
		return programs.Single(program => subprograms.DoesNotContain(program.Name)).Name;
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day07Extensions
{
}

internal sealed partial class Day07Types
{

	public sealed record Program(string Name, int Weight, List<string> ProgramNames) : IParsable<Program>
	{
		public static Program Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit([',', ' ', '-', '>', '(', ')']);
			List<string> programNames = tokens.Length > 2 ? [.. tokens[2..]] : [];
			return new(tokens[0], tokens[1].As<int>(), programNames);
		}

		public static Program Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Program result)
			=> ISimpleParsable<Program>.TryParse(s, provider, out result);
	}
}

file static class Day07Constants
{
}
