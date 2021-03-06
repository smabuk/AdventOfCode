﻿using System;
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

		private static List<(int dX, int dY)> DIRECTIONS = new()
				{ (0, -1), (0, 1), (-1, 0), (1, 0), (-1, -1), (-1, 1), (1, -1), (1, 1) };


		private static int Solution1(string[] input) {
			int roomWidth = input[0].Length;
			int roomHeight = input.Length;

			char[,] room = ParseInput(input);

			char[,] nextRoom = room;
			bool didSomething;
			int countOccupied = 0;
			do {
				countOccupied = 0;
				nextRoom = new char[roomWidth, roomHeight];
				didSomething = false;

				for (int y = 0; y < roomHeight; y++) {
					for (int x = 0; x < roomWidth; x++) {
						char current = room[x, y];
						char next = current;
						List<char> adjacent = GetAdjacentPositions(x, y, roomWidth, roomHeight, room);
						if (current == EMPTY_SEAT) {
							if (!adjacent.Any(s => s == OCCUPIED)) {
								next = OCCUPIED;
								didSomething = true;
							}
						} else if (current == OCCUPIED && adjacent.Count(s => s == OCCUPIED) >= 4) {
							next = EMPTY_SEAT;
							didSomething = true;
						}
						nextRoom[x, y] = next;
						if (next == OCCUPIED) {
							countOccupied++;
						}
					}
				}
				room = nextRoom;
			} while (didSomething);

			return countOccupied;
		}
		public static List<char> GetAdjacentPositions(int X, int Y, int width, int height, char[,] room) {
			List<char> adjacent = new();
			foreach (var (dX, dY) in DIRECTIONS) {
				int x = X + dX;
				int y = Y + dY;
				if (x >= 0 && x < width && y >= 0 && y < height) {
					if (room[x, y] != FLOOR) {
						adjacent.Add(room[x, y]);
					}
				}
			}
			return adjacent;
		}

		private static int Solution2(string[] input) {
			int roomWidth = input[0].Length;
			int roomHeight = input.Length;
			
			char[,] room = ParseInput(input);

			char[,] nextRoom = room;
			bool didSomething;
			int countOccupied = 0;
			do {
				countOccupied = 0;
				nextRoom = new char[roomWidth, roomHeight];
				didSomething = false;

				for (int y = 0; y < roomHeight; y++) {
					for (int x = 0; x < roomWidth; x++) {
						char current = room[x, y];
						char next = current;
						List<char> adjacent = GetAdjacentLines(x, y, roomWidth, roomHeight, room);
						if (current == EMPTY_SEAT) {
							if (!adjacent.Any(s => s == OCCUPIED)) {
								next = OCCUPIED;
								didSomething = true;
							} 
						} else if (current == OCCUPIED && adjacent.Count(s => s == OCCUPIED) >= 5) {
							next = EMPTY_SEAT;
							didSomething = true;
						}
						nextRoom[x, y] = next;
						if (next == OCCUPIED) {
							countOccupied++;
						}
					}
				}
				room = nextRoom;
			} while (didSomething);

			return countOccupied;
		}

		public static List<char> GetAdjacentLines(int X, int Y, int width, int height, char[,] room) {
			List<char> adjacent = new();
			foreach (var (dX, dY) in DIRECTIONS) {
				int x = X + dX;
				int y = Y + dY;
				while (x >= 0 && x < width && y >= 0 && y < height) {
					if (room[x, y] != FLOOR || x < 0 || y < 0) {
						adjacent.Add(room[x, y]);
						break;
					}
					x += dX;
					y += dY;
				}
			}
			return adjacent;
		}

		private static char[,] ParseInput(string[] input) {
			int roomWidth = input[0].Length;
			int roomHeight = input.Length;
			char[,] room = new char[roomWidth, roomHeight];

			for (int y = 0; y < roomHeight; y++) {
				string itemLine = input[y];

				for (int x = 0; x < roomWidth; x++) {
					room[x, y] = itemLine[x];
				}
			}
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
