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
			"smabuk" , "CopperBeardy", "encse", "Andriamanitra" };

		public string GetSolutionHref(int year, int day, string username) {
			const string GITHUB = "https://github.com/";
			
			if (username is null) { return ""; }
			if (KnownUsers.Contains(username) == false) { return ""; }

			string href = username.ToLower() switch {
				"andriamanitra" => $"{GITHUB}Andriamanitra/adventofcode{year}/blob/main/day{day:D2}",
				"copperbeardy" => $"{GITHUB}CopperBeardy/AdventOfCode{year}/blob/main/AdventOfCode{year}/AdventOfCode{year}/Days/Day{day}.cs",
				"encse" => $"{GITHUB}encse/adventofcode/blob/master/{year}/Day{day:D2}/Solution.cs",
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
				"copperbeardy" => $"CopperBeardy/AdventOfCode{year}/main/AdventOfCode{year}/AdventOfCode{year}/DayInputs/Day{day}.txt",
				"encse" => $"encse/adventofcode/master/{year}/Day{day:D2}/input.in",
				"smabuk" => $"smabuk/AdventOfCode/master/Data/{year}_{day:D2}.txt",
				_ => "",
			};

			var response = await _httpClient.GetAsync(path);

			return response.IsSuccessStatusCode switch {
				false => "",
				_ => await response.Content.ReadAsStringAsync()
			};
		}
	}
}
