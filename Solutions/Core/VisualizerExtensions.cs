namespace AdventOfCode.Solutions.Core;

public static class VisualizerExtensions
{
	extension(Action<string[], bool>? action)
	{
		public bool IsTestOutput() => action?.Method.DeclaringType?.FullName?.Contains(".Tests.") ?? false;
	}
}
