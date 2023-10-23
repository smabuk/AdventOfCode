namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 02: Rock Paper Scissors
/// https://adventofcode.com/2022/day/2
/// </summary>
[Description("Rock Paper Scissors")]
public sealed partial class Day02 {

	public static string Part1(string[] input, params object[]? _) => Solution(input, HandShape.Rock).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution(input, DesiredOutcome.Win).ToString();

	private static int Solution(string[] input, object type) {
		return input
			.Select(i => RockPaperScissors.Parse(i, type).Score)
			.Sum();
	}

	private enum HandShape {
		Rock     = 1,
		Paper    = 2,
		Scissors = 3,
	}

	private enum DesiredOutcome {
		Win,
		Lose,
		Draw,
	}

	record struct RockPaperScissors {
		public HandShape OpponentChoice { get; set; }
		public HandShape MyChoice { get; set; }

		public RockPaperScissors(HandShape opponentChoice, HandShape myChoice) {
			OpponentChoice = opponentChoice;
			MyChoice = myChoice;
		}

		public RockPaperScissors(HandShape opponentChoice, DesiredOutcome myOutcome) {
			OpponentChoice = opponentChoice;

			switch (myOutcome) {
				case DesiredOutcome.Draw:
					MyChoice = OpponentChoice;
					break;
				case DesiredOutcome.Win:
					MyChoice = OpponentChoice switch {
						HandShape.Rock => HandShape.Paper,
						HandShape.Paper => HandShape.Scissors,
						HandShape.Scissors => HandShape.Rock,
						_ => throw new ArgumentOutOfRangeException(nameof(OpponentChoice)),
					};
					break;
				case DesiredOutcome.Lose:
					MyChoice = OpponentChoice switch {
						HandShape.Rock => HandShape.Scissors,
						HandShape.Paper => HandShape.Rock,
						HandShape.Scissors => HandShape.Paper,
						_ => throw new ArgumentOutOfRangeException(nameof(OpponentChoice)),
					};
					break;
			}
		}

		private int MyChoiceScore => (int)MyChoice;

		public int Score {
			get {
				const int LOSS = 0;
				const int DRAW = 3;
				const int WIN = 6;
				if (OpponentChoice == MyChoice) {
					return MyChoiceScore + DRAW;
				} else if ((OpponentChoice == HandShape.Rock && MyChoice == HandShape.Paper)
					|| (OpponentChoice == HandShape.Paper && MyChoice == HandShape.Scissors)
					|| (OpponentChoice == HandShape.Scissors && MyChoice == HandShape.Rock)) {
					return MyChoiceScore + WIN;
				}
				return MyChoiceScore + LOSS;
			}
		}

		public static RockPaperScissors Parse(string input, object inputType) {
			HandShape opponent = ParseHandShape(input[0]);

			// We don't care about the value of type
			// it's just used for deciding how to parse the input
			return inputType switch {
				HandShape => new(opponent, ParseHandShape(input[2])),
				DesiredOutcome => new(opponent, ParseDesiredOutcome(input[2])),
				_ => throw new NotImplementedException(),
			};

			static HandShape ParseHandShape(char input) {
				return input switch {
					'A' or 'X' => HandShape.Rock,
					'B' or 'Y' => HandShape.Paper,
					'C' or 'Z' => HandShape.Scissors,
					_ => throw new ArgumentOutOfRangeException(nameof(input)),
				};
			}
			static DesiredOutcome ParseDesiredOutcome(char input) {
				return input switch {
					'X' => DesiredOutcome.Lose,
					'Y' => DesiredOutcome.Draw,
					'Z' => DesiredOutcome.Win,
					_ => throw new ArgumentOutOfRangeException(nameof(input)),
				};
			}
		}
	};

}
