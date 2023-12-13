namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 12: Hot Springs
/// https://adventofcode.com/2023/day/12
/// </summary>
[Description("Hot Springs")]
public sealed partial class Day12 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	public const char OPERATIONAL  = '.';
	public const char DAMAGED      = '#';
	public const char UNKNOWN      = '?';

	private static string Solution1(string[] input) {
		List<ConditionRecord> records = [.. input.As<ConditionRecord>()];

		int arrangementCount = 0;
		foreach (ConditionRecord originalRecord in records) {
			ConditionRecord record = originalRecord;
			int noOfUnknownSprings = record.UnknownSprings;
			int possibilities = 1 << noOfUnknownSprings;
			if (noOfUnknownSprings == 0) {
				if (record.IsMatch(record.Springs)) {
					arrangementCount++;
					continue;
				}
			}
			object? countLock = 1;
			_ = Parallel.For(0, possibilities, (i, state) =>
			{
				char[] newSpringArrangement = record.Springs.ToCharArray();
				char[] replacements = Convert.ToString(i, 2).ToString().PadLeft(noOfUnknownSprings, '0').ToCharArray();
				for (int b = 0; b < noOfUnknownSprings; b++) {
					int index = Array.IndexOf(newSpringArrangement, UNKNOWN);
					newSpringArrangement[index] = replacements[b] == '0' ? OPERATIONAL : DAMAGED;
				}
				string temp = new(newSpringArrangement);
				if (record.IsMatch(new(newSpringArrangement))) {
					lock(countLock) {
						arrangementCount++;
					}
				}
			});
		}
		return arrangementCount.ToString();
	}

	private static string Solution2(string[] input) {
		if (input[0] == "???#???.#??####? 5,1,5") {
			return "** Solution not written yet **";
		}

		List<ConditionRecord> records = [.. input.As<ConditionRecord>()];

		int arrangementCount = 0;
		string debug = Environment.NewLine;
		foreach (ConditionRecord originalRecord in records) {
			ConditionRecord record = originalRecord.Unfold(2);
			debug = debug + $"{record.Springs} {string.Join(",", record.DamagedGroups)}" + Environment.NewLine;
			record = record.Reduce();

			int noOfUnknownSprings = record.UnknownSprings;
			int possibilities = 1 << noOfUnknownSprings;
			int multiplier = record
				.Springs
				.TrimmedSplit((char[])[OPERATIONAL, DAMAGED, UNKNOWN])
				.As<int>()
				.Aggregate(1, (a, b) => a * b);
			//arrangementCount += record.CountSoFar;
			int count = 0;
			if (noOfUnknownSprings == 0) {
				arrangementCount += multiplier;
				count++;
				continue;
			}

			_ = Parallel.For(0, possibilities, (i, state) =>
			{
				char[] newSpringArrangement = record.Springs.ToCharArray();
				char[] replacements = Convert.ToString(i, 2).ToString().PadLeft(noOfUnknownSprings, '0').ToCharArray();
				for (int b = 0; b < noOfUnknownSprings; b++) {
					int index = Array.IndexOf(newSpringArrangement, UNKNOWN);
					newSpringArrangement[index] = replacements[b] == '0' ? OPERATIONAL : DAMAGED;
				}
				if (record.IsMatch(new(newSpringArrangement))) {
					arrangementCount += multiplier;
					count++;
				}
			});
			debug = debug + $"{record.Springs} {string.Join(",", record.DamagedGroups)}" + Environment.NewLine;
		}
		return arrangementCount.ToString()
			+ Environment.NewLine
			+ debug;
	}

	private sealed record ConditionRecord(string Springs, int[] DamagedGroups) : IParsable<ConditionRecord> {

		public int CountSoFar { get; set; } = 0;
		public int UnknownSprings => Springs.Count(spring => spring is UNKNOWN);

		public bool IsMatch(string springs)
		{
			int[] newDamagedGroups = [..GetDamagedGroups(springs)];
			if (newDamagedGroups.Length != DamagedGroups.Length) {
				return false;
			}
			for (int i = 0; i < DamagedGroups.Length; i++) {
				if (DamagedGroups[i] != newDamagedGroups[i]) {
					return false;
				}
			}
			return true;
		}

		public static IEnumerable<int> GetDamagedGroups(string springs)
		{
			char[] remove = [UNKNOWN, OPERATIONAL, '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
			string[] tokens = springs.TrimmedSplit(remove);
			foreach (string group in tokens) {
				yield return group.Length;
			}
		}


		/// <summary>
		/// This won't work as I'm not taking into account yet next tokens with UNKNOWNs
		/// </summary>
		/// <returns></returns>
		public ConditionRecord Reduce()
		{
			string[] tokens = [..Springs.TrimmedSplit(OPERATIONAL).Reverse()];
			string newSprings = "";
			int[] groups    = [..DamagedGroups];
			int[] newGroups = [];
			int countSoFar = 0;
			int i = 0;
			for (i = 0; i < tokens.Length; i++) {
				string token = tokens[i];
				//string runningtoken = "";
				if (false) {
				} else if (groups is []) {
					newSprings = $"{OPERATIONAL}1{newSprings}";
				} else if (groups.Length is 1 && token.All(t => t is UNKNOWN)) {
					if (token.Length == groups[^1]) {
						countSoFar += 1;
						groups = groups[..^1];
						newSprings = $"{OPERATIONAL}1{newSprings}";
						//} else {
						//	newSprings = $"{token}{OPERATIONAL}{newSprings}";
						//}
						//} else if (token == $"{UNKNOWN}{UNKNOWN}" && groups is [.. _, 1]) {
						//	countSoFar += 2;
						//	newSprings = $"{OPERATIONAL}2{newSprings}";
						//	groups = groups[..^1];
						//	newGroups = [.. newGroups];
						//} else if (token == $"{UNKNOWN}{UNKNOWN}{UNKNOWN}" && groups is [.. _, 1, 1]) {
						//	countSoFar += 1;
						//	newSprings = $"{OPERATIONAL}2{newSprings}";
						//	groups = groups[..^2];
						//} else if (token.All(t => t is DAMAGED)) {
						//	if (token.Length == groups[^1]) {
						//		countSoFar += 0;
						//		newSprings = $"{OPERATIONAL}1{OPERATIONAL}{newSprings}";
						//		groups = groups[..^1];
						//} else {
						//	break;
						//	countSoFar += 1;
						//	newSprings = $"{OPERATIONAL}{newSprings}";
					}
				} else {
					//	//	if (groups.Length == 0) {
					//	//		newSprings = $"{OPERATIONAL}{newSprings}";
					//	//	} else {
					//	//		int groupLength = groups[^1];
					//	//		if (token.EndsWith(DAMAGED)) {
					//	//			while (token.Length > 0) {
					//	//				token = token[..^token.Length];
					//	//				groups = groups[..^1];
					//	//				newSprings = $"{OPERATIONAL}{newSprings}";
					//	//			}
					//	//			continue;
					//	//		}
					//	//		if (i == 0) {
					//	//			newSprings = Springs;
					//	//		}else {
					//	//			newSprings = $"{string.Join(".", tokens.Reverse().Take(i - 1))}{OPERATIONAL}{newSprings}";
					//	//		}
					break;
					//	//	}
				}
		}
			countSoFar = countSoFar == 0 ? 0 : (int)Math.Pow(2, countSoFar - 1);
			newSprings = $"{string.Join(".", tokens.Reverse().Take(tokens.Length - i))}{OPERATIONAL}{newSprings}";
			return new ConditionRecord(newSprings, [.. newGroups, .. groups]) with { CountSoFar = countSoFar };
		}

		public  ConditionRecord Unfold(int count)
		{
			string newSprings = "";
			int[] newGroups = [];
			for (int i = 0; i < count; i++) {
				newSprings += $"{Springs}";
				newGroups = [.. newGroups, .. DamagedGroups];
			}
			return new(newSprings, newGroups);
		}


		public static ConditionRecord Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit(' ');
			return new(tokens[0], [..tokens[1].As<int>(',')]);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ConditionRecord result)
			=> ISimpleParsable<ConditionRecord>.TryParse(s, provider, out result);
	}

	private enum SpringType
	{
		Operational = 0,
		Damaged     = 1,
	}
}

public static class Day12Helpers
{
}

