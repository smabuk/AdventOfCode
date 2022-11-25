namespace AdventOfCode.Solutions._2021;

/// <summary>
/// Day 17: Trick Shot
/// https://adventofcode.com/2021/day/17
/// </summary>
[Description("Trick Shot")]
public class Day17 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		TargetArea targetArea = new(input[0]);
		int xDirection = Math.Sign(targetArea.XFrom);
		int minX = Math.Min(Math.Abs(targetArea.XFrom), Math.Abs(targetArea.XFrom));

		int xVelocity;
		for (xVelocity = 1; SequenceHelpers.TriangularNumber(xVelocity) < minX; xVelocity++) { }
		Point initialVelocity = new(xVelocity * xDirection, Math.Abs(targetArea.YFrom) - 1);
		(_, int height) = targetArea.WillProbeHit(initialVelocity);

		return height;
	}

	private static int Solution2(string[] input) {
		TargetArea targetArea = new(input[0]);
		int numberOfHits = 0;
		int xDirection = Math.Sign(targetArea.XFrom);
		int xMaxGuess = Math.Max(Math.Abs(targetArea.XFrom), Math.Abs(targetArea.XTo));

		for (int x = 2; x <= xMaxGuess; x++) {
			for (int y = targetArea.YFrom; y < Math.Abs(targetArea.YFrom); y++) {
				(bool success, _) = targetArea.WillProbeHit(new Point(x * xDirection, y));
				if (success) {
					numberOfHits++;
				}
			}
		}

		return numberOfHits;
	}

	public record TargetArea(int XFrom, int XTo, int YFrom, int YTo) {

		public TargetArea(string input) : this(0, 0, 0, 0) {
			Match match = Regex.Match(input,
				@"target area: x=(?<xfrom>[\-]*\d+)..(?<xto>[\-]*\d+), y=(?<yfrom>[\-]*\d+)..(?<yto>[\-]*\d+)");
			if (match.Success) {
				XFrom = int.Parse(match.Groups["xfrom"].Value);
				XTo = int.Parse(match.Groups["xto"].Value);
				YFrom = int.Parse(match.Groups["yfrom"].Value);
				YTo = int.Parse(match.Groups["yto"].Value);
			}
		}

		public bool InTargetArea(int x, int y)
			=> ((XFrom <= x && x <= XTo) || (XTo <= x && x <= XFrom)) &&
				((YFrom <= y && y <= YTo) || (YTo <= y && y <= YFrom));
		public bool InTargetArea(Point position) => InTargetArea(position.X, position.Y);


		public (bool Success, int MaxHeight) WillProbeHit(Point initialVelocity) {
			Point currentVelocity = initialVelocity;
			Point currentPosition = new(0, 0);
			int maxHeight = 0;

			while (currentPosition.Y >= YFrom) {
				currentPosition = currentPosition with {
					X = currentPosition.X + currentVelocity.X,
					Y = currentPosition.Y + currentVelocity.Y,};

				maxHeight = Math.Max(maxHeight, currentPosition.Y);

				if (InTargetArea(currentPosition)) {
					return (true, maxHeight);
				}

				currentVelocity = currentVelocity with {
					Y = currentVelocity.Y - 1,
					X = currentVelocity.X switch {
						< 0 => currentVelocity.X + 1,
						> 0 => currentVelocity.X - 1,
						_ => 0,
					},
				};
			}

			return (false, 0);
		}
	};
}
