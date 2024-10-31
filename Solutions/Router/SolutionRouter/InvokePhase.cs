namespace AdventOfCode.Solutions;
static public partial class SolutionRouter
{
	private static InvokeResult InvokePhase(string phase, string[] input, object[]? args, MethodInfo[] methods, Action<string[], bool>? visualise)
	{
		MethodInfo? method = (phase switch
		{
			PHASE_INIT => methods.Where(m => m.GetCustomAttributes().Any(attr => (attr.ToString() ?? "").EndsWith("InitAttribute"))),
			_ => methods.Where(m => m.Name == phase),
		}).SingleOrDefault();

		InvokeResult invokeResult = new(NO_SOLUTION_MESSAGE, TimeSpan.Zero);
		if (method is not null) {
			invokeResult = InvokeSolutionMethod(input, args, method, visualise);
		};
		return invokeResult;
	}
}
