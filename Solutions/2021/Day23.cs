using static AdventOfCode.Solutions._2021.Day23;
using static AdventOfCode.Solutions._2021.Day23.Game;

namespace AdventOfCode.Solutions._2021;

/// <summary>
/// Day 23: Amphipod
/// https://adventofcode.com/2021/day/23
/// </summary>
[Description("Amphipod")]
public class Day23
{

	public static string Part1(string[] input, params object[]? args)
	{
		bool testing = GetArgument(args, 1, false);
		if (testing is false) { return Solution1ByHand(input); }
		return Solution1(input).ToString();
	}
	public static string Part2(string[] input, params object[]? args)
	{
		bool testing = GetArgument(args, 1, false);
		if (testing is false) { return Solution2ByHand(input); }
		return Solution2(input).ToString();
	}

	private static int Solution1(string[] input)
	{
		Game startGameBoard = new(Game.GameType.Part1);
		startGameBoard = startGameBoard.Init(input);
		int leastEnergy = 0;

		Dictionary<GameState, int> gameStates = [];

		leastEnergy = PlayTheGame(startGameBoard);

		return leastEnergy;



		int PlayTheGame(Game gameBoard, int count = 0)
		{
			if (gameBoard.Completed) {
				return gameBoard.EnergyExpended;
			}

			// Check if we have already seen this game state
			GameState gameState = gameBoard.GetGameState();
			if (gameStates.TryGetValue(gameState, out int value)) {
				return int.Min(gameBoard.EnergyExpended, value);
			}

			Game newGame = gameBoard.Copy();
			while (!gameBoard.Completed && count <= 1_000) {
				count++;
				gameStates.Add(gameState, gameBoard.EnergyExpended);
				int leastEnergy = int.MaxValue;
				foreach (Amphipod amphipod in gameBoard.Amphipods.Values) {
					foreach (Point move in gameBoard.MovementOptions(amphipod.Name)) {
						newGame = gameBoard.Copy();
						newGame.Move(amphipod.Name, move);
						int energy = PlayTheGame(newGame, count);
						leastEnergy = int.Min(energy, leastEnergy);
					}
				}
			}

			return leastEnergy;
		}
	}


	private static int Solution2(string[] input)
	{
		//Game startGameBoard = new(Game.GameType.Part2);
		//startGameBoard = startGameBoard.Init(input);
		int leastEnergy = 0;

		return leastEnergy;
	}

