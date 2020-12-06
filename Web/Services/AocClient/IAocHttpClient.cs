using System;
using System.Threading.Tasks;

namespace AdventOfCode.Web {
	public interface IAocHttpClient : IInputDataService {
		Task<string> GetProblemDescription(int year, int day, int problemNo) => throw new NotImplementedException();
		Task<(string UserName, int NoOfStars)> GetSummaryInfo(int year) => throw new NotImplementedException();
	}
}