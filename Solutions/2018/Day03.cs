using System.Security.Claims;

namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 03: No Matter How You Slice It
/// https://adventofcode.com/2018/day/03
/// </summary>
[Description("No Matter How You Slice It")]
public sealed partial class Day03 {

	[Init]
	public static    void Init(string[] input, params object[]? _) => LoadClaims(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static IEnumerable<Claim> _claims = [];

	private static void LoadClaims(string[] input) {
		_claims = input.Select(Claim.Parse);
	}

	private static int Solution1(string[] input) {
		return _claims
			.SelectMany(claim => claim.Squares)
			.GroupBy(square => square)
			.Where(square => square.Count() > 1)
			.Count();
	}

	private static int Solution2(string[] input) {
		Dictionary<int, HashSet<Point>> Squares = _claims.ToDictionary(c => c.Id, c => c.Squares.ToHashSet());

		foreach (int claimId in Squares.Keys.Order()) {
			HashSet<Point> squares = Squares[claimId];
			List<Point> allOtherSquares = Squares
				.Where(c => c.Key != claimId)
				.SelectMany(c => c.Value)
				.ToList();
			if (!squares.Overlaps(allOtherSquares)) {
				return claimId;
			}
		}

		return 0;
	}

	private record Claim() : IParsable<Claim> {

		public int Id { get; init; }
		public Point Start { get; init; }
		public (int Wide, int Tall) Size { get; init; }
		public List<Point> Squares { get; } = [];

		[SetsRequiredMembers]
		public Claim(int id, Point start, (int Wide, int Tall) size) : this()
		{
			Id = id;
			Start = start;
			Size = size;
			Squares = GetSquares().ToList();
		}

		private IEnumerable<Point> GetSquares()
		{
			for (int x = 0; x < Size.Wide; x++) {
				for (int y = 0; y < Size.Tall; y++) {
					yield return new Point(Start.X + x, Start.Y + y);
				}
			}
		} 

		public static Claim Parse(string s) => ParseLine(s);
		public static Claim Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Claim result) => throw new NotImplementedException();
	}

	private static Claim ParseLine(string input) {
		Match match = InputRegEx().Match(input);
		if (match.Success) {
			return new(
				int.Parse(match.Groups["id"].Value),
				new(int.Parse(match.Groups["x"].Value), int.Parse(match.Groups["y"].Value)),
				new(int.Parse(match.Groups["xSize"].Value), int.Parse(match.Groups["ySize"].Value))
				);
		}
		return null!;
	}

	[GeneratedRegex("""#(?<id>\d+) @ (?<x>\d+),(?<y>\d+): (?<xSize>\d+)x(?<ySize>\d+)""")]
	private static partial Regex InputRegEx();
}