	public enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}

	/// <summary>
	/// Represents an amphipod, a type of entity characterized by its name, type, position, and energy expenditure.
	/// </summary>
	/// <remarks>Amphipods are designed to move within a grid-based environment, expending energy based on their
	/// type. Each amphipod has a target side room determined by its type, and it can move to specific positions while
	/// calculating energy costs. This record is immutable, and movement operations return new instances reflecting updated
	/// states.</remarks>
	/// <param name="Name"></param>
	/// <param name="Type"></param>
	/// <param name="Position"></param>
	public abstract record Amphipod(string Name, char Type, Point Position, int EnergyExpended)
	{
		/// <summary>
		/// Gets a value indicating whether the current position is at the home location.
		/// </summary>
		public bool IsHome => TargetSideRoom == Position.X;

		/// <summary>
		/// Gets the target side room number based on the current type.
		/// </summary>
		public abstract int TargetSideRoom { get; }

		/// <summary>
		/// Gets the energy cost associated with a single step, based on the type.
		/// </summary>
		public abstract int EnergyPerStep { get; }

		/// <summary>
		/// Moves the current <see cref="Amphipod"/> to the specified target position.
		/// </summary>
		/// <remarks>The movement is calculated based on the relative position of the target. If the target is above
		/// the current position, the amphipod moves upward first. Horizontal movement is performed before vertical movement
		/// when moving downward. The method returns a new instance of <see cref="Amphipod"/> to reflect the updated
		/// state.</remarks>
		/// <param name="target">The target position to move to, represented as a <see cref="Point"/>. The <see cref="Point"/> specifies the X and
		/// Y coordinates of the destination.</param>
		/// <returns>A new <see cref="Amphipod"/> instance representing the state of the amphipod after completing the move to the
		/// target position.</returns>
		public Amphipod Move(Point target)
		{
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

			Amphipod amphipod = this;
			foreach (Direction direction in directions) {
				amphipod = amphipod.Move(direction);
			}

			return amphipod;
		}

		/// <summary>
		/// Moves the amphipod in the specified direction by the given number of steps.
		/// </summary>
		/// <remarks>The movement updates the amphipod's position and calculates the energy expended based on the
		/// number of steps taken. The method does not modify the current instance but returns a new instance with the updated
		/// state.</remarks>
		/// <param name="direction">The direction in which to move the amphipod. Must be one of the defined <see cref="Direction"/> values.</param>
		/// <param name="steps">The number of steps to move in the specified direction. Defaults to 1. Must be a positive integer.</param>
		/// <returns>A new <see cref="Amphipod"/> instance with its position updated based on the movement and its energy expenditure
		/// adjusted accordingly.</returns>
		/// <exception cref="NotImplementedException">Thrown if the specified <paramref name="direction"/> is not implemented.</exception>
		public Amphipod Move(Direction direction, int steps = 1)
		{
			int stepsTaken;
			Point newPosition;
			(stepsTaken, newPosition) = direction switch
			{
				Direction.Up => (Position.Y - 1, Position with { Y = 1 }),
				Direction.Down => (steps, Position with { Y = Position.Y + steps }),
				Direction.Left => (steps, Position with { X = Position.X - steps }),
				Direction.Right => (steps, Position with { X = Position.X + steps }),
				_ => throw new NotImplementedException(),
			};

			return this with
			{
				Position = newPosition,
				EnergyExpended = EnergyExpended + (stepsTaken * EnergyPerStep)
			};
		}


	};

	public record AmberAmphipod(Point Position) : Amphipod($"A{Position.X}{Position.Y}", 'A', Position, 0)
	{
		public override int TargetSideRoom => 3;
		public override int EnergyPerStep  => 1;
	}
	public record BronzeAmphipod(Point Position) : Amphipod($"B{Position.X}{Position.Y}", 'B', Position, 0)
	{
		public override int TargetSideRoom =>  5;
		public override int EnergyPerStep  => 10;
	}
	public record CopperAmphipod(Point Position) : Amphipod($"C{Position.X}{Position.Y}", 'C', Position, 0)
	{
		public override int TargetSideRoom =>   7;
		public override int EnergyPerStep  => 100;
	}
	public record DesertAmphipod(Point Position) : Amphipod($"D{Position.X}{Position.Y}", 'D', Position, 0)
	{
		public override int TargetSideRoom =>    9;
		public override int EnergyPerStep  => 1000;
	}

	public record GameState(string Hash);

	public record Game(GameType Type)
	{
		//     Part1						Part 2
		//           11                            11
		//  12 4 6 8 01					  12 4 6 8 01
		// #############				 #############
		// #...........#				 #...........#
		// ###.#.#.#.###				 ###.#.#.#.###
		//   #.#.#.#.#					   #.#.#.#.#
		//   #########					   #.#.#.#.#
		//    A B C D					   #.#.#.#.#
		//								   #########
		//								    A B C D

		readonly int[] POSSIBLE_X = [1, 2, 4, 6, 8, 10, 11];
		public Dictionary<string, Amphipod> Amphipods { get; init; } = [];
		public int EnergyExpended => Amphipods.Sum(amp => amp.Value.EnergyExpended);
		public char[,] Board = new char[13, 7];
		public char[,] EmptyBoard = new char[13, 7];
		private readonly int BOTTOM = Type switch
		{
			GameType.Part1 => 4,
			GameType.Part2 => 6,
			_ => throw new NotImplementedException(),
		};

		public Game Copy()
		{
			return this with
			{
				Amphipods  = new(Amphipods),
				Board      = (char[,])Board.Clone(),
				EmptyBoard = (char[,])EmptyBoard.Clone(),
			};
		}

		public bool Completed
			=> Amphipods.Values.All(amp => amp.Position.Y > 1 && amp.IsHome);

		public bool CanMove(Amphipod amphipod)
		{
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
				if (y == BOTTOM - 1 && Board[x, y + 1] == amphipod.Type) {
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

			if (Board[amphipod.TargetSideRoom, 2] == amphipod.Type || Board[amphipod.TargetSideRoom, 2] == '.') {
				if (Board[amphipod.TargetSideRoom, 3] == amphipod.Type || Board[amphipod.TargetSideRoom, 3] == '.') {
					return true;
				}
			}

			return false;
		}

		public IEnumerable<Point> MovementOptions(string Name)
		{
			//int[] NextDoorMap = new int[] { 1, 2, 4, 6, 8, 10, 11 };
			Amphipod amphipod = Amphipods[Name];
			if (CanMove(amphipod)) {
				if (amphipod.Position.Y == 1) {
					if (Board[amphipod.TargetSideRoom, 3] == '.') {
						yield return new Point(amphipod.TargetSideRoom, 3);
					} else if (Board[amphipod.TargetSideRoom, 3] == amphipod.Type && Board[amphipod.TargetSideRoom, 2] == '.') {
						yield return new Point(amphipod.TargetSideRoom, 3);
					}
				}
			}
			char[] possibles = [.. "............."];
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

		public void Move(string Name, Direction direction)
		{
			Amphipod amphipod = Amphipods[Name];
			if (CanMove(amphipod)) {
				Amphipods[Name] = amphipod.Move(direction);
				Board = EmptyBoard.FillWithAmphipods([.. Amphipods.Values]);
			}
		}

		public void Move(string Name, Point target)
		{
			Amphipod amphipod = Amphipods[Name];
			if (CanMove(amphipod)) {
				Amphipods[Name] = amphipod.Move(target);
				Board = EmptyBoard.FillWithAmphipods([.. Amphipods.Values]);
			}
		}

		public enum GameType
		{
			Part1,
			Part2
		}
	}



	private static string Solution1ByHand(string[] input)
	{
		Game gameBoard = new(Game.GameType.Part1);
		gameBoard = gameBoard.Init(input);


		if (new string([.. gameBoard.Amphipods.Keys.Select(s => s[0])]) != "DCDCABAB") {
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

		if (gameBoard.Completed is false) {
			return "** Solution not written yet **";
		}

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

	private static string Solution2ByHand(string[] input)
	{
		Game gameBoard = new(Game.GameType.Part2);
		gameBoard = gameBoard.Init(input);

		if (new string([.. gameBoard.Amphipods.Keys.Select(s => s[0])]) != "DDDCDCBCABABAACB") {
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

		if (gameBoard.Completed is false) {
			return "** Solution not written yet **";
		}

		return $"{gameBoard.EnergyExpended}  by hand";
	}
}

file static class Day23Extensions
{
	public static Game Init(this Game game, string[] input)
	{
		for (int i = 0; i < input.Length; i++) {
			if (input[i].Length < 13) {
				input[i] = $"{input[i]}{new string(' ', 13 - (input[i].Length % 13))}";
			}
		}
		int yMax = 3;
		if (game.Type is GameType.Part2) {
			List<string> newInput = [.. input];
			newInput.Insert(3, "  #D#C#B#A#  ");
			newInput.Insert(4, "  #D#B#A#C#  ");
			input = [.. newInput];
			yMax = 5;
		}
		game.EmptyBoard = String.Join("", input)
			.Replace('A', '.')
			.Replace('B', '.')
			.Replace('C', '.')
			.Replace('D', '.')
			.To2dArray(input[0].Length);

		Dictionary<string, Amphipod> amphipods = [];

		for (int room = 0; room < 4; room++) {
			for (int y = 2; y <= yMax; y++) {
				int x = 3 + (room * 2);
				// Replace this line in Init method:
				// Amphipod amphipod = new($"{input[y][x]}{x}{y}", input[y][x], new(x, y), 0);
				Point position = new(x, y);
				Amphipod amphipod = input[y][x] switch
				{
					'A' => new AmberAmphipod(position),
					'B' => new BronzeAmphipod(position),
					'C' => new CopperAmphipod(position),
					'D' => new DesertAmphipod(position),
					_ => throw new InvalidOperationException($"Unknown amphipod type: {input[y][x]} at ({x},{y})")
				};
				amphipods.Add(amphipod.Name, amphipod);
			}
		}

		char[,] emptyBoard = (char[,])game.EmptyBoard.Clone();
		return game with
		{
			Amphipods = amphipods,
			EmptyBoard = emptyBoard,
			Board = emptyBoard.FillWithAmphipods([.. amphipods.Values]),
		};
	}

	public static char[,] FillWithAmphipods(this char[,] emptyBoard, List<Amphipod> amphipods)
	{
		char[,] board = (char[,])emptyBoard.Clone();
		foreach (Amphipod amphipod in amphipods) {
			board[amphipod.Position.X, amphipod.Position.Y] = amphipod.Type;
		}

		return board;
	}


	public static GameState GetGameState(this Game gameBoard)
	{
		string state = string.Join(',',
			gameBoard.Amphipods.Values
				.OrderBy(x => x.Name)
				.Select(amphipod => $"{amphipod.Type}({amphipod.Position.X},{amphipod.Position.Y})")
		);

		return new GameState(state);
	}


}
