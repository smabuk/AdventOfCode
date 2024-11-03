using static AdventOfCode.Solutions._2016.Day11Constants;
using static AdventOfCode.Solutions._2016.Day11Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 11: Radioisotope Thermoelectric Generators
/// https://adventofcode.com/2016/day/11
/// </summary>
[Description("Radioisotope Thermoelectric Generators")]
public sealed partial class Day11 {

	public static string Part1(string[] input, Action<string[], bool>? visualise = null, params object[]? args)
		=> Solution1(input, visualise).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();


	private static int Solution1(string[] input, Action<string[], bool>? visualise = null)
	{
		int noOfSteps = 0;
		int elevatorFloor = 0;
		List<Thing>[] floors = [];
		floors = PopulateInitialState(input, floors);
		floors.VisualiseFloors(elevatorFloor, "Initial", visualise);

		floors.VisualiseFloors(elevatorFloor, "Final", visualise);
		return noOfSteps;
	}

	private static List<Thing>[] PopulateInitialState(string[] input, List<Thing>[] floors)
	{
		floors = [[], [], [], []];
		foreach (string line in input) {
			string[] tokens = line.TrimmedSplit([' ', ',', '.']);
			int floorNo = tokens[1].GetIntFromOrdinal();
			int floorNoIndex = floorNo - 1;
			for (int i = 5; i < tokens.Length; i++) {
				if (tokens[i] == "microchip") {
					floors[floorNoIndex].Add(new Microchip(tokens[i - 1].Replace("-compatible", "")));
				} else if (tokens[i] == "generator") {
					floors[floorNoIndex].Add(new Generator(tokens[i - 1]));
				}
			}
		}
		return floors;
	}

	private static string Solution2(string[] input) => NO_SOLUTION_WRITTEN_MESSAGE;
}

file static class Day11Extensions
{
	public static int GetIntFromOrdinal(this string ordinal) => ordinal switch
	{
		"first" => 1,
		"second" => 2,
		"third" => 3,
		"fourth" => 4,
		_ => throw new ArgumentOutOfRangeException(nameof(ordinal)),
	};

	public static void VisualiseFloors(this List<Thing>[] floors, int elevatorFloor, string title, Action<string[], bool>? visualise)
	{
		const char ELEVATOR = 'E';
		const char GENERATOR = 'G';
		const char MICROCHIP = 'M';

		if (visualise is not null) {
			string[] elements = [..floors.SelectMany(f => f.Select(t => t.Name)).Distinct().Order()];
			string[] floorPlan = [];
			for (int i = floors.Length - 1; i >= 0; i--) {
				string things = "";
				foreach (string element in elements) {
					bool hasMicrochip = floors[i].Any(t => t is Microchip && t.Name == element);
					bool hasGenerator = floors[i].Any(t => t is Generator && t.Name == element);
					things = hasGenerator
						? $"{things} {element.ToUpper()[0]}{element[1]}{GENERATOR}"
						: $"{things} ...";
					things = hasMicrochip
						? $"{things} {element.ToUpper()[0]}{element[1]}{MICROCHIP}"
						: $"{things} ...";
				}
				string floor = $"""F{i + 1} {(elevatorFloor == i ? ELEVATOR : ".")} {things}""";
				floorPlan = [.. floorPlan.Append(floor)];
			}

			string[] output = ["", title, .. floorPlan];
			_ = Task.Run(() => visualise?.Invoke(output, false));
		}
	}
}

internal sealed partial class Day11Types
{
	public abstract record Thing(string Name);
	public record Generator(string Element) : Thing(Element);
	public record Microchip(string Element) : Thing(Element);
}

file static class Day11Constants
{
}
