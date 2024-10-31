namespace AdventOfCode.Solutions;
static public partial class SolutionRouter
{
	public static string? GetProblemDescription(int year, int day)
	{
		if (TryGetDayTypeInfo(year, day, out TypeInfo? dayTypeInfo) is false) {
			return NO_SOLUTION_MESSAGE;
		}

		DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])dayTypeInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
		return descriptionAttributes is not []
			? descriptionAttributes[0].Description
			: NO_SOLUTION_MESSAGE;
	}
}
