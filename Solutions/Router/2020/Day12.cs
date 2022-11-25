namespace AdventOfCode.Solutions.Year2020;

/// <summary>
/// Day 12: Rain Risk
/// https://adventofcode.com/2020/day/12
/// </summary>
[Description("Rain Risk")]
public class Day12 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record Instruction(string Command, int Value);

	private static int Solution1(string[] input) {
		IEnumerable<Instruction> instructions = input.Select(i => ParseLine(i));

		int dX = 1; //East
		int dY = 0;
		int x = 0;
		int y = 0;
		foreach (Instruction? instruction in instructions) {
			switch (instruction.Command) {
				case "N":
					y += instruction.Value;
					break;
				case "S":
					y -= instruction.Value;
					break;
				case "E":
					x += instruction.Value;
					break;
				case "W":
					x -= instruction.Value;
					break;
				case "L":
				case "R":
					(dX, dY) = ChangeDirection((dX, dY), instruction.Command, instruction.Value);
					break;
				case "F":
					x += instruction.Value * dX;
					y += instruction.Value * dY;
					break;
				default:
					break;
			}
		}

		return Math.Abs(x) + Math.Abs(y);
	}



	private static int Solution2(string[] input) {
		IEnumerable<Instruction> instructions = input.Select(i => ParseLine(i));

		int x = 0, y = 0;
		int waypointX = 10, waypointY = 1;
		foreach (Instruction? instruction in instructions) {
			switch (instruction.Command) {
				case "N":
					waypointY += instruction.Value;
					break;
				case "S":
					waypointY -= instruction.Value;
					break;
				case "E":
					waypointX += instruction.Value;
					break;
				case "W":
					waypointX -= instruction.Value;
					break;
				case "L":
				case "R":
					(waypointX, waypointY) = RotateWaypoint((waypointX, waypointY), instruction.Command, instruction.Value);
					break;
				case "F":
					x += instruction.Value * waypointX;
					y += instruction.Value * waypointY;
					break;
				default:
					break;
			}
		}
		return Math.Abs(x) + Math.Abs(y);
	}

	static (int, int) ChangeDirection((int dX, int dY) current, string direction, int Value) {
		(int, int)[] VECTORS = {    (1, 0), (0, -1), (-1, 0), (0, 1),
										(1, 0), (0, -1), (-1, 0), (0, 1),
										(1, 0), (0, -1), (-1, 0), (0, 1)  };
		int move = (Value / 90) % 4;
		int currentIndex = 0;
		for (int i = 4; i <= 7; i++) {
			if (current == VECTORS[i]) {
				currentIndex = i;
				break;
			}
		}
		return direction switch {
			"L" => VECTORS[currentIndex - move],
			"R" => VECTORS[currentIndex + move],
			_ => (-999, -999)
		};
	}

	private static (int wayPointX, int wayPointY) RotateWaypoint((int X, int Y) waypoint, string command, int value) {
		return (command, value) switch {
			("_", 180) => (-waypoint.X, -waypoint.Y),
			("R", 90) or ("L", 270) => (waypoint.Y, -waypoint.X),
			("L", 90) or ("R", 270) => (-waypoint.Y, waypoint.X),
			_ => (waypoint.X, waypoint.Y)
		};

	}

	private static Instruction ParseLine(string input) {
		Match match = Regex.Match(input, @"([NSEWLRF])(\d+)");
		if (match.Success) {
			return new(match.Groups[1].Value, int.Parse(match.Groups[2].Value));
		}
		return null!;
	}
}
