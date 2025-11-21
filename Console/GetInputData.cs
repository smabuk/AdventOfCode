using AdventOfCode.Services;

using Microsoft.Extensions.Caching.Memory;

internal partial class Program
{
	public static async Task<string[]?> GetInputData(int year, int day, bool isDownload)
	{
		string[]? input = null;

		string folder = ".";
		string filename = $"{year}_{day:D2}.txt";
		string fullFilename = Path.GetFullPath(Path.Combine(folder, filename));

		if (File.Exists(fullFilename)) {
		} else {
			folder = Environment.GetEnvironmentVariable("AocData") ?? "";
			if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder)) {
			} else {
				folder = Path.GetFullPath(Path.Combine("..", "Data"));
				if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder)) { }
			}
		}

		fullFilename = Path.GetFullPath(Path.Combine(folder, filename));
		if (File.Exists(fullFilename)) {
			input = File.ReadAllText(fullFilename).ReplaceLineEndings().Split(Environment.NewLine);
		} else if (isDownload) {
			string? cookie = Environment.GetEnvironmentVariable("AocSettings:HttpClientSettings:SessionCookie");
			if (string.IsNullOrWhiteSpace(cookie)) {
				Console.WriteLine($"** FAILED ** (environment AocSettings:HttpClientSettings:SessionCookie not set)");
				return null;
			}

			Console.WriteLine($"Downloading: {fullFilename} ...");

			HttpClient httpClient = new();
			httpClient.DefaultRequestHeaders.Add("Cookie", $"session={Environment.GetEnvironmentVariable("AocSettings:HttpClientSettings:SessionCookie")};");

			AocHttpClient aocHttpClient = new(httpClient, new MemoryCache(new MemoryCacheOptions()));
			string data = await aocHttpClient.GetInputData(year, day, "");

			if (string.IsNullOrWhiteSpace(data)) {
				return null;
			}

			if (!File.Exists(filename)) {
				await File.WriteAllTextAsync(fullFilename, data);
				input = data.ReplaceLineEndings().Split(Environment.NewLine);
				//Console.WriteLine(data);
			}

		}

		return input;
	}
}
