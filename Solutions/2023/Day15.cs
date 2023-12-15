namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 15: Lens Library
/// https://adventofcode.com/2023/day/15
/// </summary>
[Description("Lens Library")]
public sealed partial class Day15 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		return input
			.Single()
			.TrimmedSplit(",")
			.As<Step>()
			.Sum(step => Step.HashNumber(step.Name));
	}

	private static int Solution2(string[] input) {
		List<Step> initializationSequence = [..
			input
			.Single()
			.TrimmedSplit(",")
			.As<Step>()];

		Dictionary<int ,Box> boxes = Enumerable.Range(0, 256)
			.Select(i => new Box(i))
			.ToDictionary(b => b.Number, b => b);

		foreach (Step step in initializationSequence) {
			Lens lens = step.Lens;
			int boxNo = Step.HashNumber(lens.Label);
			boxes[boxNo].ProcessLens(step.Operation, lens);
		}

		return boxes.Sum(box => box.Value.FocusingPower);
	}



	private sealed record Step(string Name) : IParsable<Step>
	{
		private const char INSERT = '=';

		public readonly Operation Operation = Name.Contains(INSERT) ? Operation.Insert : Operation.Remove;
		public readonly Lens Lens = Name.As<Lens>();

		public static int HashNumber(string s)
		{
			int result = 0;
			foreach (char c in s) {
				result += c;
				result *= 17;
				result %= 256;
			}
			return result;
		}

		public static Step Parse(string s, IFormatProvider? provider) => new(s);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Step result)
			=> ISimpleParsable<Step>.TryParse(s, provider, out result);
	}



	private sealed record Box(int Number) {
		const int NOT_FOUND = -1;
		
		private readonly List<Lens> lensSlot = [];

		public int FocusingPower => lensSlot.Select((lens, index) => (Number + 1) * (index + 1) * lens.FocalLength).Sum();

		public void ProcessLens(Operation operation, Lens lens)
		{
			if (operation == Operation.Remove) {
				Remove(lens);
			} else {
				Insert(lens);
			}
		}

		private void Insert(Lens lens)
		{
			int index = lensSlot.FindIndex(l => lens.Label == l.Label);
			if (index == NOT_FOUND) {
				lensSlot.Add(lens);
			} else {
				lensSlot[index] = lens;
			}
		}

		private void Remove(Lens lens)
		{
			int index = lensSlot.FindIndex(l => lens.Label == l.Label);
			if (index != NOT_FOUND) {
				lensSlot.RemoveAt(index);
			}
		}
	}



	private sealed record Lens(string Label, int FocalLength) : IParsable<Lens> {
		public static Lens Parse(string s, IFormatProvider? provider)
		{
			const char INSERT = '=';
			const char REMOVE = '-';
			char[] splitBy = [INSERT, REMOVE];

			string[] split = s.TrimmedSplit(splitBy);
			return s.Contains(INSERT)
				? new(split[0], split[1].As<int>())
				: new(split[0], 0);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Lens result)
			=> ISimpleParsable<Lens>.TryParse(s, provider, out result);
	};



	public enum Operation
	{
		Remove,
		Insert,
	}
}
