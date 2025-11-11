namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day XX: Title
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
		_wires = [.. input
			.TakeWhile(line => line.HasNonWhiteSpaceContent())
			.Select(line =>
			{
				string[] parts = line.Split(": ");
				return new Wire(parts[0], parts[1] == "1" ? true : parts[1] == "0" ? false : null);
			})];

		_gates = [.. input
			.SkipWhile(line => line.HasNonWhiteSpaceContent())
			.Skip(1)
			.TakeWhile(line => line.HasNonWhiteSpaceContent())
			.Select<string, Gate>(line =>
			{
				string[] parts = line.Split(" -> ");
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
			})];

	}

	public static long Part1(string[] input)
	{
		Dictionary<string, Wire> wireValues = _wires.ToDictionary(w => w.Id, w => w);

		List<Gate> zGates = [.. _gates.Where(gate => gate.OutputWireId.StartsWith('z')).OrderByDescending(gate => gate.OutputWireId)];
		foreach (Gate zGate in zGates) {
			_ = ResolveWire(zGate.OutputWireId, wireValues) ?? throw new ApplicationException("This should always have a value.");
		}

		string zBinary = string.Concat(zGates.Select(gate => wireValues.TryGetValue(gate.OutputWireId, out Wire? wire) && (wire.Value ?? false) ? '1' : '0'));
		return zBinary.FromBinaryAs<long>();
	}

	public static string Part2(string[] input) => NO_SOLUTION_WRITTEN_MESSAGE;


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

	private record Wire(string Id, bool? Value);

	private abstract record Gate(List<Wire> Wires, string OutputWireId)
	{
		public Wire OutputWire => new(OutputWireId, Evaluate());
		public abstract bool? Evaluate();
	}

	private record AndGate(List<Wire> Wires, string OutputWireId) : Gate(Wires, OutputWireId)
	{
		public override bool? Evaluate() => Wires[0].Value.HasValue && Wires[1].Value.HasValue
			? Wires[0].Value!.Value && Wires[1].Value!.Value
			: null;
	}

	private record OrGate(List<Wire> Wires, string OutputWireId) : Gate(Wires, OutputWireId)
	{
		public override bool? Evaluate() => Wires[0].Value.HasValue && Wires[1].Value.HasValue
			? Wires[0].Value!.Value || Wires[1].Value!.Value
			: null;
	}

	private record XorGate(List<Wire> Wires, string OutputWireId) : Gate(Wires, OutputWireId)
	{
		public override bool? Evaluate() => Wires[0].Value.HasValue && Wires[1].Value.HasValue
			? Wires[0].Value!.Value ^ Wires[1].Value!.Value
			: null;
	}
}
