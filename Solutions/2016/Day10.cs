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
	public static string Part1(string[] _, Action<string[], bool>? visualise = null, params object[]? args)
	{
		int compare1 = GetArgument<int>(args, argumentNumber: 1, defaultResult: 61);
		int compare2 = GetArgument<int>(args, argumentNumber: 2, defaultResult: 17);
		return Solution1(compare1, compare2).ToString();
	}
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static IEnumerable<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input) => _instructions = input.As<Instruction>();

	private static int Solution1(int compare1, int compare2) {
		Dictionary<int, Bot> bots = (_instructions
			.Where(i => i is ValueInstruction vi)
			.Select(i => new Bot(((ValueInstruction)i).BotNo, [])))
			.Union(_instructions
			.Where(i => i is GiveInstruction gi && gi.BotIsLow)
			.Select(i => new Bot(((GiveInstruction)i).LowDestination, [])))
			.Union(_instructions
			.Where(i => i is GiveInstruction gi && gi.BotIsHigh)
			.Select(i => new Bot(((GiveInstruction)i).HighDestination, [])))
			.DistinctBy(b => b.No)
			.ToDictionary(b => b.No, bot => bot);

		Dictionary<int, OutputBin> bins = (_instructions
			.Where(i => i is GiveInstruction gi && gi.BotIsLow is false)
			.Select(i => new OutputBin(((GiveInstruction)i).LowDestination, [])))
			.Union(_instructions
			.Where(i => i is GiveInstruction gi && gi.BotIsHigh is false)
			.Select(i => new OutputBin(((GiveInstruction)i).HighDestination, [])))
			.DistinctBy(b => b.No)
			.ToDictionary(b => b.No, bin => bin);

		foreach (Instruction instruction in _instructions) {
			int botNo = instruction.BotNo;
			Bot bot = bots[botNo];
			if (instruction is ValueInstruction valueIns) {
				bot.Chips.Add(valueIns.Value);
			}
		}
		foreach (Instruction instruction in _instructions) {
			int botNo = instruction.BotNo;
			Bot bot = bots[botNo];
			if (instruction is GiveInstruction giveIns) {
				(int low, int high) = bot.Chips.MinMax();
				if (low == compare1 && high == compare2) {
					return botNo;
				}
				if (giveIns.BotIsLow) {
					_ = bot.Chips.Remove(low);
					bots[giveIns.LowDestination].Chips.Add(low);
				}
				if (giveIns.BotIsHigh) {
					_ = bot.Chips.Remove(high);
					bots[giveIns.HighDestination].Chips.Add(high);
				}
				if (giveIns.BotIsLow is false) {
					_ = bot.Chips.Remove(low);
					bins[giveIns.LowDestination].Chips.Add(low);
				}
				if (giveIns.BotIsHigh is false) {
					_ = bot.Chips.Remove(high);
					bins[giveIns.HighDestination].Chips.Add(high);
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

	public record Bot(int No, List<int> Chips);
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
}
