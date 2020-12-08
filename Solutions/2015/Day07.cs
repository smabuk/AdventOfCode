﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using AdventOfCode.Solutions.Helpers;

namespace AdventOfCode.Solutions.Year2015 {
	/// <summary>
	/// Day 07: Some Assembly Required
	/// https://adventofcode.com/2020/day/7
	/// </summary>
	public class Day07 {

		record Wire(string Identifier, UInt16? Value = null) {
			public List<object> Ins { get; set; } = new();
			public List<object> Outs { get; set; } = new();
		};

		record Gate(string GateType, Wire Output) {
			public List<object> Ins { get; set; } = new();
			public List<object> Outs { get; set; } = new();

			public UInt16 OutputValue {
				get {
					return OutputValue;
				}
				//set { _outputValue = value; }
			}


		};
		record Instruction(Wire OutWire);
		record NOT_Instruction(Wire OutWire, Wire InWire) : Instruction(OutWire);
		record SHIFT_Instruction(Wire OutWire, string Direction, Wire InWire1, UInt16 ShiftAmount) : Instruction(OutWire);
		record LOGICAL_Instruction(Wire OutWire, string Operand, Wire InWire1, UInt16 ShiftAmount) : Instruction(OutWire);
		record NUMBER_Instruction(Wire OutWire, string Operand, Wire InWire1, UInt16 ShiftAmount) : Instruction(OutWire);


		private static string Solution1(string[] input) {
			List<string> inputs = input.ToList();
			List<Instruction> instructions = input.Select(x => ParseLine(x)).ToList();
			Dictionary<string, Wire> wireInputs = new();
			foreach ( var wire in instructions.Where(i => i is NUMBER_Instruction).Select(i => i.wire)) {
				wireInputs.TryAdd(wire.Identifier, wire);
			}

			throw new NotImplementedException();
		}

		private static string Solution2(string[] input) {
			List<string> inputs = input.ToList();
			throw new NotImplementedException();
		}

		private static Instruction ParseLine(string input) {
			if (true) {

			}


			return null!;
		}
		//private static ConnectionInstruction ParseLine(string input) {
		//	//MatchCollection match = Regex.Matches(input, @"(opt1|opt2|opt3) ([\+\-]\d+)");
		//	Match match = Regex.Match(input, @"(\d+) -> ([a-z]+)");
		//	if (match.Success) {
		//		Wire wire = new(match.Groups[^1].Value, null);
		//		UInt16 number = UInt16.Parse(match.Groups[1].Value);
		//		return new(wire, number, null);
		//	} else {
		//		match = Regex.Match(input, @"([a-z]+) (AND|OR|LSHIFT|RSHIFT) ([a-z]+) -> ([a-z]+)");
		//		if (match.Success) {
		//			Wire outWire = new(match.Groups[^1].Value);
		//			List<Wire> sources = new();
		//			sources.Add(new Wire(match.Groups[1].Value));
		//			sources.Add(new Wire(match.Groups[3].Value));
		//			Gate gate = new(match.Groups[2].Value, outWire);
		//			gate.Ins.AddRange(sources);
		//			gate.Outs.Add(outWire);

		//			return new(outWire, null, gate);
		//		} else {
		//			match = Regex.Match(input, @"(NOT) ([a-z]+) -> ([a-z]+)");
		//			if (match.Success) {
		//				Wire outWire = new(match.Groups[^1].Value);
		//				List<Wire> sources = new();
		//				sources.Add(new Wire(match.Groups[2].Value));
		//				Gate gate = new(sources, match.Groups[1].Value, outWire);
		//				return new(outWire, null, gate);
		//			}
		//		}
		//	}
		//	return null!;
		//}





		public static string Part1(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution1(input).ToString();
		}
		public static string Part2(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution2(input).ToString();
		}
	}
}
