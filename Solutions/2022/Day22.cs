using System.Runtime.CompilerServices;

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

	private static readonly Dictionary<int, Point> TEST_CUBE_TOPOLOGY = new() {
			{ 1, new( 8, 0) },
			{ 3, new( 0, 4) },
			{ 2, new( 4, 4) },
			{ 4, new( 8, 4) },
			{ 6, new( 8, 8) },
			{ 5, new(12, 8) },
		};

	private static readonly Dictionary<int, Point> MY_CUBE_TOPOLOGY = new() {
			{ 1, new( 50,   0) },
			{ 5, new(100,   0) },
			{ 4, new( 50,  50) },
			{ 2, new(  0, 100) },
			{ 6, new( 50, 100) },
			{ 3, new(  0, 150) },
		};

	private static readonly Dictionary<(int, Facing), (int Face, Facing Facing, bool Invert)> TEST_CUBE_TRANSLATIONS = new() {
			{ (1, Facing.right), (5, Facing.down , false) },
			{ (1, Facing.down) , (4, Facing.down , false) },
			{ (1, Facing.left) , (2, Facing.down , false) },
			{ (1, Facing.up)   , (3, Facing.down , false) },
								 	 			
			{ (2, Facing.right), (4, Facing.right, false) },
			{ (2, Facing.down) , (6, Facing.right, false) },
			{ (2, Facing.left) , (3, Facing.left , false) },
			{ (2, Facing.up)   , (1, Facing.right, false) },

			{ (3, Facing.right), (2, Facing.right, false) },
			{ (3, Facing.down) , (6, Facing.up   , false) },
			{ (3, Facing.left) , (5, Facing.up   , false) },
			{ (3, Facing.up)   , (1, Facing.down , false) },

			{ (4, Facing.right), (5, Facing.down , false) },
			{ (4, Facing.down) , (6, Facing.down , false) },
			{ (4, Facing.left) , (2, Facing.left , false) },
			{ (4, Facing.up)   , (1, Facing.up   , false) },

			{ (5, Facing.right), (1, Facing.left , false) },
			{ (5, Facing.down) , (3, Facing.right, false) },
			{ (5, Facing.left) , (6, Facing.left , false) },
			{ (5, Facing.up)   , (4, Facing.left , false) },

			{ (6, Facing.right), (5, Facing.right, false) },
			{ (6, Facing.down) , (3, Facing.up   , false) },
			{ (6, Facing.left) , (2, Facing.up   , false) },
			{ (6, Facing.up)   , (4, Facing.up   , false) },
		};

	private static readonly Dictionary<(int Face, Facing Facing), (int Face, Facing Facing, bool Invert)> MY_CUBE_TRANSLATIONS = new() {
			{ (1, Facing.right), (5, Facing.right, false) },
			{ (1, Facing.down) , (4, Facing.down , false) },
			{ (1, Facing.left) , (2, Facing.right, true ) },
			{ (1, Facing.up)   , (3, Facing.right, false) },

			{ (2, Facing.right), (6, Facing.right, false) },
			{ (2, Facing.down) , (3, Facing.down , false) },
			{ (2, Facing.left) , (1, Facing.right, true ) },
			{ (2, Facing.up)   , (4, Facing.right, false) },

			{ (3, Facing.right), (6, Facing.up   , false) },
			{ (3, Facing.down) , (5, Facing.down , false) },
			{ (3, Facing.left) , (1, Facing.down , false) },
			{ (3, Facing.up)   , (2, Facing.up   , false) },

			{ (4, Facing.right), (1, Facing.up   , false) },
			{ (4, Facing.down) , (6, Facing.down , false) },
			{ (4, Facing.left) , (2, Facing.down , false) },
			{ (4, Facing.up)   , (1, Facing.up   , false) },

			{ (5, Facing.right), (6, Facing.left , true ) },
			{ (5, Facing.down) , (4, Facing.left , false) },
			{ (5, Facing.left) , (1, Facing.left , false) },
			{ (5, Facing.up)   , (3, Facing.up   , false) },

			{ (6, Facing.right), (5, Facing.left , true ) },
			{ (6, Facing.down) , (3, Facing.left , false) },
			{ (6, Facing.left) , (2, Facing.left , false) },
			{ (6, Facing.up)   , (4, Facing.up   , false) },
		};

	private static Dictionary<int, Point> _cubeTopology = MY_CUBE_TOPOLOGY;


	private static int Solution1(string[] input) {
		char[,] monkeyMap = LoadMap(input[..^2]);
		List<Instruction> instructions = Instruction.Parse(input[^1]).ToList();

		Position currentPosition = new(-1, 0, Facing.right);
		currentPosition = Next2dPosition(currentPosition, new STEP_Instruction(1), monkeyMap);

		foreach (Instruction instruction in instructions) {
			currentPosition = Next2dPosition(currentPosition, instruction, monkeyMap);
		}

		return currentPosition.Password;
	}

	private static int Solution2(string[] input, int cubeSize) {
		char[,] monkeyMap = LoadMap(input[..^2]);
		List<Instruction> instructions = Instruction.Parse(input[^1]).ToList();
		
		int startX;

		if (cubeSize == 4) {    // tests
			startX = 8;
			_cubeTopology = TEST_CUBE_TOPOLOGY;
		} else {                // live input
			startX = 50;
			_cubeTopology = MY_CUBE_TOPOLOGY;
		}

		Position currentPosition = new(startX, 0, Facing.right);

		foreach (Instruction instruction in instructions) {
			currentPosition = Next3dPosition(currentPosition, instruction, monkeyMap, cubeSize);
		}

		return currentPosition.Password;
	}


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

	private static Position Next2dPosition(Position current, Instruction instruction, char[,] map) {
		if (instruction is ROTATE_Instruction rotate) {
			return Rotate(current, rotate);
		}

		if (instruction is STEP_Instruction move) {
			for (int i = 0; i < move.Steps; i++) {
				Point movement = MovementVector(current.Facing);
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
	private static Position Next3dPosition(Position current, Instruction instruction, char[,] map, int cubeSize) {
		if (instruction is ROTATE_Instruction rotate) {
			return Rotate(current, rotate);
		}

		if (instruction is STEP_Instruction move) {
			for (int i = 0; i < move.Steps; i++) {
				current = TakeAStep(current);
			}
			return current;
		}

		throw new NotImplementedException();

		Position TakeAStep(Position position) {
			int face;

			if (cubeSize == 4) {	// tests
				face = GetTestFace(position);
			} else {				// live input
				face = GetMyFace(position);
			}

			Facing nextFacing = position.Facing;
			Point nextXY = position.XY;
			Point movement = MovementVector(nextFacing);

			nextXY += movement;
			if ( 
				   (nextXY.X < _cubeTopology[face].X)
				|| (nextXY.X >= _cubeTopology[face].X + cubeSize)
				|| (nextXY.Y < _cubeTopology[face].Y) 
				|| (nextXY.Y >= _cubeTopology[face].Y + cubeSize)) {

				(face, Facing newFacing, bool invert) = GetNextFace(face, nextFacing);
				nextXY = GetStartPosition(nextXY, face, nextFacing, newFacing, invert);
				nextFacing = newFacing;
			}

			if (map[nextXY.X, nextXY.Y] == WALL) {
				return position;
			}

			return position with { Column = nextXY.X, Row = nextXY.Y, Facing = nextFacing };

			(int, Facing, bool Invert) GetNextFace(int face, Facing facing) {
				return cubeSize switch {
					4 => TEST_CUBE_TRANSLATIONS[new(face, facing)],
					_ => MY_CUBE_TRANSLATIONS[new(face, facing)],
				};
			}

			Point GetStartPosition(Point coord, int face, Facing currFacing, Facing newFacing, bool invert) {
				Point newPoint;
				int coordX = coord.X % cubeSize;
				int coordY = coord.Y % cubeSize;

				coord = (currFacing, newFacing) switch {
					   (Facing.right, Facing.right)
					or (Facing.down , Facing.down) 
					or (Facing.left , Facing.left) 
					or (Facing.up   , Facing.up)   
					or (Facing.right, Facing.left) 
					or (Facing.left , Facing.right)
					or (Facing.up   , Facing.down) 
					or (Facing.down , Facing.up)    
					  => coord with { X = coordX, Y = coordY },
					_ => coord with { X = coordY, Y = coordX }	// swap X and Y
				};

				if (invert) {
					newPoint = newFacing switch {
						Facing.right => new(_cubeTopology[face].X               , cubeSize - coord.Y + _cubeTopology[face].Y - 1),
						Facing.left  => new(_cubeTopology[face].X + cubeSize - 1, cubeSize - coord.Y + _cubeTopology[face].Y - 1),
						_ => throw new NotImplementedException()
					};
				} else {
					newPoint = newFacing switch {
						Facing.right => new(_cubeTopology[face].X               , _cubeTopology[face].Y + coord.Y),
						Facing.down  => new(_cubeTopology[face].X + coord.X     , _cubeTopology[face].Y),
						Facing.left  => new(_cubeTopology[face].X + cubeSize - 1, _cubeTopology[face].Y + coord.Y),
						Facing.up    => new(_cubeTopology[face].X + coord.X     , _cubeTopology[face].Y + cubeSize - 1),
						_ => throw new NotImplementedException()
					};
				}
				return newPoint;
			}
			
			int GetTestFace(Position position) {
				int row = position.Row;
				int column = position.Column;
				return row switch {
					< 4                   => 1,
					< 8  when column < 4  => 3,
					< 8  when column < 8  => 2,
					< 8  when column < 12 => 4,
					< 12 when column < 12 => 6,
					< 12 when column < 16 => 5,
					_ => throw new NotImplementedException(),
				};
			}

			int GetMyFace(Position position) {
				int row = position.Row;
				int column = position.Column;
				return row switch {
					< 50  when column < 100 => 1,
					< 50  when column < 150 => 5,
					< 100                   => 4,
					< 150 when column < 50  => 2,
					< 150 when column < 100 => 6,
					< 200                   => 3,
					_ => throw new NotImplementedException(),
				};
			}

		}

	}

	private static Position Rotate(Position current, ROTATE_Instruction rotate) {
		return current with {
			Facing = rotate.Rotation switch {
				Direction.left  => (Facing)(((int)current.Facing + 3) % 4),
				Direction.right => (Facing)(((int)current.Facing + 1) % 4),
				_ => throw new NotImplementedException(),
			}
		};
	}

	private static Point MovementVector(Facing facing) {
		return facing switch {
			Facing.right => new( 1,  0),
			Facing.down  => new( 0,  1),
			Facing.left  => new(-1,  0),
			Facing.up    => new( 0, -1),
			_ => throw new NotImplementedException(),
		};
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
	private record STEP_Instruction(int Steps)            : Instruction();

	private record Position(int Column, int Row, Facing Facing) : Instruction() {
		public int Password => ((Row + 1) * 1000) + ((Column + 1) * 4) + (int)Facing;
		public Point XY => new(Column, Row);
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
