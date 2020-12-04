using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

namespace AdventOfCode.Web {
	public class AocClient : IAocClient {
		private readonly HttpClient _httpClient;

		public AocClient(HttpClient httpClient, IOptions<AocClientSettings> aocSettings) {
			httpClient.BaseAddress = new Uri(aocSettings.Value.Site);
			httpClient.DefaultRequestHeaders.Add("Cookie", $"session={aocSettings.Value.Session};");
			_httpClient = httpClient;
		}

		public async Task<string> GetInputData(int year, int day) {
			var response = await _httpClient.GetAsync($"{year}/day/{day}/input");

			string value = response.IsSuccessStatusCode switch {
				false => "",
				_ => await response.Content.ReadAsStringAsync()
			};
			value = value[..^1] ?? "";
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

		public async Task<(string UserName, int NoOfStars)> GetSummaryInfo(int year) {
			var response = await _httpClient.GetAsync($"{year}");

			if (!response.IsSuccessStatusCode) {
				return ("", 0);
			}

			string page = await response.Content.ReadAsStringAsync();
			string userName = "";
			int start = page.IndexOf("class=\"user\"") + 13;
			int end = page[start..].IndexOf("<");
			userName = page[start..(start + end)];
			start = page.IndexOf("class=\"star-count\"") + 19;
			end = page[start..].IndexOf("*");
			_ = int.TryParse(page[start..(start + end)], out int noOfStars);

			return (userName, noOfStars);
		}
	}
}
