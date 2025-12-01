namespace AdventOfCode.Solutions;
static public partial class SolutionRouter
{
	public static IEnumerable<SolutionPhaseResult> SolveDay(int year, int day, string[]? input, int? problemNo = null, Action<string[], bool>? visualise = null, params object[]? args)
	{
		if (input is null) {
			yield return SolutionPhaseResult.NoInput;
			yield break;
		}

		if (TryGetDayTypeInfo(year, day, out TypeInfo? dayTypeInfo) is false) {
			yield return SolutionPhaseResult.NoSolution;
			yield break;
		}

		MethodInfo[] methods = dayTypeInfo.GetMethods();

		string[] phases = problemNo switch
		{
			1 => [PHASE_INIT, PHASE_PART1],
			2 => [PHASE_INIT, PHASE_PART2],
			_ => [PHASE_INIT, PHASE_PART1, PHASE_PART2],
		};

		foreach (string phase in phases) {
			yield return InvokePhase(phase, input, args, methods, visualise);
		}
	}

	public static IEnumerable<SolutionPhaseResult> SolveDay(int year, int day, string? input, params object[]? args)
		=> SolveDay(year, day, input?.ReplaceLineEndings().Split(Environment.NewLine), null, null, args);

	public static IEnumerable<SolutionPhaseResult> SolveDay(int year, int day, string? input, Action<string[], bool>? visualise = null, params object[]? args)
		=> SolveDay(year, day, input?.ReplaceLineEndings().Split(Environment.NewLine), null, visualise, args);

	public static IEnumerable<SolutionPhaseResult> SolveDay(int year, int day, string[]? input, params object[]? args)
		=> SolveDay(year, day, input, null, null, args);
}
