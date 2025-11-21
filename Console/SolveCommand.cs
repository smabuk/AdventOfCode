internal sealed class SolveCommand : AsyncCommand<SolveSettings>
{
	private readonly Lock _consoleLock = new();

	public override async Task<int> ExecuteAsync(CommandContext context, SolveSettings settings, CancellationToken cancellationToken)
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8;
		AnsiConsole.Reset();

		DateOnly date = GetDate(settings);
		bool showVisuals = settings.Visual;
		bool isDebug = settings.Debug;
		bool isDownload = settings.Download;
		TimeSpan visualsTime = settings.VisualTime;
		object[] solutionArgs = ParseSolutionArgs(settings.SolutionArgs);

		int noOfDays = date.Year >= 2025 ? 12 : 25;
		long totalTime = Stopwatch.GetTimestamp();

		if (date.Month == 12 && date.Day <= noOfDays) {
			await GetInputDataAndSolve(date.Year, date.Day, _consoleLock, null, null, showVisuals, isDebug, isDownload, solutionArgs);
		} else {
			DateOnly dateNow = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-5));
			for (int day = 1; day <= noOfDays; day++) {
				if (dateNow >= new DateOnly(date.Year, 12, day)) {
					await GetInputDataAndSolve(date.Year, day, _consoleLock);
				}
			}
		}

		AnsiConsole.MarkupLine($" Total Elapsed time: [fuchsia]{Stopwatch.GetElapsedTime(totalTime)}[/]");
		if (showVisuals) {
			await Task.Delay(visualsTime, cancellationToken);
		}

		return 0;
	}

	private static DateOnly GetDate(SolveSettings settings)
	{
		DateOnly now = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-5));

		if (settings.Year.HasValue && settings.Day.HasValue) {
			return new DateOnly(settings.Year.Value, 12, settings.Day.Value);
		}

		if (settings.Year.HasValue) {
			return new DateOnly(settings.Year.Value, 1, 1);
		}

		return now;
	}

	private static object[] ParseSolutionArgs(string[]? args)
	{
		if (args is null || args.Length == 0) {
			return [];
		}

		List<object> solutionArgs = [];
		foreach (string arg in args) {
			string lowerArg = arg.ToLowerInvariant();
			solutionArgs.Add(lowerArg is "true" or "false" ? arg is "true" : arg);
		}

		return [.. solutionArgs];
	}

	private static async Task GetInputDataAndSolve(int year, int day, Lock consolelock, string? title = null, string[]? input = null, bool showVisuals = false, bool isDebug = false, bool isDownload = false, params object[]? args)
	{
		input = await Program.GetInputData(year, day, isDownload);

		if (string.IsNullOrWhiteSpace(title)) {
			title = GetProblemDescription(year, day) ?? $"";
		}

		lock (consolelock) {
			AnsiConsole.Markup($"{year} {day,2} [bold]{title,-40}[/]");
			if (input is not null) {
				Action<string[], bool>? visualiser = showVisuals ? new Action<string[], bool>((s, b) =>
				{
					lock (consolelock) {
						VisualiseOutput(s, b);
					}
				}) : null;
				IEnumerable<SolutionPhaseResult> solveResults = SolveDay(year, day, input, visualiser, args);
				foreach (SolutionPhaseResult result in solveResults) {
					OutputTimings(result.Elapsed);

					if (result.Phase == PHASE_PART1) {
						string answerColour = result.Answer.StartsWith('*') ? "[red]" : "[lime]";
						AnsiConsole.Markup($"[green] Pt1:[/] {answerColour} {result.Answer,-17}[/]");
					} else if (result.Phase == PHASE_PART2) {
						string answerColour = result.Answer.StartsWith('*') ? "[red]" : "[yellow1]";
						AnsiConsole.Markup($"[yellow] Pt2:[/] {answerColour} {result.Answer,-17}[/]");
					} else if (result.Phase == EXCEPTION_PART1) {
						AnsiConsole.Markup($"[green] Pt1:[/][red] {EXCEPTION_MESSAGE,-16}[/]");
					} else if (result.Phase == EXCEPTION_PART2) {
						AnsiConsole.Markup($"[yellow] Pt2:[/][red] {EXCEPTION_MESSAGE,-16}[/]");
					}
				}

				AnsiConsole.WriteLine();

				if (isDebug) {
					OutputExceptions(solveResults);
				}
			} else {
				AnsiConsole.WriteLine($"     ** NO INPUT DATA **");
			}
		}
	}

	private static void VisualiseOutput(string[] lines, bool clearScreen = false)
	{
		if (lines is null or []) {
			return;
		}

		if (clearScreen) {
			AnsiConsole.Clear();
		}

		AnsiConsole.Write(string.Join(Environment.NewLine, lines));
	}

	private static void OutputExceptions(IEnumerable<SolutionPhaseResult> solveResults)
	{
		foreach (SolutionPhaseResult result in solveResults.Where(r => r.Exception is not null)) {
			AnsiConsole.WriteLine();
			AnsiConsole.WriteException(result.Exception!, ExceptionFormats.ShortenPaths | ExceptionFormats.ShortenTypes |
					ExceptionFormats.ShortenMethods | ExceptionFormats.ShowLinks);
			AnsiConsole.WriteLine();
		}
	}

	private static void OutputTimings(TimeSpan elapsed)
	{
		string output = elapsed switch
		{
			{ TotalNanoseconds: <= 999 } => $"{elapsed.TotalNanoseconds,4:F0}ns",
			{ TotalMicroseconds: <= 999 } => $"{elapsed.TotalMicroseconds,4:F0}µs",
			{ TotalMilliseconds: <= 999 } => $"{elapsed.TotalMilliseconds,4:F0}ms",
			_ => $"[red]{elapsed.TotalSeconds,5:F1}s[/]"
		};

		AnsiConsole.Markup($" {output}");
	}
}
