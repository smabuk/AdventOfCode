namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 16: Packet Decoder
/// https://adventofcode.com/2021/day/16
/// </summary>
[Description("Packet Decoder")]
public class Day16 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record Packet(string Bits, int Version, int Type) {
		public List<Packet> Packets = new();
		public int TotalVersion => Version + Packets.Sum(p => p.TotalVersion);
		public virtual long Value { get; set; } = 9999;
	};

	record OuterPacket(string Bits, int Version, int Type) : Packet(Bits, Version, Type) {
		public override long Value => ((OperatorPacket)Packets.Single()).Value;
	}
	record OperatorPacket(string Bits, int Version, int Type) : Packet(Bits, Version, Type) {
		public override long Value => Type switch {
			0 => Packets.Sum(p => p.Value),
			1 => Packets.Select(p => p.Value).Aggregate((a, b) => a * b),
			2 => Packets.Select(p => p.Value).Min(),
			3 => Packets.Select(p => p.Value).Max(),
			5 => Packets[0].Value > Packets[1].Value ? 1 : 0,
			6 => Packets[0].Value < Packets[1].Value ? 1 : 0,
			7 => Packets[0].Value == Packets[1].Value ? 1 : 0,
			_ => throw new NotImplementedException(),
		};
	}

	record LiteralPacket(string Bits, int Version, int Type, string Number) : Packet(Bits, Version, Type) {
		public override long Value => Convert.ToInt64(Number, 2);
	};

	private static int Solution1(string[] input) {
		string transmission = input[0].AsBinaryFromHex();
		OuterPacket packet = ParsePackets(transmission, true).Packet as OuterPacket ?? new OuterPacket("", 0, 0);
		return packet.TotalVersion;
	}

	private static long Solution2(string[] input) {
		string transmission = input[0].AsBinaryFromHex();
		OuterPacket packet = ParsePackets(transmission, true).Packet as OuterPacket ?? new OuterPacket("", 0, 0);
		return packet.Value;
	}

	private static (Packet Packet, string RestOfString) ParsePackets(string transmission, bool isOuterPacket = false) {
		int bitsIndex = 0;

		if (isOuterPacket) {
			OuterPacket outerPacket = new("", 0, 0);
			string newTransmission = transmission[bitsIndex..];
			(Packet packet, newTransmission) = ParsePackets(newTransmission);
			outerPacket.Packets.Add(packet);
			return (outerPacket, "");
		}

		int startIndex = bitsIndex;
		int version = ToIntFromBinary(transmission[bitsIndex..(bitsIndex + 3)]);
		bitsIndex += 3;
		int type = ToIntFromBinary(transmission[bitsIndex..(bitsIndex + 3)]);
		bitsIndex += 3;
		if (type == 4) { // literal
			string number = "";
			while (transmission[bitsIndex] == '1') {
				number += transmission[(bitsIndex + 1)..(bitsIndex + 5)];
				bitsIndex += 5;
			}
			number += transmission[(bitsIndex + 1)..(bitsIndex + 5)];
			bitsIndex += 5;
			return new (new LiteralPacket(transmission[startIndex..(startIndex + bitsIndex)], version, type, number), transmission[(bitsIndex)..]);
		} else { // operator
			int packetsLength;
			int lengthTypeId = transmission[bitsIndex] == '0' ? 0 : 1;
			if (lengthTypeId == 0) {
				OperatorPacket newPacket = new("", version, type);
				packetsLength = ToIntFromBinary(transmission[(bitsIndex + 1)..(bitsIndex + 16)]);
				bitsIndex += 16;
				string newTransmission = transmission[bitsIndex..(bitsIndex + packetsLength)];
				while (newTransmission.Length > 0) {
					(Packet packet, string restTransmission) = ParsePackets(newTransmission);
					newPacket.Packets.Add(packet);
					newTransmission = restTransmission;
				}
				bitsIndex += packetsLength;
				newPacket = newPacket with { Bits = transmission[startIndex..bitsIndex] };
				return new (newPacket, transmission[bitsIndex..]);
			} else {
				OperatorPacket newPacket = new("", version, type);
				packetsLength = ToIntFromBinary(transmission[(bitsIndex + 1)..(bitsIndex + 12)]);
				bitsIndex += 12;
				string newTransmission = transmission[bitsIndex..];
				for (int i = 0; i < packetsLength; i++) {
					(Packet packet, string restTransmission) = ParsePackets(newTransmission);
					newPacket.Packets.Add(packet);
					bitsIndex += newTransmission.Length - restTransmission.Length;
					newTransmission = restTransmission;
				}
				return new (newPacket, transmission[bitsIndex..]);
			}
		}
	}

	private static int ToIntFromBinary(string binary) {
		return Convert.ToInt32(binary, 2);
	}
}
