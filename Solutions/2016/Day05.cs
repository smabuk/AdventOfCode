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
		string doorId = input[0];
		string password = "";
		long i = 0;
		while (password.Length < 8) {
			string hex = Convert.ToHexStringLower(MD5.HashData(Encoding.UTF8.GetBytes($"{doorId}{i}")));
			if (hex.StartsWith("00000")) {
				password = $"{password}{hex[5]}";
			}
			i++;
		}
		return password;
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}
