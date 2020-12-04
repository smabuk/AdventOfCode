using System;
using System.Threading.Tasks;

namespace AdventOfCode.Web {
	public interface IGithubClient {
		Task<string> GetInputData(string username, int year, int day) => throw new NotImplementedException();
	}
}
