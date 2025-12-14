using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Solutions;
public static partial class SolutionRouter
{
	private static bool TryGetDayTypeInfo(int year, int day,[NotNullWhen(true)] out TypeInfo? typeInfo)
	{
		string assemblyName = $"{SOLUTIONS_NAMESPACE}.{year}";
		Assembly assembly;
		try {
			assembly = Assembly.Load(assemblyName);
		}
		catch (Exception) {
			typeInfo = default;
			return false;
		}

		typeInfo = assembly.DefinedTypes.SingleOrDefault(x => x.Name == $"Day{day:D2}");
		return typeInfo is not null;
	}
}
