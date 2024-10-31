namespace AdventOfCode.Solutions;

static public partial class SolutionRouter {

	private static SolutionPhaseResult InvokeSolutionMethod(string phase, object input, object[]? args, MethodInfo method, Action<string[], bool>? visualise) {
		input = (input as string[]).StripTrailingBlankLineOrDefault();
		ParameterInfo[] parameters = method.GetParameters();
		int noOfParameters = parameters.Length;
		string parameterType = parameters[0].ParameterType.Name;
		object inputObject = parameterType switch {
			"String[]" => input,
			"String" => string.Join(Environment.NewLine, (string[])input),
			_ => throw new NotImplementedException(),
		};
		
		string answer;
		bool hasVisualiser = parameters.Where(p => p.Name == "visualise").Any();
		bool useVisualiser = visualise is not null;

		long startTime = Stopwatch.GetTimestamp();
		try {
			answer = (noOfParameters, hasVisualiser, useVisualiser) switch
			{
				(0, _, _)           => NO_PARAMETERS_MESSAGE,
				(1, false, _)       => method.Invoke(0, [inputObject])?.ToString() ?? "",
				( > 1, false, _)    => method.Invoke(0, [inputObject, args!])?.ToString() ?? "",
				(2, true, false)    => method.Invoke(0, [inputObject, null!])?.ToString() ?? "",
				(2, true, true)     => method.Invoke(0, [inputObject, visualise!])?.ToString() ?? "",
				( > 2, true, false) => method.Invoke(0, [inputObject, null!, args!])?.ToString() ?? "",
				( > 2, true, true)  => method.Invoke(0, [inputObject, visualise!, args!])?.ToString() ?? "",
				_                   => method.Invoke(0, [inputObject, args!])?.ToString() ?? "",
			};
		}
		catch (Exception ex) {
			return new(phase, EXCEPTION_MESSAGE, Stopwatch.GetElapsedTime(startTime), ex);
		}

		return new(phase, answer.Contains("written") ? NO_SOLUTION_MESSAGE : answer, Stopwatch.GetElapsedTime(startTime));
	}

}
