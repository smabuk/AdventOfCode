using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Helpers
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
		public static T HighestValue<T>(params T[] values)
			where T: struct, IComparable {

			return HighestValue(values.ToList());
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
		public static T LowestValue<T>(params T[] values)
			where T : struct, IComparable {

			return LowestValue(values.ToList());
		}

		/// <summary>
		/// LINQ for generating all possible permutations
		/// https://codereview.stackexchange.com/questions/226804/linq-for-generating-all-possible-permutations
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static IEnumerable<T[]> Permutate<T>(this IEnumerable<T> source) {
			return permutate(source, Enumerable.Empty<T>());
			IEnumerable<T[]> permutate(IEnumerable<T> reminder, IEnumerable<T> prefix) =>
				!reminder.Any() ? new[] { prefix.ToArray() } :
				reminder.SelectMany((c, i) => permutate(
					reminder.Take(i).Concat(reminder.Skip(i + 1)).ToArray(),
					prefix.Append(c)));
		}
	}
}
