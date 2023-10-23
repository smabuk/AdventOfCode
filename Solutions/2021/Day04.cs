namespace AdventOfCode.Solutions._2021;

/// <summary>
/// Day 04: Giant Squid
/// https://adventofcode.com/2021/day/4
/// </summary>
[Description("Giant Squid")]
public class Day04 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		List<int> numberOrder = input[0].Split(",").Select(x => int.Parse(x)).ToList();
		List<BingoBoard> bingoBoards = ParseBingoBoards(input[2..]);

		foreach (int number in numberOrder) {
			foreach (BingoBoard board in bingoBoards) {
				if (board.MarkNo(number)) {
					return board.WinningValue;
				}
			}
		}

		return -1;
	}

	private static int Solution2(string[] input) {
		List<int> numberOrder = input[0].Split(",").Select(x => int.Parse(x)).ToList();
		List<BingoBoard> bingoBoards = ParseBingoBoards(input[2..]);

		int lastWinningBoardResult = 0;
		foreach (int number in numberOrder) {
			foreach (BingoBoard board in bingoBoards) {
				if (board.MarkNo(number)) {
					lastWinningBoardResult = board.WinningValue;
				}
			}
			_ = bingoBoards.RemoveAll(b => b.IsWin);
		}

		return lastWinningBoardResult;
	}

	class BingoBoard {
		readonly List<List<int>> _lines = new();
		readonly List<int> _unmarkedNos = new();

		public bool IsWin = false;
		public int WinningValue;

		public BingoBoard(List<int> numbers) {
			_unmarkedNos = numbers;

			for (int i = 0; i < 5; i++) {
				// Rows
				_lines.Add(numbers.Skip(i * 5).Take(5).ToList());
				// Columns
				_lines.Add(
					Enumerable.Range(0, 5)
					.Select(col => numbers.ElementAt((col * 5) + i))
					.ToList()
				);
			}
		}

		public bool MarkNo(int number) {
			_ = _unmarkedNos.Remove(number);
			foreach (List<int> line in _lines) {
				if (line.Remove(number) && line.Count == 0) {
					IsWin = true;
					WinningValue = number * _unmarkedNos.Sum();
				}
			}
			return IsWin;
		}
	};

	private static List<BingoBoard> ParseBingoBoards(string[] input) {
		List<BingoBoard> bingoBoards = new();
		for (int i = 0; i < ((input.Length + 1) / 6); i++) {
			List<int> numbers = Enumerable
				.Range(0, 5)
				.SelectMany(rowNo =>
					input[(i * 6) + rowNo]
					.Split(" ", StringSplitOptions.RemoveEmptyEntries)
					.Select(x => int.Parse(x)))
				.ToList();
			bingoBoards.Add(new BingoBoard(numbers));
		}
		return bingoBoards;
	}
}
