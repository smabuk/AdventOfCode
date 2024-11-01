using System.Security.Cryptography;
using System.Text;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 05: How About a Nice Game of Chess?
/// https://adventofcode.com/2016/day/05
/// </summary>
[Description("How About a Nice Game of Chess?")]
public sealed partial class Day05 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();


	private static string Solution1(string[] input)
	{
		const int PASSWORD_CHAR_INDEX = 5;
		const int PASSWORD_LENGTH = 8;

		string doorId = input[0];
		string password = "";
		foreach (string hex in GetNextHash(doorId)) {
			password = $"{password}{hex[PASSWORD_CHAR_INDEX]}";
			if (password.Length >= PASSWORD_LENGTH) {
				break;
			}
		}

		return password;
	}

	private static string Solution2(string[] input) {
		const char PLACE_HOLDER = '_';
		const int PASSWORD_LENGTH = 8;
		const int POSITION_INDEX = 5;
		const int PASSWORD_CHAR_INDEX = 8;

		string doorId = input[0];
		Span<char> password = new string(PLACE_HOLDER, PASSWORD_LENGTH).ToCharArray();
		
		foreach (string hex in GetNextHash(doorId)) {
			if (int.TryParse($"{hex[POSITION_INDEX]}", out int position) && position < PASSWORD_LENGTH && password[position] == PLACE_HOLDER) {
				password[position] = hex[PASSWORD_CHAR_INDEX];
				if (!password.Contains(PLACE_HOLDER)) {
					break;
				}
			}
		}

		return $"{password}";
	}

	private static IEnumerable<string> GetNextHash(string doorId)
	{
		long i = 0;
		while (true) {
			string hex = Convert.ToHexStringLower(MD5.HashData(Encoding.UTF8.GetBytes($"{doorId}{i}")));
			if (hex.StartsWith("00000")) {
				yield return hex;
			}
			i++;
		}
	}
}
