using AdventOfCode.Services;

namespace AdventOfCode.Web;

/// <summary>
/// Represents a unique identifier for an Advent of Code problem.
/// </summary>
/// <param name="Year">The year of the problem.</param>
/// <param name="Day">The day of the problem (1-25).</param>
/// <param name="ProblemNo">The problem number (typically 1 or 2).</param>
public sealed record ProblemKey(int Year, int Day, int ProblemNo);

/// <summary>
/// Represents a unique identifier for an Advent of Code day.
/// </summary>
/// <param name="Year">The year of the problem.</param>
/// <param name="Day">The day of the problem (1-25).</param>
public sealed record DayKey(int Year, int Day);

/// <summary>
/// Manages session state for the Advent of Code application, including user data,
/// problem descriptions, inputs, and summaries. Provides thread-safe access to state
/// with event notifications for state changes.
/// </summary>
public class SessionState
{
	// Only needed for writes - dictionary reads are thread-safe when no concurrent writes occur
	private readonly Lock _lock = new();

	// Events
	/// <summary>
	/// Raised when general state changes occur (e.g., problem descriptions or star counts).
	/// </summary>
	public event Action? OnChange;

	/// <summary>
	/// Raised when summary data is updated.
	/// </summary>
	public event Action? OnSummaryChange;

	/// <summary>
	/// Raised when problem input data is updated.
	/// </summary>
	public event Action? OnProblemInputChange;

	/// <summary>
	/// Gets or sets the current user's name.
	/// </summary>
	public string UserName { get; set; } = "";

	// State collections - init-only to prevent reassignment
	/// <summary>
	/// Gets the number of stars earned per year.
	/// </summary>
	public Dictionary<int, int> NoOfStars { get; init; } = [];

	/// <summary>
	/// Gets the summary data for each year.
	/// </summary>
	public Dictionary<int, AocSummary> Summaries { get; init; } = [];

	/// <summary>
	/// Gets the problem descriptions keyed by year, day, and problem number.
	/// </summary>
	public Dictionary<ProblemKey, string> ProblemDescriptions { get; init; } = [];

	/// <summary>
	/// Gets the problem input data and its source keyed by year and day.
	/// </summary>
	public Dictionary<DayKey, (string InputData, string Source)> ProblemInputs { get; init; } = [];

	/// <summary>
	/// Gets the raw problem input data as string arrays keyed by year and day.
	/// </summary>
	public Dictionary<DayKey, string[]> ProblemRawInputs { get; init; } = [];

	/// <summary>
	/// Determines whether summary data has been loaded for the specified year.
	/// </summary>
	/// <param name="year">The year to check.</param>
	/// <returns><c>true</c> if summary data exists for the year; otherwise, <c>false</c>.</returns>
	public bool IsSummaryLoaded(int year) => NoOfStars.ContainsKey(year);

	/// <summary>
	/// Determines whether star count data exists for the specified year.
	/// </summary>
	/// <param name="year">The year to check.</param>
	/// <returns><c>true</c> if star count data exists for the year; otherwise, <c>false</c>.</returns>
	public bool DoesNOfStarsExist(int year) => NoOfStars.ContainsKey(year);

	/// <summary>
	/// Gets the number of stars earned for the specified year.
	/// </summary>
	/// <param name="year">The year to query.</param>
	/// <returns>The number of stars earned, or 0 if no data exists.</returns>
	public int GetNoOfStars(int year) => NoOfStars.TryGetValue(year, out int value) ? value : 0;

	/// <summary>
	/// Determines whether a problem description exists for the specified problem.
	/// </summary>
	/// <param name="year">The year of the problem.</param>
	/// <param name="day">The day of the problem.</param>
	/// <param name="problemNo">The problem number.</param>
	/// <returns><c>true</c> if the problem description exists; otherwise, <c>false</c>.</returns>
	public bool DoesProblemDescriptionExist(int year, int day, int problemNo)
		=> ProblemDescriptions.ContainsKey(new ProblemKey(year, day, problemNo));

	/// <summary>
	/// Gets the problem description for the specified problem.
	/// </summary>
	/// <param name="year">The year of the problem.</param>
	/// <param name="day">The day of the problem.</param>
	/// <param name="problemNo">The problem number.</param>
	/// <returns>The problem description, or an empty string if not found.</returns>
	public string GetProblemDescription(int year, int day, int problemNo)
		=> ProblemDescriptions.TryGetValue(new ProblemKey(year, day, problemNo), out string? value) ? value : "";

