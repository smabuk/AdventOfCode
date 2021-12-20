namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 20: Trench Map
/// https://adventofcode.com/2021/day/20
/// </summary>
[Description("Trench Map")]
public class Day20 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		char[] imageEnhancementAlgorithm = input[0].ToArray();
		int maxCols = input[2].Length;
		int maxRows = input.Length - 2;
		char[,] image = String.Join("", input[2..]).To2dArray<char>(maxCols);

		char[,] newImage = EnhanceImage(2, imageEnhancementAlgorithm, image, maxCols, maxRows);

		return newImage.Walk2dArrayWithValues().Count(x => x.Value == LIGHT);
	}

	private static int Solution2(string[] input) {
		char[] imageEnhancementAlgorithm = input[0].ToArray();
		int maxCols = input[2].Length;
		int maxRows = input.Length - 2;
		char[,] image = String.Join("", input[2..]).To2dArray<char>(maxCols);

		char[,] newImage = EnhanceImage(50, imageEnhancementAlgorithm, image, maxCols, maxRows);

		return newImage.Walk2dArrayWithValues().Count(x => x.Value == LIGHT);
	}

	public static readonly List<(int dX, int dY)> DIRECTIONS = new()
		{
		(-1, -1),
		(0, -1),
		(1, -1)
		,
		(-1, 0),
		(0, 0),
		(1, 0)
		,
		(-1, 1),
		(0, 1),
		(1, 1)
	};

	const char DARK = '.';
	const char LIGHT = '#';

	private static char[,] EnhanceImage(int steps, char[] imageEnhancementAlgorithm, char[,] image, int maxCols, int maxRows) {
		char[,] newImage = new char[1, 1];
		bool flippingAlgorithm = imageEnhancementAlgorithm[0] == LIGHT && imageEnhancementAlgorithm[511] == DARK;
		for (int i = 1; i <= steps; i++) {
			newImage = new char[maxCols + (i * 2), maxRows + (i * 2)];
			foreach ((int x, int y) in newImage.Walk2dArray()) {
				newImage[x, y] = flippingAlgorithm switch {
					true => newImage[x, y] = (i % 2) == 1 ? '.' : LIGHT,
					false => newImage[x, y] = DARK,
				};
			}
			foreach ((int x, int y, char value) in image.Walk2dArrayWithValues()) {
				newImage[x + 1, y + 1] = value;
			}
			image = new char[maxCols + (i * 2), maxRows + (i * 2)];
			Array.Copy(newImage, 0, image, 0, image.LongLength);

			foreach (Cell<char> item in image.Walk2dArrayWithValues()) {
				string binary = "";
				foreach ((int dX, int dY) in DIRECTIONS) {
					if (item.X + dX < 0 || item.X + dX > image.GetUpperBound(0) || item.Y + dY < 0 || item.Y + dY > image.GetUpperBound(1)) {
						binary = flippingAlgorithm switch {
							true => $"{binary}{(i % 2 == 1 ? '0' : '1')}",
							false => $"{binary}{0}",
						};
					} else {
						binary = $"{binary}{(image[item.X + dX, item.Y + dY] == LIGHT ? '1' : '0')}";
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
