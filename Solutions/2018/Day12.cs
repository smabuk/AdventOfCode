﻿using System.Text;

namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 12: Subterranean Sustainability
/// https://adventofcode.com/2018/day/12
/// </summary>
[Description("Subterranean Sustainability")]
public sealed partial class Day12 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static readonly char NO_PLANT = '.';
	private static readonly char PLANT    = '#';
	
	private static int Solution1(string[] input) {
		const int noOfGenerations = 20;
		string currentState = $"{NO_PLANT}{NO_PLANT}{NO_PLANT}{NO_PLANT}{input[0][15..]}{NO_PLANT}{NO_PLANT}{NO_PLANT}{NO_PLANT}";
		int offset = 4;

		HashSet<string> plantPatterns = input[2..]
			.Where(i => i.EndsWith(PLANT))
			.Select(i => i[..5])
			.ToHashSet();

		for (int gen = 1; gen <= noOfGenerations; gen++) {
			char[] newState = new char[currentState.Length - 4];
			for (int pot = 0; pot < currentState.Length - 4; pot++) {
				char plant = plantPatterns.Contains(currentState[pot..(pot + 5)]) ? PLANT : NO_PLANT;
				newState[pot] = plant;
			}
			string state = new(newState);
			offset = offset + 2 - state.IndexOf(PLANT);
			currentState = $"{NO_PLANT}{NO_PLANT}{NO_PLANT}{NO_PLANT}{state.Trim(NO_PLANT)}{NO_PLANT}{NO_PLANT}{NO_PLANT}{NO_PLANT}";
		}

		int potPlantSum = 0;
		for (int pot = 0; pot < currentState.Length - 4; pot++) {
			if (currentState[pot] == PLANT) {
				potPlantSum += pot - offset;
			}
		}

		return potPlantSum;
	}

	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		//List<Instruction> instructions = input.Select(ParseLine).ToList();
		return "** Solution not written yet **";
	}
}
