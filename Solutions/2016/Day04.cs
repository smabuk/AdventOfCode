namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 04: Security Through Obscurity
/// https://adventofcode.com/2016/day/04
/// </summary>
[Description("Security Through Obscurity")]
public sealed partial class Day04 {

	[Init]
	public static void Init(string[] input) => LoadRoomList(input);
	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static List<Room> _roomList = [];
	private static void LoadRoomList(string[] input) => _roomList = [..input.As<Room>()];

	private static int Solution1() =>
		_roomList
		.Where(room => room.IsValid)
		.Sum(room => room.SectorId);

	private static int Solution2() =>
		_roomList
		.Single(room => room.RealName.Contains("north"))
		.SectorId;

	public sealed record Room(string EncryptedName, int SectorId, string Checksum) : IParsable<Room> {

		readonly string checksum = string.Join("", 
			EncryptedName
			.Replace("-", "")
			.CountBy(c => c)
			.OrderByDescending(kv => kv.Value)
			.ThenBy(kv => kv.Key)
			.Take(5)
			.Select(kv => kv.Key));

		public bool IsValid => Checksum == checksum;
		public string RealName => IsValid ? Decrypt(EncryptedName, SectorId) : "";

		public static string Decrypt(string encrypted, int rotationAmount)
		{
			string[] tokens = encrypted.Split('-');
			for (int i = 0; i < tokens.Length; i++) {
				Span<char> word = tokens[i].ToCharArray();
				for (int c = 0; c < word.Length; c++) {
					word[c] = (char)(((word[c] - 'a' + rotationAmount) % 26) + 'a');
				}

				tokens[i] = new(word);
			}

			return string.Join(' ', tokens);
		}

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
