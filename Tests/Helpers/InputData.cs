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
        public static string[]? GetInputData(int year, int day, string username = "") {
			string filename = Path.GetFullPath(Path.Combine(".", $"{year}_{day:D2}.txt"));

			// Start: Fix for xUnit test project
			if (Path.GetFileName(Path.GetDirectoryName(filename))?.StartsWith("net") ?? false) {
				filename = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "Data", $"{year}_{day:D2}.txt"));
			}
			// End: Fix for xUnit test project
			string[]? input = null;
			if (File.Exists(filename)) {
				input = File.ReadAllText(filename).Split("\n");
			}

			return input;
		}
	}
}
