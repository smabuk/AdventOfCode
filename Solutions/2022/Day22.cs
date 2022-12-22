namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 22: Monkey Map
/// https://adventofcode.com/2022/day/22
/// </summary>
[Description("Monkey Map")]
public sealed partial class Day22 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static readonly char TILE  = '.';
	private static readonly char WALL  = '#';
	private static readonly char SPACE = ' ';

	private static char[,] LoadMap(string[] input) {
		int maxColumnSize = input.Max(i => i.Length);
		int noOfRows = input.Length;

		char[,] monkeyMap = new char[maxColumnSize, noOfRows];

		for (int row = 0; row < input.Length; row++) {
			string mapRow = input[row];
			for (int col = 0; col < maxColumnSize; col++) {
				if (col >= mapRow.Length) {
					monkeyMap[col, row] = SPACE;
				} else {
					monkeyMap[col, row] = mapRow[col];
				}
			}
		}

		return monkeyMap;
	}

	private static int Solution1(string[] input) {
		char[,] monkeyMap = LoadMap(input[..^2]);
		List<Instruction> instructions = Instruction.Parse(input[^1]).ToList();
		Position currentPosition = new(-1, 0, Facing.right);
		currentPosition = NextPosition(currentPosition, new STEP_Instruction(1), monkeyMap);

		foreach (Instruction instruction in instructions) {
			currentPosition = NextPosition(currentPosition, instruction, monkeyMap);
			Debug.WriteLine(currentPosition);
		}

		return currentPosition.Password;
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

	private static Position NextPosition(Position current, Instruction instruction, char[,] map) {
		if (instruction is ROTATE_Instruction rotate) {
			Debug.WriteLine($"ROT: {rotate.Rotation}");
			return current with {
				Facing = rotate.Rotation switch {
					Direction.left => (Facing)(((int)current.Facing + 3) % 4) ,
					Direction.right => (Facing)(((int)current.Facing + 1) % 4),
					_ => throw new NotImplementedException(),
				}
			};
		}
		if (instruction is STEP_Instruction move) {
			Debug.WriteLine($"MOV {move.Steps} {current.Facing} from {current.Column},{current.Row}");
			Point position = new(current.Column, current.Row);
			Point movement = current.Facing switch {
				Facing.right => new(1, 0),
				Facing.down => new(0, 1),
				Facing.left => new(-1, 0),
				Facing.up => new(0, -1),
				_ => throw new NotImplementedException(),
			};
			Point next = position;
			for (int i = 0; i < move.Steps; i++) {
				do {
					next += movement;
					if (next.X < 0) {
						next.X = map.NoOfColumns() - 1;
					} else if (next.X >= map.NoOfColumns()) {
						next.X = 0;
					} else if (next.Y < 0) {
						next.Y = map.NoOfRows() - 1;
					} else if (next.Y >= map.NoOfRows()) {
						next.Y = 0;
					}

					if (map[next.X, next.Y] == WALL) {
						return current with { Column = position.X, Row = position.Y };
					}
				} while (map[next.X, next.Y] != TILE);

				position = next;
			}

			return current with { Column = position.X, Row = position.Y };
		}

		throw new NotImplementedException();
	}


	private record Instruction() : IParsable<Instruction> {
		public static IEnumerable<Instruction> Parse(string s) {
			s = s.Replace("L", ",left,").Replace("R", ",right,");
			string[] tokens = s.Split(',', StringSplitOptions.RemoveEmptyEntries);
			foreach (string token in tokens) {
				if (char.IsNumber(token[0])) {
					yield return new STEP_Instruction(token.AsInt());
				} else {
					yield return new ROTATE_Instruction(Enum.Parse<Direction>(token));
				}
			}
		}

		public static Instruction Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result) => throw new NotImplementedException();
	}

	private record ROTATE_Instruction(Direction Rotation) : Instruction();
	private record STEP_Instruction(int Steps) : Instruction();

	private record Position(int Column, int Row, Facing Facing) : Instruction() {
		public int Password => ((Row + 1) * 1000) + ((Column + 1) * 4) + (int)Facing;
	};
	
	private enum Facing {
		right = 0,
		down = 1,
		left = 2,
		up = 3
	}

	private enum Direction {
		left,
		right,
	}
}
