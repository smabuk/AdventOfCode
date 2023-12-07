namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 07: Camel Cards
/// https://adventofcode.com/2023/day/07
/// </summary>
[Description("Camel Cards")]
public sealed partial class Day07 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) => new SetOfHands([.. input.As<Hand>()]).TotalWinnings;
	private static int Solution2(string[] input) => new SetOfHands([.. input.Select(i => i.Replace('J', JOKER)).As<Hand>()]).TotalWinnings;

	const char JOKER = 'j';

	private record SetOfHands(List<Hand> Hands) {
		public int TotalWinnings => Hands
			.OrderBy(h => h.SortStrength)
			.Select((hand, index) => (Hand: hand, Rank: index + 1))
			.Sum(hand => hand.Hand.BidAmount * hand.Rank);
	}

	private record Hand(List<Card> Cards, int BidAmount) : IParsable<Hand> {
		public HandType Type {
			get
			{
				List<Card> cards = [.. Cards];
				if (cards.Any(card => card.Label == JOKER)) {
					char label = cards
						.Where(card => card.Label is not JOKER)
						.GroupBy(c => c)
						.OrderByDescending(c => c.Count())
						.Select(card => card.Key.Label)
						.FirstOrDefault(JOKER);
					cards = [.. cards.Select(card => card.Label == JOKER ? card with { Label = label} : card)];
				}

				IEnumerable<IGrouping<Card, Card>> counts = cards.GroupBy(card => card);

				if (     counts.Any(c => c.Count() == 5))                { return HandType.FiveOfAKind; }
				else if (counts.Any(c => c.Count() == 4))                { return HandType.FourOfAKind; }
				else if (counts.Any(c => c.Count() == 3)
					  && counts.Any(c => c.Count() == 2))                { return HandType.FullHouse; }
				else if (counts.Any(c => c.Count() == 3))                { return HandType.ThreeOfAKind; }
				else if (counts.Where(c => c.Count() == 2).Count() == 2) { return HandType.TwoPair; }
				else if (counts.Any(c => c.Count() == 2))                { return HandType.OnePair; }
				else                                                     { return HandType.HighCard; }
			}
		}

		public string SortStrength => $"{(int)Type:D2}{Cards[0].Strength:D2}{Cards[1].Strength:D2}{Cards[2].Strength:D2}{Cards[3].Strength:D2}{Cards[4].Strength:D2}";

		public static Hand Parse(string s, IFormatProvider? provider)
		{
			 const int LABEL_INDEX = 0;
			 const int BID_INDEX = 6;
			 const int NO_OF_CARDS = 5;

			List<Card> cards = [.. s[LABEL_INDEX..(LABEL_INDEX + NO_OF_CARDS)].Select(label => new Card(label))];
			int bidAmount = s[BID_INDEX..].AsInt();

			return new(cards, bidAmount);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Hand result)
		=> ISimpleParsable<Hand>.TryParse(s, provider, out result);

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

	private record struct Card(char Label) {
		public readonly int Strength => Label switch
		{
			'A' => 14,
			'K' => 13,
			'Q' => 12,
			'J' => 11,
			'T' => 10,
			>= '1' and <= '9' => Label - '0',
			JOKER =>  1,
			_ => throw new InvalidCastException(),
		};
	}
}
