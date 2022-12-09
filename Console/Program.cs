using static AdventOfCode.Solutions.SolutionRouter;

object[]? solutionArgs = Array.Empty<object>();

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.ResetColor();

DateOnly dateNow = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-5));
DateOnly date = dateNow;
if (args.Length >= 2) {
	if (int.TryParse(args[0], out int year) && int.TryParse(args[1], out int day)) {
		date = new(year, 12, day);
	}
	if (args.Length > 2) {
		solutionArgs = new object[args.Length - 2];
		for (int i = 2; i < args.Length; i++) {
			if (args[i] is "true" or "false") {
				solutionArgs[i-2] = args[i] == "true";
			}
		}
	}
}
if (args.Length == 1) {
	if (int.TryParse(args[0], out int year)) {
		date = new(year, 1, 1);
	}
}

System.Diagnostics.Stopwatch totalTimer = new();
totalTimer.Start();

if (date.Month == 12 && date.Day <= 25) {
	GetInputDataAndSolveV2(date.Year, date.Day, null, null, solutionArgs);
} else {
	for (int day = 1; day <= 25; day++) {
		if (dateNow >= new DateOnly(date.Year, 12, day)) {
			GetInputDataAndSolveV2(date.Year, day);
		}
	}
}

totalTimer.Stop();
Console.Write($" Total Elapsed time: {totalTimer.Elapsed}");

static void GetInputDataAndSolve(int year, int day, string? title = null, string[]? input = null, params object[]? args) {
	string filename = Path.GetFullPath(Path.Combine($"{year}_{day:D2}.txt"));

	if (File.Exists(filename)) {
		input = File.ReadAllText(filename).Replace("\r", "").Split("\n");
	} else {
		filename = Path.GetFullPath(Path.Combine($"../Data/{year}_{day:D2}.txt"));
		if (File.Exists(filename)) {
			input = File.ReadAllText(filename).Replace("\r", "").Split("\n");
		}
	}

	if (String.IsNullOrWhiteSpace(title)) {
		title = GetProblemDescription(year, day) ?? $"";
	}

	System.Diagnostics.Stopwatch timer = new();
	Console.Write($"{year} {day,2} {title,-38}");
	if (input is not null) {
		ConsoleColor answerColour;
		answerColour = ConsoleColor.Green;
		timer.Start();
		string Problem1Answer;
		try {
			Problem1Answer = SolveProblem(year, day, 1, input, args);
			timer.Stop();
		} catch (Exception) {
			timer.Stop();
			Problem1Answer = "** Exception";
			answerColour = ConsoleColor.Red;
		}
		Console.Write($" Pt1:");
		OutputTimings();
		Console.ForegroundColor = answerColour;
		Console.Write($"  {Problem1Answer,-16}");
		Console.ResetColor();

		answerColour = ConsoleColor.Yellow;
		timer.Restart();
		string Problem2Answer;
		try {
			Problem2Answer = SolveProblem(year, day, 2, input, args);
			timer.Stop();
			if (Problem2Answer == "** Solution not written yet **") {
				Problem2Answer = "** No Solution";
				answerColour = ConsoleColor.White;
			}
		} catch (Exception) {
			timer.Stop();
			Problem2Answer = "** Exception";
			answerColour = ConsoleColor.Red;
		}
		Console.Write($" Pt2:");
		OutputTimings();
		Console.ForegroundColor = answerColour;
		Console.WriteLine($"  {Problem2Answer}");
		Console.ResetColor();
	} else {
		Console.WriteLine($"     ** NO INPUT DATA **");
	}

	void OutputTimings() {
		int stopWatchMicroSecondsMultiplier = (int)(System.Diagnostics.Stopwatch.Frequency / 1_000_000);
		if (System.Diagnostics.Stopwatch.IsHighResolution && (timer.ElapsedTicks / stopWatchMicroSecondsMultiplier) < 10_000) {
			Console.Write($" {timer.ElapsedTicks / stopWatchMicroSecondsMultiplier,4}µs");
		} else if (timer.ElapsedMilliseconds <= 3000) {
			Console.Write($" {timer.ElapsedMilliseconds,4}ms");
		} else {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write($" {timer.Elapsed,4:%s} s");
			Console.ResetColor();
		}
	}
}
static void GetInputDataAndSolveV2(int year, int day, string? title = null, string[]? input = null, params object[]? args) {
	string filename = Path.GetFullPath(Path.Combine($"{year}_{day:D2}.txt"));

	if (File.Exists(filename)) {
		input = File.ReadAllText(filename).Replace("\r", "").Split("\n");
	} else {
		filename = Path.GetFullPath(Path.Combine($"../Data/{year}_{day:D2}.txt"));
		if (File.Exists(filename)) {
			input = File.ReadAllText(filename).Replace("\r", "").Split("\n");
		}
	}

	if (String.IsNullOrWhiteSpace(title)) {
		title = GetProblemDescription(year, day) ?? $"";
	}

	System.Diagnostics.Stopwatch timer = new();
	Console.Write($"{year} {day,2} {title,-38}");
	if (input is not null) {
		ConsoleColor answerColour;
		answerColour = ConsoleColor.Green;
		timer.Start();
		string Problem1Answer;
		string Problem2Answer;
		try {
			(Problem1Answer, Problem2Answer) = SolveDay(year, day, input, args);
			timer.Stop();
			Console.Write($" Pt1:");
			OutputTimings();
			Console.ForegroundColor = answerColour;
			Console.Write($"  {Problem1Answer,-16}");
			Console.ResetColor();

			answerColour = ConsoleColor.Yellow;
			if (Problem2Answer == "** Solution not written yet **") {
				Problem2Answer = "** No Solution";
				answerColour = ConsoleColor.White;
			}
			Console.Write($" Pt2:");
			OutputTimings();
			Console.ForegroundColor = answerColour;
			Console.WriteLine($"  {Problem2Answer}");
			Console.ResetColor();
		} catch (Exception) {
			timer.Stop();
			Problem1Answer = "** Exception";
			answerColour = ConsoleColor.Red;
			Console.ForegroundColor = answerColour;
			Console.WriteLine($"  {Problem1Answer,-16}");
			Console.ResetColor();
		}
	} else {
		Console.WriteLine($"     ** NO INPUT DATA **");
	}

	void OutputTimings() {
		int stopWatchMicroSecondsMultiplier = (int)(System.Diagnostics.Stopwatch.Frequency / 1_000_000);
		if (System.Diagnostics.Stopwatch.IsHighResolution && (timer.ElapsedTicks / stopWatchMicroSecondsMultiplier) < 10_000) {
			Console.Write($" {timer.ElapsedTicks / stopWatchMicroSecondsMultiplier,4}µs");
		} else if (timer.ElapsedMilliseconds <= 3000) {
			Console.Write($" {timer.ElapsedMilliseconds,4}ms");
		} else {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write($" {timer.Elapsed,4:%s} s");
			Console.ResetColor();
		}
	}
}

