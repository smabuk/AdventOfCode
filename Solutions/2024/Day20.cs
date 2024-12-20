using System.Diagnostics.Tracing;

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 20: Race Condition
/// https://adventofcode.com/2024/day/20
/// </summary>
[Description("Race Condition")]
public static partial class Day20 {

	private const char TRACK = '.';
	private const char WALL  = '#';
	private const char EDGE  = '+';
	private const char START = 'S';
	private const char END   = 'E';
	private const char MOVE1 = '1';
	private const char MOVE2 = '2';

	private static char[,] _raceTrack = default!;
	private static Point _start = Point.Zero;
	private static Point _end = Point.Zero;
	private static List<Point> _raceRoute = [];
	private static int _raceLength;

	[Init]
	public static void LoadRaceTrack(string[] input)
	{
		_raceTrack = input.To2dArray();

		_start      = _raceTrack.ForEachCell().Single(cell => cell.Value == START);
		_end        = _raceTrack.ForEachCell().Single(cell => cell.Value == END);
		_raceRoute  = _raceTrack.CalculateRacePath(_start, _end);
		_raceLength = _raceRoute.Count;

		for (int x = 0; x < _raceTrack.ColsCount(); x++) {
			_raceTrack[x, 0] = EDGE;
			_raceTrack[x, _raceTrack.ColsMax()] = EDGE;
		}

		for (int y = 0; y < _raceTrack.RowsCount(); y++) {
			_raceTrack[0, y] = EDGE;
			_raceTrack[_raceTrack.RowsMax(), y] = EDGE;
		}
	}

	public static int Part1(string[] input, params object[]? args)
	{
		int psTarget = args.PsTarget();
		string cheatGrid = args.CheatGrid();
		int count = 0;
		List<Cheat> cheats = [];

		if (cheatGrid is not "") {
			char[,] track = cheatGrid.ReplaceLineEndings().Replace(Environment.NewLine, "").To2dArray(_raceTrack.ColsCount(), _raceTrack.RowsCount());
			Point move1 = track.ForEachCell().Single(cell => cell.Value == MOVE1);
			Point move2 = track.ForEachCell().Single(cell => cell.Value == MOVE2);
			cheats.Add(new(move1, move2));
		} else {
			foreach (Point point in _raceRoute) {
				foreach (Cell<char> possibleMove1 in _raceTrack.GetAdjacentCells(point).Where(c => c.Value is WALL)) {
					foreach (Cell<char> possibleMove2 in _raceTrack.GetAdjacentCells(possibleMove1).Where(c => c.Value is TRACK && c.Index != point)) {
						cheats.Add(new(possibleMove1, possibleMove2));
					}
				}
			}
		}

		var timeSavings = cheats.Select(cheat => cheat.FindTimeSaving(_raceTrack, _raceRoute)).CountBy(ps => ps);


		return timeSavings.Where(ps => ps.Key >= psTarget).Sum(ps => ps.Value);
	}

	public static string Part2(string[] input, params object[]? args) => NO_SOLUTION_WRITTEN_MESSAGE;

	private static List<Point> CalculateRacePath(this char[,] track, Point start, Point end)
	{
		List<Point> visited = [start]; 
		Point current = _start;
		while (current != end) {
			current = track.GetAdjacentCells(current).Single(cell => cell.Value is TRACK or END && visited.DoesNotContain(cell.Index));
			visited.Add(current);
		}

		return visited;
	}

	private static int FindTimeSaving(this Cheat cheat, char[,] track, List<Point> route)
	{
		if (route.DoesNotContain(cheat.Move2)) { return 0; }

		int firstIndex  = route.Index().First(p => (track.GetAdjacentCells(p.Item).Select(c => c.Index).Contains(cheat.Move1))).Index;
		int secondIndex = route.Index().First(p => (track.GetAdjacentCells(p.Item).Select(c => c.Index).Contains(cheat.Move2))).Index;


		return secondIndex - firstIndex - 1;
	}

	private record Cheat(Point Move1, Point Move2);

	private static int PsTarget(this object[]? args) => GetArgument(args, 1, 100);
	private static string CheatGrid(this object[]? args) => GetArgument(args, 2, "");

}
