using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

namespace AdventOfCode.Services {
	public class GithubHttpClient : IGithubHttpClient {
		private readonly HttpClient _httpClient;

		public GithubHttpClient(HttpClient httpClient) {
			httpClient.BaseAddress = new Uri("https://raw.githubusercontent.com/");
			_httpClient = httpClient;
		}

		public List<string> KnownUsers => new() { 
			"smabuk" , "CopperBeardy", "Rollerss", "encse", "KevinSjoberg", "Andriamanitra", "Bassel-T" };

		public string GetSolutionHref(int year, int day, string username) {
			const string GITHUB = "https://github.com/";
			
			if (username is null) { return ""; }
			if (KnownUsers.Contains(username) == false) { return ""; }

			string href = username.ToLower() switch {
				"andriamanitra" => $"{GITHUB}Andriamanitra/adventofcode{year}/blob/main/day{day:D2}",
				"bassel-t" => $"{GITHUB}Bassel-T/AdventOfCode{year}-CS/blob/main/AdventOfCode{year}/Day{day}.cs",
				"copperbeardy" => $"{GITHUB}CopperBeardy/AdventOfCode{year}/blob/main/AdventOfCode{year}/AdventOfCode{year}/Days/Day{day}.cs",
				"encse" => $"{GITHUB}encse/adventofcode/blob/master/{year}/Day{day:D2}/Solution.cs",
				//"ians-au" => $"{GITHUB}ians-au/AdventOfCode{year}/blob/main/day{day:D2}.cs",
				"kevinsjoberg" => $"{GITHUB}KevinSjoberg/aoc-{year}/blob/main/{day:D2}/day{day:D2}.cr",
				"rollerss" => $"{GITHUB}Rollerss/AOC_{year}/blob/master/AOC/Day{day:D2}.cs",
				"smabuk" => $"{GITHUB}smabuk/AdventOfCode/tree/master/Solutions/{year}/Day{day:D2}.cs",
				_ => "",
			};

			return href;
		}

		public async Task<string> GetInputData(int year, int day, string? username) {
			if (username is null) { return ""; }
			if (KnownUsers.Contains(username) == false) { return ""; }

			string path = username.ToLower() switch {
				"andriamanitra" => $"Andriamanitra/adventofcode{year}/main/day{day:D2}/input.txt",
				"bassel-t" => "",
				"copperbeardy" => $"CopperBeardy/AdventOfCode{year}/main/AdventOfCode{year}/AdventOfCode{year}/DayInputs/Day{day}.txt",
				"encse" => $"encse/adventofcode/master/{year}/Day{day:D2}/input.in",
				//"ians-au" => $"",
				"kevinsjoberg" => $"KevinSjoberg/aoc-{year}/main/{day:D2}/input.txt",
				"rollerss" => $"Rollerss/AOC_{year}/master/AOC/InputData/AOCDay{day:D2}.txt",
				"smabuk" => $"smabuk/AdventOfCode/master/Data/{year}_{day:D2}.txt",
				_ => "",
			};

			if (string.IsNullOrEmpty(path)) {
				return "";
			}
			var response = await _httpClient.GetAsync(path);

			return response.IsSuccessStatusCode switch {
				false => "",
				_ => await response.Content.ReadAsStringAsync()
			};
		}
	}
}
