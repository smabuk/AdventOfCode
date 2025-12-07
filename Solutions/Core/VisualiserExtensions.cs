namespace AdventOfCode.Solutions.Core;

public static class VisualiserExtensions
{
	extension(Action<string[], bool>? action)
	{
		/// <summary>
		/// Determines whether the current action supports markup in its output.
		/// </summary>
		/// <returns>true if the action is capable of producing markup output; otherwise, false.</returns>
		public bool IsCapableOfMarkup() => action.IsConsoleOutput();

		/// <summary>
		/// Determines whether the current output is directed to the console.
		/// </summary>
		/// <returns>true if the output is directed to the console; otherwise, false.</returns>
		public bool IsConsoleOutput() => action?.Method.DeclaringType?.FullName?.Contains("Command") ?? false;

		/// <summary>
		/// Determines whether the associated action originates from a test project or test-related code.
		/// </summary>
		/// <remarks>This method can be used to distinguish between production and test code based on naming
		/// conventions. It relies on the presence of ".Tests." in the fully qualified name of the declaring type.</remarks>
		/// <returns>true if the action's declaring type is within a namespace or type name containing ".Tests."; otherwise, false.</returns>
		public bool IsTestOutput() => action?.Method.DeclaringType?.FullName?.Contains(".Tests.") ?? false;

		/// <summary>
		/// Determines whether the associated action is defined within a type whose fully qualified name contains ".Web.".
		/// </summary>
		/// <remarks>This method can be used to distinguish actions that are part of web-related components based on
		/// naming conventions. The result depends on the presence of ".Web." in the declaring type's full name and may be
		/// affected by changes to type naming or project structure.</remarks>
		/// <returns>true if the action's declaring type name contains ".Web."; otherwise, false.</returns>
		public bool IsWebOutput() => action?.Method.DeclaringType?.FullName?.Contains(".Web.") ?? false;
	}
}
