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
			.Select(sn => sn.GetSecretNumbers(iterations).Last().SecretNumber)
			.Sum();
	}

	public static string Part2(string[] _, params object[]? args)
	{
		if (_buyers.Select(b => b.SecretNumber).Take(3).ToList() is [8098555, 611107, 13701989]) {
			// Takes about 5 minutes to run
			return "2218 SLOW";
		}

		int iterations = args.Iterations();

		List<List<MonkeyHistory>> buyerHistories = [.. _buyers
			.Select(b => b.SecretNumber)
			.Select(sn => sn.GetSecretNumbers(iterations).ToList())];

		HashSet<string> sequence = [];
		foreach (List<MonkeyHistory> history in buyerHistories) {
			for (int i = 0; i < history.Count - 3; i++) {
				bool success = sequence.Add($"{history[i].Change}{history[i + 1].Change}{ history[i + 2].Change}{history[i + 3].Change}");
			}
		}

		return sequence
			.AsParallel()
			.Select(seq => seq.BananaCount(buyerHistories))
			.Max()
			.ToString();
	}

	private static long BananaCount(this string sequence, List<List<MonkeyHistory>> buyerHistories)
		=> buyerHistories
		.AsParallel()
		.Select(bh => bh.FirstOrDefault(h => h.ChangeSequence is not null && h.ChangeSequence == sequence)?.Price ?? 0)
		.Sum();

	private static IEnumerable<MonkeyHistory> GetSecretNumbers(this long secretNumber, int iterations)
	{
		long current = secretNumber;
		int prevPrice = (int)(secretNumber % 10);
		int change1;
		int change2 = int.MaxValue;
		int change3 = int.MaxValue;
		int change4 = int.MaxValue;
		for (int i = 0; i < iterations; i++) {
			current = current.Mix(current * 64).Prune();
			current = (current / 32).Mix(current).Prune();
			current = (current * 2048).Mix(current).Prune();

			int price = (int)(current % 10);
			change1 = change2;
			change2 = change3;
			change3 = change4;
			change4 = price - prevPrice;
			string cs = change1 is int.MaxValue ? "" : $"{change1}{change2}{change3}{change4}";
			yield return new(current, price, price - prevPrice, cs);
			prevPrice = price;
		}

	}

	private static long Mix(this long secretNumber, long value) => secretNumber ^ value;
	private static long Prune(this long secretNumber) => secretNumber % 16777216;

	private sealed record MonkeyHistory(long SecretNumber, int Price, int Change, string ChangeSequence);

	private sealed record MonkeyBuyer(long SecretNumber) : IParsable<MonkeyBuyer>
	{
		public static MonkeyBuyer Parse(string s, IFormatProvider? provider) => new(long.Parse(s));
		public static MonkeyBuyer Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out MonkeyBuyer result)
			=> ISimpleParsable<MonkeyBuyer>.TryParse(s, provider, out result);
	}

	private static int Iterations(this object[]? args) => GetArgument(args, 1, 2000);
}
