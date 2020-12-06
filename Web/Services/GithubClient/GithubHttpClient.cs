using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

namespace AdventOfCode.Web {
	public class GithubHttpClient : IGithubHttpClient {
		private readonly HttpClient _httpClient;

		public GithubHttpClient(HttpClient httpClient) {
			httpClient.BaseAddress = new Uri("https://raw.githubusercontent.com/");
			_httpClient = httpClient;
		}

		public async Task<string> GetInputData(int year, int day, string? username) {
			
			if (username is null) { return ""; }

			string path = username.ToLower() switch {
				"andriamanitra" =>  $"Andriamanitra/adventofcode{year}/main/day{day:D2}/input.txt",
				"copperbeardy" =>   $"CopperBeardy/AdventOfCode{year}/main/AdventOfCode{year}/AdventOfCode{year}/DayInputs/Day{day}.txt",
				"smabuk" =>         $"smabuk/AdventOfCode/master/Data/{year}_{day:D2}.txt",
				_ => ""
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
