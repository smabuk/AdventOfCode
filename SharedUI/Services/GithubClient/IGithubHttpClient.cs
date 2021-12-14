namespace AdventOfCode.Services;

public interface IGithubHttpClient {
	List<string> KnownUsersInOrder { get; }

	string GetSolutionHref(int year, int day, string username);
	string UserLanguages(string username) => "";
}
