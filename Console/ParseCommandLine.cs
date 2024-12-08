internal partial class Program
{

	public static (DateOnly date, object[]? solutionArgs, bool showVisuals, bool isDebug, bool isDownload, TimeSpan visualsTime) ParseCommandLine(string[] args)
	{
		DateOnly date = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-5));
		bool showVisuals = false;
		TimeSpan visualsTime = new(0, 0, 0, 0, 300);
		bool isDebug = false;
		bool isDownload = false;
		object[]? solutionArgs = [];
		for (int i = 0; i < args.Length; i++) {
			if (args[i] is "--help" or "/?" or "-h") {
				ShowHelp();
				Environment.Exit(0);
			}
		}
		if (args.Length > 2) {
			for (int i = 2; i < args.Length; i++) {
				string arg = args[i].ToLowerInvariant();
				if (arg is "true" or "false") {
					solutionArgs = new object[solutionArgs.Length + 1];
					solutionArgs[^1] = args[i] == "true";
				} else if (arg is "/v" or "/visual" or "-v" or "--visual") {
					showVisuals = true;
				} else if (arg.StartsWith("/v:") || arg.StartsWith("/visual:") || arg.StartsWith("-v:") || arg.StartsWith("--visual:")) {
					showVisuals = true;
					string token = arg[(arg.IndexOf(':') + 1)..];
					visualsTime = TimeSpan.Parse(token);
				} else if (arg is "/download" or "--download") {
					isDownload = true;
				} else if (arg is "/d" or "/debug" or "-d" or "--debug") {
					isDebug = true;
				} else {
					solutionArgs = new object[solutionArgs.Length + 1];
					solutionArgs[^1] = args[i];
				}
			}
		}
		if (args.Length >= 2) {
			if (int.TryParse(args[0], out int year) && int.TryParse(args[1], out int day)) {
				date = new(year, 12, day);
			}
		}
		if (args.Length == 1) {
			if (int.TryParse(args[0], out int year)) {
				date = new(year, 1, 1);
			}
		}
		return (date, solutionArgs, showVisuals, isDebug, isDownload, visualsTime);
	}

}
