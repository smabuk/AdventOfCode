Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.ResetColor();

(DateOnly date, object[]? solutionArgs, bool showVisuals, bool isDebug, bool isDownload) = ParseCommandLine(args);

System.Diagnostics.Stopwatch totalTimer = new();
totalTimer.Start();

if (date.Month == 12 && date.Day <= 25) {
	await GetInputDataAndSolve(date.Year, date.Day, null, null, showVisuals, isDebug, isDownload, solutionArgs);
} else {
	DateOnly dateNow = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-5));
	//Console.WriteLine($"Year dd Description                              Init        Part 1                       Part 2                ");
	//Console.WriteLine($"---- -- -------------------------------------  ------  ---------------------------  ----------------------------");
	for (int day = 1; day <= 25; day++) {
		if (dateNow >= new DateOnly(date.Year, 12, day)) {
			await GetInputDataAndSolve(date.Year, day);
		}
	}
}

totalTimer.Stop();
Console.Write($" Total Elapsed time: {totalTimer.Elapsed}");

static async Task GetInputDataAndSolve(int year, int day, string? title = null, string[]? input = null, bool showVisuals = false, bool isDebug = false, bool isDownload = false, params object[]? args)
{
	input = await GetInputData(year, day, isDownload);

	if (String.IsNullOrWhiteSpace(title)) {
		title = GetProblemDescription(year, day) ?? $"";
	}

	System.Diagnostics.Stopwatch timer = new();
	Console.Write($"{year} {day,2} {title,-38}");
	if (input is not null) {
		ConsoleColor answerColour;
		timer.Start();
		Action<string[], bool>? visualiser = showVisuals ? new Action<string[], bool>(VisualiseOutput) : null;
		IEnumerable<SolutionPhaseResult> solveResults = SolveDay(year, day, input, visualiser, args);
		foreach (SolutionPhaseResult result in solveResults) {
			if (result.Phase == PHASE_INIT) {
				OutputTimings(result.Elapsed);
			} else if (result.Phase == PHASE_PART1) {
				answerColour = ConsoleColor.Green;
				OutputTimings(result.Elapsed);
				Console.ForegroundColor = answerColour;
				Console.Write($" Pt1:");
				if (result.Answer.StartsWith('*')) {
					Console.ForegroundColor = ConsoleColor.Red;
				};
				Console.Write($" {result.Answer,-16}");
			} else if (result.Phase == PHASE_PART2) {
				answerColour = ConsoleColor.Yellow;
				OutputTimings(result.Elapsed);
				Console.ForegroundColor = answerColour;
				Console.Write($" Pt2:");
				if (result.Answer.StartsWith('*')) {
					Console.ForegroundColor = ConsoleColor.Red;
				};
				Console.Write($" {result.Answer,-16}");
			} else if (result.Phase == EXCEPTION_PART1) {
				answerColour = ConsoleColor.Green;
				OutputTimings(result.Elapsed);
				Console.ForegroundColor = answerColour;
				Console.Write($" Pt1:");
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write($" {EXCEPTION_MESSAGE,-16}");
			} else if (result.Phase == EXCEPTION_PART2) {
				answerColour = ConsoleColor.Yellow;
				OutputTimings(result.Elapsed);
				Console.ForegroundColor = answerColour;
				Console.Write($" Pt2:");
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write($" {EXCEPTION_MESSAGE,-16}");
			};
			Console.ResetColor();
		};

		Console.ResetColor();
		Console.WriteLine();

		if (isDebug) {
			OutputExceptions(solveResults);
		}
	} else {
		Console.WriteLine($"     ** NO INPUT DATA **");
	}

	static void VisualiseOutput(string[] lines, bool clearScreen = false)
	{
		if (lines is null or []) {
			return;
		}

		if (clearScreen) {
			Console.Clear();
		}

		Console.WriteLine(string.Join(Environment.NewLine, lines));
	}


	static void OutputExceptions(IEnumerable<SolutionPhaseResult> solveResults)
	{
		foreach (SolutionPhaseResult result in solveResults.Where(r => r.Exception is not null)) {
			Console.WriteLine();
			AnsiConsole.WriteException(result.Exception!, ExceptionFormats.ShortenPaths | ExceptionFormats.ShortenTypes |
					ExceptionFormats.ShortenMethods | ExceptionFormats.ShowLinks);
			Console.WriteLine();
			Console.ResetColor();
		}
	}

	static void OutputTimings(TimeSpan elapsed)
	{
		if (elapsed.TotalNanoseconds <= 999) {
			Console.Write($" {elapsed.TotalNanoseconds,4:F0}ns");
		} else if (elapsed.TotalMicroseconds <= 999) {
			Console.Write($" {elapsed.TotalMicroseconds,4:F0}µs");
		} else if (elapsed.TotalMilliseconds <= 999) {
			Console.Write($" {elapsed.TotalMilliseconds,4:F0}ms");
		} else {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write($" {elapsed.TotalSeconds,5:F1}s");
			Console.ResetColor();
		}
	}
}
