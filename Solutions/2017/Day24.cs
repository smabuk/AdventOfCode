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
	public static   void  Init(string[] input, params object[]? args) => LoadComponents(input);
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static IEnumerable<MagneticComponent> _components = [];

	private static void LoadComponents(string[] input) {
		_components = input.As<MagneticComponent>();
	}

	private static int Solution1(string[] input) {
		//List<MagneticComponent> components = [.. input.As<MagneticComponent>()];
		return _components
			.Where(c => c.PortType1 == 0)
			.Select(c => new Bridge([c], c.PortType2).BridgeSize([.._components]))
			.Max();
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day24Extensions
{
	public static int BridgeSize(this Bridge bridge, List<MagneticComponent> components)
	{
		List<MagneticComponent> subcomponents = [..components.Except(bridge.MagneticComponents)];
		List<MagneticComponent> comp1 = [.. subcomponents.Where(c => c.PortType1 == bridge.End)];
		List<MagneticComponent> comp2 = [.. subcomponents.Where(c => c.PortType2 == bridge.End)];

		if (comp1 is [] && comp2 is []) {
			return bridge.Size;
		}

		int max = int.MinValue;
		foreach (MagneticComponent comp in comp1) {
			max = int.Max(max, (bridge with { MagneticComponents = [.. bridge.MagneticComponents, comp], End = comp.PortType2 }).BridgeSize(subcomponents));
		}

		foreach (MagneticComponent comp in comp2) {
			max = int.Max(
				max,
				(bridge with {
					MagneticComponents = [.. bridge.MagneticComponents, comp],
					End = comp.PortType1 }).BridgeSize(subcomponents));
		}

		return max;
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
	};
}

file static class Day24Constants
{
}
