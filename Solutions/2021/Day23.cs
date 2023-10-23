namespace AdventOfCode.Solutions._2021;

/// <summary>
/// Day 23: Amphipod
/// https://adventofcode.com/2021/day/23
/// </summary>
[Description("Amphipod")]
public class Day23 {

	public static string Part1(string[] input, params object[]? args) {
		bool testing = GetArgument(args, 1, false);
		if (testing is false) { return Solution1ByHand(input); }
		return Solution1(input, testing).ToString();
	}
	public static string Part2(string[] input, params object[]? args) {
		bool testing = GetArgument(args, 1, false);
		if (testing is false) { return Solution2ByHand(input); }
		return Solution2(input).ToString();
	}

	private static int Solution1(string[] input, bool testing) {
		Game startGameBoard = new(Game.GameType.Part1);
		startGameBoard.Init(input);
		int leastEnergy = 0;
		Dictionary<GameState, int> gameStates = [];

		leastEnergy = PlayTheGame(startGameBoard);

		return leastEnergy;



		int PlayTheGame(Game gameBoard) {
			if (gameBoard.Completed) {
				return gameBoard.EnergyExpended;
			}
			GameState gameState = gameBoard.GetGameState();
			if (gameStates.ContainsKey(gameState)) {
				return Math.Min(gameBoard.EnergyExpended, gameStates[gameState]);
			}

			Game newGame = gameBoard;
			int count = 0;
			while (!gameBoard.Completed || count > 1000) {
				count++;
				_ = gameStates.TryAdd(gameState, gameBoard.EnergyExpended);
				int leastEnergy = int.MaxValue;
				foreach (var amphipod in gameBoard.Amphipods.Values) {
					foreach (var move in gameBoard.MovementOptions(amphipod.Name)) {
						newGame = gameBoard;
						newGame.Move(amphipod.Name, move);
						int energy = PlayTheGame(newGame);
						leastEnergy = Math.Min(energy, leastEnergy);
					}
				}
			}
			return leastEnergy;
		}
	}


	private static int Solution2(string[] input) {
		Game startGameBoard = new(Game.GameType.Part1);
		startGameBoard.Init(input);
		int leastEnergy = 0;

		return leastEnergy;
	}

	public enum Direction {
		Up,
		Down,
		Left,
		Right
	}

	public record struct Amphipod(string Name, string Type, Point Position) {
		public bool IsHome => TargetSideRoom == Position.X;

		public int EnergyExpended { get; set; } = 0;
		public int TargetSideRoom => Type switch {
			"A" => 3,
			"B" => 5,
			"C" => 7,
			"D" => 9,
			_ => throw new NotImplementedException(),
		};
		public int EnergyPerStep => Type switch {
			"A" => 1,
			"B" => 10,
			"C" => 100,
			"D" => 1000,
			_ => throw new NotImplementedException(),
		};

		public Amphipod Move(Point target) {
			List<Direction> directions = [];
			if (target.Y == 1) {
				if (target.Y - Position.Y < 0) {
					directions.Add(Direction.Up);
				}
				for (int i = 0; i < Math.Abs(target.X - Position.X); i++) {
					directions.Add((target.X - Position.X) < 0 ? Direction.Left : Direction.Right);
				}
			} else {
				for (int i = 0; i < Math.Abs(target.X - Position.X); i++) {
					directions.Add((target.X - Position.X) < 0 ? Direction.Left : Direction.Right);
				}
				for (int i = 1; i < target.Y; i++) {
					directions.Add(Direction.Down);
				}
			}

			foreach (Direction direction in directions) {
				_ = Move(direction);
			}

			return this;
		}
		public Amphipod Move(Direction direction, int steps = 1) {
			int stepsTaken;
			(stepsTaken, Position) = direction switch {
				Direction.Up => (Position.Y - 1, Position with { Y = 1 }),
				Direction.Down => (steps, Position with { Y = Position.Y + steps }),
				Direction.Left => (steps, Position with { X = Position.X - steps }),
				Direction.Right => (steps, Position with { X = Position.X + steps }),
				_ => throw new NotImplementedException(),
			};

			EnergyExpended += stepsTaken * EnergyPerStep;
			return this;
		}


	};

