using static AdventOfCode.Solutions._2016.Day10Constants;
using static AdventOfCode.Solutions._2016.Day10Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 10: Balance Bots
/// https://adventofcode.com/2016/day/10
/// </summary>
[Description("Balance Bots")]
public sealed partial class Day10
{

	[Init]
	public static void Init(string[] input) => LoadInstructions(input);
	public static string Part1(string[] _, params object[]? args)
	{
		int compare1 = GetArgument<int>(args, argumentNumber: 1, defaultResult: 61);
		int compare2 = GetArgument<int>(args, argumentNumber: 2, defaultResult: 17);
		return Solution1(compare1, compare2).ToString();
	}
	public static string Part2(string[] _) => Solution2().ToString();

	private static IEnumerable<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input) => _instructions = input.As<Instruction>();

	private static int Solution1(int compare1, int compare2)
	{
		PopulateBotsAndBins(out Dictionary<int, Bot> bots, out Dictionary<int, OutputBin> bins);
		(int compareLow, int compareHigh) = ((int[])[compare1, compare2]).MinMax();
		int foundBotNo = NOT_FOUND;

		foreach (ValueInstruction instruction in _instructions.Where(i => i is ValueInstruction).Cast<ValueInstruction>()) {
			int botNo = instruction.BotNo;
			Bot bot = bots[botNo];
			bot.Chips.Add(instruction.Value);
			if (bot.Chips.Count == 2) {
				foundBotNo = bot.ExecuteRule(bots, bins, compareLow, compareHigh);
				if (foundBotNo != NOT_FOUND) {
					return foundBotNo;
				}
			}
		}

		throw new ApplicationException("Should never reach here!");
	}

	private static long Solution2()
	{
		PopulateBotsAndBins(out Dictionary<int, Bot> bots, out Dictionary<int, OutputBin> bins);

		foreach (ValueInstruction instruction in _instructions.Where(i => i is ValueInstruction).Cast<ValueInstruction>()) {
			int botNo = instruction.BotNo;
			Bot bot = bots[botNo];
			bot.Chips.Add(instruction.Value);
			if (bot.Chips.Count == 2) {
				_ = bot.ExecuteRule(bots, bins);
			}
		}

		return bins[0].Chips[0] * bins[1].Chips[0] * bins[2].Chips[0];
	}

	private static void PopulateBotsAndBins(out Dictionary<int, Bot> bots, out Dictionary<int, OutputBin> bins)
	{
		bots = (_instructions
			.Where(i => i is GiveInstruction gi)
			.Select(i => new Bot(i.BotNo, (GiveInstruction)i, [])))
			.ToDictionary(b => b.No, bot => bot);

		bins = (_instructions
			.Where(i => i is GiveInstruction gi && gi.BotIsLow is false)
			.Select(i => new OutputBin(((GiveInstruction)i).LowDestination, [])))
			.Union(_instructions
			.Where(i => i is GiveInstruction gi && gi.BotIsHigh is false)
			.Select(i => new OutputBin(((GiveInstruction)i).HighDestination, [])))
			.DistinctBy(b => b.No)
			.ToDictionary(b => b.No, bin => bin);
	}
}

file static class Day10Extensions
{
}

internal sealed partial class Day10Types
{

	public record Bot(int No, GiveInstruction Instruction, List<int> Chips)
	{
		internal int ExecuteRule(Dictionary<int, Bot> bots, Dictionary<int, OutputBin> bins, int? compareLow = null, int? compareHigh = null)
		{
			(int low, int high) = Chips.MinMax();
			if (compareLow is not null && low == compareLow && high == compareHigh) {
				return No;
			}

			if (Instruction.BotIsLow) {
				_ = Chips.Remove(low);
				bots[Instruction.LowDestination].Chips.Add(low);
				if (bots[Instruction.LowDestination].Chips.Count == 2) {
					int foundBotNo = bots[Instruction.LowDestination].ExecuteRule(bots, bins, compareLow, compareHigh);
					if (foundBotNo != NOT_FOUND) {
						return foundBotNo;
					}
				}
			}

			if (Instruction.BotIsHigh) {
				_ = Chips.Remove(high);
				bots[Instruction.HighDestination].Chips.Add(high);
				if (bots[Instruction.HighDestination].Chips.Count == 2) {
					int foundBotNo = bots[Instruction.HighDestination].ExecuteRule(bots, bins, compareLow, compareHigh);
					if (foundBotNo != NOT_FOUND) {
						return foundBotNo;
					}
				}
			}

			if (Instruction.BotIsLow is false) {
				_ = Chips.Remove(low);
				bins[Instruction.LowDestination].Chips.Add(low);
			}

			if (Instruction.BotIsHigh is false) {
				_ = Chips.Remove(high);
				bins[Instruction.HighDestination].Chips.Add(high);
			}

			return NOT_FOUND;
		}
	}

	public record OutputBin(int No, List<int> Chips);

	public abstract record Instruction(int BotNo) : IParsable<Instruction>
	{
		public static Instruction Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit(' ');
			if (tokens[0] == "value") {
				return new ValueInstruction(tokens[5].As<int>(), tokens[1].As<int>());
			}

			if (tokens[0] == "bot") {
				return new GiveInstruction(
					tokens[1].As<int>(),
					tokens[6].As<int>(),
					tokens[11].As<int>(),
					tokens[5] == "bot",
					tokens[10] == "bot");
			}

			throw new ArgumentException("Failed to parse!");
		}

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	public record ValueInstruction(int BotNo, int Value) : Instruction(BotNo);
	public record GiveInstruction(int BotNo, int LowDestination, int HighDestination, bool BotIsLow, bool BotIsHigh) : Instruction(BotNo);
}

file static class Day10Constants
{
	public const int NOT_FOUND = int.MinValue;
}
