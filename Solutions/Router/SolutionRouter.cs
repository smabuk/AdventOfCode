using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode.Solutions;

static public class SolutionRouter {
	private const string EXCEPTION     = "** Exception **";
	private const string NO_SOLUTION   = "* No Solution *";
	private const string NO_INPUT      = "* NO INPUT DATA *";
	private const string NO_PARAMETERS = "* INVALID NO OF PARAMETERS *";

	public static string? GetProblemDescription(int year, int day) {
		TypeInfo? dayTypeInfo = TryGetDayTypeInfo(year, day);
		if (dayTypeInfo is null) {
			return NO_SOLUTION;
		}

		DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])dayTypeInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
		return descriptionAttributes is not []
			? descriptionAttributes[0].Description
			: NO_SOLUTION;
	}

	public static bool HasVisualiser(int year, int day, int part) {
		TypeInfo? dayTypeInfo = TryGetDayTypeInfo(year, day);
		return dayTypeInfo is not null && 
			dayTypeInfo
			.GetMethods()
			.Where(m => m.Name == $"Part{part}")
			.Where(m => m.GetCustomAttributes().Where(attr => (attr.ToString() ?? "").EndsWith("HasVisualiserAttribute")).Any())
			.SingleOrDefault() is not null;
	}

	public static IEnumerable<SolutionPhase> SolveDay(int year, int day, string? input, params object[]? args)
		=> SolveDay(year, day, input?.ReplaceLineEndings().Split(Environment.NewLine), null, args);

	public static IEnumerable<SolutionPhase> SolveDay(int year, int day, string? input, Action<string[], bool>? visualise = null, params object[]? args)
		=> SolveDay(year, day, input?.ReplaceLineEndings().Split(Environment.NewLine), visualise, args);

	public static IEnumerable<SolutionPhase> SolveDay(int year, int day, string[]? input, params object[]? args)
		=> SolveDay(year, day, input, null, args);

	public static IEnumerable<SolutionPhase> SolveDay(int year, int day, string[]? input, Action<string[], bool>? visualise = null, params object[]? args) {
		long startTime;
		long stopTime;
		if (input is null) {
			yield return SolutionPhase.NoInput;
			yield break;
		}

		TypeInfo? dayTypeInfo = TryGetDayTypeInfo(year, day);
		if (dayTypeInfo is null) {
			yield return SolutionPhase.NoSolution;
			yield break;
		}

		MethodInfo[] methods = dayTypeInfo.GetMethods();

		startTime = Stopwatch.GetTimestamp();
		MethodInfo? initMethod = methods
			.Where(m => m.GetCustomAttributes().Where(attr => (attr.ToString() ?? "").EndsWith("InitAttribute")).Any())
			.SingleOrDefault();

		input = input.StripTrailingBlankLineOrDefault();
		startTime = Stopwatch.GetTimestamp();
		InitInput(input, args, methods, visualise);
		stopTime = Stopwatch.GetTimestamp();
		yield return new SolutionPhase(SolutionPhase.PHASE_INIT) with { Elapsed = Stopwatch.GetElapsedTime(startTime, stopTime) };


		MethodInfo ? method1 = methods
			.Where(m => m.Name == $"Part1")
			.SingleOrDefault();

		startTime = Stopwatch.GetTimestamp();
		InvokeResult invokeResult1 = new(NO_SOLUTION);
		if (method1 is not null) {
			invokeResult1 = InvokeSolutionMethod(input, args, method1, visualise);
		};
		stopTime = Stopwatch.GetTimestamp();

		if (invokeResult1.Answer.Contains("written")) {
			yield return SolutionPhase.NoSolutionPart1;
		} else if (invokeResult1.Exception is not null) {
			yield return SolutionPhase.ExceptionPart1 with { Answer = invokeResult1.Answer, Exception = invokeResult1.Exception, Elapsed = Stopwatch.GetElapsedTime(startTime, stopTime) };
		} else {
			yield return new SolutionPhase(SolutionPhase.PHASE_PART1) with { Answer = invokeResult1.Answer, Elapsed = Stopwatch.GetElapsedTime(startTime, stopTime) };
		}

		MethodInfo? method2 = methods
			.Where(m => m.Name == $"Part2")
			.SingleOrDefault();

		startTime = Stopwatch.GetTimestamp();
		InvokeResult invokeResult2 = new(NO_SOLUTION);
		if (method2 is not null) {
			invokeResult2 = InvokeSolutionMethod(input, args, method2, visualise);
		};
		stopTime = Stopwatch.GetTimestamp();

		if (invokeResult2.Answer.Contains("written")) {
			yield return SolutionPhase.NoSolutionPart2;
		} else if (invokeResult2.Exception is not null) {
			yield return SolutionPhase.ExceptionPart2 with { Answer = invokeResult2.Answer, Exception = invokeResult2.Exception, Elapsed = Stopwatch.GetElapsedTime(startTime, stopTime) };
		} else {
			yield return new SolutionPhase(SolutionPhase.PHASE_PART2) with { Answer = invokeResult2.Answer, Elapsed = Stopwatch.GetElapsedTime(startTime, stopTime) };
		}

	}

	public static string SolveProblem(int year, int day, int problemNo, string? input, params object[]? args) 
		=> SolveProblem(year, day, problemNo, input?.ReplaceLineEndings().Split(Environment.NewLine), null, args);

	public static string SolveProblem(int year, int day, int problemNo, string[]? input, params object[]? args) 
		=> SolveProblem(year, day, problemNo, input, null, args);

	public static string SolveProblem(int year, int day, int problemNo, string? input, Action<string[], bool>? visualise = null, params object[]? args) 
		=> SolveProblem(year, day, problemNo, input?.ReplaceLineEndings().Split(Environment.NewLine), visualise, args);

	public static string SolveProblem(int year, int day, int problemNo, string[]? input, Action<string[], bool>? visualise = null, params object[]? args) {
		if (input is null) {
			return NO_INPUT;
		}

		TypeInfo? dayTypeInfo = TryGetDayTypeInfo(year, day);
		if (dayTypeInfo is null) {
			return NO_SOLUTION;
		}

		MethodInfo[] methods = dayTypeInfo.GetMethods();

		InitInput(input, args, methods, visualise);

		MethodInfo? method = methods
			.Where(m => m.Name == $"Part{problemNo}")
			.SingleOrDefault();

		return method is null
			? NO_SOLUTION
			: InvokeSolutionMethod(input, args, method, visualise).Answer;
	}

	private static TypeInfo? TryGetDayTypeInfo(int year, int day) {
		string assemblyName = $"AdventOfCode.Solutions.{year}";
		Assembly assembly;
		try {
			assembly = Assembly.Load(assemblyName);
		} catch (Exception) {
			return null;
		}

		return assembly.DefinedTypes.SingleOrDefault(x => x.Name == $"Day{day:D2}");
	}

	private static void InitInput(object input, object[]? args, MethodInfo[] methods, Action<string[], bool>? visualise) {
		MethodInfo? initMethod = methods
			.Where(m => m.GetCustomAttributes().Where(attr => (attr.ToString() ?? "").EndsWith("InitAttribute")).Any())
			.SingleOrDefault();

		if (initMethod is not null) {
			_ = InvokeSolutionMethod(input, args, initMethod, visualise);
		}
	}

	private static InvokeResult InvokeSolutionMethod(object input, object[]? args, MethodInfo method, Action<string[], bool>? visualise) {
		input = (input as string[]).StripTrailingBlankLineOrDefault();
		ParameterInfo[] parameters = method.GetParameters();
		int noOfParameters = parameters.Length;
		string parameterType = parameters[0].ParameterType.Name;
		object inputObject = parameterType switch {
			"String[]" => input,
			"String" => String.Join(Environment.NewLine, (string[])input),
			_ => throw new NotImplementedException(),
		};

		bool hasVisualiser = parameters.Where(p => p.Name == "visualise").Any();
		bool useVisualiser = visualise is not null;

		try {
			return (noOfParameters, hasVisualiser, useVisualiser) switch
			{
				(0, _, _)           => new(NO_PARAMETERS),
				(1, false, _)       => new(method.Invoke(0, [inputObject])?.ToString() ?? ""),
				( > 1, false, _)    => new(method.Invoke(0, [inputObject, args!])?.ToString() ?? ""),
				(2, true, false)    => new(method.Invoke(0, [inputObject, null!])?.ToString() ?? ""),
				(2, true, true)     => new(method.Invoke(0, [inputObject, visualise!])?.ToString() ?? ""),
				( > 2, true, false) => new(method.Invoke(0, [inputObject, null!, args!])?.ToString() ?? ""),
				( > 2, true, true)  => new(method.Invoke(0, [inputObject, visualise!, args!])?.ToString() ?? ""),
				_                   => new(method.Invoke(0, [inputObject, args!])?.ToString() ?? ""),
			};
		}
		catch (Exception ex) {
			return new(EXCEPTION, ex);
		}
	}

	private record InvokeResult(string Answer, Exception? Exception = null);
}
