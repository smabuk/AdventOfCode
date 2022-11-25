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
		char[] image = String.Join("", input[2..]).ToArray();

		char[] newImage = EnhanceImage(2, imageEnhancementAlgorithm, image, maxCols, maxRows);

		return newImage.Count(x => x == LIGHT);
	}

	private static int Solution2(string[] input) {
		char[] imageEnhancementAlgorithm = input[0].ToArray();
		int maxCols = input[2].Length;
		int maxRows = input.Length - 2;
		char[] image = String.Join("", input[2..]).ToArray();

		char[] newImage = EnhanceImage(50, imageEnhancementAlgorithm, image, maxCols, maxRows);

		return newImage.Count(x => x == LIGHT);
	}

	public static readonly List<(int dX, int dY)> DIRECTIONS = new()
		{ (-1, -1), (0, -1), (1, -1)
		, (-1,  0), (0,  0), (1,  0)
		, (-1,  1), (0,  1), (1,  1) };

	const char DARK  = '.';
	const char LIGHT = '#';

	private static char[] EnhanceImage(int steps, char[] imageEnhancementAlgorithm, char[] image, int maxX, int maxY) {
		char[] newImage = new char[maxX + (50 * 2) * maxY + (50 * 2)];
		bool flippingAlgorithm = imageEnhancementAlgorithm[0] == LIGHT && imageEnhancementAlgorithm[511] == DARK;

		for (int step = 1; step <= steps; step++) {
			int xSize = maxX + (step * 2);
			int ySize = maxY + (step * 2);
			newImage = new char[xSize * ySize];
			for (int y = 0; y < ySize; y++) {
				for (int x = 0; x < xSize; x++) {
					int index = y * ySize + x;
					newImage[index] = flippingAlgorithm switch {
						true => newImage[index] = (step % 2) == 1 ? DARK : LIGHT,
						false => newImage[index] = DARK,
					};
				}
			}
			for (int y = 0; y < ySize - 2; y++) {
				for (int x = 0; x < xSize - 2; x++) {
					newImage[(y + 1) * ySize + x + 1] = image[y * (ySize - 2) + x];
				}
			}
			image = (char[])newImage.Clone();

			for (int y = 0; y < xSize; y++) {
				for (int x = 0; x < ySize; x++) {
					int index = GetIndexReference(x, y, image, step, ySize, flippingAlgorithm);
					newImage[y * ySize + x] = imageEnhancementAlgorithm[index];
				}
			}
			image = (char[])newImage.Clone();
		}

		return newImage;
	}

	private static int GetIndexReference(int x, int y, char[] image, int step, int ySize, bool flippingAlgorithm) {
		int factor = 512;
		int index = 0;
		for (int i = 0; i < 9; i++) {
			factor /= 2;
			bool oob = OutOfBounds(x, y, DIRECTIONS[i].dX, DIRECTIONS[i].dY, ySize, ySize);
			index += factor * oob switch {
				true => flippingAlgorithm ? (step % 2 == 1 ? 0 : 1) : 0,
				false => image[(y + DIRECTIONS[i].dY) * ySize + x + DIRECTIONS[i].dX] == LIGHT ? 1 : 0,
			};
		}

		return index;
	}

	private static bool OutOfBounds(int x, int y, int dX, int dY, int xSize, int ySize) 
		=> x + dX < 0 || x + dX >= xSize || y + dY < 0 || y + dY >= ySize;

}
