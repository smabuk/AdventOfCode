namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 22: Monkey Map
/// https://adventofcode.com/2022/day/22
/// </summary>
[Description("Monkey Map")]
public sealed partial class Day22 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) {
		int cubeSize = GetArgument<int>(args, argumentNumber: 1, 50);
		return Solution2(input, cubeSize).ToString();
	}

	private static readonly char TILE  = '.';
	private static readonly char WALL  = '#';
	private static readonly char SPACE = ' ';

	private static readonly Dictionary<(int, Facing), (int, Facing)> TEST_CUBE_TRANSLATIONS = new() {
			{ (1, Facing.right), (5, Facing.down) },
			{ (1, Facing.down) , (4, Facing.down)  },
			{ (1, Facing.left) , (2, Facing.down)  },
			{ (1, Facing.up)   , (3, Facing.down)    },
								 	 			
			{ (2, Facing.right), (4, Facing.right) },
			{ (2, Facing.down) , (6, Facing.right)  },
			{ (2, Facing.left) , (3, Facing.left)  },
			{ (2, Facing.up)   , (1, Facing.right)    },
								 	 			
			{ (3, Facing.right), (2, Facing.right) },
			{ (3, Facing.down) , (6, Facing.up)  },
			{ (3, Facing.left) , (5, Facing.up)  },
			{ (3, Facing.up)   , (1, Facing.down)    },
								 	 			
			{ (4, Facing.right), (5, Facing.down) },
			{ (4, Facing.down) , (6, Facing.down)  },
			{ (4, Facing.left) , (2, Facing.left)  },
			{ (4, Facing.up)   , (1, Facing.up)    },
								 	 			
			{ (5, Facing.right), (1, Facing.left) },
			{ (5, Facing.down) , (3, Facing.right)  },
			{ (5, Facing.left) , (6, Facing.left)  },
			{ (5, Facing.up)   , (4, Facing.left)    },
								 	 			
			{ (6, Facing.right), (5, Facing.right) },
			{ (6, Facing.down) , (3, Facing.up)  },
			{ (6, Facing.left) , (2, Facing.up)  },
			{ (6, Facing.up)   , (4, Facing.up)    },
		};

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

	private static int Solution2(string[] input, int cubeSize) {
		char[,] monkeyMap = LoadMap(input[..^2]);
		List<Instruction> instructions = Instruction.Parse(input[^1]).ToList();

		int startX = cubeSize == 4 ? 8 : 50;

		Position currentPosition = new(startX, 0, Facing.right);

		foreach (Instruction instruction in instructions) {
			currentPosition = NextCubePosition(currentPosition, instruction, monkeyMap, cubeSize);
			Debug.WriteLine(currentPosition);
		}

		return currentPosition.Password;
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
			for (int i = 0; i < move.Steps; i++) {
				Point movement = current.Facing switch {
					Facing.right => new(1, 0),
					Facing.down => new(0, 1),
					Facing.left => new(-1, 0),
					Facing.up => new(0, -1),
					_ => throw new NotImplementedException(),
				};
				current = TakeAStep(current, movement);
			}

			return current;
		}

		throw new NotImplementedException();

		Position TakeAStep(Position position, Point movement) {
			Point nextXY = position.XY;
			do {
				nextXY += movement;
				if (nextXY.X < 0) {
					nextXY.X = map.NoOfColumns() - 1;
				} else if (nextXY.X >= map.NoOfColumns()) {
					nextXY.X = 0;
				} else if (nextXY.Y < 0) {
					nextXY.Y = map.NoOfRows() - 1;
				} else if (nextXY.Y >= map.NoOfRows()) {
					nextXY.Y = 0;
				}

				if (map[nextXY.X, nextXY.Y] == WALL) {
					return position;
				}
			} while (map[nextXY.X, nextXY.Y] != TILE);

			return position with { Column = nextXY.X, Row = nextXY.Y };
		}
	}

	private static Position NextCubePosition(Position current, Instruction instruction, char[,] map, int cubeSize) {
		if (instruction is ROTATE_Instruction rotate) {
			return current with {
				Facing = rotate.Rotation switch {
					Direction.left => (Facing)(((int)current.Facing + 3) % 4),
					Direction.right => (Facing)(((int)current.Facing + 1) % 4),
					_ => throw new NotImplementedException(),
				}
			};
		}
		if (instruction is STEP_Instruction move) {
			Debug.WriteLine("");
			Debug.WriteLine($"MOV {move.Steps} steps {current.Facing} from {current.Column},{current.Row}");
			for (int i = 0; i < move.Steps; i++) {
				//int face = GetFace(current);
				current = TakeATestStep(current);
			}

			return current;
		}

		throw new NotImplementedException();

		Position TakeATestStep(Position position) {
			Dictionary<int, Point> testCube = new() {
				{ 1, new(8, 0) },
				{ 3, new(0, 4) },
				{ 2, new(4, 4) },
				{ 4, new(8, 4) },
				{ 6, new(8, 8) },
				{ 5, new(12, 8) }
			};

			Dictionary<int, Point> cube = new();
			if (cubeSize == 4) {
				cube = testCube;
			}

			int face = GetTestFace(position);
			Facing nextFacing = position.Facing;
			Point nextXY = position.XY;
			Point movement = nextFacing switch {
				Facing.right => new(1, 0),
				Facing.down => new(0, 1),
				Facing.left => new(-1, 0),
				Facing.up => new(0, -1),
				_ => throw new NotImplementedException(),
			};

			Debug.Write($"Face: {face} {nextFacing}  ({nextXY.X}, {nextXY.Y}) => ");
			nextXY += movement;
			if (nextXY.X < testCube[face].X) {
				(face, Facing newFacing) = GetNextFace(face, nextFacing);
				nextXY = GetStartPosition(nextXY, face, nextFacing, newFacing);
				nextFacing = newFacing;
				Debug.WriteLine($" Next: {face} {nextFacing} ({nextXY.X}, {nextXY.Y})");
			} else if (nextXY.X >= testCube[face].X + cubeSize) {
				(face, Facing newFacing) = GetNextFace(face, nextFacing);
				nextXY = GetStartPosition(nextXY, face, nextFacing, newFacing);
				nextFacing = newFacing;
				Debug.WriteLine($" Next {face} {nextFacing} ({nextXY.X}, {nextXY.Y})");
			} else if (nextXY.Y < testCube[face].Y) {
				(face, Facing newFacing) = GetNextFace(face, nextFacing);
				nextXY = GetStartPosition(nextXY, face, nextFacing, newFacing);
				nextFacing = newFacing;
				Debug.WriteLine($" Next {face} {nextFacing} ({nextXY.X}, {nextXY.Y})");
			} else if (nextXY.Y >= testCube[face].Y + cubeSize) {
				(face, Facing newFacing) = GetNextFace(face, nextFacing);
				nextXY = GetStartPosition(nextXY, face, nextFacing, newFacing);
				nextFacing = newFacing;
				Debug.WriteLine($" Next {face} {nextFacing} ({nextXY.X}, {nextXY.Y})");
			}

			if (map[nextXY.X, nextXY.Y] == WALL) {
				Debug.WriteLine($" Hit the wall at ({position.Column}, {position.Row})");
				return position;
			}

			return position with { Column = nextXY.X, Row = nextXY.Y, Facing = nextFacing };

			int GetTestFace(Position position) {
				int row = position.Row;
				int column = position.Column;
				if (row < 4) {
					return 1;
				} else if (row < 8 && column < 4) {
					return 3;
				} else if (row < 8 && column < 8) {
					return 2;
				} else if (row < 8 && column < 12) {
					return 4;
				} else if (column < 12) {
					return 6;
				} else if (column < 16) {
					return 5;
				}

				throw new NotImplementedException();
			}

			(int, Facing) GetNextFace(int face, Facing facing) => TEST_CUBE_TRANSLATIONS[new(face, facing)];

			Point GetStartPosition(Point coord, int face, Facing currFacing, Facing newFacing) {
				Point newPoint;
				int coordX = coord.X % cubeSize;
				int coordY = coord.Y % cubeSize;
				if (currFacing != newFacing) {
					coord = coord with { X = coordY, Y = coordX };	
				} else {
					coord = coord with { X = coordX, Y = coordY };	
				}
				newPoint = newFacing switch {
					Facing.right => new(cube[face].X, cube[face].Y + coord.Y),
					Facing.down => new(cube[face].X + coord.X, cube[face].Y),
					Facing.left => new(cube[face].X + cubeSize - 1, cube[face].Y + coord.Y),
					Facing.up => new(cube[face].X + coord.X, cube[face].Y + cubeSize - 1),
					_ => throw new NotImplementedException()
				};
				return newPoint;
			}
		}

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
		public Point XY => new(Column, Row);
		public override string ToString() => $"{nameof(Position)}: ({Column}, {Row}) {Facing}";
	}	
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
