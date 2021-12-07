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

	public static IEnumerable<string> PrintAsStringArray<T>(this T[,] input, int width) where T: struct {
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
