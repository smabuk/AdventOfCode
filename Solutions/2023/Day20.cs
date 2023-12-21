namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 20: Pulse Propagation
/// https://adventofcode.com/2023/day/20
/// </summary>
[Description("Pulse Propagation")]
public sealed partial class Day20 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadModules(input);
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static IEnumerable<Module> _modules = [];

	private static void LoadModules(string[] input) {
		_modules = input.As<Module>();
	}

	private static string Solution1(string[] input) {
		List<Module> modules = [.. input.As<Module>()];
		return "** Solution not written yet **";
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

	private record Module(string Name, ModuleType ModuleType, List<string> Outputs) : IParsable<Module> {
		public static Module Parse(string s, IFormatProvider? provider)
		{
			ModuleType moduleType = (ModuleType)s[0];
			string[] splitBy = ["->", ","];
			string[] tokens = s.TrimmedSplit(splitBy);
			return moduleType switch
			{

				ModuleType.Broadcast   => new Broadcaster(tokens[0], [.. tokens[1..]]),
				ModuleType.Conjunction => new ConjunctionModule(tokens[0][1..], [.. tokens[1..]]),
				ModuleType.FlipFlop    => new FlipFlopModule(tokens[0][1..], [.. tokens[1..]]),
				_ => throw new NotImplementedException(),
			};
		}

		public static Module Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Module result)
			=> ISimpleParsable<Module>.TryParse(s, provider, out result);
	}

	private sealed record Broadcaster(string Name, List<string> Outputs) : Module(Name, ModuleType.Broadcast, Outputs);
	private sealed record ConjunctionModule(string Name, List<string> Outputs) : Module(Name, ModuleType.Conjunction, Outputs);
	private sealed record FlipFlopModule(string Name, List<string> Outputs) : Module(Name, ModuleType.FlipFlop, Outputs);
	private sealed record Output(string Name) : Module(Name, ModuleType.Output, []);

	private enum ModuleType
	{
		Broadcast   = 'b', 
		Conjunction = '&',
		FlipFlop    = '%',
		Output      = 'o',
	}
}
