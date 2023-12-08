namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 20: Grove Positioning System
/// https://adventofcode.com/2022/day/20
/// </summary>
[Description("Grove Positioning System")]
public sealed partial class Day20 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static long Solution1(string[] input) {
		List<IndexedItem> gpsInput =
			input
			.Select((i, index) => new IndexedItem(index, i.As<int>()))
			.ToList();

		return MixResult(gpsInput);
	}

	private static long Solution2(string[] input) {
		const long DECRYPTION_KEY = 811589153;
		
		List<IndexedItem> gpsInput =
			input
			.Select((i, index) => new IndexedItem(index, i.As<int>() * DECRYPTION_KEY))
			.ToList();

		return MixResult(gpsInput, 10);
	}

	private static long MixResult(List<IndexedItem> gpsInput, int Repetitions = 1) {
		List<IndexedItem> gps = gpsInput.ToList();

		int listLength = gpsInput.Count;
		int newLength = listLength - 1;

		for (int repeat = 1; repeat <= Repetitions; repeat++) {
			Mix();
		}

		IndexedItem zeroItem =
			gps
			.Where(x => x.Value == 0)
			.Single();
		int posOfZero = gps.IndexOf(zeroItem);

		return gps[(posOfZero + 1000) % listLength].Value
			 + gps[(posOfZero + 2000) % listLength].Value
			 + gps[(posOfZero + 3000) % listLength].Value;


		void Mix() {
			for (int i = 0; i < gpsInput.Count; i++) {
				IndexedItem x = gpsInput[i];

				int pos = gps.IndexOf(x);
				_ = gps.Remove(x);

				int newPos = (pos + (int)(x.Value % newLength)) % newLength;
				if (newPos == 0) {
					newPos = newLength;
				} else if (newPos < 0) {
					newPos = (newLength + newPos) % newLength;
				}
				gps.Insert(newPos, x);
			}
		}
	}

	private record struct IndexedItem(int Index, long Value);

}
