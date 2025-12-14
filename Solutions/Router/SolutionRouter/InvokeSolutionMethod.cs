namespace AdventOfCode.Solutions;

public static partial class SolutionRouter {

	private static SolutionPhaseResult InvokeSolutionMethod(string phase, object input, object[]? args, MethodInfo method, Action<string[], bool>? visualise) {
		input = (input as string[]).StripTrailingBlankLineOrDefault();
		ParameterInfo[] parameters = method.GetParameters();
		int noOfParameters = parameters.Length;
		string parameterType = noOfParameters == 0 ? "NONE" : parameters[0].ParameterType.Name;
		object inputObject = parameterType switch {
			"String[]" => input,
			"String" => string.Join(Environment.NewLine, (string[])input),
			"NONE" => "",
			"Action`1" or "Action`2" or "Action`3" or "Action`4" when phase is PHASE_VISUALISER => null!,
			_ => throw new NotImplementedException(),
		};

		string answer;
		bool hasVisualiser = parameters.Where(p => p.Name == "visualise").Any();
		bool useVisualiser = visualise is not null;

		long startTime = Stopwatch.GetTimestamp();

		if (phase is PHASE_VISUALISER) {
			try {
				_ = method.Invoke(0, [visualise])?.ToString() ?? "";
			}
			catch (Exception ex) {
				return new(phase, EXCEPTION_MESSAGE, Stopwatch.GetElapsedTime(startTime), ex);
			}

			return new(phase, "", Stopwatch.GetElapsedTime(startTime));
		} else {
			try {
				answer = (noOfParameters, hasVisualiser, useVisualiser) switch
				{
					(0, _, _) => method.Invoke(0, [])?.ToString() ?? "",
					(1, false, _) => method.Invoke(0, [inputObject])?.ToString() ?? "",
					( > 1, false, _) => method.Invoke(0, [inputObject, args!])?.ToString() ?? "",
					(2, true, false) => method.Invoke(0, [inputObject, null!])?.ToString() ?? "",
					(2, true, true) => method.Invoke(0, [inputObject, visualise!])?.ToString() ?? "",
					( > 2, true, false) => method.Invoke(0, [inputObject, null!, args!])?.ToString() ?? "",
					( > 2, true, true) => method.Invoke(0, [inputObject, visualise!, args!])?.ToString() ?? "",
					_ => method.Invoke(0, [inputObject, args!])?.ToString() ?? "",
				};
			}
			catch (Exception ex) {
				return new(phase, EXCEPTION_MESSAGE, Stopwatch.GetElapsedTime(startTime), ex);
			}

			return new(phase, answer.Contains("written") ? NO_SOLUTION_MESSAGE : answer, Stopwatch.GetElapsedTime(startTime));
		}

	}

}
