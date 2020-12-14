using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

using static AdventOfCode.Solutions.Helpers.ArgumentHelpers;

namespace AdventOfCode.Solutions.Year2020 {
	/// <summary>
	/// Day XX: Docking Data
	/// https://adventofcode.com/2020/day/14
	/// </summary>
	public class Day14 {

		record Instruction(string Mask, long MemoryAddress, long Value);

		private static long Solution1(string[] input) {
			List<Instruction> instructions = Parse(input);
			Dictionary<long, long> memory = new();

			foreach (Instruction instruction in instructions) {
				if (!memory.ContainsKey(instruction.MemoryAddress)) {
					memory.Add(instruction.MemoryAddress, 0);
				}
				memory[instruction.MemoryAddress] = GetMaskedValue(instruction.Mask, instruction.Value);
			}

			return memory.Sum(m => m.Value);
		}

		private static long GetMaskedValue(string mask, long value) {
			char[] binaryString = GetBinaryString36(value).ToCharArray();
			for (int i = 1; i <= 36; i++) {
				binaryString[^i] =  mask[^i] switch {
					'1' => '1',
					'0' => '0',
					_ => binaryString[^i]
				};
			}
			return Convert.ToInt64(new(binaryString), 2);
		}
		static string GetBinaryString36(long n) {
			char[] b = new char[36];
			int pos = 35;
			int i = 0;

			while (i < 36) {
				if ((n & (1L << i)) != 0) {
					b[pos] = '1';
				} else {
					b[pos] = '0';
				}
				pos--;
				i++;
			}
			return new string(b);
		}


		private static string Solution2(string[] input) {
			return "** Solution not written yet **";
		}

		private static List<Instruction> Parse(string[] input) {
			string mask = "";
			List<Instruction> instructions = new();
			foreach (string line in input) {
				if (line.StartsWith("mask")) {
					mask = line[7..];
				} else {
					Match match = Regex.Match(line, @"mem\[(?<mem>\d+)\] = (?<value>\d+)");
					if (match.Success) {
						instructions.Add( new(mask, int.Parse(match.Groups["mem"].Value), int.Parse(match.Groups["value"].Value)));
					}
				}
			}
			return instructions;
		}





		#region Problem initialisation
		public static string Part1(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution1(input).ToString();
		}
		public static string Part2(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution2(input).ToString();
		}
		#endregion

	}
}
