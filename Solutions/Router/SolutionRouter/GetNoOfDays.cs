namespace AdventOfCode.Solutions;

public static partial class SolutionRouter
{
	/// <summary>
	/// Get number of days for the given year by reading the static constant `NO_OF_DAYS` from the year's assembly if present.
	/// Returns 0 when the assembly or constant cannot be found.
	/// </summary>
	public static int GetNoOfDays(int year)
	{
		string assemblyName = $"{SOLUTIONS_NAMESPACE}.{year}";

		Assembly assembly;
		try {
			assembly = Assembly.Load(assemblyName);
		}
		catch (Exception) {
			return 0;
		}

		int noOfDays = 0;

		Type? constantsType;
		constantsType = assembly.GetType($"{SOLUTIONS_NAMESPACE}._{year}.Constants", throwOnError: false, ignoreCase: false);

		if (constantsType is not null)
		{
			FieldInfo? field = constantsType.GetField("NO_OF_DAYS", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			if (field is not null && field.GetValue(null) is int value)
			{
				noOfDays = value;
			}
		}

		return noOfDays;
	}
}
