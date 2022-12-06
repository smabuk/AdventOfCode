using System.Reflection;

namespace AdventOfCode.Solutions;

static public class SolutionRouter {
	private const string NO_SOLUTION = "** Solution not written yet **";
	private const string NO_INPUT = "** NO INPUT DATA **";
	private const string NO_PARAMETERS = "** INVALID NO OF PARAMETERS **";

	public static string? GetProblemDescription(int year, int day) {
		Assembly assembly;
		try {
			assembly = Assembly.Load($"AdventOfCode.Solutions.{year}");
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

	public static string SolveProblem(int year, int day, int problemNo, string? input, params object[]? args) {
		return SolveProblem(year, day, problemNo, input?.Split(Environment.NewLine), args);
	}

	public static string SolveProblem(int year, int day, int problemNo, string[]? input, params object[]? args) {

		if (input is null) {
			return NO_INPUT;
		}

		Assembly assembly;
		try {
			assembly = Assembly.Load($"AdventOfCode.Solutions.{year}");
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
