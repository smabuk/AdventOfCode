using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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


		private static long Solution2(string[] input) {
			List<Instruction> instructions = Parse(input);
			Dictionary<long, long> memory = new();

			foreach (Instruction instruction in instructions) {
				List<long> memoryAddresses = GetMaskedMemoryAddresses(instruction.Mask, instruction.MemoryAddress);
				foreach (long mem in memoryAddresses) {
					if (!memory.ContainsKey(mem)) {
						memory.Add(mem, 0);
					}
					memory[mem] = instruction.Value;
				}
			}

			return memory.Sum(m => m.Value);
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

		private static long GetMaskedValue(string mask, long value) {
			char[] binaryString = GetBinaryString36(value).ToCharArray();
			for (int i = 1; i <= 36; i++) {
				binaryString[^i] = mask[^i] switch {
					'1' => '1',
					'0' => '0',
					_ => binaryString[^i]
				};
			}
			return Convert.ToInt64(new(binaryString), 2);
		}

		private static List<long> GetMaskedMemoryAddresses(string mask, long memoryAddress) {
			char[] binaryString = GetBinaryString36(memoryAddress).ToCharArray();
			List<long> memoryAddresses = new();
			List<string> memAddresses = new();
			for (int i = 1; i <= 36; i++) {
				binaryString[^i] = mask[^i] switch {
					'1' => '1',
					'0' => binaryString[^i],
					'X' => 'X',
					_ => binaryString[^i]
				};
			}
			if (binaryString.Contains('X')) {
				int[] xIndex = binaryString
					.Select((x, idx) => (X: x, index: 36 - idx))
					.Where(x => x.X == 'X')
					.Select(x => x.index)
					.ToArray();
				for (int i = 0; i < Math.Pow(2, xIndex.Length); i++) {
					char[] temp = binaryString;
					string replacementString = GetBinaryString36(i);
					for (int j = 0; j < xIndex.Length; j++) {
						int idx = xIndex[j];
						temp[^idx] = replacementString[^(j + 1)];
					}
					memoryAddresses.Add(Convert.ToInt64(new(temp), 2));
				}
			} else {
				memoryAddresses.Add(Convert.ToInt64(new(binaryString), 2));
			}
			return memoryAddresses;
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
