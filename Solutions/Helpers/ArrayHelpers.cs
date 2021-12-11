namespace AdventOfCode.Solutions.Helpers;

public static partial class ArrayHelpers {

	/// <summary>
	/// Finds the mean average and returns it as a double
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="numbers"></param>
	/// <returns></returns>
	public static double Mean<T>(this IEnumerable<T> numbers) where T : struct =>
		numbers.Select(n => Convert.ToDouble(n)).ToArray().Average();

	/// <summary>
	/// Finds the Median value and returns it as double
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="numbers"></param>
	/// <returns></returns>
	public static double MedianAsDouble<T>(this T[] numbers) where T : struct {
		IOrderedEnumerable<T> sortedNumbers = numbers.OrderBy(n => n);
		int midPoint = numbers.Length / 2;
		return (numbers.Length % 2) switch {
			0 => (Convert.ToDouble(sortedNumbers.ElementAt(midPoint))
				+ Convert.ToDouble(sortedNumbers.ElementAt(midPoint - 1)))
				/ 2.0,
			_ => Convert.ToDouble(sortedNumbers.ElementAt(midPoint))
		};
	}

	/// <summary>
	/// Finds the Median value and returns it as double
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="numbers"></param>
	/// <returns></returns>
	public static double Median<T>(this IEnumerable<T> numbers) where T : struct {
		return numbers.ToArray().MedianAsDouble();
	}

	/// <summary>
	/// Finds the Median value and returns it as an int
	/// </summary>
	/// <param name="numbers"></param>
	/// <returns></returns>
	public static int Median(this IEnumerable<int> numbers) {
		return Convert.ToInt32(numbers.ToArray().MedianAsDouble());
	}

	/// <summary>
	/// Finds the Median value and returns it as a long
	/// </summary>
	/// <param name="numbers"></param>
	/// <returns></returns>
	public static long Median(this IEnumerable<long> numbers) {
		return Convert.ToInt64(numbers.ToArray().MedianAsDouble());
	}


	/// <summary>
	/// Returns the values occuring the most times
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <returns></returns>
	public static IEnumerable<T> Modes<T>(this T[] array) {
		(T Key, int Count)[] counts = array
			.GroupBy(x => x)
			.Select(g => (g.Key, Count: g.Count()))
			.ToArray();

		int maxCount = counts.Max(c => c.Count);

		IEnumerable<T>? modes = counts
			.Where(m => m.Count == maxCount)
			.Select(item => item.Key);

		foreach (T item in modes) {
			yield return item;
		}
	}


	/// <summary>
	/// Finds the highest value and returns it
	/// </summary>
	/// <param name="input"></param>
	/// <returns>The highest value</returns>
	public static T HighestValue<T>(this IEnumerable<T> values)
		where T : struct, IComparable {

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
		where T : struct, IComparable {

		return HighestValue(values.ToList());
	}




	/// <summary>
	/// Finds the lowest value and returns it
	/// </summary>
	/// <param name="input"></param>
	/// <returns>The lowest value</returns>
	public static T LowestValue<T>(this IEnumerable<T> values)
		where T : struct, IComparable {

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
	public static IEnumerable<T[]> Permute<T>(this IEnumerable<T> source) {
		return permute(source.ToArray(), Enumerable.Empty<T>());
		IEnumerable<T[]> permute(IEnumerable<T> remainder, IEnumerable<T> prefix) =>
			!remainder.Any() ? new[] { prefix.ToArray() } :
			remainder.SelectMany((c, i) => permute(
				remainder.Take(i).Concat(remainder.Skip(i + 1)).ToArray(),
				prefix.Append(c)));
	}


	public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k) {
		return k == 0 ? new[] { Array.Empty<T>() } :
		  elements.SelectMany((e, i) =>
			elements.Skip(i + 1)
					.Combinations(k - 1)
					.Select(c => (new[] { e })
					.Concat(c)));
	}
}
