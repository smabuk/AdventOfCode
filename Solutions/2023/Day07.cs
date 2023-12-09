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

	private sealed record SetOfHands(List<Hand> Hands) {
		public int TotalWinnings => Hands
			.Order()
			.Select((hand, index) => (Hand: hand, Rank: index + 1))
			.Sum(hand => hand.Hand.BidAmount * hand.Rank);
	}

	private sealed record Hand(IReadOnlyList<Card> Cards, int BidAmount) : IParsable<Hand>, IComparable<Hand> {
		public HandType Type {
			get
			{
				List<Card> cards = [.. Cards];
				if (cards.Any(card => card.Label == JOKER)) {
					char label = cards
						.Where(card => card.Label is not JOKER)
						.GroupBy(c => c)
						.MaxBy(c => c.Count())
						?.FirstOrDefault().Label ?? JOKER;
					cards = [.. cards.Select(card => card.Label == JOKER ? card with { Label = label} : card)];
				}

				List<int> counts = [.. cards.GroupBy(card => card).Select(cards => cards.Count()).OrderDescending()];

				return counts switch
				{
					[5]          => HandType.FiveOfAKind,
					[4, _]       => HandType.FourOfAKind,
					[3, 2]       => HandType.FullHouse,
					[3, _, _]    => HandType.ThreeOfAKind,
					[2, 2, _]    => HandType.TwoPair,
					[2, _, _, _] => HandType.OnePair,
					_            => HandType.HighCard,
				};
			}
		}

		public static Hand Parse(string s, IFormatProvider? provider)
		{
			 const int LABEL_INDEX = 0;
			 const int BID_INDEX   = 6;
			 const int NO_OF_CARDS = 5;

			List<Card> cards = [.. s[LABEL_INDEX..(LABEL_INDEX + NO_OF_CARDS)].Select(label => new Card(label))];
			int bidAmount = s[BID_INDEX..].As<int>();

			return new(cards, bidAmount);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Hand result)
			=> ISimpleParsable<Hand>.TryParse(s, provider, out result);

		public int CompareTo(Hand? other)
		{
			if (other is null) {
				return this is null ? 0 : 1;
			}

			string thisSortStrength  = $"{(int)Type:D2}{Cards[0].Strength:D2}{Cards[1].Strength:D2}{Cards[2].Strength:D2}{Cards[3].Strength:D2}{Cards[4].Strength:D2}";
			string otherSortStrength = $"{(int)other.Type:D2}{other.Cards[0].Strength:D2}{other.Cards[1].Strength:D2}{other.Cards[2].Strength:D2}{other.Cards[3].Strength:D2}{other.Cards[4].Strength:D2}";

			return thisSortStrength.CompareTo(otherSortStrength);
		}

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
