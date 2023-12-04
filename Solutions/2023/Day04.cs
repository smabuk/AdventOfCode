namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 04: Scratchcards 
/// https://adventofcode.com/2023/day/04
/// </summary>
[Description("Scratchcards ")]
public sealed partial class Day04 {

	[Init]
	public static   void  Init(string[] input, params object[]? _) => LoadCards(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static IEnumerable<Card> _cards = [];

	private static void LoadCards(string[] input)
	{
		_cards = input.Select(Card.Parse);
	}

	private static int Solution1(string[] input) {
		return _cards.Sum(card => card.Points);
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

	private record Card(int Id, List<int> WinningNumbers, List<int> Numbers) : IParsable<Card> {

		public int NoOfWinners { get; } = Numbers.Count(n => WinningNumbers.Contains(n));
		public int Points => (int)Math.Pow(2, NoOfWinners - 1);

		public static Card Parse(string s)
		{
			char COLON   = ':';
			char BAR     = '|';
			char SPACE   = ' ';
			char[] COLON_AND_BAR = [COLON, BAR];

			const int ID = 0;
			const int WINNERS = 1;
			const int NUMBERS = 2;
			const int ID_OFFSET = 5;

			string[] tokens = s.Split(COLON_AND_BAR, StringSplitOptions.TrimEntries);

			return new(
				tokens[ID][ID_OFFSET..].AsInt(),
				[.. tokens[WINNERS].Split(SPACE, StringSplitOptions.RemoveEmptyEntries).AsInts()],
				[.. tokens[NUMBERS].Split(SPACE, StringSplitOptions.RemoveEmptyEntries).AsInts()]
				);
		}

		public static Card Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Card result) => throw new NotImplementedException();
	}

	[GeneratedRegex("""Card\s+(?<id>\d+): | (?<winners> \d+)+ (?<numbers> \d+)""")]
	private static partial Regex InputRegEx();
}
