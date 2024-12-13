using HugeLocation = (long X, long Y);

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 13: Claw Contraption
/// https://adventofcode.com/2024/day/13
/// </summary>
[Description("Claw Contraption")]
public static partial class Day13 {

	private static IEnumerable<ClawMachine> _clawMachines = [];

	[Init]
	public static void LoadClawMachines(string[] input)
		=> _clawMachines = [.. input.Chunk(4).Select(ClawMachine.Parse)];

	public static long Part1(string[] _)
		=> _clawMachines.Sum(claw => claw.TrySolveCost(out long cost) ? cost : 0);

	public static long Part2(string[] _)
		=> _clawMachines.Sum(claw => claw.MakeHuge().TrySolveCost(out long cost) ? cost : 0);



	private static bool TrySolveCost(this ClawMachine clawmachine, out long cost)
	{
		const int buttonACost = 3;
		
		cost = 0;

		long ax = clawmachine.A.DX;
		long bx = clawmachine.B.DX;
		long px = clawmachine.PrizeLocation.X;

		long ay = clawmachine.A.DY;
		long by = clawmachine.B.DY;
		long py = clawmachine.PrizeLocation.Y;

		long axby = ax * by;
		long pxby = px * by;

		long aybx = ay * bx;
		long pybx = py * bx;

		long X = axby - aybx;
		long P = pxby - pybx;

		(long a, long remainder) = long.DivRem(P, X);
		if (remainder is not 0) {
			return false;
		}

		(long b, remainder) = long.DivRem(px - (ax * a), bx);
		if (remainder is not 0) {
			return false;
		}

		cost = (a * buttonACost) + b;
		return true;
	}

	private static ClawMachine MakeHuge(this ClawMachine clawMachine)
		=> clawMachine with
		{ PrizeLocation = new(clawMachine.PrizeLocation.X + 10_000_000_000_000, clawMachine.PrizeLocation.Y + 10_000_000_000_000) };


	private sealed record Button(int DX, int DY);

	private sealed record ClawMachine(Button A, Button B, HugeLocation PrizeLocation)
	{
		public static ClawMachine Parse(IEnumerable<string> s)
		{
			string[] lines = [.. s];

			int[] numbers = [.. lines[0][12..].Replace("Y+", "").AsInts()];
			Button buttonA = new(numbers[0], numbers[1]);

			numbers = [..lines[1][12..].Replace("Y+", "").AsInts()];
			Button buttonB = new(numbers[0], numbers[1]);

			numbers = [.. lines[2][9..].Replace("Y=", "").AsInts()];
			Point prizeLocation = new(numbers[0], numbers[1]);

			return new(buttonA, buttonB, prizeLocation);
		}
	}
}
