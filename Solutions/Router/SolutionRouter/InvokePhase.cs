namespace AdventOfCode.Solutions;
public static partial class SolutionRouter
{
	private static SolutionPhaseResult InvokePhase(string phase, string[] input, object[]? args, MethodInfo[] methods, Action<string[], bool>? visualise)
	{
		MethodInfo? method = FindPhaseMethod(phase, methods).SingleOrDefault();

		return method is null
			? new(phase, NO_SOLUTION_MESSAGE, TimeSpan.Zero)
			: InvokeSolutionMethod(phase, input, args, method, visualise);
	}

	private static IEnumerable<MethodInfo> FindPhaseMethod(string phase, MethodInfo[] methods)
	{
		List<MethodInfo> methodByAttribute = [.. methods
			.Where(m => m.GetCustomAttributes()
						.Any(attr => (attr.ToString() ?? "")
						.EndsWith($"{phase}Attribute")))];
		return methodByAttribute is [] ? methods.Where(m => m.Name == phase) : methodByAttribute;
	}
}
