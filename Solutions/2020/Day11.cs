using System;
using System.Collections.Generic;
using System.Linq;

using AdventOfCode.Solutions.Helpers;

namespace AdventOfCode.Solutions.Year2020 {
	/// <summary>
	/// Day 11: Seating System
	/// https://adventofcode.com/2020/day/11
	/// </summary>
	public static class Day11 {

		public const char EMPTY_SEAT = 'L';
		public const char FLOOR = '.';
		public const char OCCUPIED = '#';

		public enum PositionState {
			Floor,
			Empty,
			Occupied
		}

		public record Position {
			public int Id { get; init; }

			public int X { get; init; }
			public int Y { get; init; }
			public PositionState State { get; set; } = PositionState.Floor;
			public List<Position> AdjacentPositions { get; set; } = new();

			public Position(int id, int x, int y) {
				Id = id;
				X = x;
				Y = y;
			}
			public Position(int id, int x, int y, PositionState state) {
				Id = id;
				X = x;
				Y = y;
				State = state;
			}

			public bool Occupied => State == PositionState.Occupied;
			public bool Empty => State == PositionState.Empty;
			public bool Floor => State == PositionState.Floor;

			public (bool , Position) NextState() {
				bool didSomething = false;
				Position next;
				if (Empty && !AdjacentPositions.Any(s => s.Occupied)) {
					next = this with { State = PositionState.Occupied };
					didSomething = true;
				} else if (Occupied && AdjacentPositions.Count(s => s.Occupied) >= 4) {
					next = this with { State = PositionState.Empty };
					didSomething = true;
				} else {
					next = this;
					didSomething = false;
				}
				return (didSomething, next);
			}

		}

		public static List<Position> GetAdjacentPositions(int x, int y, IEnumerable<Position> positions) {
			if (positions is null) {
				throw new ArgumentNullException(nameof(positions));
			}
			IEnumerable<Position> adjacentSlots =
				positions.Where(slot => slot.X >= (x - 1) && slot.X <= (x + 1) && slot.Y >= (y - 1) && slot.Y <= (y + 1));
			IEnumerable<Position> currentSlot = positions.Where(slot => slot.X == x && slot.Y == y);
			return adjacentSlots.Except(currentSlot).ToList();
		}

		private static int Solution1(string[] input) {
			List<Position> room = ParseInput(input);

			List<Position> nextRoom = room;
			bool didSomething;
			do {
				nextRoom = new();
				didSomething = false;

				foreach (Position position in room) {
					(bool changed, Position nextPosition) = position.NextState();
					if (changed) {
						didSomething = true;
					}
					nextRoom.Add(nextPosition);
				}
				room = nextRoom;
				room.ForEach(s => s.AdjacentPositions = GetAdjacentPositions(s.X, s.Y, room));
			} while (didSomething);


			int countOccupied = room.Count(s => s.Occupied);

			return countOccupied;
		}

		private static string Solution2(string[] input) {
			//string inputLine = input[0];
			List<string> inputs = input.ToList();
			//inputs.Add("");
			throw new NotImplementedException();
		}

		private static List<Position> ParseInput(string[] input) {
			List<Position> room = new();

			int id = 0;
			for (int y = 0; y < input.Length; y++) {
				string itemLine = input[y];

				for (int x = 0; x < itemLine.Length; x++) {
					char seatInput = itemLine[x];
					room.Add(new Position(
						id,
						x,
						y,
						seatInput switch {
							FLOOR => PositionState.Floor,
							EMPTY_SEAT => PositionState.Empty,
							OCCUPIED => PositionState.Occupied,
							_ => throw new ArgumentOutOfRangeException(nameof(Solution1), $"{nameof(seatInput)}={seatInput}")
						}));
					id++;
				}
			}

			//position.ForEach(s => s.AdjacentPositions = GetAdjacentPositions(s.X, s.Y, position));
			return room;
		}





		#region Problem initialisation
		/// <summary>
		/// Sets up the inputs for Part1 of the problem and calls Solution1
		/// </summary>
		/// <param name="input"></param>
		/// Array of strings
		/// <param name="args"></param>
		/// Optional extra parameters that may be required as input to the problem
		/// <returns></returns>
		public static string Part1(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			// int arg1 = GetArgument(args, 1, 25);
			input = input.StripTrailingBlankLineOrDefault();
			return Solution1(input).ToString();
		}
		/// <summary>
		/// Sets up the inputs for Part2 of the problem and calls Solution2
		/// </summary>
		/// <param name="input"></param>
		/// Array of strings
		/// <param name="args"></param>
		/// Optional extra parameters that may be required as input to the problem
		/// <returns></returns>
		public static string Part2(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			// int arg1 = GetArgument(args, 1, 25);
			input = input.StripTrailingBlankLineOrDefault();
			return Solution2(input).ToString();
		}
		#endregion

	}
}
