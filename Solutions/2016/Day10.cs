using static AdventOfCode.Solutions._2016.Day10Constants;
using static AdventOfCode.Solutions._2016.Day10Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 10: Balance Bots
/// https://adventofcode.com/2016/day/10
/// </summary>
[Description("Balance Bots")]
public sealed partial class Day10 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadInstructions(input);
	public static string Part1(string[] _, params object[]? args)
	{
		int compare1 = GetArgument<int>(args, argumentNumber: 1, defaultResult: 61);
		int compare2 = GetArgument<int>(args, argumentNumber: 2, defaultResult: 17);
		return Solution1(compare1, compare2).ToString();
	}
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static IEnumerable<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input) => _instructions = input.As<Instruction>();

	private static int Solution1(int compare1, int compare2) {
		(int compareLow, int compareHigh) = ((int[])[compare1, compare2]).MinMax();
		int foundBotNo = NOT_FOUND;
		Dictionary<int, Bot> bots = (_instructions
			.Where(i => i is GiveInstruction gi)
			.Select(i => new Bot(i.BotNo, (GiveInstruction)i, [])))
			.ToDictionary(b => b.No, bot => bot);

		Dictionary<int, OutputBin> bins = (_instructions
			.Where(i => i is GiveInstruction gi && gi.BotIsLow is false)
			.Select(i => new OutputBin(((GiveInstruction)i).LowDestination, [])))
			.Union(_instructions
			.Where(i => i is GiveInstruction gi && gi.BotIsHigh is false)
			.Select(i => new OutputBin(((GiveInstruction)i).HighDestination, [])))
			.DistinctBy(b => b.No)
			.ToDictionary(b => b.No, bin => bin);

		foreach (ValueInstruction instruction in _instructions.Where(i => i is ValueInstruction).Cast<ValueInstruction>()) {
			int botNo = instruction.BotNo;
			Bot bot = bots[botNo];
			bot.Chips.Add(instruction.Value);
			if (bot.Chips.Count == 2) {
				foundBotNo = bot.ExecuteRule(bots, bins, compareLow , compareHigh);
				if (foundBotNo != NOT_FOUND) {
					return foundBotNo;
				}
			}
		}

		throw new ApplicationException("Should never reach here!");
	}

	private static string Solution2(string[] input) {
		//List<Instruction> instructions = [.. input.As<Instruction>()];
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day10Extensions
{
}

internal sealed partial class Day10Types
{

	public record Bot(int No, GiveInstruction Rule, List<int> Chips)
	{
		internal int ExecuteRule(Dictionary<int, Bot> bots, Dictionary<int, OutputBin> bins, int compareLow, int compareHigh)
		{
			(int low, int high) = Chips.MinMax();
			if (low == compareLow && high == compareHigh) {
				return No;
			}
			if (Rule.BotIsLow) {
				_ = Chips.Remove(low);
				bots[Rule.LowDestination].Chips.Add(low);
				if (bots[Rule.LowDestination].Chips.Count == 2) {
					int foundBotNo = bots[Rule.LowDestination].ExecuteRule(bots, bins, compareLow, compareHigh);
					if (foundBotNo != NOT_FOUND) {
						return foundBotNo;
					}
				}
			}
			if (Rule.BotIsHigh) {
				_ = Chips.Remove(high);
				bots[Rule.HighDestination].Chips.Add(high);
				if (bots[Rule.HighDestination].Chips.Count == 2) {
					int foundBotNo = bots[Rule.HighDestination].ExecuteRule(bots, bins, compareLow, compareHigh);
					if (foundBotNo != NOT_FOUND) {
						return foundBotNo;
					}
				}
			}
			if (Rule.BotIsLow is false) {
				_ = Chips.Remove(low);
				bins[Rule.LowDestination].Chips.Add(low);
			}
			if (Rule.BotIsHigh is false) {
				_ = Chips.Remove(high);
				bins[Rule.HighDestination].Chips.Add(high);
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
			return null!;
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
