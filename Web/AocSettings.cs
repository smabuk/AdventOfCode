namespace AdventOfCode.Web;

public class AocSettings
{
	public string Site { get; init; } = "https://adventofcode.com/";
	public string LocalDataPath { get; init; } = ".";
	public HttpClientSettings HttpClientSettings { get; init; } = default!;
}

public class HttpClientSettings
{
	public string SessionCookie { get; init; } = "";
}
