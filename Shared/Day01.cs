using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Shared
{
	/// <summary>
	/// https://adventofcode.com/2020/day/1
	/// </summary>
	public static class Day01
	{
		public static long Part1()
		{
			FindSumsEqualTo(2020, _expenseValues.ToList(), 2, out List<int> entries);
			return entries.Aggregate(1, (product, entry) => product * entry);
		}

		public static long Part2()
		{
			FindSumsEqualTo(2020, _expenseValues.ToList(), 3, out List<int> entries);
			return entries.Aggregate(1, (product, entry) => product * entry);
		}


		public static bool FindSumsEqualTo(int value, List<int> expenseValues, int noOfEntries, out List<int> foundEntries)
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

		static readonly int[] _expenseValues =
		{
				1768,
				1847,
				1905,
				1713,
				1826,
				1846,
				1824,
				1976,
				1687,
				1867,
				1665,
				1606,
				1946,
				1886,
				1858,
				346 ,
				1739,
				1752,
				1700,
				1922,
				1865,
				1609,
				1617,
				1932,
				1346,
				1213,
				1933,
				834 ,
				1598,
				1191,
				1979,
				1756,
				1216,
				1820,
				1792,
				1537,
				1341,
				1390,
				1709,
				1458,
				1808,
				1885,
				1679,
				1977,
				1869,
				1614,
				1938,
				1622,
				1868,
				1844,
				1969,
				1822,
				1510,
				1994,
				1337,
				1883,
				1519,
				1766,
				1554,
				1825,
				1828,
				1972,
				1380,
				1878,
				1345,
				1469,
				1794,
				1898,
				1805,
				1911,
				1913,
				1910,
				1318,
				1862,
				1921,
				1753,
				1823,
				1896,
				1316,
				1381,
				1430,
				1962,
				1958,
				1702,
				1923,
				1993,
				1789,
				2002,
				1788,
				1970,
				1955,
				1887,
				1870,
				225 ,
				1696,
				1975,
				699 ,
				294 ,
				1605,
				1500,
				1777,
				1750,
				1857,
				1540,
				1329,
				1974,
				1947,
				1516,
				1925,
				1945,
				350 ,
				1669,
				1775,
				1536,
				1871,
				1917,
				1249,
				1971,
				2009,
				1585,
				1986,
				1701,
				1832,
				1754,
				1195,
				1697,
				1941,
				1919,
				2006,
				1667,
				1816,
				1765,
				1631,
				2003,
				1861,
				1000,
				1791,
				1786,
				1843,
				1939,
				1951,
				269 ,
				1790,
				1895,
				1355,
				1833,
				1466,
				1998,
				1806,
				1881,
				1234,
				1856,
				1619,
				1727,
				1874,
				1877,
				195 ,
				1783,
				1797,
				2010,
				1764,
				1863,
				1852,
				1841,
				1892,
				1562,
				1650,
				1942,
				1695,
				1730,
				1965,
				1632,
				1981,
				1900,
				1991,
				1884,
				1278,
				1062,
				1394,
				1999,
				2000,
				1827,
				1873,
				1926,
				1434,
				1802,
				1579,
				1879,
				1671,
				1549,
				1875,
				1838,
				1338,
				1864,
				1718,
				1800,
				1928,
				1749,
				1990,
				1705
			};

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
