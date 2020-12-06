using System;
using System.IO;

using static AdventOfCode.Shared.SolutionRouter;

	GetInputDataAndSolve(2020, 01, "Report Repair");
	GetInputDataAndSolve(2020, 02, "Password Philosphy");
	GetInputDataAndSolve(2020, 03, "Toboggan Trajectory");
	GetInputDataAndSolve(2020, 04, "Passport Processing");
	GetInputDataAndSolve(2020, 05, "Binary Boarding");
/*
	GetInputDataAndSolve(2020, 06, "");
	GetInputDataAndSolve(2020, 07, "");
	GetInputDataAndSolve(2020, 08, "");
	GetInputDataAndSolve(2020, 09, "");
	GetInputDataAndSolve(2020, 10, "");
	GetInputDataAndSolve(2020, 11, "");
	GetInputDataAndSolve(2020, 12, "");
	GetInputDataAndSolve(2020, 13, "");
	GetInputDataAndSolve(2020, 14, "");
	GetInputDataAndSolve(2020, 15, "");
	GetInputDataAndSolve(2020, 16, "");
	GetInputDataAndSolve(2020, 17, "");
	GetInputDataAndSolve(2020, 18, "");
	GetInputDataAndSolve(2020, 19, "");
	GetInputDataAndSolve(2020, 20, "");
	GetInputDataAndSolve(2020, 21, "");
	GetInputDataAndSolve(2020, 22, "");
	GetInputDataAndSolve(2020, 23, "");
	GetInputDataAndSolve(2020, 24, "");
	GetInputDataAndSolve(2020, 25, "");
*/


void GetInputDataAndSolve(int year, int day, string title) {
	string[]? input = null;
	string filename = Path.GetFullPath(Path.Combine($"{year}_{day:D2}.txt"));

	if (File.Exists(filename)) {
		input = File.ReadAllText(filename).Split("\n");
	}

	Console.WriteLine();
	Console.WriteLine($"{year} DAY {day, 2} - {title}");
	Console.WriteLine($"     Part 1: {SolveProblem(year, day, 1, input)}");
	Console.WriteLine($"     Part 2: {SolveProblem(year, day, 2, input)}");
}

