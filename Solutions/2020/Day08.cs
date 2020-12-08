﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

namespace AdventOfCode.Solutions {
	/// <summary>
	/// Day 08: Handheld Halting
	/// https://adventofcode.com/2020/day/8
	/// </summary>
	public class Solution_2020_08 {

		private record Instruction(string Command, int Value);

		private int Accumulator { get; set; }
		
		private int Solution1(string[] input) {
			List<Instruction> program = input.Select(x => GetCommand(x)).ToList();
			List<int> VisitedLineNos = new();

			Accumulator = 0;
			int lineNo = 0;
			int accumulatorPreExecution;
			do {
				VisitedLineNos.Add(lineNo);
				accumulatorPreExecution = Accumulator;
				lineNo += ExecuteCommand(program[lineNo]);
			} while (VisitedLineNos.Contains(lineNo) == false);

			return accumulatorPreExecution;
		}
		private int Solution2(string[] input) {
			List<Instruction> program = input.Select(x => GetCommand(x)).ToList();

			List<int> ChangedLines = new();
			int lineNo;
			do {
				Accumulator = 0;
				lineNo = 0;
				List<int> VisitedLineNos = new();
				bool instructionChanged = false;
				do {
					Instruction instruction = program[lineNo];
					if (ChangedLines.Contains(lineNo) == false && instructionChanged == false) {
						instruction = SwapJmpNop(instruction);
						instructionChanged = true;
						ChangedLines.Add(lineNo);
					}
					VisitedLineNos.Add(lineNo);
					lineNo += ExecuteCommand(instruction);
				} while (VisitedLineNos.Contains(lineNo) == false && lineNo < program.Count);

			} while (lineNo < program.Count);

			return Accumulator;
		}

		private static Instruction SwapJmpNop(Instruction instruction) => instruction with
		{
			Command = (instruction.Command switch {
				"jmp" => "nop",
				"nop" => "jmp",
				_ => instruction.Command
			})
		};

		private int ExecuteCommand(Instruction instruction) {
			int lineJump = 1;
			switch (instruction.Command) {
				case "acc":
					Accumulator += instruction.Value;
					break;
				case "jmp":
					lineJump += instruction.Value - 1;
					break;
				case "nop":
				default:
					break;
			}
			return lineJump;
		}

		private static Instruction GetCommand(string input) {
			Match match = Regex.Match(input, @"(nop|acc|jmp) ([\+\-]\d+)");
			if (match.Success) {
				return new(match.Groups[1].Value, int.Parse(match.Groups[2].Value));
			}
			return null!;
		}













		public string Part1(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution1(input).ToString();
		}
		public string Part2(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution2(input).ToString();
		}

	}
}
