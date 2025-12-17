namespace AdventOfCode.Solutions.Core;
public static class VisualiseStringsExtensions
{
	public static void VisualiseStrings(this IEnumerable<string> strings, string title, Action<string[], bool>? visualise)
	{
		if (visualise is not null) {
			string[] output = ["", title, .. strings];
			visualise?.Invoke(output, false);
		}
	}
}
