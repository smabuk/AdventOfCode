namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 07: Internet Protocol Version 7
/// https://adventofcode.com/2016/day/07
/// </summary>
[Description("Internet Protocol Version 7")]
public sealed partial class Day07 {

	[Init]
	public static void Init(string[] input, params object[]? args) => LoadIPAddresses(input);
	public static string Part1(string[] input, params object[]? args) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static List<IPAddressV7> _ipAddresses = [];

	private static void LoadIPAddresses(string[] input) => _ipAddresses = [.. input.As<IPAddressV7>()];

	private static int Solution1() => 
		_ipAddresses
		.Count(ipAddress => ipAddress.OtherSequences.Any(ContainsABBA) && !ipAddress.HypernetSequences.Any(ContainsABBA));

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}

	private static bool ContainsABBA(string s)
	{
		if (s.Length < 4) {  return false; }
		for (int i = 0; i < s.Length - 3; i++) {
			if (IsABBA(s[i..(i+4)])) { return true; }
		}
		return false;
	}

	private static bool IsABBA(string s) => 
		s.Length == 4
		&& s[0] != s[1]
		&& s[0] == s[3]
		&& s[1] == s[2];

	private sealed record IPAddressV7(string IPAddress) : IParsable<IPAddressV7> {

		public string[] HypernetSequences { get; set; } = 
			InputRegEx()
			.Matches(IPAddress)
			.Select(x => x.Value)
			.Where(x => x.StartsWith('['))
			.Select(x => x[1..^1])
			.ToArray();

		public string[] OtherSequences { get; set; } = 
			InputRegEx()
			.Matches(IPAddress)
			.Select(x => x.Value)
			.NotWhere(x => x.StartsWith('[') || x == "")
			.ToArray();

		public static IPAddressV7 Parse(string s, IFormatProvider? provider) => new(s);
		public static IPAddressV7 Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out IPAddressV7 result)
			=> ISimpleParsable<IPAddressV7>.TryParse(s, provider, out result);
	}

	[GeneratedRegex("""\[(?<hypernet>\w+)\]|(?<number>[\w+]?)*""")]
	private static partial Regex InputRegEx();
}
