namespace AdventOfCode.Solutions._2020;

/// <summary>
/// Day 4: Passport Processing
/// https://adventofcode.com/2020/day/4
/// </summary>
[Description("Passport Processing")]
public static class Day04 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	public static string Solution1(string[] input) {
		return CountValidPassports(input, 1).ToString();
	}

	public static string Solution2(string[] input) {
		return CountValidPassports(input, 2).ToString();
	}

	public static long CountValidPassports(string[] input, int problemNo) {

		List<Passport> passports = new();
		List<string> inputs = input.ToList();
		inputs.Add("");
		Dictionary<string, string> passportFields = new();
		foreach (string inputLine in inputs) {
			if (string.IsNullOrWhiteSpace(inputLine)) {
				Passport passport = new(passportFields);
				passports.Add(passport);
				passportFields = new();
			} else {
				string[] pairs = inputLine.Split(" ");
				foreach (string pair in pairs) {
					string[] field = pair.Split(":");
					passportFields.Add(field[0], field[1]);
				}
			}

		}
		return problemNo switch {
			1 => passports.Count(p => p.IsValidPart1),
			2 => passports.Count(p => p.IsValidPart2),
			_ => passports.Count(p => p.IsValidPart1)
		};
	}

	public record Passport(Dictionary<string, string> Fields) {
		public bool IsValidPart1 {
			get {
				string[]? f = Fields.Keys.OrderBy(k => k).ToArray();
				if (string.Join(",", f).Replace("cid,", "") == "byr,ecl,eyr,hcl,hgt,iyr,pid") {
				} else {
					return false;
				};
				return true;
			}
		}
		public bool IsValidPart2 {
			get {
				string[]? f = Fields.Keys.OrderBy(k => k).ToArray();
				if (string.Join(",", f).Replace("cid,", "") == "byr,ecl,eyr,hcl,hgt,iyr,pid") {
					foreach (KeyValuePair<string, string> item in Fields) {
						if (!IsFieldValid(item.Key, item.Value)) {
							return false;
						}
					}
				} else {
					return false;
				};
				return true;
			}
		}
	};

	private static bool IsFieldValid(string key, string value) {
		(int intValue, string strValue, string units) = GetFieldParts(key, value);
		return key switch {
			"byr" => intValue is >= 1920 and <= 2002,
			"iyr" => intValue is >= 2010 and <= 2020,
			"eyr" => intValue is >= 2020 and <= 2030,
			"ecl" => new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(value),
			"hcl" => units == "#" && intValue > 0 && strValue.Length == 6,
			"hgt" when units == "cm" => intValue is >= 150 and <= 193,
			"hgt" when units == "in" => intValue is >= 59 and <= 76,
			"pid" => intValue > 0 && strValue.Length == 9,
			"cid" => true,
			_ => false
		};
	}

	private static (int intValue, string strValue, string units) GetFieldParts(string key, string value) {
		int intValue = 0;
		string strValue = "";
		string units = "";
		switch (key) {
			case "byr":
			case "iyr":
			case "eyr":
			case "pid":
				_ = int.TryParse(value, out intValue);
				strValue = value;
				break;
			case "hcl":
				try {
					intValue = int.Parse(value[1..], System.Globalization.NumberStyles.HexNumber);
				} catch (System.Exception) {
				}
				strValue = value[1..];
				units = value[..1];
				break;
			case "hgt":
				_ = int.TryParse(value.Replace("cm", "").Replace("in", ""), out intValue);
				units = value.EndsWith("in") ? "in" : value.EndsWith("cm") ? "cm" : "";
				break;
			default:
				break;
		}
		return (intValue, strValue, units);
	}
}
