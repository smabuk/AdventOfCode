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

	private static char[,]                _raceTrack = default!;
	private static List<Point>            _raceRoute = [];
	private static Dictionary<Point, int> _codePath  = [];
	private static Point _start = Point.Zero;
	private static Point _end   = Point.Zero;

	[Init]
	public static void LoadRaceTrack(string[] input)
	{
		_raceTrack = input.To2dArray();

		_start      = _raceTrack.ForEachCell().Single(cell => cell.Value == START);
		_end        = _raceTrack.ForEachCell().Single(cell => cell.Value == END);
		_raceRoute  = _raceTrack.CalculateRacePath(_start, _end);

		_codePath = _raceRoute.Index().ToDictionary(p => p.Item, p => p.Index);

		for (int x = 0; x < _raceTrack.ColsCount(); x++) {
			_raceTrack[x, 0] = EDGE;
			_raceTrack[x, _raceTrack.ColsMax()] = EDGE;
		}

		for (int y = 0; y < _raceTrack.RowsCount(); y++) {
			_raceTrack[0, y] = EDGE;
			_raceTrack[_raceTrack.RowsMax(), y] = EDGE;
		}
	}

	public static int Part1(string[] _, params object[]? args)
	{
		int psTarget     = args.PsTarget();
		string cheatGrid = args.CheatGridForTests();
		List<Cheat> cheats = [];

		if (cheatGrid is not "") {
			char[,] track = cheatGrid.ReplaceLineEndings().Replace(Environment.NewLine, "").To2dArray(_raceTrack.ColsCount(), _raceTrack.RowsCount());
			Point move1 = track.ForEachCell().Single(cell => cell.Value == MOVE1);
			Point move2 = track.ForEachCell().Single(cell => cell.Value == MOVE2);
			cheats.Add(new(Point.Zero, move1, move2));
		} else {
			cheats = [.. _raceRoute.FindCheats(_raceTrack, _codePath)];
		}

		return cheats
			.Select(cheat => cheat.FindTimeSaving(_raceTrack, _codePath, _raceRoute))
			.CountBy(ps => ps)
			.Where(ps => ps.Key >= psTarget)
			.Sum(ps => ps.Value);
	}

	public static string Part2(string[] _, params object[]? args)
	{
		return NO_SOLUTION_WRITTEN_MESSAGE;
		//int psTarget = args.PsTarget();
		//string cheatGrid = args.CheatGridForTests();
		//List<Cheat> cheats = [];

		//if (cheatGrid is not "") {
		//	char[,] track = cheatGrid.ReplaceLineEndings().Replace(Environment.NewLine, "").To2dArray(_raceTrack.ColsCount(), _raceTrack.RowsCount());
		//	Point move1 = track.ForEachCell().Single(cell => cell.Value == MOVE1);
		//	Point move2 = track.ForEachCell().Single(cell => cell.Value == MOVE2);
		//	cheats.Add(new(Point.Zero, move1, move2));
		//} else {
		//	cheats = [.. _raceRoute.FindCheatsPart2(_raceTrack, _codePath)];
		//}

		//return cheats
		//	.Select(cheat => cheat.FindTimeSaving(_raceTrack, _codePath, _raceRoute))
		//	.CountBy(ps => ps)
		//	.Where(ps => ps.Key >= psTarget)
		//	.Sum(ps => ps.Value)
		//	.ToString()
		//	;
	}

	private static List<Point> CalculateRacePath(this char[,] track, Point start, Point end)
	{
		HashSet<Point> visited = [start];
		List<Point> codePath = [start];
		Point current = _start;
		while (current != end) {
			current = track.GetAdjacentCells(current).Single(cell => cell.Value is TRACK or END && visited.DoesNotContain(cell.Index));
			_ = visited.Add(current);
			codePath.Add(current);
		}

		return codePath;
	}

	private static IEnumerable<Cheat> FindCheats(this List<Point> route, char[,] track, Dictionary<Point, int> codePath)
	{
		foreach ((int routeIndex, Point startCheat) in route.Index()) {
		foreach (Cell<char> possibleMove1 in track.GetAdjacentCells(startCheat)        .Where(c => c.Value is WALL)) {
		foreach (Cell<char> possibleMove2 in track.GetAdjacentCells(possibleMove1).Where(c => c.Value is TRACK or END && c.Index != startCheat)) {
			if (track.GetAdjacentCells(possibleMove2).Any(cell => routeIndex > codePath.GetValueOrDefault(cell.Index, -1))) {
				yield return new(startCheat, possibleMove1, possibleMove2);
			}
		}
		}
		}
	}

	private static IEnumerable<Cheat> FindCheatsPart2(this List<Point> route, char[,] track, Dictionary<Point, int> codePath)
	{
		foreach ((int routeIndex, Point point) in route.Index()) {
			foreach (Cell<char> possibleMove1 in track.GetAdjacentCells(point).Where(c => c.Value is WALL)) {
				foreach (Cell<char> possibleMove2 in track.GetAdjacentCells(possibleMove1).Where(c => c.Value is TRACK or END && c.Index != point)) {
					if (track.GetAdjacentCells(possibleMove2).Any(cell => routeIndex > codePath.GetValueOrDefault(cell.Index, -1))) {
						yield return new(point, possibleMove1, possibleMove2);
					}
				}
			}
		}
	}

	private static int FindTimeSaving(this Cheat cheat, char[,] track, Dictionary<Point, int> codePath, List<Point> route)
	{
		int firstIndex = cheat.Start == Point.Zero
			? route.Index().First(p => (track.GetAdjacentCells(p.Item).Select(c => c.Index).Contains(cheat.Move1))).Index
			: codePath[cheat.Start] + 1;
		int secondIndex = codePath[cheat.End];

		return secondIndex - firstIndex - 1;
	}

	private record Cheat(Point Start, Point Move1, Point End);

	private static int PsTarget(this object[]? args) => GetArgument(args, 1, 100);
	private static string CheatGridForTests(this object[]? args) => GetArgument(args, 2, "");
}
