namespace AdventOfCode.Services;

public interface IGithubHttpClient {
	List<string> KnownUsers { get; }

	string GetSolutionHref(int year, int day, string username);
	string UserLanguages(string username) => "";
}
