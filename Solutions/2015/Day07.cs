using System;
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
		record Instruction(string OutWire);
		record NOT_Instruction(string OutWire, string Operand, string InWire) : Instruction(OutWire);
		record SHIFT_Instruction(string OutWire, string Direction, string InWire1, UInt16 ShiftAmount) : Instruction(OutWire);
		record LOGICAL_Instruction(string OutWire, string Operand, string InWire1, string InWire2) : Instruction(OutWire);
		record NUMBER_Instruction(string OutWire, UInt16 InputValue) : Instruction(OutWire);

		//record Instruction(Wire OutWire);
		//record NOT_Instruction(Wire OutWire, Wire InWire) : Instruction(OutWire);
		//record SHIFT_Instruction(Wire OutWire, string Direction, Wire InWire1, UInt16 ShiftAmount) : Instruction(OutWire);
		//record LOGICAL_Instruction(Wire OutWire, string Operand, Wire InWire1, Wire InWire2) : Instruction(OutWire);
		//record NUMBER_Instruction(Wire OutWire, UInt16 InputValue) : Instruction(OutWire);


		private static string Solution1(string[] input) {
			List<string> inputs = input.ToList();
			List<Instruction> instructions = input.Select(x => ParseLine(x)).ToList();
			Dictionary<string, Wire> wireInputs = new();
			Dictionary<string, Gate> gates = new();
			foreach ( NUMBER_Instruction instruction in instructions.Where(i => i is NUMBER_Instruction)) {
				//instruction.OutWire.Ins.Add(instruction.InputValue);
				//wireInputs.TryAdd(instruction.OutWire.Identifier, instruction.OutWire);
			}

			throw new NotImplementedException();
		}

		private static string Solution2(string[] input) {
			List<string> inputs = input.ToList();
			throw new NotImplementedException();
		}

		private static Instruction ParseLine(string input) {
			string[] parts = input.Split(" -> ");
			string outWire = parts[^1];
			if (char.IsDigit(input[0])) {
				NUMBER_Instruction instruction = new(outWire, UInt16.Parse(parts[0]));
				return instruction;
			}
			if (input.StartsWith("NOT")) {
				string[] moreParts = parts[0].Split(" ");
				NOT_Instruction instruction = new(outWire, "NOT", moreParts[1]);
				return instruction;
			}
			if (input.Contains("SHIFT")) {
				string[] moreParts = parts[0].Split(" ");
				SHIFT_Instruction instruction = new(outWire, moreParts[1], moreParts[0], UInt16.Parse(moreParts[2]));
				return instruction;
			}
			if (parts.Length > 0) {
				string[] moreParts = parts[0].Split(" ");
				LOGICAL_Instruction instruction = new(outWire, moreParts[1], moreParts[0],moreParts[2]);
				return instruction;
			}
			return null!;
		}

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