	public record GameState(string Hash);

	public record struct Game(Game.GameType Type) {
		readonly int[] POSSIBLE_X = [1, 2, 4, 6, 8, 10, 11];
		public Dictionary<string, Amphipod> Amphipods = [];
		public int EnergyExpended => Amphipods.Sum(amp => amp.Value.EnergyExpended);
		public char[,] Board = new char[13, 7];
		public char[,] board = new char[13, 7];
		private int BOTTOM = 4;

		public void Init(string[] input) {
			for (int i = 0; i < input.Length; i++) {
				if (input[i].Length < 13) {
					input[i] = $"{input[i]}{new string(' ', 13 - (input[i].Length % 13))}";
				}
			}
			int yMax = 3;
			if (Type is GameType.Part2) {
				List<string> newInput = input.ToList();
				newInput.Insert(3, "  #D#C#B#A#  ");
				newInput.Insert(4, "  #D#B#A#C#  ");
				input = newInput.ToArray();
				yMax = 5;
				BOTTOM = 6;
			}
			board = String.Join("", input)
				.Replace("A", ".")
				.Replace("B", ".")
				.Replace("C", ".")
				.Replace("D", ".")
				.To2dArray(input[0].Length);
			Amphipods.Clear();

			for (int room = 0; room < 4; room++) {
				for (int y = 2; y <= yMax; y++) {
					int x = 3 + (room * 2);
					Amphipod amphipod = new($"{input[y][x]}{x}{y}", $"{input[y][x]}", new(x, y));
					Amphipods.Add(amphipod.Name, amphipod);
				}
			}

			Remap();
		}
		public void Remap() {
			Board = (char[,])board.Clone();
			foreach (Amphipod amphipod in Amphipods.Values) {
				Board[amphipod.Position.X, amphipod.Position.Y] = amphipod.Type[0];
			}
		}

		public bool Completed => Amphipods.Values
			.All(amp => amp.Position.Y > 1 && amp.IsHome);

		public bool CanMove(Amphipod amphipod) {
			//int[] NextDoorMap = new int[] { 1, 2, 4, 6, 8, 10, 11 };
			(int x, int y) = amphipod.Position;
			if (y > BOTTOM) {
				return false;
			}
			if (amphipod.IsHome) {
				if (y == BOTTOM) {
					return false;
				}
					// TODO extra rows above
				if (y == BOTTOM - 1 && Board[x, y + 1] == amphipod.Type[0]) {
					return false;
				}
			}
			if (y > 1 || y < BOTTOM) {
				if (Board[x, y - 1] == '.' && (Board[x - 1, 1] == '.' || Board[x + 1, 1] == '.')) {
					return true;
				}
			}

			for (int i = Math.Min(x, amphipod.TargetSideRoom); i < Math.Min(x, amphipod.TargetSideRoom); i++) {
				if (Board[i, 1] != '.') {
					return false;
				}
			}

			if (Board[amphipod.TargetSideRoom, 2] == amphipod.Type[0] || Board[amphipod.TargetSideRoom, 2] == '.') {
				if (Board[amphipod.TargetSideRoom, 3] == amphipod.Type[0] || Board[amphipod.TargetSideRoom, 3] == '.') {
					return true;
				}
			}

			return false;
		}

		public IEnumerable<Point> MovementOptions(string Name) {
			//int[] NextDoorMap = new int[] { 1, 2, 4, 6, 8, 10, 11 };
			Amphipod amphipod = Amphipods[Name];
			if (CanMove(amphipod)) {
				if (amphipod.Position.Y == 1) {
					if (Board[amphipod.TargetSideRoom, 3] == '.') {
						yield return new Point(amphipod.TargetSideRoom, 3);
					} else if (Board[amphipod.TargetSideRoom, 3] == amphipod.Type[0] && Board[amphipod.TargetSideRoom, 2] == '.') {
						yield return new Point(amphipod.TargetSideRoom, 3);
					}
				}
			}
			char[] possibles = ".............".ToCharArray();
			foreach (int x in POSSIBLE_X) {
				possibles[x] = Board[x, 1];
			}
			foreach (int x in POSSIBLE_X) {
				if (amphipod.Position.X < x) {
					if (new string(possibles[amphipod.Position.X..x]) == new string('.', x - amphipod.Position.X)) {
						yield return new Point(x, 1);
					}
				}
				if (amphipod.Position.X > x) {
					if (new string(possibles[x..amphipod.Position.X]) == new string('.', amphipod.Position.X - x)) {
						yield return new Point(x, 1);
					}
				}
			}
		}

