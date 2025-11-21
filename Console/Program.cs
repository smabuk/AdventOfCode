internal partial class Program
{
	static async Task<int> Main(string[] args)
	{
		CommandApp<SolveCommand> app = new();
		app.Configure(config => _ = config
			.SetApplicationName("AdventOfCode.Console")
			.SetApplicationVersion("1.0")
			.ValidateExamples());

		return await app.RunAsync(args);
	}
}
