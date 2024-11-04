using static AdventOfCode.Solutions._2016.Day19Constants;
using static AdventOfCode.Solutions._2016.Day19Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 19: An Elephant Named Joseph
/// https://adventofcode.com/2016/day/19
/// </summary>
[Description("An Elephant Named Joseph")]
public sealed partial class Day19 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		Queue<Elf> circle = 
			new(Enumerable.Range(1, input[0].As<int>()).Select(i => new Elf(i, 1)));
		
		while (circle.Count > 1) {
			Elf thief = circle.Dequeue();
			Elf victim = circle.Dequeue();
			circle.Enqueue(thief with { NoOfPresents = thief.NoOfPresents + victim.NoOfPresents });
		};
		
		return circle.Peek().Id;
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day19Extensions
{
}

internal sealed partial class Day19Types
{
	public sealed record Elf(int Id, int NoOfPresents);
}

file static class Day19Constants
{
}
