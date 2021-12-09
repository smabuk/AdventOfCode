namespace AdventOfCode.Solutions.Helpers;

public static class ArrayHelpers {
	public static T[,] AsArray<T>(this T[] input, int cols, int? rows = null) {
		int inputLength = input.Length;
		rows ??= inputLength % cols == 0 ? inputLength / cols : (inputLength / cols) + 1;
		T[,] result = new T[cols, (int)rows];
		int i = 0;
		for (int r = 0; r < rows; r++) {
			for (int c = 0; c < cols; c++) {
				result[c, r] = input[i++];
				if (i >= inputLength) {
					return result;
				}
			}
		}
		return result;
	}
	public static T[,] AsArray<T>(this IEnumerable<T> input, int cols, int? rows = null) {
		return AsArray<T>(input.ToArray(), cols, rows);
	}

	public static IEnumerable<string> PrintAsStringArray<T>(this T[,] input, int width) where T : struct {
		for (int r = 0; r <= input.GetUpperBound(1); r++) {
			string line = "";
			for (int c = 0; c <= input.GetUpperBound(0); c++) {
				string cell = input[c, r].ToString() ?? "";
				line += $"{new string(' ', width - cell.Length)}{cell}";
			}
			yield return line;
		}
	}

	/// <summary>
	/// Finds the mean average and returns it as a double
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="numbers"></param>
	/// <returns></returns>
	public static double Mean<T>(this T[] numbers) =>
		numbers.Select(n => Convert.ToDouble(n)).ToArray().Average();

	/// <summary>
	/// Finds the Median value and returns it
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="numbers"></param>
	/// <returns></returns>
	public static double Median<T>(this T[] numbers) {
		IOrderedEnumerable<T> sortedNumbers = numbers.OrderBy(n => n);
		int midPoint = numbers.Length / 2;
		return (numbers.Length % 2) switch {
			0 => (Convert.ToDouble(sortedNumbers.ElementAt(midPoint))
				+ Convert.ToDouble(sortedNumbers.ElementAt(midPoint - 1)))
				/ 2.0
,
			_ => Convert.ToDouble(sortedNumbers.ElementAt(midPoint))
		};
	}

	/// <summary>
	/// Returns the values occuring the most times
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <returns></returns>
	public static IEnumerable<T> Modes<T>(this T[] array) {
		(T Key, int Count)[] counts = array.GroupBy(n => n)
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
