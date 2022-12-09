using System.Reflection;

namespace AdventOfCode.Solutions;

static public class SolutionRouter {
	private const string NO_SOLUTION = "** Solution not written yet **";
	private const string NO_INPUT = "** NO INPUT DATA **";
	private const string NO_PARAMETERS = "** INVALID NO OF PARAMETERS **";

	public static string? GetProblemDescription(int year, int day) {
		string assemblyName = $"AdventOfCode.Solutions.{year}";
		Assembly assembly;
		try {
			assembly = Assembly.Load(assemblyName);
		} catch (Exception) {
			return NO_SOLUTION;
		}

		Type? type =
			(from a in assembly.GetTypes()
			 from m in a.GetMethods()
			 where m.Name == $"Part1" && (m.ReflectedType?.FullName?.EndsWith($".Day{day:D2}") ?? false)
			 select ((MethodInfo)m)).SingleOrDefault()?.DeclaringType;

		if (type is null) {
			return NO_SOLUTION;
		}

		DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])type.GetCustomAttributes(typeof(DescriptionAttribute), false);
		return descriptionAttributes.Length > 0
			? ((DescriptionAttribute)descriptionAttributes.First()).Description
			: NO_SOLUTION;
	}

	public static (string answer1, string answer2) SolveDay(int year, int day, string[]? input, params object[]? args) {
		if (input is null) {
			return (NO_INPUT, NO_INPUT);
		}

		string assemblyName = $"AdventOfCode.Solutions.{year}";
		Assembly assembly;
		try {
			assembly = Assembly.Load(assemblyName);
		} catch (Exception) {
			return (NO_SOLUTION, NO_SOLUTION);
		}

		var classDay = assembly.DefinedTypes.Where(x => x.Name == $"Day{day:D2}").SingleOrDefault();

		if (classDay is null) {
			return (NO_SOLUTION, NO_SOLUTION);
		}

		input = input.StripTrailingBlankLineOrDefault();

		var initMethod =
			(from m in classDay.GetMethods()
			 where m.GetCustomAttributes().Where(a => (a.ToString() ?? "").EndsWith("InitAttribute")).Any()
			 select ((MethodInfo)m))
			.SingleOrDefault();

		if (initMethod is not null) {
			ParameterInfo[] parameters = initMethod.GetParameters();
			int noOfParameters = parameters.Length;
			string parameterType = parameters[0].ParameterType.Name;
			object inputObject = parameterType switch {
				"String[]" => input,
				"String" => String.Join(Environment.NewLine, input),
				_ => throw new NotImplementedException(),
			};
			if (noOfParameters == 0) {
				initMethod.Invoke(0, null);
			} else if (noOfParameters == 1) {
				initMethod.Invoke(0, new object[] { inputObject });
			} else {
				initMethod.Invoke(0, new object[] { inputObject, args! });
			};

		}

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

		string assemblyName = $"AdventOfCode.Solutions.{year}";
		Assembly assembly;
		try {
			assembly = Assembly.Load(assemblyName);
		} catch (Exception) {
			return NO_SOLUTION;
		}

		MethodInfo? method =
			(from a in assembly.GetTypes()
			 from m in a.GetMethods()
			 where m.Name == $"Part{problemNo}" && (m.ReflectedType?.FullName?.EndsWith($".Day{day:D2}") ?? false)
			 select ((MethodInfo)m)).SingleOrDefault();

		if (method is null) {
			return NO_SOLUTION;
		}

		input = input.StripTrailingBlankLineOrDefault();
		ParameterInfo[] parameters = method.GetParameters();
		int noOfParameters = parameters.Length;
		string parameterType = parameters[0].ParameterType.Name;
		object inputObject = parameterType switch {
			"String[]" => input,
			"String" => String.Join(Environment.NewLine, input),
			_ => throw new NotImplementedException(),
		};

		return noOfParameters switch {
			0 => NO_PARAMETERS,
			1 => method.Invoke(0, new object[] { inputObject })?.ToString() ?? "",
			_ => method.Invoke(0, new object[] { inputObject, args! })?.ToString() ?? ""
		};

	}
}
