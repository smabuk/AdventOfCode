namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 24: Crossed Wires
/// https://adventofcode.com/2024/day/244
/// </summary>
[Description("Crossed Wires")]
public partial class Day24
{

	private static List<Wire> _wires = [];
	private static List<Gate> _gates = [];

	[Init]
	public static void LoadMonitoringDevice(string[] input)
	{
		_wires = [.. input.TakeWhile(line => line.HasNonWhiteSpaceContent()).As<Wire>()];
		_gates = [.. input.Skip(_wires.Count + 1).As<Gate>()];
	}

	public static long Part1()
	{
		Dictionary<string, Wire> wireValues = _wires.ToDictionary(w => w.Id, w => w);

		List<Gate> zGates = [.. _gates.Where(gate => gate.OutputWireId.StartsWith('z')).OrderByDescending(gate => gate.OutputWireId)];
		foreach (Gate zGate in zGates) {
			_ = ResolveWire(zGate.OutputWireId, wireValues) ?? throw new ApplicationException("This should always have a value.");
		}

		string zBinary = string.Concat(zGates.Select(gate => wireValues.TryGetValue(gate.OutputWireId, out Wire? wire) && (wire.Value ?? false) ? '1' : '0'));
		return zBinary.FromBinaryAs<long>();
	}

	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;


	static Wire? ResolveWire(string wireId, Dictionary<string, Wire> wireValues)
	{
		if (wireValues.TryGetValue(wireId, out Wire? knownWire)) {
			return knownWire;
		}

		Gate gate = _gates.Single(g => g.OutputWireId == wireId);

		gate = gate with
		{
			Wires = [
				gate.Wires[0] with { Value = ResolveWire(gate.Wires[0].Id, wireValues)!.Value},
				gate.Wires[1] with { Value = ResolveWire(gate.Wires[1].Id, wireValues)!.Value},
				]
		};

		wireValues[wireId] = gate.OutputWire;
		return gate.OutputWire;
	}

	private record Wire(string Id, bool? Value) : IParsable<Wire>
	{
		public static Wire Parse(string s, IFormatProvider? provider)
		{
			string[] parts = s.Split(": ");
			return new Wire(parts[0], parts[1] == "1" ? true : parts[1] == "0" ? false : null);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Wire result) => ISimpleParsable<Wire>.TryParse(s, provider, out result);
	}

	private abstract record Gate(List<Wire> Wires, string OutputWireId) : IParsable<Gate>
	{
		public bool HasWireValues => Wires.All(wire => wire.Value.HasValue);
		public Wire OutputWire => new(OutputWireId, Evaluate());

		public abstract bool? Evaluate();

		public static Gate Parse(string s, IFormatProvider? provider)
		{
			string[] parts = s.Split(" -> ");
			string[] exprParts = parts[0].Split(' ');
			return exprParts[1] switch
			{
				"AND" => new AndGate(
					[new Wire(exprParts[0], _wires.FirstOrDefault(w => w.Id == exprParts[0])?.Value),
						 new Wire(exprParts[2], _wires.FirstOrDefault(w => w.Id == exprParts[2])?.Value)],
					parts[1]),
				"OR" => new OrGate(
					[new Wire(exprParts[0], _wires.FirstOrDefault(w => w.Id == exprParts[0])?.Value),
						 new Wire(exprParts[2], _wires.FirstOrDefault(w => w.Id == exprParts[2])?.Value)],
					parts[1]),
				"XOR" => new XorGate(
					[new Wire(exprParts[0], _wires.FirstOrDefault(w => w.Id == exprParts[0])?.Value),
						 new Wire(exprParts[2], _wires.FirstOrDefault(w => w.Id == exprParts[2])?.Value)],
					parts[1]),
				_ => throw new InvalidOperationException("Unknown gate type.")
			};
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Gate result) => ISimpleParsable<Gate>.TryParse(s, provider, out result);
	}

	private record AndGate(List<Wire> Wires, string OutputWireId) : Gate(Wires, OutputWireId)
	{
		public override bool? Evaluate() => HasWireValues ? Wires[0].Value!.Value && Wires[1].Value!.Value : null;
	}

	private record OrGate(List<Wire> Wires, string OutputWireId) : Gate(Wires, OutputWireId)
	{
		public override bool? Evaluate() => HasWireValues ? Wires[0].Value!.Value || Wires[1].Value!.Value : null;
	}

	private record XorGate(List<Wire> Wires, string OutputWireId) : Gate(Wires, OutputWireId)
	{
		public override bool? Evaluate() => HasWireValues ? Wires[0].Value!.Value ^ Wires[1].Value!.Value : null;
	}
}
