namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 21: Dirac Dice
/// https://adventofcode.com/2021/day/21
/// </summary>
[Description("Dirac Dice")]
public class Day21 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record Player(string Name) {

		public int Position { get; set; }
		public int DieRolls { get; set; }
		public int Points { get; set; }
		public long Score => Points * DieRolls;

		public Player(string name, int position) : this(name) {
			Position = position;
		}

		public void Move(int sumOfDieRolls) {
			Position += sumOfDieRolls;
			Position = (Position - 1) % 10 + 1;
			Points += Position;
			DieRolls += 3;
		}

	};

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
		List<Player> players = input.Select(i => ParseLine(i)).ToList();

		int deterministicDie = 0;

		int turn = 0;
		int dieRolls = 0;
		while (players.Where(p => p.Points > 21).Any() is false) {
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

	private static Player ParseLine(string input) {
		Match match = Regex.Match(input, @"(?<name>.*) starting position: (?<start>\d+)");
		if (match.Success) {
			return new(match.Groups["name"].Value, int.Parse(match.Groups["start"].Value));
		}
		throw new NotSupportedException();
	}
}
