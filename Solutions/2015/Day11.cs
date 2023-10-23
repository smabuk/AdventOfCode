using System.Text;

namespace AdventOfCode.Solutions._2015;

/// <summary>
/// Day 11: Corporate Policy
/// https://adventofcode.com/2015/day/11
/// </summary>
[Description("Corporate Policy")]
public class Day11 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static string Solution1(string[] input) {
		string startingPassword = input[0];
		int pLength = startingPassword.Length;
		string newPassword = startingPassword;

		bool notValid = false;
		do {
			newPassword = IncrementPasswordBy(newPassword, 1);
			notValid = true;

			// i, l, o
			int foundAt = newPassword.IndexOfAny(new char[] { 'i', 'o', 'l' });
			int col = GetSignficantDigit(foundAt, pLength);
			while (foundAt >= 0) {
				newPassword = IncrementPasswordBy(newPassword, PowersOf26[col]);
				newPassword = newPassword[..(foundAt + 1)] + new string('a', col - 1);
				foundAt = newPassword.IndexOfAny(new char[] { 'i', 'o', 'l' });
				col = GetSignficantDigit(foundAt, pLength);
			}

			// double chars
			List<(char value, int pos)> doublePos = [];
			bool foundDoubles = false;
			for (int i = 0; i < pLength - 1; i++) {
				if (newPassword[i] == newPassword[i + 1]) {
					doublePos.Add((newPassword[i], i));
				}
			}
			foreach ((char value, int pos) in doublePos) {
				if (doublePos.Where(p => p.value != value && Math.Abs(p.pos - pos) > 1).Any()) {
					foundDoubles = true;
				} else {
					notValid = true;
					continue;
				}
			}

			if (foundDoubles) {
				// consecutive run of 3
				for (int i = 0; i < pLength - 2; i++) {
					if (newPassword[i] == newPassword[i + 1] - 1
						&& newPassword[i] == newPassword[i + 2] - 2) {
						notValid = false;
						break;
					}
				}
			}

		} while (notValid);

		return newPassword;
	}

	private static string Solution2(string[] input) {
		string startingPassword = input[0];
		string newPassword = startingPassword;

		newPassword = Solution1(new string[] { newPassword });
		newPassword = Solution1(new string[] { newPassword });

		return newPassword;
	}

	private static int GetSignficantDigit(int foundAt, int pLength) => pLength - foundAt;


	/* https://www.minus40.info/sky/powers64.html */
	protected static readonly long[] PowersOf26 = {
			0,
			1,
			26,
			676,
			17576,
			456976,
			11881376,
			308915776,
			8031810176,
			208827064576,
			5429503678976,
			141167095653376
		};

	public static string IncrementPasswordBy(string password, long increment) {
		// a = 0 -> z = 25
		int pLength = password.Length;

		long value = 0;
		for (int i = 1; i <= pLength; i++) {
			int v = password[pLength - i] - 'a';
			value += v * PowersOf26[i];
		}

		value += increment;
		int[] values = new int[pLength];
		for (int i = pLength; i >= 1; i--) {
			int v = (int)(value % 26);
			values[i - 1] = 'a' + v;
			value /= 26;
		}

		StringBuilder sb = new();
		for (int i = 0; i < values.Length; i++) {
			_ = sb.Append((char)values[i]);
		}

		return sb.ToString();
	}
}
