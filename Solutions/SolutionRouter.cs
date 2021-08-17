using System.Reflection;

namespace AdventOfCode.Solutions;

static public class SolutionRouter {
	private const string NO_SOLUTION = "** Solution not written yet **";
	private const string NO_INPUT = "** NO INPUT DATA **";
	private const string NO_PARAMETERS = "** INVALID NO OF PARAMETERS **";

	public static string SolveProblem(int year, int day, int problemNo, string[]? input, params object[]? args) {

		if (input is null) {
			return NO_INPUT;
		}

		Assembly assembly = Assembly.GetExecutingAssembly();

		MethodInfo? method =
			(from a in assembly.GetTypes()
			 from m in a.GetMethods()
			 where m.Name == $"Part{problemNo}" && (m.ReflectedType?.FullName?.EndsWith($"Year{year}.Day{day:D2}") ?? false)
			 select ((MethodInfo)m)).SingleOrDefault();

		if (method is null) {
			return NO_SOLUTION;
		}

		int noOfParameters = method.GetParameters().Length;

		return noOfParameters switch {
			0 => NO_PARAMETERS,
			1 => method.Invoke(0, new object[] { input })?.ToString() ?? "", { } => method.Invoke(0, new object[] { input, args! })?.ToString() ?? ""
		};

	}
}
