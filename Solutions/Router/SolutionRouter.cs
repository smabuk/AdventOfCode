using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode.Solutions;

static public class SolutionRouter {
	private const string NO_SOLUTION = "* No Solution *";
	private const string NO_INPUT = "* NO INPUT DATA *";
	private const string NO_PARAMETERS = "* INVALID NO OF PARAMETERS *";

	public static string? GetProblemDescription(int year, int day) {
		TypeInfo? dayTypeInfo = TryGetDayTypeInfo(year, day);
		if (dayTypeInfo is null) {
			return NO_SOLUTION;
		}

		DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])dayTypeInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
		return descriptionAttributes.Length > 0
			? ((DescriptionAttribute)descriptionAttributes.First()).Description
			: NO_SOLUTION;
	}

	public static IEnumerable<SolutionPhase> SolveDay(int year, int day, string? input, params object[]? args)
		=> SolveDay(year, day, input?.Split(Environment.NewLine), null, args);

	public static IEnumerable<SolutionPhase> SolveDay(int year, int day, string? input, Action<string[], bool>? visualise = null, params object[]? args)
		=> SolveDay(year, day, input?.Split(Environment.NewLine), visualise, args);

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
		yield return new SolutionPhase("Init") with { Elapsed = Stopwatch.GetElapsedTime(startTime, stopTime) };


		MethodInfo ? method1 = methods
			.Where(m => m.Name == $"Part1")
			.SingleOrDefault();

		startTime = Stopwatch.GetTimestamp();
		string answer1 = NO_SOLUTION;
		if (method1 is not null) {
			answer1 = InvokeSolutionMethod(input, args, method1, visualise);
		}
		stopTime = Stopwatch.GetTimestamp();

		yield return answer1.Contains("written") switch {
			false => new SolutionPhase("Part1") with { Answer = answer1, Elapsed = Stopwatch.GetElapsedTime(startTime, stopTime) },
			true => SolutionPhase.NoSolutionPart1,
		};

		MethodInfo? method2 = methods
			.Where(m => m.Name == $"Part2")
			.SingleOrDefault();

		string answer2 = NO_SOLUTION;
		startTime = Stopwatch.GetTimestamp();
		if (method2 is not null) {
			answer2 = InvokeSolutionMethod(input, args, method2, visualise);
		}
		stopTime = Stopwatch.GetTimestamp();

		yield return answer2.Contains("written") switch {
			false => new SolutionPhase("Part2") with { Answer = answer2, Elapsed = Stopwatch.GetElapsedTime(startTime, stopTime) },
			true => SolutionPhase.NoSolutionPart2,
		};

	}

	public static string SolveProblem(int year, int day, int problemNo, string? input, params object[]? args) 
		=> SolveProblem(year, day, problemNo, input?.Split(Environment.NewLine), null, args);

	public static string SolveProblem(int year, int day, int problemNo, string[] input, params object[]? args) 
		=> SolveProblem(year, day, problemNo, input, null, args);

	public static string SolveProblem(int year, int day, int problemNo, string? input, Action<string[], bool>? visualise = null, params object[]? args) 
		=> SolveProblem(year, day, problemNo, input?.Split(Environment.NewLine), visualise, args);

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
			: InvokeSolutionMethod(input, args, method, visualise);
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

	private static string InvokeSolutionMethod(object input, object[]? args, MethodInfo method, Action<string[], bool>? visualise) {
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

		return (noOfParameters, hasVisualiser, useVisualiser) switch {
			( 0, _    , _    ) => NO_PARAMETERS,
			( 1, false, _    ) => method.Invoke(0, new object[] { inputObject })                   ?.ToString() ?? "",
			(>1, false, _    ) => method.Invoke(0, new object[] { inputObject, args! })            ?.ToString() ?? "",
			( 2, true , false) => method.Invoke(0, new object[] { inputObject, null! })            ?.ToString() ?? "",
			( 2, true , true ) => method.Invoke(0, new object[] { inputObject, visualise! })       ?.ToString() ?? "",
			(>2, true , false) => method.Invoke(0, new object[] { inputObject, null!, args! })     ?.ToString() ?? "",
			(>2, true , true ) => method.Invoke(0, new object[] { inputObject, visualise!, args! })?.ToString() ?? "",
			_                  => method.Invoke(0, new object[] { inputObject, args! })            ?.ToString() ?? "",
		};
	}
}
