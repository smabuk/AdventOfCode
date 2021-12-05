namespace AdventOfCode.Services;

public interface IAocHttpClient {
	Task<string> GetProblemDescription(int year, int day, int problemNo) => throw new NotImplementedException();
	Task<AocSummary?> GetSummaryInfo(int year) => throw new NotImplementedException();
}
