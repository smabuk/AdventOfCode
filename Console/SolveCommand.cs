internal sealed class SolveCommand : AsyncCommand<SolveSettings>
{
	private readonly Lock _consoleLock = new();

	public override async Task<int> ExecuteAsync(CommandContext context, SolveSettings settings, CancellationToken cancellationToken)
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8;
		Console.ResetColor();

		DateOnly date = GetDate(settings);
		bool showVisuals = settings.Visual;
		bool isDebug = settings.Debug;
		bool isDownload = settings.Download;
		TimeSpan visualsTime = settings.VisualTime;
		object[]? solutionArgs = ParseSolutionArgs(settings.SolutionArgs);

		int noOfDays = date.Year >= 2025 ? 12 : 25;
		long totalTime = Stopwatch.GetTimestamp();

		if (date.Month == 12 && date.Day <= noOfDays)
		{
			await GetInputDataAndSolve(date.Year, date.Day, _consoleLock, null, null, showVisuals, isDebug, isDownload, solutionArgs);
		}
		else
		{
			DateOnly dateNow = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-5));
			for (int day = 1; day <= noOfDays; day++)
			{
				if (dateNow >= new DateOnly(date.Year, 12, day))
				{
					await GetInputDataAndSolve(date.Year, day, _consoleLock);
				}
			}
		}

		Console.Write($" Total Elapsed time: {Stopwatch.GetElapsedTime(totalTime)}");
		if (showVisuals)
		{
			await Task.Delay(visualsTime, cancellationToken);
		}

		return 0;
	}

	private static DateOnly GetDate(SolveSettings settings)
	{
		DateOnly now = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-5));

		if (settings.Year.HasValue && settings.Day.HasValue)
		{
			return new DateOnly(settings.Year.Value, 12, settings.Day.Value);
		}

		if (settings.Year.HasValue)
		{
			return new DateOnly(settings.Year.Value, 1, 1);
		}

		return now;
	}

	private static object[]? ParseSolutionArgs(string[]? args)
	{
		if (args is null || args.Length == 0)
		{
			return [];
		}

		List<object> solutionArgs = [];
		foreach (string arg in args)
		{
			string lowerArg = arg.ToLowerInvariant();
			if (lowerArg is "true" or "false")
			{
				solutionArgs.Add(arg == "true");
			}
			else
			{
				solutionArgs.Add(arg);
			}
		}

		return [.. solutionArgs];
	}

	private static async Task GetInputDataAndSolve(int year, int day, Lock consolelock, string? title = null, string[]? input = null, bool showVisuals = false, bool isDebug = false, bool isDownload = false, params object[]? args)
	{
		input = await Program.GetInputData(year, day, isDownload);

		if (string.IsNullOrWhiteSpace(title))
		{
			title = GetProblemDescription(year, day) ?? $"";
		}

		lock (consolelock)
		{
			Console.Write($"{year} {day,2} {title,-40}");
			if (input is not null)
			{
				ConsoleColor answerColour;
				Action<string[], bool>? visualiser = showVisuals ? new Action<string[], bool>((s, b) =>
				{
					lock (consolelock)
					{
						VisualiseOutput(s, b);
					}
				}) : null;
				IEnumerable<SolutionPhaseResult> solveResults = SolveDay(year, day, input, visualiser, args);
				foreach (SolutionPhaseResult result in solveResults)
				{
					if (result.Phase == PHASE_INIT)
					{
						OutputTimings(result.Elapsed);
					}
					else if (result.Phase == PHASE_PART1)
					{
						answerColour = ConsoleColor.Green;
						OutputTimings(result.Elapsed);
						Console.ForegroundColor = answerColour;
						Console.Write($" Pt1:");
						if (result.Answer.StartsWith('*'))
						{
							Console.ForegroundColor = ConsoleColor.Red;
						}

						Console.Write($" {result.Answer,-17}");
					}
					else if (result.Phase == PHASE_PART2)
					{
						answerColour = ConsoleColor.Yellow;
						OutputTimings(result.Elapsed);
						Console.ForegroundColor = answerColour;
						Console.Write($" Pt2:");
						if (result.Answer.StartsWith('*'))
						{
							Console.ForegroundColor = ConsoleColor.Red;
						}

						Console.Write($" {result.Answer,-17}");
					}
					else if (result.Phase == EXCEPTION_PART1)
					{
						answerColour = ConsoleColor.Green;
						OutputTimings(result.Elapsed);
						Console.ForegroundColor = answerColour;
						Console.Write($" Pt1:");
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write($" {EXCEPTION_MESSAGE,-16}");
					}
					else if (result.Phase == EXCEPTION_PART2)
					{
						answerColour = ConsoleColor.Yellow;
						OutputTimings(result.Elapsed);
						Console.ForegroundColor = answerColour;
						Console.Write($" Pt2:");
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write($" {EXCEPTION_MESSAGE,-16}");
					}

					Console.ResetColor();
				}

				Console.ResetColor();
				Console.WriteLine();

				if (isDebug)
				{
					OutputExceptions(solveResults);
				}
			}
			else
			{
				Console.WriteLine($"     ** NO INPUT DATA **");
			}
		}
	}

	private static void VisualiseOutput(string[] lines, bool clearScreen = false)
	{
		if (lines is null or [])
		{
			return;
		}

		if (clearScreen)
		{
			Console.Clear();
		}

		Console.Write(string.Join(Environment.NewLine, lines));
	}

	private static void OutputExceptions(IEnumerable<SolutionPhaseResult> solveResults)
	{
		foreach (SolutionPhaseResult result in solveResults.Where(r => r.Exception is not null))
		{
			Console.WriteLine();
			AnsiConsole.WriteException(result.Exception!, ExceptionFormats.ShortenPaths | ExceptionFormats.ShortenTypes |
					ExceptionFormats.ShortenMethods | ExceptionFormats.ShowLinks);
			Console.WriteLine();
			Console.ResetColor();
		}
	}

	private static void OutputTimings(TimeSpan elapsed)
	{
		if (elapsed.TotalNanoseconds <= 999)
		{
			Console.Write($" {elapsed.TotalNanoseconds,4:F0}ns");
		}
		else if (elapsed.TotalMicroseconds <= 999)
		{
			Console.Write($" {elapsed.TotalMicroseconds,4:F0}µs");
		}
		else if (elapsed.TotalMilliseconds <= 999)
		{
			Console.Write($" {elapsed.TotalMilliseconds,4:F0}ms");
		}
		else
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write($" {elapsed.TotalSeconds,5:F1}s");
			Console.ResetColor();
		}
	}
}
