namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 04: Security Through Obscurity
/// https://adventofcode.com/2016/day/04
/// </summary>
[Description("Security Through Obscurity")]
public sealed partial class Day04 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();


	private static int Solution1(string[] input) =>
		input
		.As<Room>()
		.Where(room => room.IsValid)
		.Sum(room => room.SectorId);

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}

	private sealed record Room(string EncryptedName, int SectorId, string Checksum) : IParsable<Room> {

		readonly string checksum = string.Join("", 
			EncryptedName
			.Replace("-", "")
			.CountBy(c => c)
			.OrderByDescending(kv => kv.Value)
			.ThenBy(kv => kv.Key)
			.Take(5)
			.Select(kv => kv.Key));

		public bool IsValid => Checksum == checksum;

		public static Room Parse(string s, IFormatProvider? provider)
		{
			Match match = InputRegEx().Match(s);
			if (match.Success) {
				return new(match.Groups["encryptedName"].Value, match.As<int>("number"), match.Groups["checksum"].Value);
			}

			return null!;
		}

		public static Room Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Room result)
			=> ISimpleParsable<Room>.TryParse(s, provider, out result);
	}

	[GeneratedRegex("""(?<encryptedName>([a-z\-]+)+)\-(?<number>\d+)\[(?<checksum>[a-z]+)\]""")]
	private static partial Regex InputRegEx();
}
