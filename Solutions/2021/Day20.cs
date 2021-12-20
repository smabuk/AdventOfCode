using System;

using AdventOfCode.Solutions.Helpers;

namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 20: Trench Map
/// https://adventofcode.com/2021/day/20
/// </summary>
[Description("Trench Map")]
public class Day20 {

	public static string Part1(string[] input, params object[]? args) {
		bool testing = GetArgument(args, argumentNumber: 1, defaultResult: false);
		return Solution1(input, testing).ToString();
	}
	public static string Part2(string[] input, params object[]? args) {
		bool testing = GetArgument(args, argumentNumber: 1, defaultResult: false);
		return Solution2(input, testing).ToString();
	}

	public static readonly List<(int dX, int dY)> DIRECTIONS = new()
		{ (-1, -1), (0, -1), (1, -1) 
		, (-1,  0), (0,  0), (1,  0)
		, (-1,  1), (0,  1), (1,  1)
	};

	private static int Solution1(string[] input, bool testing) {
		char[] imageEnhancementAlgorithm = input[0].ToArray();
		char[,] image = String.Join("", input[2..]).To2dArray<char>(input[2].Length);

		int maxCols = input[2].Length;
		int maxRows = input.Length - 2;

		char[,] newImage = EnhanceImage(testing, 2, imageEnhancementAlgorithm, image, maxCols, maxRows);

		return newImage.Walk2dArrayWithValues().Count(x => x.Value == '#');
	}
	private static int Solution2(string[] input, bool testing) {
		char[] imageEnhancementAlgorithm = input[0].ToArray();
		char[,] image = String.Join("", input[2..]).To2dArray<char>(input[2].Length);

		int maxCols = input[2].Length;
		int maxRows = input.Length - 2;

		char[,] newImage = EnhanceImage(testing, 50, imageEnhancementAlgorithm, image, maxCols, maxRows);

		return newImage.Walk2dArrayWithValues().Count(x => x.Value == '#');
	}

	private static char[,] EnhanceImage(bool testing, int steps, char[] imageEnhancementAlgorithm, char[,] image, int maxCols, int maxRows) {
		char[,] newImage = new char[1, 1];
		for (int i = 1; i <= steps; i++) {
			newImage = new char[maxCols + (i * 2), maxRows + (i * 2)];
			foreach ((int x, int y) in newImage.Walk2dArray()) {
				if (testing) {
					newImage[x, y] = '.';
				} else {
					newImage[x, y] = (i % 2) == 1 ? '.' : '#';
				}
			}
			foreach ((int x, int y, char value) in image.Walk2dArrayWithValues()) {
				newImage[x + 1, y + 1] = value;
			}
			image = new char[maxCols + (i * 2), maxRows + (i * 2)];
			Array.Copy(newImage, 0, image, 0, image.LongLength);

			foreach (Cell<char> item in image.Walk2dArrayWithValues()) {
				string binary = "";
				foreach ((int dX, int dY) in DIRECTIONS) {
					try {
						binary = $"{binary}{(image[item.X + dX, item.Y + dY] == '#' ? '1' : '0')}";
					} catch (Exception) {
						if (testing) {
							binary = $"{binary}{0}";
						} else {
							binary = $"{binary}{(i % 2 == 1 ? '0' : '1')}";
						}
					}
				}
				int index = Convert.ToInt32(binary, 2);
				char newChar = imageEnhancementAlgorithm[index];
				newImage[item.X, item.Y] = newChar;
			}
			image = new char[maxCols + (i * 2), maxRows + (i * 2)];
			Array.Copy(newImage, 0, image, 0, image.LongLength);
		}

		return newImage;
	}

}
