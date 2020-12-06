using System.Collections.Generic;
using System.Linq;

using AdventOfCode.Shared.Helpers;

namespace AdventOfCode.Shared
{
	/// <summary>
	/// https://adventofcode.com/2020/day/1
	/// </summary>
	public static class Solution_2020_01
	{
		public static long Part1(string[]? input = null)
		{
			input = input.StripTrailingBlankLineOrDefault();
			List<int> expenseValues = new();
			foreach (string item in input) {
				expenseValues.Add(int.Parse(item));
			}
			FindSumsEqualTo(2020, expenseValues, 2, out List<int> entries);
			return entries.Aggregate(1, (product, entry) => product * entry);
		}

		public static long Part2(string[]? input = null)
		{
			input = input.StripTrailingBlankLineOrDefault();
			List<int> expenseValues = new();
			foreach (string item in input) {
				expenseValues.Add(int.Parse(item));
			}
			FindSumsEqualTo(2020, expenseValues, 3, out List<int> entries);
			return entries.Aggregate(1, (product, entry) => product * entry);
		}


		public static bool FindSumsEqualTo(int value, IEnumerable<int> expenseValues, int noOfEntries, out List<int> foundEntries)
		{
			foundEntries = new();

			foreach (int entry in expenseValues) {
				if (noOfEntries > 2) {
					List<int> x = new List<int> { entry };
					if (FindSumsEqualTo(value - entry, expenseValues.Except(x).ToList(), noOfEntries - 1, out List<int> resultT)) {
						foundEntries.Add(entry);
						foundEntries.AddRange(resultT);
						return true;
					}
				} else {
					int matchValue = expenseValues.Where(e => e == value - entry).FirstOrDefault();
					if (matchValue != 0) {
						foundEntries.Add(entry);
						foundEntries.Add(matchValue);
						return true;
					}
				}
			}

			return false;
		}

		public static List<int> Find2SumsEqualTo2020(int[] expenseValues)
		{
			List<int> result = new();

			for (int i = 0; i < expenseValues.Length; i++) {
				for (int j = 0; j < expenseValues.Length; j++) {
					if (i == j) {
						continue;
					}

					if (expenseValues[i] + expenseValues[j] == 2020) {
						result.Add(expenseValues[i]);
						result.Add(expenseValues[j]);
						return result;
					}
				}
			}

			return result;
		}

		public static List<int> Find3SumsEqualTo2020(int[] expenseValues)
		{
			List<int> result = new();

			for (int i = 0; i < expenseValues.Length; i++) {
				for (int j = 0; j < expenseValues.Length; j++) {
					for (int k = 0; k < expenseValues.Length; k++) {
						if (i == j || i == k || j == k) {
							continue;
						}

						if (expenseValues[i] + expenseValues[j] + expenseValues[k] == 2020) {
							result.Add(expenseValues[i]);
							result.Add(expenseValues[j]);
							result.Add(expenseValues[k]);
							return result;
						}
					}
				}
			}

			return result;
		}

	}

	/*
	 * https://adventofcode.com/2020/day/1
	 * 
	 * --- Day 1: Report Repair ---
	 * After saving Christmas five years in a row, you've decided to take a vacation at a nice 
	 * resort on a tropical island. Surely, Christmas will go on without you.
	 * 
	 * The tropical island has its own currency and is entirely cash-only. The gold coins used there 
	 * have a little picture of a starfish; the locals just call them stars. None of the currency 
	 * exchanges seem to have heard of them, but somehow, you'll need to find fifty of these coins 
	 * by the time you arrive so you can pay the deposit on your room.
	 * 
	 * To save your vacation, you need to get all fifty stars by December 25th.
	 * 
	 * Collect stars by solving puzzles. Two puzzles will be made available on each day in the 
	 * Advent calendar; the second puzzle is unlocked when you complete the first. Each puzzle 
	 * grants one star. Good luck!
	 * 
	 * Before you leave, the Elves in accounting just need you to fix your expense report (your 
	 * puzzle input); apparently, something isn't quite adding up.
	 * 
	 * Specifically, they need you to find the two entries that sum to 2020 and then multiply those 
	 * two numbers together.
	 * 
	 * For example, suppose your expense report contained the following:
	 * 
	 * 1721
	 * 979
	 * 366
	 * 299
	 * 675
	 * 1456
	 * In this list, the two entries that sum to 2020 are 1721 and 299. Multiplying them together 
	 * produces 1721 * 299 = 514579, so the correct answer is 514579.
	 * 
	 * Of course, your expense report is much larger. Find the two entries that sum to 2020; what do 
	 * you get if you multiply them together?
	 * 
	 * To begin, get your puzzle input.
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * --- Part Two ---
	 * The Elves in accounting are thankful for your help; one of them even offers you a starfish 
	 * coin they had left over from a past vacation. They offer you a second one if you can find 
	 * three numbers in your expense report that meet the same criteria.
	 * 
	 * Using the above example again, the three entries that sum to 2020 are 979, 366, and 675. 
	 * Multiplying them together produces the answer, 241861950.
	 * 
	 * In your expense report, what is the product of the three entries that sum to 2020?
	 * 
	 */

}