		public void Move(string Name, Direction direction) {
			Amphipod amphipod = Amphipods[Name];
			if (CanMove(amphipod)) {
				Amphipods[Name] = amphipod.Move(direction);
				Remap();
			}
		}
		public void Move(string Name, Point target) {
			Amphipod amphipod = Amphipods[Name];
			if (CanMove(amphipod)) {
				Amphipods[Name] = amphipod.Move(target);
				Remap();
			}
		}

		public enum GameType {
			Part1,
			Part2
		}

		public GameState GetGameState() {
			string state = "";
			foreach (Amphipod amphipod in Amphipods.Values.OrderBy(x => x.Name)) {
				state += $"{amphipod.Name}{amphipod.Position.X}{amphipod.Position.Y}";
			}
			return new GameState(state);
		}

	}

	private static string Solution1ByHand(string[] input) {
		Game gameBoard = new(Game.GameType.Part1);
		gameBoard.Init(input);


		if (new string(gameBoard.Amphipods.Keys.Select(s => s[0]).ToArray()) != "DCDCABAB") {
			return "** Solution not written yet **";
		}
		gameBoard.Move("A92", new Point(10, 1));
		gameBoard.Move("A72", new Point(2, 1));
		gameBoard.Move("B93", new Point(4, 1));
		gameBoard.Move("D52", new Point(6, 1));
		gameBoard.Move("D52", new Point(9, 3));
		gameBoard.Move("B73", new Point(8, 1));
		gameBoard.Move("C53", new Point(6, 1));
		gameBoard.Move("C53", new Point(7, 3));
		gameBoard.Move("B93", new Point(5, 3));
		gameBoard.Move("B73", new Point(5, 2));
		gameBoard.Move("D32", new Point(4, 1));
		gameBoard.Move("D32", new Point(9, 2));
		gameBoard.Move("C33", new Point(4, 1));
		gameBoard.Move("C33", new Point(7, 2));
		gameBoard.Move("A92", new Point(3, 3));
		gameBoard.Move("A72", new Point(3, 2));

		return $"{gameBoard.EnergyExpended}  by hand";

		// A92 => U   R
		// A72 => U   LLLLL
		// B92 => UU  LLLLL
		// D52 => U   RRRR   DD
		// B53 => UU  R
		// C53 => UU  RR     DD
		// B92 => R   DD
		// B73 => LLL D
		// D32 => U   RRRRRR D
		// C33 => UU  RRRR   D
		// A92 => LLLLLLL    DD
		// A72 => RD

		// #############
		// #...........#
		// ###D#D#A#A###
		//   #C#C#B#B#
		//   #########
		// 2
		// 
		// #############
		// #.........A.#
		// ###D#D#A# ###
		//   #C#C#B#B#
		//   #########
		// 2
		// 
		// #############
		// #.A.......A.#
		// ###D#D# # ###
		//   #C#C#B#B#
		//   #########
		// 2 + 6
		// 
		// #############
		// #.A.B.....A.#
		// ###D#D# # ###
		//   #C#C#B# #
		//   #########
		// 2 + 6 + 70
		// 
		// #############
		// #.A.B.....A.#
		// ###D# # # ###
		//   #C#C#B#D#
		//   #########
		// 2 + 6 + 70 + 7000
		// 
		// #############
		// #.A.B...B.A.#
		// ###D# # # ###
		//   #C#C# #D#
		//   #########
		// 2 + 6 + 70 + 7000 + 30
		// 
		// #############
		// #.A.B...B.A.#
		// ###D# # # ###
		//   #C# #C#D#
		//   #########
		// 2 + 6 + 70 + 7000 + 30 + 600
		// 
		// #############
		// #.A.....B.A.#
		// ###D# # # ###
		//   #C#B#C#D#
		//   #########
		// 2 + 6 + 70 + 7000 + 30 + 600 + 30
		// 
		// #############
		// #.A.......A.#
		// ###D#B# # ###
		//   #C#B#C#D#
		//   #########
		// 2 + 6 + 70 + 7000 + 30 + 600 + 30 + 40
		// 
		// #############
		// #.A.......A.#
		// ### #B# #D###
		//   #C#B#C#D#
		//   #########
		// 2 + 6 + 70 + 7000 + 30 + 600 + 30 + 40 + 8000
		// 
		// #############
		// #.A.......A.#
		// ### #B#C#D###
		//   # #B#C#D#
		//   #########
		// 2 + 6 + 70 + 7000 + 30 + 600 + 30 + 40 + 8000 + 700
		// 
		// #############
		// #.A.........#
		// ### #B#C#D###
		//   #A#B#C#D#
		//   #########
		// 2 + 6 + 70 + 7000 + 30 + 600 + 30 + 40 + 8000 + 700 + 9
		// 
		// #############
		// #...........#
		// ###A#B#C#D###
		//   #A#B#C#D#
		//   #########
		// 2 + 6 + 70 + 7000 + 30 + 600 + 30 + 40 + 8000 + 700 + 9 + 2
		// 
		// A =>    19 (2 + 6 + 9 + 2)
		// B =>   170 (70 + 30 + 30 + 40)
		// C =>  1300 (600 + 700)
		// D => 15000 (7000 + 8000)
		//      16489

	}

