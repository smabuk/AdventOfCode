namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 04: Scratchcards 
/// https://adventofcode.com/2023/day/04
/// </summary>
[Description("Scratchcards ")]
public sealed partial class Day04 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadCards(input);
	public static string Part1(string[] input, params object[]? args) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2().ToString();

	private static List<Card> _cards = [];

	private static void LoadCards(string[] input) => _cards = [.. input.As<Card>()];

	private static int Solution1() => _cards.Sum(card => card.Points);

	private static int Solution2() {
		Dictionary<int, int> cards = _cards.ToDictionary(card => card.Id, _ => 1);

		for (int i = 0; i < _cards.Count; i++) {
			Card card = _cards[i];
			foreach (Card copiedCard in _cards.Skip(i + 1).Take(card.NoOfWinners)) {
				cards[copiedCard.Id] += cards[card.Id];
			}
		}

		return cards.Sum(card => card.Value);
	}

	private record Card(int Id, HashSet<int> WinningNumbers, HashSet<int> Numbers) : IParsable<Card> {

		public int NoOfWinners { get; } = Numbers.Intersect(WinningNumbers).Count();
		public int Points => (int)Math.Pow(2, NoOfWinners - 1);

		public static Card Parse(string s, IFormatProvider? provider)
		{
			string[] SEPS = ["Card ", ":", "|"];

			return s.Split(SEPS, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries) switch
			{
				[string id, string winners, string numbers] => new(id.AsInt(), [.. winners.AsInts()], [.. numbers.AsInts()]),
				_ => throw new InvalidCastException(),
			};
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Card result)
			=> ISimpleParsable<Card>.TryParse(s, provider, out result);
	}
}
