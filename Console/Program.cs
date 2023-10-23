using static AdventOfCode.Solutions.SolutionRouter;

object[]? solutionArgs = [];

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
	GetInputDataAndSolve(date.Year, date.Day, null, null, solutionArgs);
} else {
	for (int day = 1; day <= 25; day++) {
		if (dateNow >= new DateOnly(date.Year, 12, day)) {
			GetInputDataAndSolve(date.Year, day);
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
		timer.Start();
		foreach (var result in SolveDay(year, day, input, args)) {
			if (result.Phase == "Init") {
				OutputTimings(result.Elapsed);
			} else if (result.Phase == "Part1") {
				answerColour = ConsoleColor.Green;
				OutputTimings(result.Elapsed);
				Console.ForegroundColor = answerColour;
				Console.Write($" Pt1:");
				if (result.Answer.StartsWith('*')) {
					Console.ForegroundColor = ConsoleColor.Red;
				}
				Console.Write($"  {result.Answer,-16}");
			} else if (result.Phase == "Part2") {
				answerColour = ConsoleColor.Yellow;
				OutputTimings(result.Elapsed);
				Console.ForegroundColor = answerColour;
				Console.Write($" Pt2:");
				if (result.Answer.StartsWith('*')) {
					Console.ForegroundColor = ConsoleColor.Red;
				}
				Console.Write($"  {result.Answer,-16}");
			}
			Console.ResetColor();
		};

		Console.ResetColor();
		Console.WriteLine();
	} else {
		Console.WriteLine($"     ** NO INPUT DATA **");
	}

	static void OutputTimings(TimeSpan elapsed) {
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

