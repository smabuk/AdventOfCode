namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 08: Two-Factor Authentication
/// https://adventofcode.com/2016/day/08
/// </summary>
[Description("Two-Factor Authentication")]
public sealed partial class Day08 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadInstructions(input);
	public static string Part1(string[] input, Action<string[], bool>? visualise = null, params object[]? args)
	{
		int screenWidth = GetArgument<int>(args, argumentNumber: 1, defaultResult: 50);
		int screenHeight = GetArgument<int>(args, argumentNumber: 2, defaultResult: 6);
		return Solution1(screenWidth, screenHeight, visualise).ToString();
	}

	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private const char OFF = '.';
	private const char ON = '#';

	private static List<Instruction> _instructions = [];

	private static void LoadInstructions(string[] input) => _instructions = [.. input.As<Instruction>()];

	private static int Solution1(int screenWidth, int screenHeight, Action<string[], bool>? visualise = null)
	{
		char[,] screen = Smab.Helpers.ArrayHelpers.Create2dArray(screenWidth ,screenHeight, OFF);
		DisplayScreen(screen, "Initial", visualise);

		foreach (Instruction instruction in _instructions) {
			screen = instruction.Action(screen);
			DisplayScreen(screen, $"{instruction.ToString().Replace("Instruction", "")}", visualise);
		}

		return screen.WalkWithValues().Count(pixel => pixel.Value == ON);
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}

	private static void DisplayScreen(char[,] screen, string title, Action<string[], bool>? visualise)
	{
		if (visualise is not null) {
			string[] output = ["", title, .. screen.PrintAsStringArray(0)];
			_ = Task.Run(() => visualise?.Invoke(output, false));
		}
	}
	
	private abstract record Instruction() : IParsable<Instruction> {
		public abstract char[,] Action(char[,] screen);
		public static Instruction Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit([' ', 'x', 'y', '=']);
			return tokens[0] switch
			{
				"rect" => new RectInstruction(tokens[1].As<int>(), tokens[2].As<int>()),
				"rotate" when tokens[1] == "row"    => new RotateRowInstruction(tokens[2].As<int>(), tokens[4].As<int>()),
				"rotate" when tokens[1] == "column" => new RotateColInstruction(tokens[2].As<int>(), tokens[4].As<int>()),
				_ => null!,
			};
		}

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	private record RectInstruction(int Width, int Height) : Instruction()
	{
		public override char[,] Action(char[,] screen)
		{
			char[,] screenCopy = (char[,])screen.Clone();

			for (int row = 0; row < Height; row++) {
			for (int col = 0; col < Width; col++) {
				screenCopy[col, row] = ON;
			}
			}

			return screenCopy;
		}
	}

	private record RotateRowInstruction(int Row, int By) : Instruction()
	{
		public override char[,] Action(char[,] screen)
		{
			char[,] screenCopy = (char[,])screen.Clone();
			int cols = screen.ColsCount();

			for (int col = 0; col < cols; col++) {
				screenCopy[(col + By) % cols, Row] = screen[col, Row];
			}

			return screenCopy;
		}
	}

	private record RotateColInstruction(int Col, int By) : Instruction()
	{
		public override char[,] Action(char[,] screen)
		{
			char[,] screenCopy = (char[,])screen.Clone();

			int rows = screen.RowsCount();

			for (int row = 0; row < rows; row++) {
				screenCopy[Col, (row + By) % rows] = screen[Col, row];
			}

			return screenCopy;
		}
	}
}
