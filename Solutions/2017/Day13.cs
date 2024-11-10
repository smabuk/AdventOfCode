using static AdventOfCode.Solutions._2017.Day13Constants;
using static AdventOfCode.Solutions._2017.Day13Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 13: Packet Scanners
/// https://adventofcode.com/2016/day/13
/// </summary>
[Description("Packet Scanners")]
public sealed partial class Day13 {

	[Init]
	public static   void  Init(string[] input) => LoadInstructions(input);
	public static string Part1(string[] _, Action<string[], bool>? visualise = null)
		=> Solution1(visualise).ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static Dictionary<int, Scanner> _scanners = [];

	private static void LoadInstructions(string[] input) => 
		_scanners = input.As<Scanner>().ToDictionary(s => s.Depth);

	private static int Solution1(Action<string[], bool>? visualise = null) {
		int maxDepth = _scanners.Max(s => s.Key);
		_scanners.VisualiseScanners(-1, 0, "Initial", visualise);
		int sum = 0;
		for (int tick = 0; tick <= maxDepth; tick++) {
			_scanners.VisualiseScanners(tick, tick, $"Picosecond {tick}:", visualise);
			Scanner? scanner = _scanners.GetValueOrDefault(tick);
			if (scanner is not null && scanner.IsCaptured(0, tick)) {
				sum += scanner.Depth * scanner.Range;
			}
		}

		return sum;
	}

	private static int Solution2() {
		int delay = 0;
		int maxDepth = _scanners.Max(s => s.Key);
		bool caught = true;
		do {
			delay++;
			caught = false;
			for (int tick = 0; tick <= maxDepth; tick++) {
				Scanner? scanner = _scanners.GetValueOrDefault(tick);
				if (scanner is not null && scanner.IsCaptured(0, tick + delay)) {
					caught = true;
					break;
				}
			}
		} while (caught is true);

		return delay;
	}
}

file static class Day13Extensions
{
	public static bool IsCaptured(this Scanner scanner, int rangePosition, int tick)
	{
		int length = scanner.Range + scanner.Range - 2;
		if (tick % length == rangePosition) {
			return true;
		} else if (tick % length == scanner.Range - 1 + (scanner.Range - 1 - rangePosition)) {
			return true;
		}

		return false;
	}

	public static void VisualiseScanners(this Dictionary<int, Scanner> scanners, int current, int tick, string title, Action<string[], bool>? visualise)
	{
		const char SCANNER = 'S';

		if (visualise is not null) {
			int maxDepth = scanners.Max(s => s.Key);
			int maxRange = scanners.Max(s => s.Value.Range);

			string[] firewallDiagram = [];
			for (int range = 0; range < maxRange; range++) {
				string items = "";
				for (int layer = 0; layer <= maxDepth; layer++) {
					Scanner? scanner = scanners.GetValueOrDefault(layer);
					string item = "";
					if (scanner is null) {
						item = $"{(range == 0 ? "..." : "   ")}";
					} else if (range < scanner.Range) {
						item = scanner.IsCaptured(range, tick)
							? $"[{SCANNER}]"
							: $"[ ]";
					} else {
						item = $"   ";
					}

					if (range == 0 && current == layer) {
						item = item
							.Replace('[', '(')
							.Replace(']', ')')
							.Replace("...", "(.)");
					}

					items = $"{items}{item} ";
				}

				firewallDiagram = [.. firewallDiagram, items.TrimEnd()];
			}

			string[] output = ["", title, .. firewallDiagram];
			_ = Task.Run(() => visualise?.Invoke(output, false));
		}
	}
}

internal sealed partial class Day13Types
{
	public sealed record Scanner(int Depth, int Range) : IParsable<Scanner>
	{
		public static Scanner Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit(':');
			return new(tokens[0].As<int>(), tokens[1].As<int>());
		}

		public static Scanner Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Scanner result)
			=> ISimpleParsable<Scanner>.TryParse(s, provider, out result);
	}
}

file static class Day13Constants
{
}
