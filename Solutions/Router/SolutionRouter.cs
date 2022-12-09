using System.Reflection;

namespace AdventOfCode.Solutions;

static public class SolutionRouter {
	private const string NO_SOLUTION = "** Solution not written yet **";
	private const string NO_INPUT = "** NO INPUT DATA **";
	private const string NO_PARAMETERS = "** INVALID NO OF PARAMETERS **";

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

	public static (string answer1, string answer2) SolveDay(int year, int day, string? input, params object[]? args) {
		return SolveDay( year,  day, input?.Split(Environment.NewLine), args);
	}

	public static (string answer1, string answer2) SolveDay(int year, int day, string[]? input, params object[]? args) {
		if (input is null) {
			return (NO_INPUT, NO_INPUT);
		}

		TypeInfo? dayTypeInfo = TryGetDayTypeInfo(year, day);
		if (dayTypeInfo is null) {
			return (NO_SOLUTION, NO_SOLUTION);
		}

		input = input.StripTrailingBlankLineOrDefault();
		InitInput(input, args, dayTypeInfo);

		string answer1 = SolveProblem(year, day, 1, input, args);
		string answer2 = SolveProblem(year, day, 2, input, args);

		return (answer1, answer2);
	}

	public static string SolveProblem(int year, int day, int problemNo, string? input, params object[]? args) {
		return SolveProblem(year, day, problemNo, input?.Split(Environment.NewLine), args);
	}

	public static string SolveProblem(int year, int day, int problemNo, string[]? input, params object[]? args) {
		if (input is null) {
			return NO_INPUT;
		}

		TypeInfo? dayTypeInfo = TryGetDayTypeInfo(year, day);
		if (dayTypeInfo is null) {
			return NO_SOLUTION;
		}

		InitInput(input, args, dayTypeInfo);

		MethodInfo? method =
			dayTypeInfo.GetMethods()
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

	private static void InitInput(object input, object[]? args, TypeInfo classDay) {
		MethodInfo? initMethod =
			classDay.GetMethods()
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
