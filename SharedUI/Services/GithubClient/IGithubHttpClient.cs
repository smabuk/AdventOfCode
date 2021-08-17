namespace AdventOfCode.Services;

public interface IGithubHttpClient : IInputDataService {
	List<string> KnownUsers { get; }

	string GetSolutionHref(int year, int day, string username);
}
