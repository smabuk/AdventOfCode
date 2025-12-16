namespace AdventOfCode.Solutions._1111;

/// <summary>
/// Day 01: GenerateIParsable (simple)
/// https://adventofcode.com/
/// Samples and examples and tests
/// </summary>
[Description("GenerateIParsable (simple)")]
public partial class Day01 {

	public static int Part1(string[] input)
	{
		if (input is []) {
			input = ["65", "34", "12", "44", "123"];
		}

		List<IntRecord> intRecords = [.. input.Select(IntRecord.Parse)];
		List<StringRecord> stringRecords = [.. input.Select(StringRecord.Parse)];

		List<TRecord<int>> tIntRecords = [.. input.Select(TRecord<int>.Parse)];
		List<TRecord<uint>> tUintRecords = [.. input.Select(TRecord<uint>.Parse)];
		List<TRecord<string>> tStringRecords = [.. input.Select(TRecord<string>.Parse)];

		return intRecords.Count + tIntRecords.Count + tUintRecords.Count + tStringRecords.Count + stringRecords.Count;
	}

	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;

	[GenerateIParsable] private sealed partial record TRecord<T>(T Value);
	[GenerateIParsable] private sealed partial record IntRecord(int Value);
	[GenerateIParsable] private sealed partial record StringRecord(string Value);
	[GenerateIParsable] private sealed partial record StringIntRecord(string Name, int Value);

	//[GenerateIParsable] private sealed partial record TValuesRecord<T>(T Value, List<T> Values);
}
