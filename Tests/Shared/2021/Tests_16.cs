namespace AdventOfCode.Tests.Year2021;

public class Tests_16_Packet_Decoder {
	[Theory]
	[InlineData(new string[] { "D2FE28" }, 6)]
	[InlineData(new string[] { "38006F45291200" }, 9)]
	[InlineData(new string[] { "EE00D40C823060" }, 14)]
	[InlineData(new string[] { "8A004A801A8002F478" }, 16)]
	[InlineData(new string[] { "620080001611562C8802118E34" }, 12)]
	[InlineData(new string[] { "C0015000016115A2E0802F182340" }, 23)]
	[InlineData(new string[] { "A0016C880162017C3686B18A3D4780" }, 31)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 16, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { "C200B40A82" }, 3)]
	[InlineData(new string[] { "04005AC33890" }, 54)]
	[InlineData(new string[] { "880086C3E88112" }, 7)]
	[InlineData(new string[] { "CE00C43D881120" }, 9)]
	[InlineData(new string[] { "D8005AC2A8F0" }, 1)]
	[InlineData(new string[] { "F600BC2D8F" }, 0)]
	[InlineData(new string[] { "9C005AC2F8F0" }, 0)]
	[InlineData(new string[] { "9C0141080250320F1802104A08" }, 1)]
	public void Part2(string[] input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2021, 16, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}
}
