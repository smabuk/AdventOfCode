using System;
using System.Diagnostics;
using System.IO;

using static AdventOfCode.Solutions.SolutionRouter;
GetInputDataAndSolve(2020, 01, "Report Repair");
GetInputDataAndSolve(2020, 02, "Password Philosophy");
GetInputDataAndSolve(2020, 03, "Toboggan Trajectory");
GetInputDataAndSolve(2020, 04, "Passport Processing");
GetInputDataAndSolve(2020, 05, "Binary Boarding");
GetInputDataAndSolve(2020, 06, "Custom Customs");
GetInputDataAndSolve(2020, 07, "Handy Haversacks");
GetInputDataAndSolve(2020, 08, "Handheld Halting");
GetInputDataAndSolve(2020, 09, "Encoding Error",null, 25);
GetInputDataAndSolve(2020, 10, "Adapter Array");
/*
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
	GetInputDataAndSolve(2015, 07, "Some Assembly Required", null, "a");
	GetInputDataAndSolve(2015, 08, "Matchsticks");
	GetInputDataAndSolve(2015, 09, "All in a Single Night");
	GetInputDataAndSolve(2015, 10, "Elves Look, Elves Say");

	GetInputDataAndSolve(2015, 11, "");
	GetInputDataAndSolve(2015, 12, "");
	GetInputDataAndSolve(2015, 13, "");
	GetInputDataAndSolve(2015, 14, "");
	GetInputDataAndSolve(2015, 15, "");
	GetInputDataAndSolve(2015, 16, "");
	GetInputDataAndSolve(2015, 17, "");
	GetInputDataAndSolve(2015, 18, "");
	GetInputDataAndSolve(2015, 19, "");
	GetInputDataAndSolve(2015, 20, "");
	GetInputDataAndSolve(2015, 21, "");
	GetInputDataAndSolve(2015, 22, "");
	GetInputDataAndSolve(2015, 23, "");
	GetInputDataAndSolve(2015, 24, "");
	GetInputDataAndSolve(2015, 25, "");
*/


static void GetInputDataAndSolve(int year, int day, string title, string[]? input = null, params object[]? args) {
	string filename = Path.GetFullPath(Path.Combine($"{year}_{day:D2}.txt"));

	if (File.Exists(filename)) {
		input = File.ReadAllText(filename).Split("\n");
	}

	Console.WriteLine();
	Console.WriteLine($"{year} DAY {day, 2} - {title}");
	if (input is not null) {
		Stopwatch sw = new Stopwatch();
		sw.Start();
		Console.WriteLine($"  {sw.ElapsedTicks, 5} ticks   Part 1: {SolveProblem(year, day, 1, input, args)}");
		sw.Restart();
		Console.WriteLine($"  {sw.ElapsedTicks, 5} ticks   Part 2: {SolveProblem(year, day, 2, input, args)}");
		sw.Stop();
	} else {
		Console.WriteLine($"     ** NO INPUT DATA **");
	}

}

