namespace AdventOfCode.Solutions.Core;
public static class GlobalConstants
{
	public const string SOLUTIONS_NAMESPACE = "AdventOfCode.Solutions";

	public const int PART1 = 1;
	public const int PART2 = 2;

	public const string EXCEPTION = "Exception";
	public const string PHASE_SOLUTION = "Solution";
	public const string PHASE_INIT = "Init";
	public const string PHASE_PART = "Part";
	public const string PHASE_PART0 = "Part0";
	public const string PHASE_PART1 = "Part1";
	public const string PHASE_PART2 = "Part2";
	public static readonly string EXCEPTION_PART1 = $"{EXCEPTION}{PHASE_PART1}";
	public static readonly string EXCEPTION_PART2 = $"{EXCEPTION}{PHASE_PART2}";

	public static readonly string EXCEPTION_MESSAGE = $"** {EXCEPTION} **";
	public static readonly string NO_INPUT_MESSAGE = "* NO INPUT DATA *";
	public static readonly string NO_PARAMETERS_MESSAGE = "* NO PARAMETERS *";
	public static readonly string NO_SOLUTION_MESSAGE = "* No solution *";
	public static readonly string NO_SOLUTION_WRITTEN_MESSAGE = "** Solution not written yet **";
}
