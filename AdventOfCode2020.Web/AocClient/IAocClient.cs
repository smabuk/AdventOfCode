using System;
using System.Threading.Tasks;

namespace AdventOfCode2020.Web {
	public interface IAocClient {
		Task<string> GetInputData(int year, int day) => throw new NotImplementedException();
		Task<string> GetProblemDescription(int year, int day, int problemNo) => throw new NotImplementedException();
		Task<(string UserName, int NoOfStars)> GetSummaryInfo(int year) => throw new NotImplementedException();
	}
}