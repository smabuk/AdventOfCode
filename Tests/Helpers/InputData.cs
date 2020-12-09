using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Tests
{
    public static partial class Helpers
    {
        public static string[]? GetInputData(int year, int day, string? username = "") {
			string[]? input = null;

			string filename;
			if (string.IsNullOrWhiteSpace(username)) {
				filename = $"{year}_{day:D2}.txt";
			} else {
				filename = $"{year}_{day:D2}_{username}.txt";
			}

			string fullFilename = Path.GetFullPath(Path.Combine(".", filename));

			// Start: Fix for xUnit test project
			if (Path.GetFileName(Path.GetDirectoryName(fullFilename))?.StartsWith("net") ?? false) {
				fullFilename = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "Data", filename));
			}
			// End: Fix for xUnit test project

			if (File.Exists(fullFilename)) {
				input = File.ReadAllText(fullFilename).Split("\n");
			}

			return input;
		}
	}
}
