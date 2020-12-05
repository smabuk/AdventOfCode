using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

namespace AdventOfCode.Web {
	public class GithubClient : IGithubClient {
		private readonly HttpClient _httpClient;

		public GithubClient(HttpClient httpClient) {
			httpClient.BaseAddress = new Uri("https://raw.githubusercontent.com/");
			_httpClient = httpClient;
		}

		public async Task<string> GetInputData(string username, int year, int day) {
			string path = username.ToLower() switch {
				"copperbeardy" => $"CopperBeardy/AdventOfCode{year}/main/AdventOfCode{year}/AdventOfCode{year}/DayInputs/Day{day}.txt",
				"andriamanitra" => $"Andriamanitra/adventofcode{year}/main/day{day:D2}/input.txt",
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
