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

	private static string Solution1(string[] input) => input.As<Program>().FindTowerBase().Name;

	private static int Solution2(string[] input) {
		List<Program> programs = [.. input.As<Program>()];
		Program currentBase = programs.FindTowerBase();

		Program prevBase = currentBase;
		int newWeight = int.MaxValue;
		int prevWeight = 0;
		
		while (newWeight != 0) {
			newWeight = 
				currentBase
				.ProgramNames
				.CountBy(pn => programs.GetProgram(pn).TotalWeight(programs))
				.Where(p => p.Value == 1).SingleOrDefault().Key;
			if (newWeight != 0) {
				prevBase = currentBase;
				prevWeight = newWeight;
				currentBase = programs.Single(p => p.TotalWeight(programs) == newWeight);
			}
		}

		int shouldBeWeight = programs
			.GetProgram(prevBase.ProgramNames.First(pn => pn != currentBase.Name))
			.TotalWeight(programs);

		return currentBase.Weight - (prevWeight - shouldBeWeight);
	}
}

file static class Day07Extensions
{

	public static Program FindTowerBase(this IEnumerable<Program> programs)
	{
		HashSet<string> subprograms =[.. programs.SelectMany(program => program.ProgramNames)];
		return programs.Single(program => subprograms.DoesNotContain(program.Name));
	}

	public static Program GetProgram(this List<Program> programs, string programName) =>
		programs.Single(p => p.Name == programName);

	public static int TotalWeight(this Program program, List<Program> programs)
	{
		return program.Weight
			+ programs.Where(p => program.ProgramNames.Contains(p.Name)).Sum(p => p.TotalWeight(programs));
	}
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
