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
		=> SolveDay(year, day, input?.Split(Environment.NewLine), args);

	public static IEnumerable<SolutionPhase> SolveDay(int year, int day, string[]? input, params object[]? args) {
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
		InitInput(input, args, methods);
		stopTime = Stopwatch.GetTimestamp();
		yield return new SolutionPhase("Init") with { Elapsed = Stopwatch.GetElapsedTime(startTime, stopTime) };


		MethodInfo ? method1 = methods
			.Where(m => m.Name == $"Part1")
			.SingleOrDefault();

		startTime = Stopwatch.GetTimestamp();
		string answer1 = NO_SOLUTION;
		if (method1 is not null) {
			answer1 = InvokeSolutionMethod(input, args, method1);
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
			answer2 = InvokeSolutionMethod(input, args, method2);
		}
		stopTime = Stopwatch.GetTimestamp();

		yield return answer2.Contains("written") switch {
			false => new SolutionPhase("Part2") with { Answer = answer2, Elapsed = Stopwatch.GetElapsedTime(startTime, stopTime) },
			true => SolutionPhase.NoSolutionPart2,
		};

	}

	public static string SolveProblem(int year, int day, int problemNo, string? input, params object[]? args) 
		=> SolveProblem(year, day, problemNo, input?.Split(Environment.NewLine), args);

	public static string SolveProblem(int year, int day, int problemNo, string[]? input, params object[]? args) {
		if (input is null) {
			return NO_INPUT;
		}

		TypeInfo? dayTypeInfo = TryGetDayTypeInfo(year, day);
		if (dayTypeInfo is null) {
			return NO_SOLUTION;
		}

		MethodInfo[] methods = dayTypeInfo.GetMethods();

		InitInput(input, args, methods);

		MethodInfo? method = methods
			.Where(m => m.Name == $"Part{problemNo}")
			.SingleOrDefault();

		if (method is null) {
			return NO_SOLUTION;
		}

		return InvokeSolutionMethod(input, args, method);
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

	private static void InitInput(object input, object[]? args, MethodInfo[] methods) {
		MethodInfo? initMethod = methods
			.Where(m => m.GetCustomAttributes().Where(attr => (attr.ToString() ?? "").EndsWith("InitAttribute")).Any())
			.SingleOrDefault();

		if (initMethod is not null) {
			_ = InvokeSolutionMethod(input, args, initMethod);
		}
	}

	private static string InvokeSolutionMethod(object input, object[]? args, MethodInfo method) {
		input = (input as string[]).StripTrailingBlankLineOrDefault();
		ParameterInfo[] parameters = method.GetParameters();
		int noOfParameters = parameters.Length;
		string parameterType = parameters[0].ParameterType.Name;
		object inputObject = parameterType switch {
			"String[]" => input,
			"String" => String.Join(Environment.NewLine, (string[])input),
			_ => throw new NotImplementedException(),
		};

		return noOfParameters switch {
			0 => NO_PARAMETERS,
			1 => method.Invoke(0, new object[] { inputObject })?.ToString() ?? "",
			_ => method.Invoke(0, new object[] { inputObject, args! })?.ToString() ?? ""
		};
	}
}
