using System.Diagnostics;

using static AdventOfCode.Solutions.SolutionRouter;

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

if (date.Month == 12 && date.Day <= 25) {
	GetInputDataAndSolve(date.Year, date.Day);
} else {
	for (int day = 1; day <= 25; day++) {
		GetInputDataAndSolve(date.Year, day);
	}
}

//Console.WriteLine();
//Console.Write("Press a key to continue ... ");
//Console.ReadKey();

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

	Stopwatch timer = new();

	Console.Write($"{year} {day,2} {title,-38}");
	if (input is not null) {
		timer.Start();
		string Problem1Answer;
		try {
			Problem1Answer = SolveProblem(year, day, 1, input, args);
		} catch (Exception) {
			Problem1Answer = "** Exception";
		}
		timer.Stop();
		Console.Write($" Pt1: {timer.ElapsedMilliseconds,4}ms  {Problem1Answer,-16}");

		timer.Restart();
		string Problem2Answer;
		try {
			Problem2Answer = SolveProblem(year, day, 2, input, args);
			if (Problem2Answer == "** Solution not written yet **") {
				Problem2Answer = "** No Solution";
			}
		} catch (Exception) {
			Problem2Answer = "** Exception";
		}
		timer.Stop();
		Console.WriteLine($"Pt2: {timer.ElapsedMilliseconds,5}ms  {Problem2Answer}");
	} else {
		Console.WriteLine($"     ** NO INPUT DATA **");
	}
}

