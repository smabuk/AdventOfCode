using static AdventOfCode.Solutions._2017.Day12Constants;
using static AdventOfCode.Solutions._2017.Day12Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 12: Digital Plumber
/// https://adventofcode.com/2016/day/12
/// </summary>
[Description("Digital Plumber")]
public sealed partial class Day12 {

	[Init]
	public static   void  Init(string[] input) => LoadPipes(input);
	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static List<Pipe> _pipes = [];

	private static void LoadPipes(string[] input) {
		foreach (string line in input) {
			int[] tokens = [..line.TrimmedSplit(['<', '-', '>', ',']).Select(id => id.As<int>())];
			int id1 = tokens[0];
			foreach (int id2 in tokens[1..]) {
				_pipes.Add(new(id1, id2));
				_pipes.Add(new(id2, id1));
			}
		}
		_pipes = [.._pipes.Distinct()];
	}

	private static int Solution1() => _pipes.PipesInGroup(0).Count;
	private static int Solution2() => _pipes.GroupsInPipes();
}

file static class Day12Extensions
{
	public static HashSet<int> PipesInGroup(this IEnumerable<Pipe> pipes, int id)
	{
		HashSet<int> group = [id];
		int added = 0;
		do {
			added = 0;
			foreach (var pipe in pipes
				.Where(p => group.Contains(p.Id1)
				&& group.DoesNotContain(p.Id2))) {

				if (group.Add(pipe.Id2)) {
					added++;
				}
			}
		} while (added != 0);
		return group;
	}

	public static int GroupsInPipes(this IEnumerable<Pipe> pipes)
	{
		int count = 0;
		HashSet<int> seen = [];

		foreach (int id in pipes.Select(p => p.Id1).Distinct()) {
			if (seen.DoesNotContain(id)) {
				seen.UnionWith(pipes.PipesInGroup(id));
				count++;
			}
		}

		return count;
	}
}

internal sealed partial class Day12Types
{
	public record Pipe(int Id1, int Id2);
}

file static class Day12Constants
{
}
