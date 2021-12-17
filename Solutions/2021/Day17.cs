namespace AdventOfCode.Solutions.Year2021;

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
		int maxHeight = 0;

		for (int x = 2; x < targetArea.XFrom; x++) {
			for (int y = 0; y < 100; y++) {
				(bool success, int height) = targetArea.WillProbeHit(new Point(x, y));
				if (success) {
					maxHeight = Math.Max(maxHeight, height);
				}
			}
		}

		return maxHeight;
	}

	private static string Solution2(string[] input) {
		TargetArea targetArea = new(input[0]);
		return "** Solution not written yet **";
	}

	record TargetArea(int XFrom, int XTo, int YFrom, int YTo) {

		public HashSet<Point> TargetRange = new();

		public TargetArea(string input) : this(0, 0, 0, 0) {
			Match match = Regex.Match(input,
				@"target area: x=(?<xfrom>[\-]*\d+)..(?<xto>[\-]*\d+), y=(?<yfrom>[\-]*\d+)..(?<yto>[\-]*\d+)");
			if (match.Success) {
				XFrom = int.Parse(match.Groups["xfrom"].Value);
				XTo = int.Parse(match.Groups["xto"].Value);
				YFrom = int.Parse(match.Groups["yfrom"].Value);
				YTo = int.Parse(match.Groups["yto"].Value);
			}
			for (int y = YFrom; y <= YTo; y++) {
				for (int x = XFrom; x <= XTo; x++) {
					TargetRange.Add(new Point(x, y));
				}
			}
		}

		public (bool Success, int MaxHeight) WillProbeHit(Point initialVelocity) {
			Point currentVelocity = initialVelocity;
			Point currentPosition = new(0, 0);
			int maxHeight = 0;

			while (currentPosition.X <= XTo && currentPosition.Y >= YFrom) {
				currentPosition = currentPosition with {
					X = currentPosition.X + currentVelocity.X,
					Y = currentPosition.Y + currentVelocity.Y,
				};
				maxHeight = Math.Max(maxHeight, currentPosition.Y);

				if (TargetRange.Contains(currentPosition)) {
					return (true, maxHeight);
				}

				int newXVelocity = 0;
				if (currentVelocity.X < 0) {
					newXVelocity = currentVelocity.X + 1;
				} else if (currentVelocity.X > 0) {
					newXVelocity = currentVelocity.X - 1;
				}

				currentVelocity = currentVelocity with {
					X = newXVelocity,
					Y = currentVelocity.Y - 1,
				};
			}

			return (false, 0);
		}


	};



}
