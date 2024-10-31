namespace AdventOfCode.Solutions;
static public partial class SolutionRouter
{
	public static bool HasVisualiser(int year, int day, int part)
	{
		_ = TryGetDayTypeInfo(year, day, out TypeInfo? dayTypeInfo);
		return dayTypeInfo is not null &&
			dayTypeInfo
			.GetMethods()
			.Where(m => m.Name == $"{PHASE_PART}{part}")
			.SingleOrDefault(m => m.GetCustomAttributes().Any(attr => (attr.ToString() ?? "").EndsWith("HasVisualiserAttribute"))) is not null;
	}
}
