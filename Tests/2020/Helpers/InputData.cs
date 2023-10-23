namespace AdventOfCode.Tests;

public static partial class Helpers {
	public static string[]? GetInputData(int year, int day, string? username = "") {
		string[]? input = null;

		string filename = string.IsNullOrWhiteSpace(username)
			? $"{year}_{day:D2}.txt"
			: $"{year}_{day:D2}_{username}.txt";
		string fullFilename = Path.GetFullPath(Path.Combine(".", filename));

		// Start: Fix for xUnit test project
		if (Path.GetFileName(Path.GetDirectoryName(fullFilename))?.StartsWith("net") ?? false) {
			fullFilename = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "..", "Data", filename));
		}
		// End: Fix for xUnit test project

		if (File.Exists(fullFilename)) {
			input = File.ReadAllText(fullFilename).Replace("\r", "").Split("\n");
		}

		return input;
	}
}
