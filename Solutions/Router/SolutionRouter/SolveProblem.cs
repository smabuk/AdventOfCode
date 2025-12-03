namespace AdventOfCode.Solutions;
static public partial class SolutionRouter
{
	public static string SolveProblem(int year, int day, int problemNo, string[]? input, Action<string[], bool>? visualise = null, params object[]? args)
	{
		if (input is null) {
			return NO_INPUT_MESSAGE;
		}

		if (TryGetDayTypeInfo(year, day, out TypeInfo? dayTypeInfo) is false) {
			return NO_SOLUTION_MESSAGE;
		}

		MethodInfo[] methods = dayTypeInfo.GetMethods();

		_ = InvokePhase(PHASE_VISUALISER, input, args, methods, visualise);
		_ = InvokePhase(PHASE_INIT, input, args, methods, visualise);
		return InvokePhase($"{PHASE_PART}{problemNo}", input, args, methods, visualise).Answer;
	}

	public static string SolveProblem(int year, int day, int problemNo, string? input, params object[]? args)
		=> SolveProblem(year, day, problemNo, input?.ReplaceLineEndings().Split(Environment.NewLine), null, args);

	public static string SolveProblem(int year, int day, int problemNo, string[]? input, params object[]? args)
		=> SolveProblem(year, day, problemNo, input, null, args);

	public static string SolveProblem(int year, int day, int problemNo, string? input, Action<string[], bool>? visualise = null, params object[]? args)
		=> SolveProblem(year, day, problemNo, input?.ReplaceLineEndings().Split(Environment.NewLine), visualise, args);
}
