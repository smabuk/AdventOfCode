using System;
using System.Threading.Tasks;

namespace AdventOfCode.Services {
	public interface IAocHttpClient : IInputDataService {
		Task<string> GetProblemDescription(int year, int day, int problemNo) => throw new NotImplementedException();
		Task<AocSummary?> GetSummaryInfo(int year) => throw new NotImplementedException();
	}
}