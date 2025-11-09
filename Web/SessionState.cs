using AdventOfCode.Services;

namespace AdventOfCode.Web;

public class SessionState
{
	public event Action? OnChange;
	private void NotifyStateChanged() => OnChange?.Invoke();

	public string UserName { get; set; } = "";
	public Dictionary<int, int> NoOfStars { get; set; } = [];
	public bool IsSummaryLoaded(int year) => NoOfStars.ContainsKey(year);
	public Dictionary<Tuple<int, int, int>, string> ProblemDescriptions { get; set; } = [];
	public Dictionary<Tuple<int, int>, (string InputData, string Source)> ProblemInputs { get; set; } = [];
	public Dictionary<Tuple<int, int>, string[]> ProblemRawInputs { get; set; } = [];

	public event Action? OnSummaryChange;
	private void NotifySummaryChanged() => OnSummaryChange?.Invoke();
	public Dictionary<int, AocSummary> Summaries { get; set; } = [];
	public void SetSummary(int year, AocSummary value)
	{
		if (!Summaries.TryAdd(year, value)) {
			Summaries[year] = value;
		}

		NotifySummaryChanged();
	}



	public bool DoesNOfStarsExist(int year) => NoOfStars.ContainsKey(year);
	public int GetNoOfStars(int year) => NoOfStars.TryGetValue(year, out int value) ? value : 0;
	//public int GetNoOfStars(int year, int day) => NoOfStars.TryGetValue(year, out int value) ? value : 0;
	public void SetNoOfStars(int year, int value)
	{
		if (!NoOfStars.TryAdd(year, value)) {
			NoOfStars[year] = value;
		}

		NotifyStateChanged();
	}

	public bool DoesProblemDescriptionExist(int year, int day, int problemNo) => ProblemDescriptions.ContainsKey(new(year, day, problemNo));
	public string GetProblemDescription(int year, int day, int problemNo) => ProblemDescriptions.ContainsKey(new(year, day, problemNo)) ? ProblemDescriptions[new(year, day, problemNo)] : "";
	public void SetProblemDescription(int year, int day, int problemNo, string value)
	{
		Tuple<int, int, int> key = new(year, day, problemNo);
		if (!ProblemDescriptions.TryAdd(key, value)) {
			ProblemDescriptions[key] = value;
		}

		NotifyStateChanged();
	}

	public event Action? OnProblemInputChange;
	private void NotifyProblemInputChanged() => OnProblemInputChange?.Invoke();
	public bool DoesProblemInputExist(int year, int day) => ProblemInputs.ContainsKey(new(year, day));
	public (string InputData, string Source) GetProblemInputAsString(int year, int day) => ProblemInputs.ContainsKey(new(year, day)) ? ProblemInputs[new(year, day)] : ("", "");
	public string[]? GetProblemInputAsArray(int year, int day) => ProblemInputs.ContainsKey(new(year, day)) ? ProblemRawInputs[new(year, day)] : null;
	public void SetProblemInput(int year, int day, string value, string? source = "")
	{
		if (string.IsNullOrWhiteSpace(value)) {
			return;
		}

		string[] rawValue = value.Split("\n");
		Tuple<int, int> key = new(year, day);
		source ??= "";
		if (ProblemInputs.ContainsKey(key)) {
			ProblemInputs[key] = (value, source);
			ProblemRawInputs[key] = rawValue;
		} else {
			ProblemInputs.Add(key, (value, source));
			ProblemRawInputs.Add(key, rawValue);
		}

		NotifyProblemInputChanged();
	}

}
