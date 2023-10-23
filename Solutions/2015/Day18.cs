namespace AdventOfCode.Solutions._2015;

/// <summary>
/// Day 18: Like a GIF For Your Yard
/// https://adventofcode.com/2015/day/18
/// </summary>
[Description("Like a GIF For Your Yard")]
public class Day18 {

	public static string Part1(string[] input, params object[]? args) {
		int noOfIterations = GetArgument(args, 1, 100);
		return Solution1(input, noOfIterations).ToString();
	}

	public static string Part2(string[] input, params object[]? args) {
		int noOfIterations = GetArgument(args, 1, 100);
		return Solution2(input, noOfIterations).ToString();
	}

	public const char OFF = '.';
	public const char ON = '#';

	private static readonly List<(int dX, int dY)> DIRECTIONS = [(0, -1), (0, 1), (-1, 0), (1, 0), (-1, -1), (-1, 1), (1, -1), (1, 1)];

	private static int Solution1(string[] input, int noOfIterations) {
		int width = input[0].Length;
		int height = input.Length;

		char[,] lights = ParseInput(input);


		char[,] nextLights = lights;
		int countOn = 0;
		for (int i = 0; i < noOfIterations; i++) {

			countOn = 0;
			nextLights = new char[width, height];

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					char current = lights[x, y];
					char next = current;
					List<char> adjacent = GetAdjacentLights(x, y, width, height, lights);
					if (current == OFF) {
						if (adjacent.Count(s => s == ON) == 3) {
							next = ON;
						}
					} else if (current == ON) {
						if (adjacent.Count(s => s == ON) is not 2 and not 3) {
							next = OFF;
						}
					}
					nextLights[x, y] = next;
					if (next == ON) {
						countOn++;
					}
				}
			}
			lights = nextLights;
		}

		return countOn;
	}

	private static int Solution2(string[] input, int noOfIterations) {
		int width = input[0].Length;
		int height = input.Length;

		char[,] lights = ParseInput(input);

		lights[0, 0] = ON;
		lights[0, height - 1] = ON;
		lights[width - 1, 0] = ON;
		lights[width - 1, height - 1] = ON;

		char[,] nextLights = lights;
		int countOn = 0;
		for (int i = 0; i < noOfIterations; i++) {

			countOn = 0;
			nextLights = new char[width, height];

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					char current = lights[x, y];
					char next = current;
					List<char> adjacent = GetAdjacentLights(x, y, width, height, lights);
					if (current == OFF) {
						if (adjacent.Count(s => s == ON) == 3) {
							next = ON;
						}
					} else if (current == ON) {
						if (adjacent.Count(s => s == ON) is not 2 and not 3) {
							next = OFF;
						}
					}
					nextLights[x, y] = next;
					if (next == ON) {
						countOn++;
					}
				}
			}
			nextLights[0, 0] = ON;
			nextLights[0, height - 1] = ON;
			nextLights[width - 1, 0] = ON;
			nextLights[width - 1, height - 1] = ON;
			lights = nextLights;
		}

		countOn = 0;
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				if (lights[x, y] == ON) {
					countOn++;
				}
			}
		}

		return countOn;
	}

	private static char[,] ParseInput(string[] input) {
		int width = input[0].Length;
		int height = input.Length;
		char[,] lights = new char[width, height];

		for (int y = 0; y < height; y++) {
			string itemLine = input[y];

			for (int x = 0; x < width; x++) {
				lights[x, y] = itemLine[x];
			}
		}
		return lights;
	}

	public static List<char> GetAdjacentLights(int X, int Y, int width, int height, char[,] lights) {
		List<char> adjacent = [];
		foreach (var (dX, dY) in DIRECTIONS) {
			int x = X + dX;
			int y = Y + dY;
			if (x >= 0 && x < width && y >= 0 && y < height) {
				adjacent.Add(lights[x, y]);
			}
		}
		return adjacent;
	}
}