	/// <summary>
	/// Determines whether problem input data exists for the specified day.
	/// </summary>
	/// <param name="year">The year of the problem.</param>
	/// <param name="day">The day of the problem.</param>
	/// <returns><c>true</c> if problem input data exists; otherwise, <c>false</c>.</returns>
	public bool DoesProblemInputExist(int year, int day)
		=> ProblemInputs.ContainsKey(new DayKey(year, day));

	/// <summary>
	/// Gets the problem input data as a string along with its source.
	/// </summary>
	/// <param name="year">The year of the problem.</param>
	/// <param name="day">The day of the problem.</param>
	/// <returns>A tuple containing the input data and source, or empty strings if not found.</returns>
	public (string InputData, string Source) GetProblemInputAsString(int year, int day)
		=> ProblemInputs.TryGetValue(new DayKey(year, day), out (string, string) value) ? value : ("", "");

	/// <summary>
	/// Gets the problem input data as an array of strings.
	/// </summary>
	/// <param name="year">The year of the problem.</param>
	/// <param name="day">The day of the problem.</param>
	/// <returns>An array of strings representing the input data, or <c>null</c> if not found.</returns>
	public string[]? GetProblemInputAsArray(int year, int day)
		=> ProblemRawInputs.TryGetValue(new DayKey(year, day), out string[]? value) ? value : null;

	/// <summary>
	/// Sets the summary data for the specified year and notifies subscribers of the change.
	/// </summary>
	/// <param name="year">The year for which to set the summary.</param>
	/// <param name="value">The summary data to set.</param>
	public void SetSummary(int year, AocSummary value)
	{
		lock (_lock) {
			Summaries[year] = value;
			NotifySummaryChanged();
		}
	}

	/// <summary>
	/// Sets the number of stars earned for the specified year and notifies subscribers of the change.
	/// </summary>
	/// <param name="year">The year for which to set the star count.</param>
	/// <param name="value">The number of stars earned.</param>
	public void SetNoOfStars(int year, int value)
	{
		lock (_lock) {
			NoOfStars[year] = value;
			NotifyStateChanged();
		}
	}

	/// <summary>
	/// Sets the problem description for the specified problem and notifies subscribers of the change.
	/// </summary>
	/// <param name="year">The year of the problem.</param>
	/// <param name="day">The day of the problem.</param>
	/// <param name="problemNo">The problem number.</param>
	/// <param name="value">The problem description text.</param>
	public void SetProblemDescription(int year, int day, int problemNo, string value)
	{
		lock (_lock) {
			ProblemKey key = new(year, day, problemNo);
			ProblemDescriptions[key] = value;
			NotifyStateChanged();
		}
	}

	/// <summary>
	/// Sets the problem input data for the specified day and notifies subscribers of the change.
	/// Ignores null, empty, or whitespace-only input.
	/// </summary>
	/// <param name="year">The year of the problem.</param>
	/// <param name="day">The day of the problem.</param>
	/// <param name="value">The input data as a string.</param>
	/// <param name="source">Optional source identifier for the input data.</param>
	public void SetProblemInput(int year, int day, string value, string? source = "")
	{
		if (string.IsNullOrWhiteSpace(value)) {
			return;
		}

		string[] rawValue = value.Split("\n");
		DayKey key = new(year, day);
		source ??= "";

		lock (_lock) {
			ProblemInputs[key] = (value, source);
			ProblemRawInputs[key] = rawValue;
			NotifyProblemInputChanged();
		}
	}

	// Event invocation - must be called within lock to ensure atomicity
	/// <summary>
	/// Notifies subscribers that general state has changed.
	/// </summary>
	private void NotifyStateChanged() => OnChange?.Invoke();

	/// <summary>
	/// Notifies subscribers that summary data has changed.
	/// </summary>
	private void NotifySummaryChanged() => OnSummaryChange?.Invoke();

	/// <summary>
	/// Notifies subscribers that problem input data has changed.
	/// </summary>
	private void NotifyProblemInputChanged() => OnProblemInputChange?.Invoke();
}
