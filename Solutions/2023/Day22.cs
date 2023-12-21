namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 22: Sand Slabs
/// https://adventofcode.com/2023/day/22
/// </summary>
[Description("Sand Slabs")]
public sealed partial class Day22 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		List<Brick> bricks = [.. input.As<Brick>()];
		List<Brick> orderedBricks = [..bricks.OrderBy(brick => brick.Start.Z)];

		for (int i = 0; i < bricks.Count; i++) {
			Brick brick = orderedBricks[i];
			List<Brick> bricksBelow = orderedBricks[0..i];
			int newZ = bricksBelow
				.Where(b => (brick.Start.X, brick.End.X).TryGetOverlap((b.Start.X, b.End.X), out _) 
				  && (brick.Start.Y, brick.End.Y).TryGetOverlap((b.Start.Y, b.End.Y), out _))
				.DefaultIfEmpty(new Brick(new(0,0,0), new(0,0,0)))
				.Max(b => b.End.Z) + 1;
			orderedBricks[i] = brick with { Start = new(brick.Start.X, brick.Start.Y, newZ),
											End = new(brick.End.X, brick.End.Y, newZ + brick.End.Z - brick.Start.Z)};
		}

		Dictionary<string, (HashSet<string> SupportedBy, HashSet<string> Supporting)> aboveAndBelow = [];


		for (int i = 0; i < bricks.Count; i++) {
			Brick brick = orderedBricks[i];
			List<Brick> bricksAbove = orderedBricks[(i+1)..];
			HashSet<string> supporting = [..orderedBricks
				.Where(b => brick.End.Z == b.Start.Z - 1 
						&& (brick.Start.X, brick.End.X).TryGetOverlap((b.Start.X, b.End.X), out _)
						&& (brick.Start.Y, brick.End.Y).TryGetOverlap((b.Start.Y, b.End.Y), out _))
				.Select(b => b.Name)];
			HashSet<string> supportedBy = [..orderedBricks
				.Where(b => brick.Start.Z == b.End.Z + 1 
						&& (brick.Start.X, brick.End.X).TryGetOverlap((b.Start.X, b.End.X), out _)
						&& (brick.Start.Y, brick.End.Y).TryGetOverlap((b.Start.Y, b.End.Y), out _))
				.Select(b => b.Name)];
			aboveAndBelow[brick.Name] = (supportedBy, supporting);
		}

		int count = 0;
		for (int i = 0; i < orderedBricks.Count; i++) {
			Brick brick = orderedBricks[i];
			if (aboveAndBelow[brick.Name].Supporting.Count == 0) {
				count++;
				continue;
			}
			bool ok = true;
			foreach (string aboveName in aboveAndBelow[brick.Name].Supporting) {
				if (aboveAndBelow[aboveName].SupportedBy.Count == 1) {
					ok = false;
					break;
				}
			}
			if (ok) {
				count++;
			}
		}

		return count;
		// 183 - too low
		// 441 - too high, but someone else's answer
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

	private sealed record Brick(Point3d Start, Point3d End) : IParsable<Brick> {
		public string Name => $"({Start.X},{Start.Y},{Start.Z})-({End.X},{End.Y},{End.Z})";

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
