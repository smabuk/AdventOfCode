using static AdventOfCode.Solutions._2021.Day16.Packet;
using static AdventOfCode.Solutions._2021.Day16.Packet.Operation;

namespace AdventOfCode.Solutions._2021;

/// <summary>
/// Day 16: Packet Decoder
/// https://adventofcode.com/2021/day/16
/// </summary>
[Description("Packet Decoder")]
public class Day16 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		string transmission = input[0].AsBinaryFromHex();
		return Packet.Parse(transmission).SumOfVersions;
	}

	private static long Solution2(string[] input) {
		string transmission = input[0].AsBinaryFromHex();
		return Packet.Parse(transmission).Value;
	}


	public abstract record Packet(int Version, Operation Type) {
		public List<Packet> Packets = new();
		protected List<long> PacketValues => Packets.Select(p => p.Value).ToList();
		public abstract long Value { get; }
		public int SumOfVersions => Version + Packets.Sum(p => p.SumOfVersions);
		
		
		public static Packet Parse(string transmission) => ParsePackets(transmission).Packet;
		private static (Packet Packet, string RestOfTransmission) ParsePackets(string transmission) {
			int bitsIndex = 0;

			int version = ToIntFromBinary(transmission[bitsIndex..(bitsIndex += 3)]);
			Operation type = (Operation)ToIntFromBinary(transmission[bitsIndex..(bitsIndex += 3)]);
			if (type == Literal) {
				string number = "";
				while (transmission[bitsIndex] == '1') {
					number += transmission[(bitsIndex + 1)..(bitsIndex += 5)];
				}
				number += transmission[(bitsIndex + 1)..(bitsIndex += 5)];
				return (new LiteralPacket(version, type, number), transmission[bitsIndex..]);
			} else {
				int packetsLength;
				int lengthTypeId = ToIntFromBinary(transmission[bitsIndex].ToString());
				OperatorPacket newPacket = new(version, type);
				if (lengthTypeId == 0) {
					packetsLength = ToIntFromBinary(transmission[(bitsIndex + 1)..(bitsIndex += 16)]);
					string newTransmission = transmission[bitsIndex..(bitsIndex + packetsLength)];
					while (newTransmission.Length > 0) {
						(Packet packet, string restOfTransmission) = ParsePackets(newTransmission);
						newPacket.Packets.Add(packet);
						newTransmission = restOfTransmission;
					}
					bitsIndex += packetsLength;
					return (newPacket, transmission[bitsIndex..]);
				} else {
					packetsLength = ToIntFromBinary(transmission[(bitsIndex + 1)..(bitsIndex += 12)]);
					string newTransmission = transmission[bitsIndex..];
					for (int i = 0; i < packetsLength; i++) {
						(Packet packet, string restOfTransmission) = ParsePackets(newTransmission);
						newPacket.Packets.Add(packet);
						bitsIndex += newTransmission.Length - restOfTransmission.Length;
						newTransmission = restOfTransmission;
					}
					return (newPacket, transmission[bitsIndex..]);
				}
			}
		}

		private static int ToIntFromBinary(string binary) => Convert.ToInt32(binary, 2);

		public enum Operation {
			Sum         = 0,
			Product     = 1,
			Minimum     = 2,
			Maximum     = 3,
			Literal     = 4,
			LessThan    = 5,
			GreaterThan = 6,
			EqualTo     = 7
		}
	};

	public record LiteralPacket(int Version, Operation Type, string Number) : Packet(Version, Type) {
		public override long Value => Convert.ToInt64(Number, 2);
	};

	public record OperatorPacket(int Version, Operation Type) : Packet(Version, Type) {
		public override long Value => Type switch {
			Sum         => PacketValues.Sum(),
			Product     => PacketValues.Aggregate((a, b) => a * b),
			Minimum     => PacketValues.Min(),
			Maximum     => PacketValues.Max(),
			LessThan    => PacketValues[0] >  PacketValues[1] ? 1 : 0,
			GreaterThan => PacketValues[0] <  PacketValues[1] ? 1 : 0,
			EqualTo     => PacketValues[0] == PacketValues[1] ? 1 : 0,
			_ => throw new NotImplementedException(),
		};
	}
}
