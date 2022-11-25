namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 21: Dirac Dice
/// https://adventofcode.com/2021/day/21
/// </summary>
[Description("Dirac Dice")]
public class Day21 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static long Solution1(string[] input) {
		List<Player> players = input.Select(i => ParseLine(i)).ToList();

		int deterministicDie = 0;
		int turn = 0;
		int dieRolls = 0;
		while (players.Where(p => p.Points >= 1000).Any() is false) {
			int dieScore = 0;
			dieScore += (++deterministicDie - 1) % 100 + 1;
			dieScore += (++deterministicDie - 1) % 100 + 1;
			dieScore += (++deterministicDie - 1) % 100 + 1;
			dieRolls += 3;

			Player player = players[turn++ % 2];
			player.Move(dieScore);
		}

		return players.Where(p => p.Points < 1000).Single().Points * dieRolls;
	}


	private static long Solution2(string[] input) {
		List<Player> playersInput = input.Select(i => ParseLine(i)).ToList();
		Dictionary<int, int> DieRollFrequency = new();
		Dictionary<GamePosition, (long, long)> GamePositions = new();

		// Each roll is a 3,4,5,6,7,8 or 9
		for (int i = 1; i <= 3; i++) {
			for (int j = 1; j <= 3; j++) {
				for (int k = 1; k <= 3; k++) {
					DieRollFrequency[i + j + k] = DieRollFrequency.GetValueOrDefault(i + j + k, 0) + 1;
				}
			}
		}

		int p1Start = playersInput[0].Position;
		int p2Start = playersInput[1].Position;

		(long WinsForPlayer1, long WinsForPlayer2) = CountDiracDiceWins(p1Start, 0, p2Start, 0);

		return Math.Max(WinsForPlayer1, WinsForPlayer2);

		// Local functions --------------------------------------------------------

		(long p1Count, long p2Count) CountDiracDiceWins(int p1Position, int p1Score, int p2Position, int p2Score) {
			if (p1Score >= 21) {
				return (1, 0);
			}
			if (p2Score >= 21) {
				return (0, 1);
			}

			GamePosition gamePosition = new(p1Position, p1Score, p2Position, p2Score);
			if (GamePositions.ContainsKey(gamePosition)) {
				return GamePositions[gamePosition];
			}

			long p1Wins = 0;
			long p2Wins = 0;

			for (int roll = 3; roll <= 9; roll++) {
				int newP1Position = (p1Position + roll - 1) % 10 + 1;
				int newP1Score = p1Score + newP1Position;

				// Other players turn
				(long p2w, long p1w) = CountDiracDiceWins(p2Position, p2Score, newP1Position, newP1Score);
				p1Wins += p1w * DieRollFrequency[roll];
				p2Wins += p2w * DieRollFrequency[roll];
			}

			GamePositions[gamePosition] = (p1Wins, p2Wins);

			return (p1Wins, p2Wins);
		}
	}


	record Player(string Name) {
		public int Position { get; set; }
		public int Points { get; set; }

		public Player(string name, int position) : this(name) {
			Position = position;
		}

		public void Move(int sumOfDieRolls) {
			Position += sumOfDieRolls;
			Position = (Position - 1) % 10 + 1;
			Points += Position;
		}

	};

	record struct GamePosition(int P1Pos, int P1Score, int P2Pos, int P2Score);

	private static Player ParseLine(string input) {
		Match match = Regex.Match(input, @"(?<name>.*) starting position: (?<start>\d+)");
		if (match.Success) {
			return new(match.Groups["name"].Value, int.Parse(match.Groups["start"].Value));
		}
		throw new NotSupportedException();
	}
}
