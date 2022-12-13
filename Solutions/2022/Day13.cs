using System.Text.Json;

namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 13: Distress Signal
/// https://adventofcode.com/2022/day/13
/// </summary>
[Description("Distress Signal")]
public sealed partial class Day13 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		List<Pair> pairs = new();
		List<int> correctPairs = new();

		for (int i = 0; i < input.Length; i += 3) {
			pairs.Add(new (Packet.Parse(input[i]), Packet.Parse(input[i + 1])));
		}

		for (int i = 0; i < pairs.Count; i++) {
			if (pairs[i].Left.CompareItem(pairs[i].Right) ?? false) {
				correctPairs.Add(i);
			}
		}

		return correctPairs.Sum(x => x + 1);
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

	private record Pair(Packet Left, Packet Right) {
		public static Pair Parse(string left, string right)
			=>  new (Packet.Parse(left), Packet.Parse(right));
	};

	private abstract record Packet {
		public static Packet Parse(string value)
			=> Parse(JsonSerializer.Deserialize<JsonElement>(value));

		public static Packet Parse(JsonElement el) {
			Packet value = el.ValueKind switch {
				JsonValueKind.Number => new NumberPacket(el.GetInt32()),
				JsonValueKind.Array => new ListPacket(el.EnumerateArray().Select(Parse).ToList()),
				_ => throw new NotImplementedException(),
			};
			return value; 
		}

		// null represents continue checking
		public bool? CompareItem(Packet right) {
			Packet left = this;
			
			if (left is not null && right is null) {
				return false;
			}

			if (left is null && right is not null) {
				return true;
			}

			if (left is NumberPacket ln && right is NumberPacket rn) {
				if (ln.Value < rn.Value) {
					return true;
				} else if (ln.Value > rn.Value) {
					return false;
				} else {
					return null;
				}
			} 

			if (left is ListPacket && right is NumberPacket) {
				return left.CompareItem(new ListPacket(new() { right }));
			}

			if (left is NumberPacket && right is ListPacket) {
				return new ListPacket(new() { left }).CompareItem(right);
			}

			if (left is ListPacket lp && right is ListPacket rp) {
				for (int i = 0; i < lp.Packets.Count; i++) {
					if (rp.Packets.Count <= i) {
						return false;
					}
					bool? res = lp.Packets[i].CompareItem(rp.Packets[i]);
					if (res is not null) {
						return res;
					};
				} 
				return true;
			}
			return null;
		}
	};

	private record NumberPacket(int Value) : Packet {
		public override string ToString() => $"{Value}";
	}

	private record ListPacket(List<Packet> Packets) : Packet {
		public override string ToString() => $"[{String.Join(",", Packets)}]";
	};


}
