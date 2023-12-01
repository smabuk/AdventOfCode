namespace AdventOfCode.Services;

public class GithubHttpClient : IGithubHttpClient, IInputDataService {
	private readonly HttpClient _httpClient;

	record GithubProfile(string Name, string UserLanguages, string SolutionTemplate, string InputTemplate);

	private static readonly List<GithubProfile> Users = [
		new("andriamanitra", "multi", "{GITHUB}Andriamanitra/adventofcode{year}/blob/main/day{day:D2} ", "Andriamanitra/adventofcode{year}/main/day{day:D2}/input.txt "),
		new("bassel-t", "c#", "{GITHUB}Bassel-T/AdventOfCode{year}-CS/blob/main/AdventOfCode{year}/Day{day}.cs ", " "),
		new("copperbeardy", "c#", "{GITHUB}CopperBeardy/AdventOfCode{year}/blob/main/AdventOfCode{year}/AdventOfCode{year}/Days/Day{day}.cs ", "CopperBeardy/AdventOfCode{year}/main/AdventOfCode{year}/AdventOfCode{year}/DayInputs/Day{day}.txt"),
		new("dylanbeattie", "c#", "{GITHUB}dylanbeattie/advent-of-code-{year}/blob/main/day{day}/Program.cs ", "dylanbeattie/advent-of-code-{year}/main/day{day}/input.txt "),
		new("encse", "c#", "{GITHUB}encse/adventofcode/blob/master/{year}/Day{day:D2}/Solution.cs ", "encse/adventofcode/master/{year}/Day{day:D2}/input.in "),
		new("glombek", "c#", "{GITHUB}glombek/advent-of-code-{year % 1000}/blob/main/Days/Day{day}.cs ", "glombek/advent-of-code-{year % 1000}/main/Inputs/Day{day}/input.txt "),
		new("internetbird", "c#", "{GITHUB}internetbird/AOC{year}/blob/master/PuzzleSolvers/Day{day}PuzzleSolver.cs ", "internetbird/AOC{year}/master/InputFiles/day{day}.txt "),
		new("kevinsjoberg", "crystal", "{GITHUB}KevinSjoberg/aoc-{year}/blob/main/{day:D2}/day{day:D2}.cr ", "KevinSjoberg/aoc-{year}/main/{day:D2}/input.txt "),
		new("pseale", "c#", "{GITHUB}pseale/advent-of-code/blob/main/{year}-csharp/Day{day:D2}/Program.cs ", "Rollerss/AOC_{year}/master/AOC/InputData/AOCDay{day:D2}.txt "),
		new("rollerss", "c#", "{GITHUB}Rollerss/AOC_{year}/blob/master/AOC/Day{day:D2}.cs ", "pseale/advent-of-code/main/{year}-csharp/Day{day:D2}/input.txt "),
		new("smabuk", "c#", "{GITHUB}smabuk/AdventOfCode/tree/main/Solutions/{year}/Day{day:D2}.cs ", "smabuk/AdventOfCode/main/Data/{year}_{day:D2}.txt "),
		new("tasagent", "c#", "{GITHUB}TasAgent/AdventOfCode{year}/blob/master/Day{day:D2}/Program.cs ", "TASagent/AdventOfCode{year}/master/input{day:D2}.txt "),
		new("unclescientist", "rust", "{GITHUB}UncleScientist/aoclib-rs/blob/main/crates/aoc{year}/src/aoc{year}_{day:D2}.rs ", "UncleScientist/aoclib-rs/main/crates/aoc{year}/input/{year}-{day:D2}.txt "),
	];

	public GithubHttpClient(HttpClient httpClient) {
		httpClient.BaseAddress = new Uri("https://raw.githubusercontent.com/");
		_httpClient = httpClient;
	}

	public List<string> KnownUsersInOrder => [
		"smabuk",
		"encse",
		"UncleScientist",
		"glombek",
		"dylanbeattie",
		"internetbird",
		"pseale",
		"TASagent",
		"CopperBeardy",
		"Rollerss",
		"KevinSjoberg",
		"Andriamanitra",
		"Bassel-T"
	];

	public string UserLanguages(string username) => 
		Users.Where(user => user.Name == username.ToLower()).Single().UserLanguages;

	public string GetSolutionHref(int year, int day, string username) {
		if (String.IsNullOrWhiteSpace(username)) { return ""; }

		string href = Users.Where(user => user.Name == username.ToLower()).Single().SolutionTemplate;
		return ParseTemplate(href, year, day);
	}

	public async Task<string> GetInputData(int year, int day, string? username) {
		if (username is null) { return ""; }
		if (KnownUsersInOrder.Contains(username) == false) { return ""; }

		string inputTemplate = Users.Where(user => user.Name == username.ToLower()).Single().InputTemplate;
		string path = ParseTemplate(inputTemplate, year, day);

		if (string.IsNullOrEmpty(path)) {
			return "";
		}
		HttpResponseMessage response = await _httpClient.GetAsync(path);

		return response.IsSuccessStatusCode switch {
			false => "",
			_ => await response.Content.ReadAsStringAsync()
		};
	}

	private static string ParseTemplate(string template, int year, int day) {
		return template
			.Replace("{GITHUB}", "https://github.com/")
			.Replace("{year}", $"{year}")
			.Replace("{year % 1000}", $"{year % 1000}")
			.Replace("{day:D2}", $"{day:D2}")
			.Replace("{day}", $"{day}");
	}

}
