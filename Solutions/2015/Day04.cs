﻿using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Solutions._2015;

/// <summary>
/// Day 04: The Ideal Stocking Stuffer
/// https://adventofcode.com/2015/day/4
/// </summary>
[Description("The Ideal Stocking Stuffer")]
public class Day04 {
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static long Solution1(string[] input) {
		string secretKey = input[0];

		long i;
		for (i = 0; i < 999999999999; i++) {
			string hashSource = $"{secretKey}{i}";
			byte[] bytes = Encoding.UTF8.GetBytes(hashSource);
			byte[] hashBytes = MD5.HashData(bytes);
			string hex = BitConverter.ToString(hashBytes).Replace("-", "");
			if (hex.StartsWith("00000")) {
				break;
			}
		}
		return i;
	}

	private static long Solution2(string[] input) {
		string secretKey = input[0];

		long i;
		for (i = 0; i < 9999999999; i++) {
			string hashSource = $"{secretKey}{i}";
			byte[] bytes = Encoding.UTF8.GetBytes(hashSource);
			byte[] hashBytes = MD5.HashData(bytes);
			string hex = BitConverter.ToString(hashBytes).Replace("-", "");
			if (hex.StartsWith("000000")) {
				break;
			}
		}
		return i;
	}
}
