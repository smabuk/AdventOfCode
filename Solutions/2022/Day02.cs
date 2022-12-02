namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 02: Rock Paper Scissors
/// https://adventofcode.com/2022/day/2
/// </summary>
[Description("Rock Paper Scissors")]
public sealed partial class Day02 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		List<RockPaperScissors> round = input.Select(i => ParseLinePt1(i)).ToList();
		return round.Sum(x => x.Score);
	}

	private static int Solution2(string[] input) {
		List<RockPaperScissors> round = input.Select(i => ParseLinePt2(i)).ToList();
		return round.Sum(x => x.Score);
	}

	private enum HandShape {
		Rock = 0,
		Paper = 1,
		Scissors = 2,
	}

	private enum Outcome {
		Win,
		Lose,
		Draw,
	}

	sealed record class RockPaperScissors {
		public HandShape OpponentChoice { get; set; }
		public HandShape MyChoice { get; set; }

		public RockPaperScissors(HandShape opponentChoice, HandShape myChoice) {
			OpponentChoice = opponentChoice;
			MyChoice = myChoice;
		}

		public RockPaperScissors(HandShape opponentChoice, Outcome myOutcome) {
			OpponentChoice = opponentChoice;

			switch (myOutcome) {
				case Outcome.Draw:
					MyChoice = OpponentChoice;
					break;
				case Outcome.Win:
					MyChoice = OpponentChoice switch {
						HandShape.Rock => HandShape.Paper,
						HandShape.Paper => HandShape.Scissors,
						HandShape.Scissors => HandShape.Rock,
						_ => throw new ArgumentOutOfRangeException(nameof(OpponentChoice)),
					};
					break;
				case Outcome.Lose:
					MyChoice = OpponentChoice switch {
						HandShape.Rock => HandShape.Scissors,
						HandShape.Paper => HandShape.Rock,
						HandShape.Scissors => HandShape.Paper,
						_ => throw new ArgumentOutOfRangeException(nameof(OpponentChoice)),
					};
					break;
			}
		}

		private int MyChoiceScore => MyChoice switch {
			HandShape.Rock => 1,
			HandShape.Paper => 2,
			HandShape.Scissors => 3,
			_ => 0,
		};

		public int Score {
			get {
				const int LOSS = 0;
				const int DRAW = 3;
				const int WIN = 6;
				if (OpponentChoice == MyChoice) {
					return MyChoiceScore + DRAW;
				} else if (OpponentChoice == HandShape.Rock && MyChoice == HandShape.Paper
					|| OpponentChoice == HandShape.Paper && MyChoice == HandShape.Scissors
					|| OpponentChoice == HandShape.Scissors && MyChoice == HandShape.Rock) {
					return MyChoiceScore + WIN;
				}
				return MyChoiceScore + LOSS;
			}
		}
	};

	private static RockPaperScissors ParseLinePt1(string input) {
		Match match = InputRegEx().Match(input);
		if (match.Success) {
			HandShape opponent = match.Groups["opp"].Value switch {
				"A" => HandShape.Rock,
				"B" => HandShape.Paper,
				"C" => HandShape.Scissors,
				_ => throw new ArgumentOutOfRangeException("opponent"),
			};
			HandShape me = match.Groups["hint"].Value switch {
				"X" => HandShape.Rock,
				"Y" => HandShape.Paper,
				"Z" => HandShape.Scissors,
				_ => throw new ArgumentOutOfRangeException("hint"),
			};
			return new(opponent, me);
		}
		return null!;
	}

	private static RockPaperScissors ParseLinePt2(string input) {
		Match match = InputRegEx().Match(input);
		if (match.Success) {
			HandShape opponent = match.Groups["opp"].Value switch {
				"A" => HandShape.Rock,
				"B" => HandShape.Paper,
				"C" => HandShape.Scissors,
				_ => throw new ArgumentOutOfRangeException("opponent"),
			};
			Outcome choice = match.Groups["hint"].Value switch {
				"X" => Outcome.Lose,
				"Y" => Outcome.Draw,
				"Z" => Outcome.Win,
				_ => throw new ArgumentOutOfRangeException("hint"),
			};
			return new(opponent, choice);
		}
		return null!;
	}

	[GeneratedRegex("""(?<opp>A|B|C) (?<hint>\D)""")]
	private static partial Regex InputRegEx();
}
