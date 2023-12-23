namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 20: Pulse Propagation
/// https://adventofcode.com/2023/day/20
/// </summary>
[Description("Pulse Propagation")]
public sealed partial class Day20 {

	public static string Part1(string[] input, params object[]? args)
	{
		int noOfButtonPushes = GetArgument(args, argumentNumber: 1, defaultResult: 1000);
		return Solution1(input, noOfButtonPushes).ToString();
	}

	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	public const bool LOW_PULSE  = false;
	public const bool HIGH_PULSE = true;
	public const bool OFF = false;
	public const bool ON  = true;


	private static long Solution1(string[] input, int noOfButtonPushes) {
		Dictionary<string, Module> modules = input.As<Module>().ToDictionary(m => m.Name);
		Queue<Pulse> queue = [];

		List<string> existingModules = [.. modules.Keys];
		foreach (string moduleName in existingModules) {
			Module module = modules[moduleName];
			foreach (string name in module.Outputs) {
				if (existingModules.DoesNotContain(name)) {
					modules.Add(name, new Output(name));
				}
				modules[name].Inputs.Add(module.Name, LOW_PULSE);
			}
		}

		int noOfLowPulses  = 0;
		int noOfHighPulses = 0;
		for (int i = 0; i < noOfButtonPushes; i++) {
			Button.Push(queue);
			(int lowPulses, int highPulses) = ProcessQueue(queue, modules);
			noOfLowPulses  += lowPulses;
			noOfHighPulses += highPulses;
		}

		return noOfLowPulses * noOfHighPulses;
	}

	private static string Solution2(string[] input)
	{
		return "** Solution not written yet **";
	}

	private static (int lowPulses, int highPulses) ProcessQueue(Queue<Pulse> queue, Dictionary<string, Module> modules)
	{
		int lowPulses  = 0;
		int highPulses = 0;

		while (queue.Count > 0) {
			Pulse pulse = queue.Dequeue();

			if (pulse.Payload == LOW_PULSE) {
				lowPulses++;
			} else {
				highPulses++;
			}

			foreach (Pulse newPulse in modules[pulse.Destination].Receive(pulse)) {
				queue.Enqueue(newPulse);
			}
		}

		return (lowPulses, highPulses);
	}

	private record Module(string Name, List<string> Outputs) : IParsable<Module> {

		public Dictionary<string, bool> Inputs { get; set; } = [];
		
		public virtual IEnumerable<Pulse> Receive(Pulse pulse) { yield break; }

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

	private sealed record Button(string Name, List<string> Outputs) : Module(Name, Outputs)
	{
		public static void Push(Queue<Pulse> queue) => queue.Enqueue(new Pulse(LOW_PULSE, "button", "broadcaster")); 
	}

	private sealed record Broadcaster(string Name, List<string> Outputs) : Module(Name, Outputs)
	{
		public override IEnumerable<Pulse> Receive(Pulse pulse)
		{
			foreach (string name in Outputs) {
				yield return new Pulse(pulse.Payload, Name, name);
			}
		}
	}

	private sealed record ConjunctionModule(string Name, List<string> Outputs) : Module(Name, Outputs)
	{
		public override IEnumerable<Pulse> Receive(Pulse pulse)
		{
			Inputs[pulse.Source] = pulse.Payload;
			bool pulseType = !Inputs.Values.All(v => v == HIGH_PULSE);

			foreach (string name in Outputs) {
				yield return new Pulse(pulseType, Name, name);
			}
		}
	}

	private sealed record FlipFlopModule(string Name, List<string> Outputs) : Module(Name, Outputs)
	{
		public bool Status { get; set; } = OFF;
		public override IEnumerable<Pulse> Receive(Pulse pulse)
		{
			if (pulse.Payload == HIGH_PULSE) {
				yield break;
			}

			Status = !Status;

			foreach (string name in Outputs) {
				yield return new Pulse(Status == ON, Name, name);
			}
		}
	}

	private sealed record Output(string Name) : Module(Name, []);

	private sealed record Pulse(bool Payload, string Source, string Destination);

	private enum ModuleType
	{
		Broadcast   = 'b', 
		Conjunction = '&',
		FlipFlop    = '%',
		Output      = 'o',
	}
}
