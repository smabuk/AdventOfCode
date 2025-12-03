namespace AdventOfCode.Solutions.Core;

public static class VisualiserExtensions
{
	extension(Action<string[], bool>? action)
	{
		public bool IsTestOutput() => action?.Method.DeclaringType?.FullName?.Contains(".Tests.") ?? false;
	}
}
