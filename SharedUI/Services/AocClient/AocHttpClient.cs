﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

namespace AdventOfCode.Services {
	public class AocHttpClient : IAocHttpClient {
		private readonly HttpClient _httpClient;

		public AocHttpClient(HttpClient httpClient) {
			httpClient.BaseAddress = new Uri(@"https://adventofcode.com/");
			_httpClient = httpClient;
		}

		public async Task<string> GetInputData(int year, int day) {
			var response = await _httpClient.GetAsync($"{year}/day/{day}/input");

			string value = response.IsSuccessStatusCode switch {
				false => "",
				_ => await response.Content.ReadAsStringAsync()
			};

			return value;
		}

		public async Task<string> GetProblemDescription(int year, int day, int problemNo = 1) {
			var response = await _httpClient.GetAsync($"{year}/day/{day}");

			if (!response.IsSuccessStatusCode) {
				return "";
			}

			string page = await response.Content.ReadAsStringAsync();
			string article = "";
			string part = page;
			for (int i = 0; i < problemNo; i++) {
				int start = part.IndexOf("<article");
				if (start <= 0) {
					article = "";
					break;
				}
				int end = part[start..].IndexOf("</article>") + 10;
				article = part[start..(start + end)];
				part = part[(start + end + 1)..];
			}

			return article;

		}

		public async Task<AocSummary?> GetSummaryInfo(int year) {
			var response = await _httpClient.GetAsync($"{year}");

			if (response.IsSuccessStatusCode == false) {
				return null;
			}
			AocSummary summary = new();

			string page = await response.Content.ReadAsStringAsync();

			int start = page.IndexOf("class=\"user\"") + 13;
			int end = page[start..].IndexOf("<");
			summary.UserName = page[start..(start + end)];

			start = page.IndexOf("class=\"star-count\"") + 19;
			end = page[start..].IndexOf("*");
			_ = int.TryParse(page[start..(start + end)], out int noOfStars);
			summary.NoOfStars = noOfStars;

			for (int day = 1; day <= 25; day++) {
				end = 0;
				noOfStars = 0;
				start = page.IndexOf($"a aria-label=\"Day {day}, ") + 21;
				if (start > 0) {
					if (page[start..(start + 3)] == "one") {
						noOfStars = 1;
					} else if (page[start..(start + 3)] == "two") {
						noOfStars = 2;
					}
				}
				DailySummary dayInfo = new() {
					Day = day,
					NoOfStars = noOfStars
				};
				summary.Days.TryAdd(day, dayInfo);
			}

			return summary;
		}
	}
}
