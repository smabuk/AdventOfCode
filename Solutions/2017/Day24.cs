using static AdventOfCode.Solutions._2017.Day24Constants;
using static AdventOfCode.Solutions._2017.Day24Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 24: Electromagnetic Moat
/// https://adventofcode.com/2016/day/24
/// </summary>
[Description("Electromagnetic Moat")]
public sealed partial class Day24 {

	[Init]
	public static   void  Init(string[] input) => LoadBridges(input);
	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static List<MagneticComponent> _components = [];
	private static List<Bridge> _bridges = [];

	private static void LoadBridges(string[] input) {
		_components = [.. input.As<MagneticComponent>()];
		_bridges = [
			.. _components
			.Where(mc => mc.PortType1 == 0)
			.SelectMany(mc => new Bridge([mc], mc.PortType2).BuildBridges([.. _components.Except([mc])]))
			];
	}

	private static int Solution1() => _bridges.Max(b => b.Size);

	private static int Solution2() {
		return _bridges
			.OrderByDescending(b => b.Length)
			.ThenByDescending(b => b.Size)
			.First()
			.Size;
	}
}

file static class Day24Extensions
{
	public static List<Bridge> BuildBridges(this Bridge bridge, List<MagneticComponent> components)
	{
		List<MagneticComponent> components1 = [.. components.Where(c => c.PortType1 == bridge.End)];
		List<MagneticComponent> components2 = [.. components.Where(c => c.PortType2 == bridge.End && c.PortType1 != c.PortType2)];

		return components1 switch
		{
			[] when components2 is [] => [bridge],
			_ => [
				..components1
					.SelectMany(comp => (bridge with { MagneticComponents = [.. bridge.MagneticComponents, comp], End = comp.PortType2 })
					.BuildBridges([..components.Except([comp])])),
				..components2
					.SelectMany(comp => (bridge with { MagneticComponents = [.. bridge.MagneticComponents, comp], End = comp.PortType1 })
					.BuildBridges([..components.Except([comp])])),
				]
		};
	} 
}

internal sealed partial class Day24Types
{

	public sealed record MagneticComponent(string Name, int PortType1, int PortType2) : IParsable<MagneticComponent>
	{
		public int Size => PortType1 + PortType2;

		public static MagneticComponent Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit('/');
			return new (s, tokens[0].As<int>(), tokens[1].As<int>());
		}

		public static MagneticComponent Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out MagneticComponent result)
			=> ISimpleParsable<MagneticComponent>.TryParse(s, provider, out result);
	}

	public record Bridge(List<MagneticComponent> MagneticComponents, int End)
	{
		public int Size => MagneticComponents.Sum(mc => mc.Size);
		public int Length => MagneticComponents.Count;
	};
}

file static class Day24Constants
{
}
