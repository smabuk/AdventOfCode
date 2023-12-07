namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 07: Camel Cards
/// https://adventofcode.com/2023/day/07
/// </summary>
[Description("Camel Cards")]
public sealed partial class Day07 {

	[Init]
	public static    void Init(string[] input, params object[]? args) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static IEnumerable<Hand> _instructions = [];

	private static void LoadInstructions(string[] input) {
		_instructions = input.As<Hand>();
	}

	private static int Solution1(string[] input) => new SetOfHands([.. input.As<Hand>()]).TotalWinnings;

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}


	private record SetOfHands(List<Hand> Hands) { 

		public int TotalWinnings {  get {
				List<Hand> rankedHands = [.. Hands
					.OrderBy(h => h.Type)
					.ThenBy(h => h.Cards[0].Strength)
					.ThenBy(h => h.Cards[1].Strength)
					.ThenBy(h => h.Cards[2].Strength)
					.ThenBy(h => h.Cards[3].Strength)
					.ThenBy(h => h.Cards[4].Strength)
					];

				return rankedHands.Select((hand, index) => (Hand: hand, Rank: index + 1)).Sum(cards => cards.Hand.BidAmount * cards.Rank);
			}
		}

	};

	private record Hand(List<Card> Cards, int BidAmount) : IParsable<Hand> {

		public HandType Type => GetHandType();

		private HandType GetHandType()
		{
			var groupedCards = Cards.GroupBy(c => c);
			if (groupedCards.Any(c => c.Count() == 5)) {
				return HandType.FiveOfAKind;
			} else if (groupedCards.Any(c => c.Count() == 4)) {
				return HandType.FourOfAKind;
			} else if (groupedCards.Any(c => c.Count() == 3) && groupedCards.Any(c => c.Count() == 2)) {
				return HandType.FullHouse;
			} else if (groupedCards.Any(c => c.Count() == 3)) {
				return HandType.ThreeOfAKind;
			} else if (groupedCards.Where(c => c.Count() == 2).Count() == 2) {
				return HandType.TwoPair;
			} else if (groupedCards.Any(c => c.Count() == 2)) {
				return HandType.OnePair;
			}
			return HandType.HighCard;
		}

		public static Hand Parse(string s, IFormatProvider? provider)
		{
			List<Card> cards = [.. s[0..5].Select(c => c.ToString()).As<Card>()];
			return new([.. cards], s[6..].AsInt());
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Hand result)
			=> ISimpleParsable<Hand>.TryParse(s, provider, out result);
	}

	private record Card(string Label) : IParsable<Card> {
		public int Strength => Label[0] switch
		{
			'A' => 14,
			'K' => 13,
			'Q' => 12,
			'J' => 11,
			'T' => 10,
			>= '1' and <= '9' => Label[0] - '0',
			_ => throw new InvalidCastException(),
		};

		public static Card Parse(string s, IFormatProvider? provider) => new(s);

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Card result)
			=> ISimpleParsable<Card>.TryParse(s, provider, out result);
	};

	public enum HandType
	{
		HighCard,
		OnePair,
		TwoPair,
		ThreeOfAKind,
		FullHouse,
		FourOfAKind,
		FiveOfAKind,
	}


}
