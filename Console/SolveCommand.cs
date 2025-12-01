internal sealed class SolveCommand : AsyncCommand<SolveSettings>
{
	private readonly Lock _consoleLock = new();

	public override async Task<int> ExecuteAsync(CommandContext context, SolveSettings settings, CancellationToken cancellationToken)
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8;
		AnsiConsole.Reset();

		DateOnly date = GetDate(settings);
		int? phase = settings.Phase;
		bool showVisuals = settings.Visual;
		bool isDebug = settings.Debug;
		bool isDownload = settings.Download;
		TimeSpan visualsTime = settings.VisualTime;
		object[] solutionArgs = ParseSolutionArgs(settings.SolutionArgs);

		int noOfDays = date.Year >= 2025 ? 12 : 25;
		long totalTime = Stopwatch.GetTimestamp();

		if (date.Month == 12 && date.Day <= noOfDays) {
			await GetInputDataAndSolve(date.Year, date.Day, phase, _consoleLock, showVisuals, isDebug, isDownload, solutionArgs);
		} else {
			DateOnly dateNow = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-5));
			for (int day = 1; day <= noOfDays; day++) {
				if (dateNow >= new DateOnly(date.Year, 12, day)) {
					await GetInputDataAndSolve(date.Year, day, phase, _consoleLock);
				}
			}
		}

		AnsiConsole.MarkupLine($" Total Elapsed time: [bold fuchsia]{Stopwatch.GetElapsedTime(totalTime)}[/]");
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

	private static async Task GetInputDataAndSolve(int year, int day, int? phase, Lock consolelock, bool showVisuals = false, bool isDebug = false, bool isDownload = false, params object[]? args)
	{
		string[]? input = await Program.GetInputData(year, day, isDownload);
		string title = GetProblemDescription(year, day) ?? $"";

		lock (consolelock) {
			AnsiConsole.Markup($"{year} {day,2} [bold]{title,-40}[/]");

			if (input is not null) {
				Action<string[], bool>? visualiser = showVisuals ? new Action<string[], bool>((s, b) =>
				{
					lock (consolelock) {
						VisualiseOutput(s, b);
					}
				}) : null;


				IEnumerable<SolutionPhaseResult> solveResults = SolveDay(year, day, input, phase, visualiser, args);

				foreach (SolutionPhaseResult result in solveResults) {
					if (showVisuals) {
						AnsiConsole.WriteLine();
					}

					DisplayOutputTimings(result.Elapsed);

					const int answerLength = 17;
					string? answerMarkup = result.Phase switch
					{
						PHASE_PART1 => $"[green] Pt1:[/] {(result.Answer.StartsWith('*') ? "[red]" : "[lime]")} {result.Answer,-answerLength}[/]",
						PHASE_PART2 => $"[yellow] Pt2:[/] {(result.Answer.StartsWith('*') ? "[red]" : "[yellow1]")} {result.Answer,-answerLength}[/]",
						EXCEPTION_PART1 => $"[green] Pt1:[/] [red] {EXCEPTION_MESSAGE,-answerLength}[/]",
						EXCEPTION_PART2 => $"[yellow] Pt2:[/] [red] {EXCEPTION_MESSAGE,-answerLength}[/]",
						_ => null,
					};

					if (answerMarkup is not null) {
						AnsiConsole.Markup(answerMarkup);
					}
				}

				AnsiConsole.WriteLine();

				if (isDebug) {
					DisplayOutputExceptions(solveResults);
				}
			} else {
				AnsiConsole.MarkupLine($"     [dim red]** NO INPUT DATA **[/]");
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

		if (lines[0].Contains("markup", StringComparison.OrdinalIgnoreCase)) {
			AnsiConsole.MarkupLine(string.Join(Environment.NewLine, lines[1..]));
		} else {
			AnsiConsole.WriteLine(string.Join(Environment.NewLine, lines));
		}
	}

	private static void DisplayOutputExceptions(IEnumerable<SolutionPhaseResult> solveResults)
	{
		foreach (SolutionPhaseResult result in solveResults.Where(r => r.Exception is not null)) {
			AnsiConsole.WriteLine();
			AnsiConsole.WriteException(result.Exception!, ExceptionFormats.ShortenPaths | ExceptionFormats.ShortenTypes |
					ExceptionFormats.ShortenMethods | ExceptionFormats.ShowLinks);
			AnsiConsole.WriteLine();
		}
	}

	private static void DisplayOutputTimings(TimeSpan elapsed)
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
