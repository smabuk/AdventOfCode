using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

using static AdventOfCode.Solutions.Helpers.ArgumentHelpers;

namespace AdventOfCode.Solutions.Year2015 {
	/// <summary>
	/// Day 15: Science for Hungry People
	/// https://adventofcode.com/2015/day/15
	/// </summary>
	public class Day15 {

		record Ingredient(string Name, int Capacity, int Durability, int Flavour, int Texture, int Calories) {
			public long TotalScore  => 0;
		};

		private static long Solution1(string[] input) {
			List<Ingredient> ingredients = input.Select(i => ParseLine(i)).ToList();

			Ingredient ingredient1 = ingredients[0];
			Ingredient ingredient2 = ingredients[1];
			long highScore = 0;
			//for (int i = 1; i <= 100; i++) {
			for (int i = 1; i <= 100; i++) {
				int qty1 = i;
				int qty2 = 100 - i;
				int capacity = ingredient1.Capacity * qty1 + ingredient2.Capacity * qty2;
				int durability = ingredient1.Durability * qty1 + ingredient2.Durability * qty2;
				int flavour = ingredient1.Flavour * qty1 + ingredient2.Flavour * qty2;
				int texture = ingredient1.Texture * qty1 + ingredient2.Texture * qty2;
				capacity = capacity < 0 ? 0 : capacity;
				durability = durability < 0 ? 0 : durability;
				flavour = flavour < 0 ? 0 : flavour;
				texture = texture < 0 ? 0 : texture; 
				long totalScore = capacity * durability * flavour * texture;
				if (totalScore > highScore) {
					highScore = totalScore;
				}
			}

			return highScore;
		}


		private static string Solution2(string[] input) {
			Dictionary<(string, string), int> lookup = new();
			List<Ingredient> ingredients = input.Select(i => ParseLine(i)).ToList();
			List<string> guests = new();

			return "*** No solution yet ***";
		}

		private static Ingredient ParseLine(string input) {
			Match match = Regex.Match(input, @"(\w+): capacity ([\+\-]*\d+), durability ([\+\-]*\d+), flavor ([\+\-]*\d+), texture ([\+\-]*\d+), calories ([\+\-]*\d+)");
			if (match.Success) {
				return new Ingredient(
					match.Groups[1].Value,
					int.Parse(match.Groups[2].Value),
					int.Parse(match.Groups[3].Value),
					int.Parse(match.Groups[4].Value),
					int.Parse(match.Groups[5].Value),
					int.Parse(match.Groups[6].Value)
					);
			}
			return null!;
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
