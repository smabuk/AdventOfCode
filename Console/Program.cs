Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.ResetColor();

(DateOnly date, bool showVisuals, bool isDebug, object[]? solutionArgs) = ParseCommandLine(args);

System.Diagnostics.Stopwatch totalTimer = new();
totalTimer.Start();

if (date.Month == 12 && date.Day <= 25) {
	GetInputDataAndSolve(date.Year, date.Day, null, null, showVisuals, isDebug, solutionArgs);
} else {
	DateOnly dateNow = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-5));
	//Console.WriteLine($"Year dd Description                              Init        Part 1                       Part 2                ");
	//Console.WriteLine($"---- -- -------------------------------------  ------  ---------------------------  ----------------------------");
	for (int day = 1; day <= 25; day++) {
		if (dateNow >= new DateOnly(date.Year, 12, day)) {
			GetInputDataAndSolve(date.Year, day);
		}
	}
}

totalTimer.Stop();
Console.Write($" Total Elapsed time: {totalTimer.Elapsed}");

static void GetInputDataAndSolve(int year, int day, string? title = null, string[]? input = null, bool showVisuals = false, bool isDebug = false, params object[]? args)
{
	string filename = Path.GetFullPath(Path.Combine($"{year}_{day:D2}.txt"));

	if (File.Exists(filename)) {
		input = File.ReadAllText(filename).ReplaceLineEndings().Split(Environment.NewLine);
	} else {
		filename = Path.GetFullPath(Path.Combine($"{Environment.GetEnvironmentVariable("AocData")}", $"{year}_{day:D2}.txt"));
		if (File.Exists(filename)) {
			input = File.ReadAllText(filename).ReplaceLineEndings().Split(Environment.NewLine);
		} else {
			filename = Path.GetFullPath(Path.Combine("..", "Data", $"{year}_{day:D2}.txt"));
			if (File.Exists(filename)) {
				input = File.ReadAllText(filename).ReplaceLineEndings().Split(Environment.NewLine);
			}
		}
	}

	if (String.IsNullOrWhiteSpace(title)) {
		title = GetProblemDescription(year, day) ?? $"";
	}

	System.Diagnostics.Stopwatch timer = new();
	Console.Write($"{year} {day,2} {title,-38}");
	if (input is not null) {
		ConsoleColor answerColour;
		timer.Start();
		Action<string[], bool>? visualiser = showVisuals ? new Action<string[], bool>(VisualiseOutput) : null;
		IEnumerable<SolutionPhase> solveResults = SolveDay(year, day, input, visualiser, args);
		foreach (SolutionPhase result in solveResults) {
			if (result.Phase == SolutionPhase.PHASE_INIT) {
				OutputTimings(result.Elapsed);
			} else if (result.Phase == SolutionPhase.PHASE_PART1) {
				answerColour = ConsoleColor.Green;
				OutputTimings(result.Elapsed);
				Console.ForegroundColor = answerColour;
				Console.Write($" Pt1:");
				if (result.Answer.StartsWith('*')) {
					Console.ForegroundColor = ConsoleColor.Red;
				}
				Console.Write($" {result.Answer,-16}");
			} else if (result.Phase == SolutionPhase.PHASE_PART2) {
				answerColour = ConsoleColor.Yellow;
				OutputTimings(result.Elapsed);
				Console.ForegroundColor = answerColour;
				Console.Write($" Pt2:");
				if (result.Answer.StartsWith('*')) {
					Console.ForegroundColor = ConsoleColor.Red;
				}
				Console.Write($" {result.Answer,-16}");
			} else if (result.Phase == SolutionPhase.EXCEPTION_PART1) {
				answerColour = ConsoleColor.Green;
				OutputTimings(result.Elapsed);
				Console.ForegroundColor = answerColour;
				Console.Write($" Pt1:");
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write($" {SolutionPhase.EXCEPTION_MESSAGE,-16}");
			} else if (result.Phase == SolutionPhase.EXCEPTION_PART2) {
				answerColour = ConsoleColor.Yellow;
				OutputTimings(result.Elapsed);
				Console.ForegroundColor = answerColour;
				Console.Write($" Pt2:");
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write($" {SolutionPhase.EXCEPTION_MESSAGE,-16}");
			}
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


	static void OutputExceptions(IEnumerable<SolutionPhase> solveResults)
	{
		foreach (SolutionPhase result in solveResults.Where(r => r.Result is SolutionResultType.EXCEPTION)) {
			Console.WriteLine();
			AnsiConsole.WriteException(result.Exception, ExceptionFormats.ShortenPaths | ExceptionFormats.ShortenTypes |
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

static void ShowHelp()
{
	Console.WriteLine($"Calculates and displays solutions to the Advent of Code problems");
	Console.WriteLine();
	Console.WriteLine($"[YYYY [dd]] [/visual] [arg1 arg2 arg3 ...]");
	Console.WriteLine();
	Console.WriteLine($"   YYYY         Defaults to the latest year of puzzles.");
	Console.WriteLine($"     dd         The puzzle day to solve (e.g. 5).");
	Console.WriteLine($"     /D         Show debug information (e.g. details of exceptions.");
	Console.WriteLine($"     /V         Uses the visualiser if one exists.");
}

static (DateOnly date, bool showVisuals, bool isDebug, object[]? solutionArgs) ParseCommandLine(string[] args)
{
	DateOnly date = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-5));
	bool showVisuals = false;
	bool isDebug = false;
	object[]? solutionArgs = [];
	for (int i = 0; i < args.Length; i++) {
		if (args[i] is "--help" or "/?" or "-h") {
			ShowHelp();
			Environment.Exit(0);
		}
	}
	if (args.Length > 2) {
		for (int i = 2; i < args.Length; i++) {
			if (args[i].ToLowerInvariant() is "true" or "false") {
				solutionArgs = new object[solutionArgs.Length + 1];
				solutionArgs[^1] = args[i] == "true";
			} else if (args[i].ToLowerInvariant() is "/v" or "/visual" or "-v" or "--visual") {
				showVisuals = true;
			} else if (args[i].ToLowerInvariant() is "/d" or "/debug" or "-d" or "--debug") {
				isDebug = true;
			} else {
				solutionArgs = new object[solutionArgs.Length + 1];
				solutionArgs[^1] = args[i];
			}
		}
	}
	if (args.Length >= 2) {
		if (int.TryParse(args[0], out int year) && int.TryParse(args[1], out int day)) {
			date = new(year, 12, day);
		}
	}
	if (args.Length == 1) {
		if (int.TryParse(args[0], out int year)) {
			date = new(year, 1, 1);
		}
	}
	return (date, showVisuals, isDebug, solutionArgs);
}
