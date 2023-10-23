namespace AdventOfCode.Services;

public class AocSummary {
	public string UserName { get; set; } = null!;
	public int NoOfStars { get; set; }
	public Dictionary<int, DailySummary> Days { get; set; } = [];
}

public class DailySummary {
	public int Day { get; set; }
	public int NoOfStars { get; set; }
	public string PictureLine { get; set; } = null!;
}
