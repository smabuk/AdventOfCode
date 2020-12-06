using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode.Services {
	public interface IGithubHttpClient : IInputDataService {
		List<string> KnownUsers { get; }

		string GetSolutionHref(int year, int day, string username);
	}
}
