namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 22: Sand Slabs
/// https://adventofcode.com/2023/day/22
/// </summary>
[Description("Sand Slabs")]
public sealed partial class Day22 {

	[Init]
	public static void Init(string[] input, params object[]? args) => LoadBricks(input);
	public static string Part1(string[] input, params object[]? args) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2().ToString();

	private static List<Brick> _bricks = [];
	private static List<string> _bricksWillFall = [];
	private static Dictionary<string, (HashSet<string> SupportedBy, HashSet<string> Supporting)> _aboveAndBelow = [];

	private static void LoadBricks(string[] input)
	{
		_bricks         = AllowBricksToFall([.. input.As<Brick>().OrderBy(brick => brick.Start.Z)]);
		_aboveAndBelow  = CalculateSupportingStructure(_bricks);
		_bricksWillFall = WhichBricksWillFall(_bricks, _aboveAndBelow);
	}

	private static int Solution1() => _bricks.Count - _bricksWillFall.Count;

	private static int Solution2() {
		HashSet<string> disintegratedOrFallen;

		int count = 0;
		foreach (string brickName in _bricksWillFall) {
			disintegratedOrFallen = [brickName];
			count += CountFallersAbove(brickName);
		}

		return count;

		int CountFallersAbove(string brickName)
		{
			int count = 0;
			foreach (string name in _aboveAndBelow[brickName].Supporting) {
				if (IsNotSupportedByAnyBricks(name)) {
					_ = disintegratedOrFallen.Add(name);
					count += 1 + CountFallersAbove(name);
				}
			}

			return count;

			bool IsNotSupportedByAnyBricks(string name) => !_aboveAndBelow[name].SupportedBy.Except(disintegratedOrFallen).Any();
		}
	}

	private static List<Brick> AllowBricksToFall(List<Brick> bricks)
	{
		for (int i = 0; i < bricks.Count; i++) {
			Brick brick = bricks[i];
			int newZ = bricks[0..i]
				.Where(brick.XYOverlaps)
				.DefaultIfEmpty(new Brick(new(0, 0, 0), new(0, 0, 0)))
				.Max(b => b.End.Z) + 1;
			bricks[i] = brick with
			{
				Start = new(brick.Start.X, brick.Start.Y, newZ),
				End   = new(brick.End.X,   brick.End.Y,   newZ + brick.End.Z - brick.Start.Z)
			};
		}

		return bricks;
	}


	private static Dictionary<string, (HashSet<string> SupportedBy, HashSet<string> Supporting)> CalculateSupportingStructure(List<Brick> bricks)
	{
		Dictionary<string, (HashSet<string> SupportedBy, HashSet<string> Supporting)> aboveAndBelow = [];
		for (int i = 0; i < bricks.Count; i++) {
			Brick brick = bricks[i];
			HashSet<string> supportedBy = [..bricks
				.Where(b => brick.Start.Z == b.End.Z + 1 && brick.XYOverlaps(b))
				.Select(b => b.Name)];
			HashSet<string> supporting = [..bricks
				.Where(b => brick.End.Z == b.Start.Z - 1 && brick.XYOverlaps(b))
				.Select(b => b.Name)];
			aboveAndBelow[brick.Name] = (supportedBy, supporting);
		}
		return aboveAndBelow;
	}
	
	private static List<string> WhichBricksWillFall(List<Brick> bricks, Dictionary<string, (HashSet<string> SupportedBy, HashSet<string> Supporting)> aboveAndBelow)
	{
		List<string> bricksWillFall = [];
		foreach (Brick brick in bricks) {
			if (aboveAndBelow[brick.Name].Supporting.Count == 0) {
				continue;
			}

			foreach (string aboveName in aboveAndBelow[brick.Name].Supporting) {
				if (aboveAndBelow[aboveName].SupportedBy.Count == 1) {
					bricksWillFall.Add(brick.Name);
					break;
				}
			}
		}

		return bricksWillFall;
	}

	private sealed record Brick(Point3d Start, Point3d End) : IParsable<Brick> {
		public string Name => $"({Start.X},{Start.Y},{Start.Z})-({End.X},{End.Y},{End.Z})";

		public bool XYOverlaps(Brick otherBrick) => 
			   (Start.X, End.X).TryGetOverlap((otherBrick.Start.X, otherBrick.End.X), out _)
			&& (Start.Y, End.Y).TryGetOverlap((otherBrick.Start.Y, otherBrick.End.Y), out _);

		public static Brick Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit('~');
			return new(tokens[0].As<Point3d>(), tokens[1].As<Point3d>());
		}

		public static Brick Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Brick result)
			=> ISimpleParsable<Brick>.TryParse(s, provider, out result);
	}
}
