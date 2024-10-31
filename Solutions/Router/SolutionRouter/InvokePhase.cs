namespace AdventOfCode.Solutions;
static public partial class SolutionRouter
{
	private static SolutionPhaseResult InvokePhase(string phase, string[] input, object[]? args, MethodInfo[] methods, Action<string[], bool>? visualise)
	{
		MethodInfo? method = (phase switch
		{
			PHASE_INIT => methods.Where(m => m.GetCustomAttributes().Any(attr => (attr.ToString() ?? "").EndsWith("InitAttribute"))),
			_ => methods.Where(m => m.Name == phase),
		}).SingleOrDefault();

		return method is null 
			? new(phase, NO_SOLUTION_MESSAGE, TimeSpan.Zero)
			: InvokeSolutionMethod(phase, input, args, method, visualise);
	}
}
