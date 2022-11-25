namespace AdventOfCode.Solutions._2020;

/// <summary>
/// Day 22: Crab Combat
/// https://adventofcode.com/2021/day/22
/// </summary>
[Description("Crab Combat")]
public class Day22 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record RecordType(string Name, int Value);

	record Player(string Name, Queue<int> Deck) {
		public int Score => Deck
			.ToList()
			.Select((x, idx) => x * (Deck.Count - idx))
			.Sum();
		public string DeckString => string.Join(",", Deck);
	};

	private static int Solution1(string[] input) {
		Player player1;
		Player player2;
		(player1, player2) = ParseInput(input);


		while (player1.Deck.Count != 0 && player2.Deck.Count != 0) {
			int p1card = player1.Deck.Dequeue();
			int p2card = player2.Deck.Dequeue();
			if (p1card > p2card) {
				player1.Deck.Enqueue(p1card);
				player1.Deck.Enqueue(p2card);
			} else {
				player2.Deck.Enqueue(p2card);
				player2.Deck.Enqueue(p1card);
			}
		}

		return player1.Deck.Count == 0 ? player2.Score : player1.Score;
	}

	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		//List<RecordType> instructions = input.Select(i => ParseLine(i)).ToList();
		return "** Solution not written yet **";
	}

	private static (Player player1, Player player2) ParseInput(string[] input) {
		Queue<int> deck = new();
		string name = input[0];

		int i = 1;
		while (!string.IsNullOrWhiteSpace(input[i])) {
			deck.Enqueue(int.Parse(input[i]));
			i++;
		}

		Player player1 = new(name, deck);
		i++;
		deck = new();
		name = input[i++];
		while (i < input.Length && !string.IsNullOrWhiteSpace(input[i])) {
			deck.Enqueue(int.Parse(input[i]));
			i++;
		}
		Player player2 = new(name, deck);

		return (player1, player2);
	}

}
