using System.ComponentModel;

internal sealed class SolveSettings : CommandSettings
{
	[CommandArgument(0, "[year]")]
	[Description("The year of puzzles to solve. Defaults to the latest year.")]
	public int? Year { get; init; }

	[CommandArgument(1, "[day]")]
	[Description("The puzzle day to solve (e.g. 5). If not specified, solves all days for the year.")]
	public int? Day { get; init; }

	[CommandOption("-p|--phase|--part")]
	[Description("The puzzle phase to solve.")]
	public int? Phase { get; init; }

	[CommandOption("-d|--debug")]
	[Description("Show debug information (e.g. details of exceptions).")]
	[DefaultValue(false)]
	public bool Debug { get; init; }

	[CommandOption("-v|--visual")]
	[Description("Uses the visualiser if one exists.")]
	[DefaultValue(false)]
	public bool Visual { get; init; }

	[CommandOption("--visual-time <TIMESPAN>")]
	[Description("Time to wait for visuals (format: hh:mm:ss.ms). Default is 00:00:00.300")]
	public TimeSpan VisualTime { get; init; } = new TimeSpan(0, 0, 0, 0, 300);

	[CommandOption("--download")]
	[Description("Download the puzzle input.")]
	[DefaultValue(false)]
	public bool Download { get; init; }

	[CommandArgument(2, "[args]")]
	[Description("Additional arguments to pass to the solution.")]
	public string[]? SolutionArgs { get; init; }
}
