using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Shared.Helpers
{
    public static class ArrayHelpers
    {
		/// <summary>
		/// Finds the highest value and returns it
		/// </summary>
		/// <param name="input"></param>
		/// <returns>The highest value</returns>
		public static T HighestValue<T>(this IEnumerable<T> values)
			where T: struct, IComparable {

			bool firstIteration = true;
			T value = default;
			foreach (var item in values) {
				if (firstIteration) {
					value = item;
					firstIteration = false;
				} else if (item.CompareTo(value) >= 0) {
					value = item;
				}
			}
			return value;
		}
		/// <summary>
		/// Finds the lowest value and returns it
		/// </summary>
		/// <param name="input"></param>
		/// <returns>The lowest value</returns>
		public static T LowestValue<T>(this IEnumerable<T> values)
			where T: struct, IComparable {

			bool firstIteration = true;
			T value = default;
			foreach (var item in values) {
				if (firstIteration) {
					value = item;
					firstIteration = false;
				} else if (item.CompareTo(value) <= 0) {
					value = item;
				}
			}
			return value;
		}
	}
}
