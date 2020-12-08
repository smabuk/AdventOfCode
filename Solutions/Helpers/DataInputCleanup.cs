using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Helpers
{
    public static class DataInputCleanup
    {
		/// <summary>
		/// If a blank line is found at the end of the array then remove it.
		/// Never returns null.
		/// </summary>
		/// <param name="input"></param>
		/// <returns>The input with the final string removed if it is empty or whitespace. Returns an empty array if null.</returns>
		public static string[] StripTrailingBlankLineOrDefault(this string[]? input) => RemoveBlankLineFromEnd(input);
		private static string[] RemoveBlankLineFromEnd(string[]? input) {
			if (input is null) { return Array.Empty<string>(); }
			if (string.IsNullOrWhiteSpace(input[^1])) {
				return input[..^1];
			} else {
				return input;
			}
		}
    }
}
