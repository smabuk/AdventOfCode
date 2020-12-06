using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode.Web {
	public interface IGithubHttpClient : IInputDataService {
		List<string> KnownUsers { get; }

		string GetSolutionHref(int year, int day, string username);
	}
}
