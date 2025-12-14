namespace AdventOfCode.Solutions;
public static partial class SolutionRouter
{
	public static bool HasVisualiser(int year, int day, int part)
	{
		_ = TryGetDayTypeInfo(year, day, out TypeInfo? dayTypeInfo);
		return dayTypeInfo is not null &&
			(dayTypeInfo.GetMethods().Any(m => m.Name == PHASE_VISUALISER)
			|| dayTypeInfo.GetMethods().Where(m => m.Name == $"{PHASE_PART}{part}")
				.SingleOrDefault(m => m.GetCustomAttributes().Any(attr => (attr.ToString() ?? "").EndsWith("VisualiserAttribute"))) is not null);
	}
}