	private static string Solution2ByHand(string[] input) {
		Game gameBoard = new(Game.GameType.Part2);
		gameBoard.Init(input);

		if (new string(gameBoard.Amphipods.Keys.Select(s => s[0]).ToArray()) != "DDDCDCBCABABAACB") {
			return "** Solution not written yet **";
		}

		gameBoard.Move("A92", new Point(1, 1));
		gameBoard.Move("A93", new Point(2, 1));
		gameBoard.Move("C94", new Point(11, 1));
		gameBoard.Move("B95", new Point(10, 1));
		gameBoard.Move("D52", new Point(6, 1));
		gameBoard.Move("D52", new Point(9, 5));
		gameBoard.Move("D32", new Point(4, 1));
		gameBoard.Move("D32", new Point(9, 4));
		gameBoard.Move("D33", new Point(4, 1));
		gameBoard.Move("D33", new Point(9, 3));
		gameBoard.Move("D34", new Point(4, 1));
		gameBoard.Move("D34", new Point(9, 2)); // Pretty sure up to here

		gameBoard.Move("C35", new Point(8, 1));
		gameBoard.Move("A93", new Point(3, 5));
		gameBoard.Move("A92", new Point(3, 4));
		gameBoard.Move("A72", new Point(6, 1));
		gameBoard.Move("A72", new Point(3, 3));
		gameBoard.Move("B73", new Point(2, 1));
		gameBoard.Move("A74", new Point(4, 1));
		gameBoard.Move("A74", new Point(3, 2));
		gameBoard.Move("B75", new Point(4, 1));
		gameBoard.Move("C35", new Point(7, 5));
		gameBoard.Move("C53", new Point(6, 1));
		gameBoard.Move("C53", new Point(7, 4));
		gameBoard.Move("B54", new Point(8, 1));
		gameBoard.Move("C55", new Point(6, 1));
		gameBoard.Move("C55", new Point(7, 3));
		gameBoard.Move("B54", new Point(5, 5));
		gameBoard.Move("B95", new Point(5, 4));
		gameBoard.Move("C94", new Point(7, 2));
		gameBoard.Move("B75", new Point(5, 3));
		gameBoard.Move("B73", new Point(5, 2));

		return $"{gameBoard.EnergyExpended}  by hand";
	}
}
