namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 11: Chronal Charge
/// https://adventofcode.com/2018/day/11
/// </summary>
[Description("Chronal Charge")]
public sealed partial class Day11 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static string Solution1(string[] input) {
		int gridSerialNo = input[0].As<int>();

		int[,] fuelCells = new int[300, 300];
		foreach ((int cellX, int cellY) in fuelCells.Indexes()) {
			int value = CalculatePowerValue(gridSerialNo, cellX + 1, cellY + 1);
			fuelCells[cellX, cellY] = value;
		}

		int maxValue = int.MinValue;
		string topLeft = "";
		foreach (Cell<int> fuelCell in fuelCells.ForEachCell()) {
			if (fuelCell.X < 298 && fuelCell.Y < 298) {
				int value = fuelCell.Value
					+ fuelCells[fuelCell.X + 1, fuelCell.Y]
					+ fuelCells[fuelCell.X + 2, fuelCell.Y]
					+ fuelCells[fuelCell.X + 0, fuelCell.Y + 1]
					+ fuelCells[fuelCell.X + 1, fuelCell.Y + 1]
					+ fuelCells[fuelCell.X + 2, fuelCell.Y + 1]
					+ fuelCells[fuelCell.X + 0, fuelCell.Y + 2]
					+ fuelCells[fuelCell.X + 1, fuelCell.Y + 2]
					+ fuelCells[fuelCell.X + 2, fuelCell.Y + 2];
				if (value > maxValue) {
					maxValue = value;
					topLeft = $"{fuelCell.X + 1},{fuelCell.Y + 1}";
				}
			}
		}

		return topLeft;
	}

	private static string Solution2(string[] input)
	{
		const int MAX_GRID_SIZE = 21; // This works for me and keeps the speed down
		int gridSerialNo = input[0].As<int>();

		int[,] fuelCells = new int[300, 300];
		foreach ((int cellX, int cellY) in fuelCells.Indexes()) {
			int value = CalculatePowerValue(gridSerialNo, cellX + 1, cellY + 1);
			fuelCells[cellX, cellY] = value;
		}

		int maxValue = int.MinValue;
		string topLeft = "";
		for (int y = 0; y < 300; y++) {
			for (int x = 0; x < 300; x++) {
				for (int gridSize = 3; gridSize < MAX_GRID_SIZE; gridSize++) {
					if (x + gridSize >= 300 || y + gridSize >= 300) {
						break;
					}
					int value = 0;
					for (int dy = 0; dy < gridSize; dy++) {
						for (int dx = 0; dx < gridSize; dx++) {
							value += fuelCells[x + dx, y + dy];
						}
					}
					if (value > maxValue) {
						maxValue = value;
						topLeft = $"{x + 1},{y + 1},{gridSize}";
					}
				}
			}
		}

		return topLeft;
	}

	public static int CalculatePowerValue(int gridSerialNo, int x, int y)
	{
		int rackId = x + 10;
		int value = rackId * y;
		value += gridSerialNo;
		value *= rackId;
		value = value switch
		{
			>= 100 => $"{value}"[^3] - '0',
			_ => 0,
		};
		value -= 5;
		return value;
	}
}
