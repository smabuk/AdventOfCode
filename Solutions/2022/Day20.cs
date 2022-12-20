namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 20: Grove Positioning System
/// https://adventofcode.com/2022/day/20
/// </summary>
[Description("Grove Positioning System")]
public sealed partial class Day20 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		List<IndexedItem> gpsInput = input.Select((i , index) => new IndexedItem(index, i.AsInt())).ToList();
		List<IndexedItem> gps = input.Select((i , index) => new IndexedItem(index, i.AsInt())).ToList();

		int listLength = gpsInput.Count;
		int newLength = listLength - 1;

		string T = string.Join(", ", gps);

		for (int i = 0; i < gpsInput.Count; i++) {
			string T1 = string.Join(",", gps.Take(10));
			IndexedItem x = gpsInput[i];
			if (x.Value == 0) {
				continue;
			}
			int pos = gps.IndexOf(x);
			gps.Remove(x);
			if (pos == 0) {
				pos = newLength;
			}
			int newPos = (pos + (x.Value % newLength)) % newLength;
			if (newPos == 0 && x.Value < 0) {
				newPos = newLength;
			} else if (newPos < 0) {
				newPos = (newLength + newPos) % newLength;
			}
			gps.Insert(newPos, x);
			string T2 = string.Join(",", gps.Take(10));
		}

		T = string.Join(", ", gps);
		IndexedItem zeroItem = gps.Where(x => x.Value== 0).FirstOrDefault();
		int posOfZero = gps.IndexOf(zeroItem);

		return gps[(posOfZero + 1000) % listLength].Value
			 + gps[(posOfZero + 2000) % listLength].Value
			 + gps[(posOfZero + 3000) % listLength].Value;
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

	private record Instruction(string Name, int Value) : IParsable<Instruction> {
		public static Instruction Parse(string s) => throw new NotImplementedException();
		public static Instruction Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result) => throw new NotImplementedException();
	}

	private record struct IndexedItem(int Index, int Value);

}
