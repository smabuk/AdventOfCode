using Smab.Helpers;

namespace AdventOfCode.Solutions.Core;
public static class VisualiseGridExtensions
{
	public static void VisualiseGrid(this char[,] grid, string title, Action<string[], bool>? visualise)
	{
		if (visualise is not null) {
			string[] output = ["", title, .. grid.AsStrings()];
			visualise?.Invoke(output, false);
		}
	}
}
