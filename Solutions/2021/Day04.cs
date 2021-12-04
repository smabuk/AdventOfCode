namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 04: Giant Squid
/// https://adventofcode.com/2021/day/4
/// </summary>
public class Day04 {

	class BingoBoard {
		readonly List<List<int>> _lines = new();
		readonly List<int> _unmarkedNos = new();

		public bool IsWin = false;
		public int WinningValue;

		public BingoBoard(List<int> boardNos) {
			_unmarkedNos = boardNos;

			for (int i = 0; i < 5; i++) {
				// Rows
				_lines.Add(boardNos.Skip(i * 5).Take(5).ToList());
				// Columns
				_lines.Add(
					Enumerable.Range(0, 5)
					.Select(col => boardNos.ElementAt((col * 5) + i))
					.ToList()
				);
			}
		}

		public bool MarkNo(int number) {
			_unmarkedNos.Remove(number);
			foreach (List<int> line in _lines) {
				line.Remove(number);
				if (line.Count == 0) {
					IsWin = true;
					WinningValue = number * _unmarkedNos.Sum();
				}
			}
			return IsWin;
		}
	};

	private static string Solution1(string[] input) {
		List<int> numberOrder = input[0].Split(",").Select(x => int.Parse(x)).ToList();
		List<BingoBoard> bingoBoards = ParseBingoBoards(input[2..]);

		foreach (int no in numberOrder) {
			foreach (BingoBoard board in bingoBoards) {
				if (board.MarkNo(no)) {
					return board.WinningValue.ToString();
				}
			}
		}

		return "** MISTAKE **";
	}

	private static string Solution2(string[] input) {
		List<int> numberOrder = input[0].Split(",").Select(x => int.Parse(x)).ToList();
		List<BingoBoard> bingoBoards = ParseBingoBoards(input[2..]);

		int lastWinningBoardResult = 0;
		foreach (int no in numberOrder) {
			foreach (BingoBoard board in bingoBoards) {
				if (board.MarkNo(no)) {
					lastWinningBoardResult = board.WinningValue;
				}
			}
			bingoBoards.RemoveAll(b => b.IsWin);
		}

		return lastWinningBoardResult.ToString();
	}

	private static List<BingoBoard> ParseBingoBoards(string[] input) {
		List<BingoBoard> bingoBoards = new();
		for (int boardNo = 0; boardNo < ((input.Length + 1) / 6); boardNo++) {
			List<int> boardNos = Enumerable
				.Range(0, 5)
				.SelectMany(rowNo =>
					input[(boardNo * 6) + rowNo]
					.Split(" ", StringSplitOptions.RemoveEmptyEntries)
					.Select(x => int.Parse(x)))
				.ToList();
			bingoBoards.Add(new BingoBoard(boardNos));
		}
		return bingoBoards;
	}




	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
