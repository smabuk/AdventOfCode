using System;
using System.IO;

using static AdventOfCode.Solutions.SolutionRouter;

	GetInputDataAndSolve(2020, 01, "Report Repair");
	GetInputDataAndSolve(2020, 02, "Password Philosphy");
	GetInputDataAndSolve(2020, 03, "Toboggan Trajectory");
	GetInputDataAndSolve(2020, 04, "Passport Processing");
	GetInputDataAndSolve(2020, 05, "Binary Boarding");
	GetInputDataAndSolve(2020, 06, "Custom Customs");
	GetInputDataAndSolve(2020, 07, "Handy Haversacks");
	GetInputDataAndSolve(2020, 08, "Handheld Halting");
/*
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
	GetInputDataAndSolve(2015, 01, "Not Quite Lisp");
	GetInputDataAndSolve(2015, 02, "I Was Told There Would Be No Math");
	GetInputDataAndSolve(2015, 03, "Perfectly Spherical Houses in a Vacuum");
	GetInputDataAndSolve(2015, 04, "The Ideal Stocking Stuffer");
	GetInputDataAndSolve(2015, 05, "Doesn't He Have Intern-Elves For This?");
	GetInputDataAndSolve(2015, 06, "Probably a Fire Hazard");
*/


static void GetInputDataAndSolve(int year, int day, string title) {
	string[]? input = null;
	string filename = Path.GetFullPath(Path.Combine($"{year}_{day:D2}.txt"));

	if (File.Exists(filename)) {
		input = File.ReadAllText(filename).Split("\n");
	}

	Console.WriteLine();
	Console.WriteLine($"{year} DAY {day, 2} - {title}");
	if (input is not null) {
		Console.WriteLine($"     Part 1: {SolveProblem(year, day, 1, input)}");
		Console.WriteLine($"     Part 2: {SolveProblem(year, day, 2, input)}");
	}
}

