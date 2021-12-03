namespace AdventOfCode.Services;

public class GithubHttpClient : IGithubHttpClient {
	private readonly HttpClient _httpClient;

	public GithubHttpClient(HttpClient httpClient) {
		httpClient.BaseAddress = new Uri("https://raw.githubusercontent.com/");
		_httpClient = httpClient;
	}

	public List<string> KnownUsers => new() {
		"smabuk",
		"encse",
		"glombek",
		"pseale",
		"CopperBeardy",
		"Rollerss",
		"KevinSjoberg",
		"Andriamanitra",
		"Bassel-T"
	};

	public string UserLanguages(string username) => username switch {
			"smabuk" => "c#",
			"encse" => "c#",
			"glombek" => "c#",
			"pseale" => "c#",
			"CopperBeardy" => "c#",
			"Rollerss" => "c#",
			"KevinSjoberg" => "crystal",
			"Andriamanitra" => "multi",
			"Bassel-T" => "c#",
			_ => ""
	};

	public string GetSolutionHref(int year, int day, string username) {
		const string GITHUB = "https://github.com/";

		if (username is null) { return ""; }
		if (KnownUsers.Contains(username) == false) { return ""; }

		string href = username.ToLower() switch {
			"andriamanitra" => $"{GITHUB}Andriamanitra/adventofcode{year}/blob/main/day{day:D2}",
			"bassel-t" => $"{GITHUB}Bassel-T/AdventOfCode{year}-CS/blob/main/AdventOfCode{year}/Day{day}.cs",
			"copperbeardy" => $"{GITHUB}CopperBeardy/AdventOfCode{year}/blob/main/AdventOfCode{year}/AdventOfCode{year}/Days/Day{day}.cs",
			"encse" => $"{GITHUB}encse/adventofcode/blob/master/{year}/Day{day:D2}/Solution.cs",
			"glombek" => $"{GITHUB}glombek/advent-of-code-{year % 1000}/blob/main/Days/Day{day}.cs",
			//"ians-au" => $"{GITHUB}ians-au/AdventOfCode{year}/blob/main/day{day:D2}.cs",
			"kevinsjoberg" => $"{GITHUB}KevinSjoberg/aoc-{year}/blob/main/{day:D2}/day{day:D2}.cr",
			"pseale" => $"{GITHUB}pseale/advent-of-code/blob/main/{year}-csharp/Day{day:D2}/Program.cs",
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
			"glombek" => $"glombek/advent-of-code-{year % 1000}/main/Inputs/Day{day}/input.txt",
			//"ians-au" => $"",
			"kevinsjoberg" => $"KevinSjoberg/aoc-{year}/main/{day:D2}/input.txt",
			"rollerss" => $"Rollerss/AOC_{year}/master/AOC/InputData/AOCDay{day:D2}.txt",
			"pseale" => $"pseale/advent-of-code/main/{year}-csharp/Day{day:D2}/input.txt",
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
