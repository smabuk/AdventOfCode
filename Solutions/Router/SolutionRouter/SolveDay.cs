namespace AdventOfCode.Solutions;
static public partial class SolutionRouter
{
	public static IEnumerable<SolutionPhase> SolveDay(int year, int day, string[]? input, Action<string[], bool>? visualise = null, params object[]? args)
	{
		if (input is null) {
			yield return SolutionPhase.NoInput;
			yield break;
		}

		if (TryGetDayTypeInfo(year, day, out TypeInfo? dayTypeInfo) is false) {
			yield return SolutionPhase.NoSolution;
			yield break;
		}

		MethodInfo[] methods = dayTypeInfo.GetMethods();

		string[] phases = [PHASE_INIT, PHASE_PART1, PHASE_PART2];
		foreach (string phase in phases) {
			InvokeResult invokeResult = InvokePhase(phase, input, args, methods, visualise);
			if (invokeResult.Answer.Contains("written")) {
				yield return SolutionPhase.NoSolution with { Phase = phase };
			} else if (invokeResult.Exception is not null) {
				yield return SolutionPhase.ExceptionPart(phase) with { Answer = invokeResult.Answer, Exception = invokeResult.Exception, Elapsed = invokeResult.Elapsed };
			} else {
				yield return new SolutionPhase(phase) with { Answer = invokeResult.Answer, Elapsed = invokeResult.Elapsed };
			}
		}
	}

	public static IEnumerable<SolutionPhase> SolveDay(int year, int day, string? input, params object[]? args)
		=> SolveDay(year, day, input?.ReplaceLineEndings().Split(Environment.NewLine), null, args);

	public static IEnumerable<SolutionPhase> SolveDay(int year, int day, string? input, Action<string[], bool>? visualise = null, params object[]? args)
		=> SolveDay(year, day, input?.ReplaceLineEndings().Split(Environment.NewLine), visualise, args);

	public static IEnumerable<SolutionPhase> SolveDay(int year, int day, string[]? input, params object[]? args)
		=> SolveDay(year, day, input, null, args);
}
