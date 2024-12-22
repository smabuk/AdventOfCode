namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 22: Monkey Market
/// https://adventofcode.com/2024/day/22
/// </summary>
[Description("Monkey Market")]
public static partial class Day22 {

	private static IEnumerable<MonkeyBuyer> _buyers = [];

	[Init]
	public static void LoadBuyers(string[] input) => _buyers = [.. input.As<MonkeyBuyer>()];

	public static long Part1(string[] _, params object[]? args)
	{
		int iterations = args.Iterations();
		return _buyers
			.Select(b => b.SecretNumber)
			.Select(sn => sn.GetSecretNumbers(iterations).Last())
			.Sum();
	}

	public static string Part2(string[] input, params object[]? args) => NO_SOLUTION_WRITTEN_MESSAGE;


	private static IEnumerable<long> GetSecretNumbers(this long secretNumber, int iterations)
	{
		long current = secretNumber;
		for (int i = 0; i < iterations; i++) {
			current = current.Mix(current * 64).Prune();
			current = (current / 32).Mix(current).Prune();
			current = (current * 2048).Mix(current).Prune();
			yield return current;
		}

	}

	private static long Mix(this long secretNumber, long value) => secretNumber ^ value;
	private static long Prune(this long secretNumber) => secretNumber % 16777216;


	public sealed record MonkeyBuyer(long SecretNumber) : IParsable<MonkeyBuyer>
	{
		public static MonkeyBuyer Parse(string s, IFormatProvider? provider) => new(long.Parse(s));
		public static MonkeyBuyer Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out MonkeyBuyer result)
			=> ISimpleParsable<MonkeyBuyer>.TryParse(s, provider, out result);
	}

	private static int Iterations(this object[]? args) => GetArgument(args, 1, 2000);
}
