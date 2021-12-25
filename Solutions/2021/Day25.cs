namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 25: Sea Cucumber
/// https://adventofcode.com/2021/day/25
/// </summary>
[Description("Sea Cucumber")]
public class Day25 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	const char FLOOR = '.';
	const char CUCUMBER_RIGHT = '>';
	const char CUCUMBER_DOWN = 'v';

	private static int Solution1(string[] input) {
		char[] seaFloor = input
			.SelectMany(i => i.ToCharArray())
			.ToArray();

		int cols = input[0].Length;
		int rows = input.Length;
		int steps = 0;

		char[] herdCucumberRight = new char[cols * rows];
		char[] herdCucumberDown = new char[cols * rows];

		bool noMovement;
		do {
			noMovement = true;
			
			Array.Fill(herdCucumberRight, FLOOR);
			Array.Fill(herdCucumberDown, FLOOR);

			for (int y = 0; y < rows; y++) {
				for (int x = 0; x < cols; x++) {
					int i = y * cols + x;
					int nextI = y * cols + ((x + 1) % cols);
					if (seaFloor[i] == CUCUMBER_RIGHT) {
						if (seaFloor[nextI] == FLOOR) {
							noMovement = false;
							herdCucumberRight[i] = FLOOR;
							herdCucumberRight[nextI] = CUCUMBER_RIGHT;
							herdCucumberDown[nextI] = CUCUMBER_RIGHT;
						} else {
							herdCucumberRight[i] = CUCUMBER_RIGHT;
							herdCucumberDown[i] = CUCUMBER_RIGHT;
						}
					}
				}
			}

			for (int i = 0; i < seaFloor.Length; i++) {
				if (seaFloor[i] == CUCUMBER_RIGHT) {
					seaFloor[i] = FLOOR;
				}
				if (herdCucumberRight[i] == CUCUMBER_RIGHT) {
					seaFloor[i] = CUCUMBER_RIGHT;
				}
			}

			for (int y = 0; y < rows; y++) {
				for (int x = 0; x < cols; x++) {
					int i = y * cols + x;
					int nextI = (y + 1) % rows * cols + x;
					if (seaFloor[i] == CUCUMBER_DOWN) {
						if (seaFloor[nextI] == FLOOR) {
							noMovement = false;
							herdCucumberDown[i] = FLOOR;
							herdCucumberDown[nextI] = CUCUMBER_DOWN;
						} else {
							herdCucumberDown[i] = CUCUMBER_DOWN;
						}
					}
				}
			}

			for (int i = 0; i < seaFloor.Length; i++) {
				seaFloor[i] = herdCucumberDown[i];
			}

			steps++;

		} while (noMovement is false);

		return steps;
	}

	private static string Solution2(string[] _) {
		return "** CONGRATULATIONS **";
	}
}
