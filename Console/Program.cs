﻿using static AdventOfCode.Solutions.SolutionRouter;

Console.ResetColor();

DateOnly date = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-5));
if (args.Length == 2) {
	if (int.TryParse(args[0], out int year) && int.TryParse(args[1], out int day)) {
		date = new(year, 12, day);
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
	GetInputDataAndSolve(date.Year, date.Day);
} else {
	for (int day = 1; day <= 25; day++) {
		GetInputDataAndSolve(date.Year, day);
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

	Console.Write($"{year} {day,2} {title,-38}");
	if (input is not null) {
		ConsoleColor answerColour;
		answerColour = ConsoleColor.Green;
		System.Diagnostics.Stopwatch timer = new();
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
		if (timer.ElapsedMilliseconds <= 3000) {
			Console.Write($" {timer.ElapsedMilliseconds,4}ms");
		} else {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write($" {timer.Elapsed,4:%s} s");
			Console.ResetColor();
		}
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
				answerColour= ConsoleColor.White;
			}
		} catch (Exception) {
			timer.Stop();
			Problem2Answer = "** Exception";
			answerColour= ConsoleColor.Red;
		}
		Console.Write($" Pt2:");
		if (timer.ElapsedMilliseconds <= 3000) {
			Console.Write($" {timer.ElapsedMilliseconds,4}ms");
		} else {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write($" {timer.Elapsed,4:%s} s");
			Console.ResetColor();
		}
		Console.ForegroundColor = answerColour;
		Console.WriteLine($"  {Problem2Answer}");
		Console.ResetColor();
	} else {
		Console.WriteLine($"     ** NO INPUT DATA **");
	}
}

