using System.Collections.Generic;

namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 12: Hot Springs
/// https://adventofcode.com/2023/day/12
/// </summary>
[Description("Hot Springs")]
public sealed partial class Day12 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	public const char OPERATIONAL  = '.';
	public const char DAMAGED      = '#';
	public const char UNKNOWN      = '?';

	private static IEnumerable<ConditionRecord> _conditionRecords = [];

	private static void LoadInstructions(string[] input) {
		_conditionRecords = input.As<ConditionRecord>();
	}

	private static int Solution1(string[] input) {
		List<ConditionRecord> records = [.. input.As<ConditionRecord>()];

		int arrangementCount = 0;
		foreach (ConditionRecord record in records) {
			int noOfUnknownSprings = record.UnknownSprings;
			int possibilities = 1 << noOfUnknownSprings;
			if (noOfUnknownSprings == 0) {
				if (record.IsMatch(record.Springs)) {
					arrangementCount++;
					continue;
				}
			}
			for (int i = 0; i < possibilities; i++) {
				char[] newSpringArrangement = record.Springs.ToCharArray();
				char[] replacements = Convert.ToString(i, 2).ToString().PadLeft(noOfUnknownSprings, '0').ToCharArray();
				for (int b = 0; b < noOfUnknownSprings; b++) {
					int index = Array.IndexOf(newSpringArrangement, UNKNOWN);
					newSpringArrangement[index] = replacements[b] == '0' ? OPERATIONAL : DAMAGED;
				}
				if (record.IsMatch(new(newSpringArrangement))) {
					arrangementCount++;
				}
			}
		}
		return arrangementCount;
	}

	private static int Solution2(string[] input) {
		List<ConditionRecord> records = [.. input.As<ConditionRecord>()];

		int arrangementCount = 0;
		foreach (ConditionRecord originalRecord in records) {
			ConditionRecord record = originalRecord.Unfold();
			int noOfUnknownSprings = record.UnknownSprings;
			int possibilities = 1 << noOfUnknownSprings;
			if (noOfUnknownSprings == 0) {
				if (record.IsMatch(record.Springs)) {
					arrangementCount++;
					continue;
				}
			}
			for (int i = 0; i < possibilities; i++) {
				char[] newSpringArrangement = record.Springs.ToCharArray();
				char[] replacements = Convert.ToString(i, 2).ToString().PadLeft(noOfUnknownSprings, '0').ToCharArray();
				for (int b = 0; b < noOfUnknownSprings; b++) {
					int index = Array.IndexOf(newSpringArrangement, UNKNOWN);
					newSpringArrangement[index] = replacements[b] == '0' ? OPERATIONAL : DAMAGED;
				}
				string temp = new(newSpringArrangement);
				if (record.IsMatch(new(newSpringArrangement))) {
					arrangementCount++;
				}
			}
		}
		return arrangementCount;
	}

	private sealed record ConditionRecord(string Springs, int[] DamagedGroups) : IParsable<ConditionRecord> {

		public int UnknownSprings = Springs.Count(spring => spring is UNKNOWN);

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
			char[] remove = [UNKNOWN, OPERATIONAL];
			string[] tokens = springs.TrimmedSplit(remove);
			foreach (string group in tokens) {
				yield return group.Length;
			}
		}

		public  ConditionRecord Unfold()
		{
			string newSprings = $"{Springs}?{Springs}?{Springs}?{Springs}?{Springs}";
			int[] newGroups = [.. DamagedGroups, .. DamagedGroups, .. DamagedGroups, .. DamagedGroups, .. DamagedGroups];
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

