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

Console.WriteLine();
Console.Write("Press a key to continue ... ");
Console.ReadKey();

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

	//Console.WriteLine();
	Console.Write($"{year} DAY {day,2} - {title,-35}");
	if (input is not null) {
		Console.Write($"  Part 1:  {SolveProblem(year, day, 1, input, args),-15}");
		Console.WriteLine($"     Part 2:  {SolveProblem(year, day, 2, input, args)}");
	} else {
		Console.WriteLine($"     ** NO INPUT DATA **");
	}
}

