namespace AdventOfCode2020.Web {
	public record AocClientSettings {
		public string Site { get; init; } = "https://adventofcode.com/";
		public string Session { get; init; } = "";
	}
}
